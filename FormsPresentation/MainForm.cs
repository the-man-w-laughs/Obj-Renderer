using Business.Contracts;
using Business.Contracts.Parser;
using Business.Contracts.Transformer.Providers;
using Domain.ObjClass;
using Drawer;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Specialized;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using Transformer.Transpormers;
using static System.Windows.Forms.AxHost;

namespace Renderer
{
    public partial class MainForm : Form
    {
        private readonly IObjFileParcer _objFileParcer;
        private readonly ITransformationHelper _transformationHelper;
        private readonly IFastObjDrawer _fastObjDrawer;        
        private readonly Bitmap _bitMap;
        private readonly Obj _obj;

        private List<Vector4> vertices;

        private double R = 15;
        private double Alpha = 0;
        private double Beta = 0;  
        private int Scale = 1;        

        private Vector3 Eye
        {
            get
            {
                double x = R * Math.Sin(Alpha) * Math.Cos(Beta);
                double y = R * Math.Sin(Alpha) * Math.Sin(Beta);
                double z = R * Math.Cos(Alpha);

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

            string objFilePath = @"D:\Projects\7thSem\Graphics\Renderer\Tests\Parser\TestData\Character_female_1.obj";
            _obj = objFileParcer.ParseObjFile(objFilePath);            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            vertices = _transformationHelper.ConvertToGlobalCoordinates(_obj, Scale);
            timer1.Interval = 100;
            timer1.Start();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics g = Graphics.FromImage(_bitMap))
            {
                g.Clear(Color.White); // Change the background color to match your needs
            }

            _fastObjDrawer.Draw(_obj.FaceList, vertices, _bitMap, Eye);

            e.Graphics.DrawImage(_bitMap, 0, 0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Beta = 0.2;
            Alpha += 0.1;
            pictureBox.Invalidate();
        }
    }
}