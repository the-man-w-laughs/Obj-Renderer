using Contracts.Parser;
using Contracts.Transformer;
using Domain.ObjClass;
using Drawer;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IObjFileParcer objFileParcer;
        private readonly IProjectionMatrixProvider _projectionMatrixProvider;
        private readonly ITransformationMatrixProvider _transformationProvider;
        private readonly IViewMatrixProvider _viewMatrixProvider;
        private readonly IViewportMatrixProvider _viewportMatrixProvider;
        private readonly Rectangle _screenBounds;
        private readonly Bitmap _bitMap;
        private readonly Obj _obj;

        private List<Vector4> vertices;

        private double R = 10;
        private double Alpha = 0;// Math.PI / 2;
        private double Beta = 0;// Math.PI / 2;
        private int Scale = 1;

        private Vector3 eye =>
            new(
                (float)(R * Math.Sin(Alpha) * Math.Cos(Beta)),
                (float)(R * Math.Sin(Alpha) * Math.Sin(Beta)),
                (float)(R * Math.Cos(Beta)));

        public MainForm(IObjFileParcer objFileParcer,
                        IProjectionMatrixProvider projectionMatrixProvider,
                        ITransformationMatrixProvider transformationProvider,
                        IViewMatrixProvider viewMatrixProvider,
                        IViewportMatrixProvider viewportMatrixProvider)
        {                                   
            InitializeComponent();
            this.objFileParcer = objFileParcer;
            this._projectionMatrixProvider = projectionMatrixProvider;
            this._transformationProvider = transformationProvider;
            this._viewMatrixProvider = viewMatrixProvider;
            this._viewportMatrixProvider = viewportMatrixProvider;

            _screenBounds = Screen.PrimaryScreen.Bounds;
            _bitMap = new Bitmap(_screenBounds.Width, _screenBounds.Height);

            string objFilePath = "D:\\Projects\\7thSem\\Graphics\\Renderer\\Tests\\Parser\\TestData\\Mario.obj";
            _obj = objFileParcer.ParseObjFile(objFilePath);

            // Define transformation parameters            
            var scale = Scale * new Vector3(1, 1, 1);
            var rotationAxis = new Vector3(1.0f, 0.0f, 0.0f);
            var rotationAngleDegrees = 0;
            var size = _obj.Size;
            var translation = new Vector3((size.XMax + size.XMin) / 2, (size.YMax + size.YMin) / 2, (size.ZMax + size.ZMin) / 2);

            // Compute transformation matrices
            var translationMatrix = _transformationProvider.CreateTranslationMatrix(-translation.X, -translation.Y, -translation.Z);            
            var scaleMatrix = _transformationProvider.CreateScaleMatrix(scale.X, scale.Y, scale.Z);
            var rotationMatrix = _transformationProvider.CreateRotationMatrix(rotationAxis, rotationAngleDegrees);
            var modelMatrix = translationMatrix * rotationMatrix * scaleMatrix;
            vertices = _obj.VertexList.ToList().ApplyTransformAndCopy(modelMatrix);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;           

            pictureBox.Invalidate();
        }
        
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            // Define view and projection parameters            
            var target = new Vector3(0, 0, 0);
            var up = Math.Cos(Beta) >= 0 ? Vector3.UnitY : -Vector3.UnitY;
            var viewMatrix = _viewMatrixProvider.WorldToViewMatrix(eye, target, up);
            var projectionMatrix = _projectionMatrixProvider.CreatePerspectiveProjectionMatrix(90.0f, _screenBounds.Width / _screenBounds.Height, 1.0f, 100.0f);

            // Precompute the viewport matrix if it doesn't change
            var viewportMatrix = _viewportMatrixProvider.CreateProjectionToViewportMatrix(_screenBounds.Width, _screenBounds.Height, 0, 0);

            // Combine matrices
            var finalMatrix = viewMatrix * projectionMatrix * viewportMatrix;

            vertices.ApplyTransformAndDivideByW(finalMatrix);

            foreach (var face in _obj.FaceList)
            {
                for (int i = 0; i < face.VertexIndexList.Count() - 1; i++)
                {
                    var startPoint = vertices[face.VertexIndexList[i] - 1];
                    var endPoint = vertices[face.VertexIndexList[i + 1] - 1];

                    // Adjust the coordinates and scale based on your requirements
                    int startX = (int)startPoint.X;
                    int startY = (int)startPoint.Y;
                    int endX = (int)endPoint.X;
                    int endY = (int)endPoint.Y;

                    // Call the function to draw if it intersects with _screenBounds
                    DrawLineIfIntersects(_bitMap, startX, startY, endX, endY);
                }

                // To connect the last vertex to the first vertex, if needed
                var lastPoint = vertices[face.VertexIndexList.Last() - 1];
                var firstPoint = vertices[face.VertexIndexList.First() - 1];

                // Adjust the coordinates and scale based on your requirements
                int lastX = (int)lastPoint.X;
                int lastY = (int)lastPoint.Y;
                int firstX = (int)firstPoint.X;
                int firstY = (int)firstPoint.Y;

                // Call the function to draw if it intersects with _screenBounds
                DrawLineIfIntersects(_bitMap, lastX, lastY, firstX, firstY);
            }            
            // Draw the bitmap onto the pictureBox
            
            e.Graphics.DrawImage(_bitMap, 0, 0);
        }


        private void DrawLineIfIntersects(Bitmap bitmap, int startX, int startY, int endX, int endY)
        {
            if (_screenBounds.Contains(startX, startY) && _screenBounds.Contains(endX, endY))
            {
                bitmap.DrawLine(Color.Black, startX, startY, endX, endY);
            }
        }

    }
}