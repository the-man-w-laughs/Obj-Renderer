using Business.Contracts;
using Business.Contracts.Parser;
using Business.Contracts.Transformer.Providers;
using Domain.ObjClass;
using Drawer;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Parser;
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
        private readonly Obj _obj;

        private List<Vector4> vertices;

        private double _alpha = Math.PI / 2;
        private double _beta = 0;

        private double _r = 30;

        public double R
        {
            get { return _r; }
            set
            {
                if (value > 5 && value < 100)
                {
                    _r = value;
                }
            }
        }

        public double Alpha
        {
            get { return _alpha; }
            set
            {
                if (value >= 0 && value <= Math.PI)
                {
                    _alpha = value;
                }
            }
        }

        public double Beta
        {
            get { return _beta; }
            set
            {
                if (value >= 0 && value <= 2 * Math.PI)
                {
                    _beta = value;
                }
            }
        }

        public void ChangeAlpha(double delta)
        {
            _alpha = (_alpha + delta) % (2 * Math.PI);
            if (_alpha < 0)
            {
                _alpha += 2 * Math.PI;
            }
        }
        public void ChangeBeta(double delta)
        {
            _beta = (_beta + delta) % (2 * Math.PI);
            if (_beta < 0)
            {
                _beta += 2 * Math.PI;
            }
        }


        private int Scale = 1;
        private Point _startPoint;
        private bool _isDown;
        private int _count;

        private Vector3 Eye
        {
            get
            {
                double x = R * Math.Sin(Alpha) * Math.Sin(Beta);
                double y = R * Math.Cos(Alpha);
                double z = R * Math.Sin(Alpha) * Math.Cos(Beta);

                return new Vector3((float)x, (float)y, (float)z);
            }
        }


        public MainForm(IObjFileParcer objFileParcer,
                        ITransformationHelper transformationHelper,
                        IFastObjDrawer fastObjDrawer)
        {
            InitializeComponent();
            _objFileParcer = objFileParcer;
            _transformationHelper = transformationHelper;
            _fastObjDrawer = fastObjDrawer;
            _bitMap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            string objFilePath = @"D:\Projects\7thSem\Graphics\Renderer\Tests\Parser\TestData\12140_Skull_v3_L2.obj";

            _obj = objFileParcer.ParseObjFile(objFilePath);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            _isDown = false;
            _count = 0;

            vertices = _transformationHelper.ConvertToGlobalCoordinates(_obj, Scale);
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
            _fastObjDrawer.Draw(_obj.FaceList, vertices, _bitMap, Eye);
            stopwatch.Stop();
            logger.Info(Eye);
            logger.Info(stopwatch.Elapsed.Milliseconds);
            e.Graphics.DrawImage(_bitMap, 0, 0);
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            Alpha = Math.PI / 4;
            Beta = Math.PI / 4;
            pictureBox.Invalidate();
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
                ChangeBeta(-angleDelta);
            }
            else if (e.KeyCode == Keys.Right)
            {                
                ChangeBeta(angleDelta);
            }
            else if (e.KeyCode == Keys.Up)
            {                
                ChangeAlpha(-angleDelta);
            }
            else if (e.KeyCode == Keys.Down)
            {                     
                ChangeAlpha(angleDelta);
            }            
            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
            {
                R -= radiusDelta;
            }
            else if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus)
            {
                R += radiusDelta;
            }
            pictureBox.Invalidate();
        }

    }
}