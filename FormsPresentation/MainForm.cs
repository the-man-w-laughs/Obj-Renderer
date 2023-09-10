using Contracts.Parser;
using Contracts.Transformer;
using Domain.ObjClass;
using Drawer;
using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
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
        private Obj _obj;

        public MainForm(IObjFileParcer objFileParcer,
                        IProjectionMatrixProvider projectionMatrixProvider,
                        ITransformationMatrixProvider transformationProvider,
                        IViewMatrixProvider viewMatrixProvider,
                        IViewportMatrixProvider viewportMatrixProvider)
        {
            Assembly assembly = Assembly.GetEntryAssembly();

            // Get the full assembly name
            string assemblyName = assembly.FullName;
            InitializeComponent();
            this.objFileParcer = objFileParcer;
            this._projectionMatrixProvider = projectionMatrixProvider;
            this._transformationProvider = transformationProvider;
            this._viewMatrixProvider = viewMatrixProvider;
            this._viewportMatrixProvider = viewportMatrixProvider;

            _screenBounds = Screen.PrimaryScreen.Bounds;
            _bitMap = new Bitmap(_screenBounds.Width, _screenBounds.Height);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _obj = objFileParcer.ParseObjFile("D:\\Projects\\7thSem\\Graphics\\Renderer\\Tests\\Parser\\TestData\\Mario.obj");
            
            // Define transformation parameters
            var translation = new Vector3(0, 0, 0);
            var scale = new Vector3(0.1f, 0.1f, 0.1f);
            var rotationAngleDegrees = 0f; // For example, rotate by 45 degrees around X-axis

            // Create transformation matrices
            var translationMatrix = _transformationProvider.CreateTranslationMatrix(translation.X, translation.Y, translation.Z);
            var scaleMatrix = _transformationProvider.CreateScaleMatrix(scale.X, scale.Y, scale.Z);
            var rotationAxis = new Vector3(1.0f, 0.0f, 0.0f); // X-axis rotation
            var rotationMatrix = _transformationProvider.CreateRotationMatrix(rotationAxis, rotationAngleDegrees);

            // Create the model matrix by multiplying the individual transformations
            var modelMatrix = translationMatrix * rotationMatrix * scaleMatrix;

            var eye = new Vector3(0.0f, 0.0f, -10f);
            var target = new Vector3((_obj.Size.XMax + _obj.Size.XMin) / 2,
                                     (_obj.Size.YMax + _obj.Size.YMin) / 2,
                                     (_obj.Size.ZMax + _obj.Size.ZMin) / 2);
            var up = new Vector3(0.0f, 1.0f, 0.0f);
            
            // Create the view matrix
            var viewMatrix = _viewMatrixProvider.WorldToViewMatrix(eye, target, up);

            // Create the projection matrix
            var projectionMatrix = _projectionMatrixProvider.CreatePerspectiveProjectionMatrix(30.0f, 16.0f / 9.0f, 0.1f, 100.0f);

            // Create the viewport matrix
            var viewportMatrix = _viewportMatrixProvider.CreateProjectionToViewportMatrix(_screenBounds.Width, _screenBounds.Height, _obj.Size.XMin, _obj.Size.YMin);

            // Combine all transformations by multiplying matrices in the correct order
            var finalMatrix = viewportMatrix * projectionMatrix * viewMatrix * modelMatrix;

            // Apply the final transformation to obj.VertexList
            for (int i = 0; i < _obj.VertexList.Count; i++)
            {
                // Apply the final transformation
                _obj.VertexList[i] = Vector3.Transform(_obj.VertexList[i], finalMatrix);
            }

            pictureBox.Invalidate();
        }



        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics graphics = Graphics.FromImage(_bitMap))
            {
                graphics.Clear(Color.White); // Clear the bitmap before drawing

                foreach (var face in _obj.FaceList)
                {
                    for (int i = 0; i < face.VertexIndexList.Count() - 1; i++)
                    {
                        Vector3 startPoint = _obj.VertexList[face.VertexIndexList[i] - 1];
                        Vector3 endPoint = _obj.VertexList[face.VertexIndexList[i + 1] - 1];

                        // Adjust the coordinates and scale based on your requirements
                        int startX = Math.Abs((int)startPoint.X);
                        int startY = Math.Abs((int)startPoint.Y);
                        int endX = Math.Abs((int)endPoint.X);
                        int endY = Math.Abs((int)endPoint.Y);

                        // Call the function to draw if it intersects with _screenBounds
                        DrawLineIfIntersects(graphics, startX, startY, endX, endY);
                    }

                    // To connect the last vertex to the first vertex, if needed
                    Vector3 lastPoint = _obj.VertexList[face.VertexIndexList.Last() - 1];
                    Vector3 firstPoint = _obj.VertexList[face.VertexIndexList.First() - 1];

                    // Adjust the coordinates and scale based on your requirements
                    int lastX = Math.Abs((int)lastPoint.X);
                    int lastY = Math.Abs((int)lastPoint.Y);
                    int firstX = Math.Abs((int)firstPoint.X);
                    int firstY = Math.Abs((int)firstPoint.Y);

                    // Call the function to draw if it intersects with _screenBounds
                    DrawLineIfIntersects(graphics, lastX, lastY, firstX, firstY);
                }
            }
            // Draw the bitmap onto the pictureBox
            e.Graphics.DrawImage(_bitMap, 0, 0);
        }


        private void DrawLineIfIntersects(Graphics graphics, int startX, int startY, int endX, int endY)
        {
            if (_screenBounds.Contains(startX, startY) || _screenBounds.Contains(endX, endY))
            {
                graphics.DrawLine(Pens.Black, startX, startY, endX, endY);
            }
        }

    }
}