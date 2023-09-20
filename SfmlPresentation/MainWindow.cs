using Business.Contracts.Parser;
using SFML.Graphics;
using SFML.Window;
using SfmlPresentation.Contracts;
using System.Drawing;
using System.Numerics;
using Image = SFML.Graphics.Image;
using Color = SFML.Graphics.Color;
using Domain.ObjClass;
using System.Diagnostics;
using SfmlPresentation.Scene;
using static SFML.Window.Keyboard;
using Business.Contracts;
using SFML.System;

public partial class MainWindow
{
    private readonly IObjFileParcer _objFileParcer;
    private readonly ITransformationHelper _transformationHelper;    
    private readonly IRasterizationObjDrawer _rasterizationObjDrawer;

    public MainWindow(IObjFileParcer objFileParcer,
                      ITransformationHelper transformationHelper,                      
                      IRasterizationObjDrawer rasterizationObjDrawer)
    {
        this._objFileParcer = objFileParcer;
        this._transformationHelper = transformationHelper;        
        _rasterizationObjDrawer = rasterizationObjDrawer;
    }

    private List<Vector3> _vertices;    
    private Texture _pixelTexture;
    private Image _image;
    private Sprite _pixelSprite;

    private int _scale = 12;  

    private Camera _camera = new Camera(Math.PI / 2, 0, 7);
    private Camera _light = new Camera(Math.PI / 2, 0, 7);
    private bool _isSticky = false;
    private bool[] keyHandled = new bool[(int)Key.KeyCount];
    private Obj _obj;

    private RenderWindow _app;
    private uint _screenWidth;
    private uint _screenHeight;


    private long _elapsedTicks;
    private long _elapsedMilliseconds;
    private Point _startPosition;    
    private bool _isDown;

    private float _Smoothness = 0.004f;
    private float _rSmoothness = 1f;

    private float _multiplyer = 0.0001f;

    private float _lightSmoothnessX = 0.005f;
    private float _lightSmoothnessY = 0.005f;
    private float _lightSmoothnessR = 0.02f;
    private float _cameraSmoothnessX = 0.002f;
    private float _cameraSmoothnessY = 0.002f;
    private float _cameraSmoothnessR = 0.01f;
    private bool _isMoving = true;

    void AppConfiguration()
    {
        var desktopMode = VideoMode.DesktopMode;
        //_app = new RenderWindow(new VideoMode(20, 20), "Renderer", Styles.Default);
        _app = new RenderWindow(desktopMode, "Renderer", Styles.Default);
        _app.Closed += (sender, e) => _app.Close();
        _app.MouseButtonPressed += App_MouseButtonPressed;
        _app.MouseMoved += _app_MouseMoved;        
        _app.MouseButtonReleased += _app_MouseButtonReleased;
        _app.MouseWheelScrolled += _app_MouseWheelScrolled;
        _isDown = false;
        _app.Resized += _app_Resized;
        _screenWidth = desktopMode.Width;
        _screenHeight = desktopMode.Height;
    }

    private void _app_Resized(object? sender, SizeEventArgs e)
    {
        _screenWidth = e.Width;
        _screenHeight = e.Height;

        FloatRect visibleArea = new FloatRect(0, 0, _screenWidth, _screenHeight);
        _app.SetView(new View(visibleArea));

        CanvasConfiguration(_screenWidth, _screenHeight);
        _isMoving = true;
    }

    void LoadScene(string path)
    {
        _obj = _objFileParcer.ParseObjFile(path);
        _vertices = _transformationHelper.ConvertToGlobalCoordinates(_obj, _scale, (new Vector3(1, 1, 1)), 0);             
    }

    void CanvasConfiguration(uint screenWidth, uint screenHeight)
    {
        _pixelTexture = new Texture(screenWidth, screenHeight);
        _image = new Image(screenWidth, screenHeight);
        _pixelSprite = new Sprite(_pixelTexture);        
    }
    public void Run()
    {
        AppConfiguration();
        LoadScene(@"D:\Projects\7thSem\Graphics\Renderer\Tests\Parser\TestData\dragon.obj");
        CanvasConfiguration(_screenWidth, _screenHeight);

        var stopwatch = new Stopwatch();
        while (_app.IsOpen)
        {
            _elapsedTicks = stopwatch.ElapsedTicks;
            _elapsedMilliseconds = stopwatch.ElapsedMilliseconds;            
            stopwatch.Restart();
            _app.DispatchEvents();
            HandleKeyboardInput();
            
            //if (_isMoving) 
                DrawImage();
            _isMoving = false;
            
            if (_elapsedMilliseconds > 0)
            {
                Console.WriteLine($"FPS: {(_elapsedMilliseconds != 0 ? (1000.0f / _elapsedMilliseconds).ToString() : "inf")} ({_elapsedMilliseconds} ms/frame);");            
            }
        }
    }

