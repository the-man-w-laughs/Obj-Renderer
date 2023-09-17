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

public partial class MainWindow
{
    private readonly IObjFileParcer _objFileParcer;
    private readonly ITransformationHelper _transformationHelper;
    private readonly IPolygonObjDrawer _polygonObjDrawer;
    private readonly IRasterizationObjDrawer _rasterizationObjDrawer;

    public MainWindow(IObjFileParcer objFileParcer,
                      ITransformationHelper transformationHelper,
                      IPolygonObjDrawer fastObjDrawer,
                      IRasterizationObjDrawer rasterizationObjDrawer)
    {
        this._objFileParcer = objFileParcer;
        this._transformationHelper = transformationHelper;
        this._polygonObjDrawer = fastObjDrawer;
        _rasterizationObjDrawer = rasterizationObjDrawer;
    }

    private List<Vector3> _vertices;
    private List<Vector3> _oldVertices;
    private Texture _pixelTexture;
    private Image _image;
    private Sprite _pixelSprite;

    private int _scale = 1;

    private Point _startPoint;
    private double _alpha;
    private double _beta;
    private bool _isDown;
    private float _smoothness = 200;
    private float _rSmoothness = 0.7f;

    private Camera _camera = new Camera(Math.PI / 2, 0, 100);
    private Camera _light = new Camera(Math.PI / 2, 0, 100);
    private Obj _obj;

    private RenderWindow _app;
    private uint _screenWidth;
    private uint _screenHeight;



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

    void LoadScene(string path)
    {
        _obj = _objFileParcer.ParseObjFile(path);
        _vertices = _transformationHelper.ConvertToGlobalCoordinates(_obj, 10, new Vector3(1, 1, 1), 0);
        _oldVertices = new List<Vector3>();        
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
        LoadScene(@"D:\Projects\7thSem\Graphics\Renderer\Tests\Parser\TestData\cube.obj");
        CanvasConfiguration(_screenWidth, _screenHeight);

        Stopwatch stopwatch = new Stopwatch();
        while (_app.IsOpen)
        {
            stopwatch.Start();
            _app.DispatchEvents();
            HandleKeyboardInput();

            ClearImage(_image, Color.Black);
            //if (_oldVertices.Count != 0)
            //{
            //    _rasterizationObjDrawer.Draw(_obj.FaceList, _oldVertices, _image);
            //}
            
            var verticesToDraw = _transformationHelper.ConvertTo2DCoordinates(_vertices, (int)_screenWidth, (int)_screenHeight, _camera.Eye);
            var lightToDraw = _transformationHelper.ConvertTo2DCoordinates(_light.Eye, (int)_screenWidth, (int)_screenHeight, _camera.Eye);
            _oldVertices = verticesToDraw.ToList();
            DrawImage(verticesToDraw, lightToDraw);

            stopwatch.Stop();
            var elapsed = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"FPS: {(elapsed != 0 ? (1000.0f / elapsed).ToString() : "inf")} ({elapsed} ms/frame);");
            stopwatch.Reset();
        }
    }

    void DrawImage(List<Vector3> vertices, Vector3 light)
    {
        _rasterizationObjDrawer.Draw(_obj.FaceList, vertices, _image, light);
        _pixelTexture.Update(_image);
        _app.Draw(_pixelSprite);
        _app.Display();
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