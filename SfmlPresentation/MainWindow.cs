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

public class MainWindow
{
    private readonly IObjFileParcer objFileParcer;
    private readonly ITransformationHelper transformationHelper;
    private readonly IFastObjDrawer fastObjDrawer;

    public MainWindow(IObjFileParcer objFileParcer,
                      ITransformationHelper transformationHelper,
                      IFastObjDrawer fastObjDrawer)
    {
        this.objFileParcer = objFileParcer;
        this.transformationHelper = transformationHelper;
        this.fastObjDrawer = fastObjDrawer;
    }

    private List<Vector4> vertices;

    private int Scale = 1;

    private Point _startPoint;
    private bool _isDown;

    private Camera camera;
    private Obj obj;

    private RenderWindow app;
    private uint screenWidth;
    private uint screenHeight;

    private Texture pixelTexture;
    private Image image;
    private Sprite pixelSprite;

    void AppConfiguration()
    {
        var desktopMode = VideoMode.DesktopMode;
        app = new RenderWindow(desktopMode, "Renderer", Styles.Default);
        app.Closed += (sender, e) => app.Close();
        screenWidth = desktopMode.Width;
        screenHeight = desktopMode.Height;
    }

    void LoadModel(string path)
    {
        obj = objFileParcer.ParseObjFile(path);
        vertices = transformationHelper.ConvertToGlobalCoordinates(obj, 1, new Vector3(1, 1, 1), 0);
        camera = new Camera(Math.PI / 2, 0, 7);
    }

    void CanvasConfiguration(uint screenWidth, uint screenHeight)
    {
        pixelTexture = new Texture(screenWidth, screenHeight);
        image = new Image(screenWidth, screenHeight);
        pixelSprite = new Sprite(pixelTexture);
    }
    public void Run()
    {
        AppConfiguration();
        LoadModel(@"D:\Projects\7thSem\Graphics\Renderer\Tests\Parser\TestData\Mario.obj");
        CanvasConfiguration(screenWidth, screenHeight);

        Stopwatch stopwatch = new Stopwatch();
        while (app.IsOpen)
        {
            stopwatch.Start();
            app.DispatchEvents();

            HandleKeyboardInput();

            ClearImage(image, new Color(0, 0, 0, 0));
            app.Clear(Color.Black);            
            
            var verticesToDraw = transformationHelper.ConvertTo2DCoordinates(vertices, (int)screenWidth, (int)screenHeight, camera.Eye);
            fastObjDrawer.Draw(obj.FaceList, verticesToDraw, image, Color.Green);
            pixelTexture.Update(image);

            app.Draw(pixelSprite);                        
            app.Display();

            stopwatch.Stop();
            Console.WriteLine($"Frame Time (milliseconds): {stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();            
        }
    }

    void HandleKeyboardInput()
    {        
        float deltaX = 0.05f;
        float deltaY = 0.05f;
        float deltaR = 1f;

        if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
        {            
            camera.ChangeBeta(-deltaX);
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
        {         
            camera.ChangeBeta(deltaX);
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
        {         
            camera.ChangeAlpha(-deltaY);
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
        {         
            camera.ChangeAlpha(deltaY);
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.LBracket))
        {         
            camera.R += deltaR;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.RBracket))
        {         
            camera.R = deltaR;
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