    void DrawImage()
    {
        ClearImage(Color.Black);
        _rasterizationObjDrawer.Draw(_obj.FaceList, _vertices, _image, _camera.Eye, _light.Eye);
        _pixelTexture.Update(_image);
        _app.Draw(_pixelSprite);
        _app.Display();     
    }

    private void _app_MouseWheelScrolled(object? sender, MouseWheelScrollEventArgs e)
    {
        _camera.R += -e.Delta * _rSmoothness;
        _isMoving = true;
    }    

    private void _app_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
    {
        _isDown = false;
    }

    private void _app_MouseMoved(object? sender, MouseMoveEventArgs e)
    {
        if (!_isDown) return;    
        var newPosition = new Point(e.X, e.Y);
        var deltaX = _startPosition.X - newPosition.X;
        var deltaY = _startPosition.Y - newPosition.Y;
        _startPosition = newPosition;

        _camera.Alpha += deltaY * _Smoothness;
        _camera.ChangeBetaIncrement(deltaX * _Smoothness);
        _isMoving = true;       
    }

    private void App_MouseButtonPressed(object? sender, MouseButtonEventArgs e)
    {
        _isDown = true;
        _startPosition = new Point(e.X, e.Y);
    }
    void HandleKeyboardInput()
    {        
        if (Keyboard.IsKeyPressed(Keyboard.Key.Space) && !keyHandled[(int)Key.Space])
        {
            keyHandled[(int)Key.Space] = true;
            if (!_isSticky)
            {
                _isSticky = true;                             
                _light = _camera;                
            }
            else
            {
                _isSticky = false;
                _light = new Camera(_camera.Alpha, _camera.Beta, _camera.R);                
            }
            _isMoving = true;
        }
        else if (!Keyboard.IsKeyPressed(Key.Space))
        {
            keyHandled[(int)Key.Space] = false;
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
        {            
            _camera.ChangeBetaIncrement(_elapsedTicks * _multiplyer * - _cameraSmoothnessX);
            _isMoving = true;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
        {         
            _camera.ChangeBetaIncrement(_elapsedTicks * _multiplyer * _cameraSmoothnessX);
            _isMoving = true;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
        {         
            _camera.ChangeAlphaIncrement(_elapsedTicks * _multiplyer * - _cameraSmoothnessY);
            _isMoving = true;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
        {         
            _camera.ChangeAlphaIncrement(_elapsedTicks * _multiplyer * _cameraSmoothnessY);
            _isMoving = true;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.LBracket))
        {         
            _camera.R += _elapsedTicks * _multiplyer * _cameraSmoothnessR;
            _isMoving = true;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.RBracket))
        {         
            _camera.R -= _elapsedTicks * _multiplyer * _cameraSmoothnessR;
            _isMoving = true;
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.A))
        {
            _light.ChangeBetaIncrement(_elapsedTicks * _multiplyer * - _lightSmoothnessX);
            _isMoving = true;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.D))
        {
            _light.ChangeBetaIncrement(_elapsedTicks * _multiplyer * _lightSmoothnessX);
            _isMoving = true;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.W))
        {
            _light.ChangeAlphaIncrement(_elapsedTicks * _multiplyer * - _lightSmoothnessY);
            _isMoving = true;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.S))
        {
            _light.ChangeAlphaIncrement(_elapsedTicks * _multiplyer * _lightSmoothnessY);
            _isMoving = true;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Q))
        {
            _light.R += _elapsedTicks * _multiplyer * _lightSmoothnessR;
            _isMoving = true;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.E))
        {
            _light.R -= _elapsedTicks * _multiplyer * _lightSmoothnessR;
            _isMoving = true;
        }     
    }

    private void ClearImage(Color clearColor)
    {
        for (uint x = 0; x < _image.Size.X; x++)
        {
            for (uint y = 0; y < _image.Size.Y; y++)
            {
                _image.SetPixel(x, y, clearColor);
            }
        }
    }
}