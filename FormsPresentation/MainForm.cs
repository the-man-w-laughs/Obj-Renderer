using Business.Contracts;
using Business.Contracts.Parser;
using Business.Contracts.Transformer.Providers;
using Domain.ObjClass;
using FormsPresentation.Contracts;
using FormsPresentation.Utils;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Parser;
using SfmlPresentation.Scene;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using Transformer.Transpormers;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.AxHost;

namespace Renderer
{
    public partial class MainForm : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IObjFileParcer _objFileParcer;
        private readonly ITransformationHelper _transformationHelper;
        private readonly IFastObjDrawer _fastObjDrawer;
        private readonly Bitmap _bitMap;
        private readonly Camera camera;
        private readonly Obj _obj;

        private List<Vector4> vertices;


        private int Scale = 1;
        private Point _startPoint;
        private bool _isDown;
        private int _count;

        public MainForm(IObjFileParcer objFileParcer,
                        ITransformationHelper transformationHelper,
                        IFastObjDrawer fastObjDrawer)
        {
            InitializeComponent();
            _objFileParcer = objFileParcer;
            _transformationHelper = transformationHelper;
            _fastObjDrawer = fastObjDrawer;
            _bitMap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            camera = new Camera(Math.PI / 2, 0, 30);

            string objFilePath = @"D:\Projects\7thSem\Graphics\Renderer\Tests\Parser\TestData\12140_Skull_v3_L2.obj";

            _obj = objFileParcer.ParseObjFile(objFilePath);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            _isDown = false;
            _count = 0;

            vertices = _transformationHelper.ConvertToGlobalCoordinates(_obj, Scale, new Vector3(1,1,1), 0);
            //timer1.Interval = 100;
            //timer1.Start();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics g = Graphics.FromImage(_bitMap))
            {
                g.Clear(Color.White); // Change the background color to match your needs
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var verticesToDraw = _transformationHelper.ConvertTo2DCoordinates(vertices, _bitMap.Width, _bitMap.Height, camera.Eye);

            _fastObjDrawer.Draw(_obj.FaceList, verticesToDraw, _bitMap);
            stopwatch.Stop();            
            logger.Info(stopwatch.Elapsed.Milliseconds);
            e.Graphics.DrawImage(_bitMap, 0, 0);
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            //Alpha = Math.PI / 4;
            //Beta = Math.PI / 4;
            //pictureBox.Invalidate();
        }
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            //_isDown = true;
            //_startPoint = new Point(e.X, e.Y);
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            //if (!_isDown) return;
            //logger.Info($"count {_count};");
            //_count += 1;
            //if (_count % 5 != 0) return;
            //_count = 0;
            //int deltaX = e.X - _startPoint.X, deltaY = e.Y - _startPoint.Y;
            //logger.Info($"deltaX = {deltaX}; deltaY = {deltaY};");
            //Alpha += deltaY / 100;
            //Beta += deltaX / 100;
            //pictureBox.Invalidate();
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            //_isDown = false;
            //_count = 0;
        }

        private void pictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            //R += e.Delta / 10;
            //logger.Info($"R = {R};");
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            var angleDelta = 0.2;
            var radiusDelta = 5;
            if (e.KeyCode == Keys.Left)
            {
                camera.ChangeBeta(-angleDelta);
            }
            else if (e.KeyCode == Keys.Right)
            {                
                camera.ChangeBeta(angleDelta);
            }
            else if (e.KeyCode == Keys.Up)
            {
                camera.ChangeAlpha(-angleDelta);
            }
            else if (e.KeyCode == Keys.Down)
            {                     
                camera.ChangeAlpha(angleDelta);
            }            
            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
            {
                camera.R -= radiusDelta;
            }
            else if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus)
            {
                camera.R += radiusDelta;
            }
            pictureBox.Invalidate();
        }

    }
}