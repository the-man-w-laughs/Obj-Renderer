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

    public void Run()
    {
        var desktopMode = VideoMode.DesktopMode;

        RenderWindow app = new RenderWindow(desktopMode, "SFML Works!", Styles.Default);

        var screenWidth = desktopMode.Width;
        var screenHeight = desktopMode.Height;

        var obj = objFileParcer.ParseObjFile(@"D:\Projects\7thSem\Graphics\Renderer\Tests\Parser\TestData\Mario.obj");
        var vertices = transformationHelper.ConvertToGlobalCoordinates(obj, 1, new Vector3(1, 1, 1), 0);                

        Texture pixelTexture = new Texture(screenWidth, screenHeight);

        Image image = new Image(screenWidth, screenHeight);
       
        Sprite pixelSprite = new Sprite(pixelTexture);
        app.Closed += (sender, e) => app.Close();

        Stopwatch stopwatch = new Stopwatch();

        camera = new Camera(Math.PI / 2, 0, 7);

        while (app.IsOpen)
        {
            app.DispatchEvents();

            stopwatch.Start();

            ClearImage(image, new Color(0, 0, 0, 0));

            app.Clear();
            camera.ChangeBeta(0.1);
            var verticesToDraw = transformationHelper.ConvertTo2DCoordinates(vertices, (int)screenWidth, (int)screenHeight, camera.Eye);
            fastObjDrawer.Draw(obj.FaceList, verticesToDraw, image);
            pixelTexture.Update(image);
            app.Draw(pixelSprite);

            app.Display();

            stopwatch.Stop();
            Console.WriteLine($"Frame Time (milliseconds): {stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();
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