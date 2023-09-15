using Business.Contracts.Parser;
using Business.Contracts;
using Microsoft.Extensions.Logging;
using SFML.Graphics;
using SFML.Window;
using SfmlPresentation;
using SfmlPresentation.Contracts;
using System.Drawing;
using System.Numerics;
using Image = SFML.Graphics.Image;
using Color = SFML.Graphics.Color;
using Business;
using Domain.ObjClass;
using System.Diagnostics;
using SfmlPresentation.Scene;
using SFML.System;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

public class MainWindow
{
    private readonly IObjFileParcer _objFileParcer;
    private readonly ITransformationHelper _transformationHelper;
    private readonly IFastObjDrawer _fastObjDrawer;

    public MainWindow(IObjFileParcer objFileParcer,
                      ITransformationHelper transformationHelper,
                      IFastObjDrawer fastObjDrawer)
    {
        this._objFileParcer = objFileParcer;
        this._transformationHelper = transformationHelper;
        this._fastObjDrawer = fastObjDrawer;
    }

    private List<Vector4> _vertices;

    private int _scale = 1;

    private Point _startPoint;
    private double _alpha;
    private double _beta;
    private bool _isDown;
    private float _smoothness = 200;
    private float _rSmoothness = 0.7f;

    private Camera _camera;
    private Obj _obj;

    private RenderWindow _app;
    private uint _screenWidth;
    private uint _screenHeight;

    private Texture _pixelTexture;
    private Image _image;
    private Sprite _pixelSprite;
    private List<Vector4> _oldVertices;


    void AppConfiguration()
    {
        var desktopMode = VideoMode.DesktopMode;
        _app = new RenderWindow(desktopMode, "Renderer", Styles.Default);
        _app.Closed += (sender, e) => _app.Close();
        _app.MouseButtonPressed += App_MouseButtonPressed;
        _app.MouseMoved += _app_MouseMoved;        
        _app.MouseButtonReleased += _app_MouseButtonReleased;
        _app.MouseWheelMoved += _app_MouseWheelMoved;
        _isDown = false;

        _screenWidth = desktopMode.Width;
        _screenHeight = desktopMode.Height;
    }

    private void _app_MouseWheelMoved(object? sender, MouseWheelEventArgs e)
    {
        _camera.R -= e.Delta * _rSmoothness;
    }

    private void _app_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
    {
        _isDown = false;        
    }

    private void _app_MouseMoved(object? sender, MouseMoveEventArgs e)
    {
        if (!_isDown) return;
        int deltaX = e.X - _startPoint.X, deltaY = e.Y - _startPoint.Y;
        _camera.Alpha = _alpha - deltaY / _smoothness;
        _camera.ChangeBetaAssign(_beta - deltaX / _smoothness);        
    }

    private void App_MouseButtonPressed(object? sender, MouseButtonEventArgs e)
    {
        _isDown = true;
        _startPoint = new Point(e.X, e.Y);
        _alpha = _camera.Alpha;
        _beta = _camera.Beta;
    }

    void LoadScene(string path)
    {
        _obj = _objFileParcer.ParseObjFile(path);
        _vertices = _transformationHelper.ConvertToGlobalCoordinates(_obj, 10, new Vector3(1, 1, 1), 0);
        _oldVertices = new List<Vector4>();
        _camera = new Camera(Math.PI / 2, 0, 7);
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

        Stopwatch stopwatch = new Stopwatch();
        while (_app.IsOpen)
        {
            stopwatch.Start();
            _app.DispatchEvents();
            //ClearImage(_image, Color.Black);
            HandleKeyboardInput();            
            if (_oldVertices.Count != 0)
            {
                _fastObjDrawer.Draw(_obj.FaceList, _oldVertices, _image, Color.Black);
            }
            
            var verticesToDraw = _transformationHelper.ConvertTo2DCoordinates(_vertices, (int)_screenWidth, (int)_screenHeight, _camera.Eye);
            _oldVertices = verticesToDraw.ToList();
            DrawImage(_obj.FaceList, verticesToDraw, _image, Color.White);

            stopwatch.Stop();
            Console.WriteLine($"Frame Time (milliseconds): {stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();
        }
    }

    void DrawImage(List<Face> faces, List<Vector4> vertices, Image image, Color color)
    {
        _fastObjDrawer.Draw(_obj.FaceList, vertices, image, color);
        _pixelTexture.Update(image);
        _app.Draw(_pixelSprite);
        _app.Display();
    }

    void HandleKeyboardInput()
    {        
        float deltaX = 0.05f;
        float deltaY = 0.05f;
        float deltaR = 1f;

        if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
        {            
            _camera.ChangeBetaIncrement(-deltaX);
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
        {         
            _camera.ChangeBetaIncrement(deltaX);
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
        {         
            _camera.ChangeAlphaIncrement(-deltaY);
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
        {         
            _camera.ChangeAlphaIncrement(deltaY);
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.LBracket))
        {         
            _camera.R += deltaR;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.RBracket))
        {         
            _camera.R -= deltaR;
        }
    }

    private void ClearImage(Image image, Color clearColor)
    {
        for (uint x = 0; x < image.Size.X; x++)
        {
            for (uint y = 0; y < image.Size.Y; y++)
            {
                image.SetPixel(x, y, clearColor);
            }
        }
    }
}