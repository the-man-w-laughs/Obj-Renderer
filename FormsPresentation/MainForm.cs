using Contracts.Parser;
using Contracts.Transformer;
using Drawer;
using Microsoft.Extensions.DependencyInjection;
using System.Numerics;
using System.Reflection;

namespace Renderer
{
    public partial class MainForm : Form
    {
        private readonly IObjFileParcer objFileParcer;
        private readonly IProjectionMatrixProvider _projectionMatrixProvider;
        private readonly ITransformationProvider _transformationProvider;
        private readonly IViewMatrixProvider _viewMatrixProvider;
        private readonly IViewportMatrixProvider _viewportMatrixProvider;
        private readonly Rectangle _screenBounds;
        private Bitmap _bitMap;        

        public MainForm(IObjFileParcer objFileParcer,
                        IProjectionMatrixProvider projectionMatrixProvider,
                        ITransformationProvider transformationProvider,
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
            var obj = objFileParcer.ParseObjFile("D:\\Projects\\7thSem\\Graphics\\Renderer\\Tests\\Parser\\TestData\\Mario.obj");

            // Define transformation parameters
            var translation = new Vector3(0, 0, 0);
            var scale = new Vector3(1, 1, 1);
            var rotationAngleDegrees = 45.0f; // For example, rotate by 45 degrees around X-axis

            // Create transformation matrices
            var translationMatrix = _transformationProvider.CreateTranslationMatrix(translation.X, translation.Y, translation.Z);
            var scaleMatrix = _transformationProvider.CreateScaleMatrix(scale.X, scale.Y, scale.Z);
            var rotationAxis = new Vector3(1.0f, 0.0f, 0.0f); // X-axis rotation
            var rotationMatrix = _transformationProvider.CreateRotationMatrix(rotationAxis, rotationAngleDegrees);

            // Create the model matrix by multiplying the individual transformations
            var modelMatrix = translationMatrix * rotationMatrix * scaleMatrix;

            var eye = new Vector3(0.0f, 0.0f, 5.0f);
            var target = new Vector3(0.0f, 0.0f, 0.0f);
            var up = new Vector3(0.0f, 1.0f, 0.0f);

            // Create the view matrix
            var viewMatrix = _viewMatrixProvider.WorldToViewMatrix(eye, target, up);

            // Create the projection matrix
            var projectionMatrix = _projectionMatrixProvider.CreatePerspectiveProjectionMatrix(60.0f, 16.0f / 9.0f, 0.1f, 100.0f);

            // Create the viewport matrix
            var viewportMatrix = _viewportMatrixProvider.CreateProjectionToViewportMatrix(_screenBounds.Width, _screenBounds.Height);

            // Combine all transformations by multiplying matrices in the correct order
            var finalMatrix = viewportMatrix * projectionMatrix * viewMatrix * modelMatrix;

            // Apply the final transformation to obj.VertexList
            obj.VertexList.ForEach(vertex =>
            {
                // Apply the final transformation
                vertex = Vector3.Transform(vertex, finalMatrix);
            });

            pictureBox.Invalidate();
        }



        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            _bitMap.DrawLine(Color.Black, 0, 0, 100, 100);
            e.Graphics.DrawImage(_bitMap, 0, 0);
        }
    }
}