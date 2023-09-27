using System;
using System.Numerics;
using Business.Contracts.Transformer.Providers;
using Xunit;

namespace Tests.Transformer
{
    public class AllTransformationTests
    {
        private readonly IProjectionMatrixProvider _projectionMatrixProvider;
        private readonly ITransformationMatrixProvider _transformationMatrixProvider;
        private readonly IViewMatrixProvider _viewMatrixProvider;
        private readonly IViewportMatrixProvider _viewportMatrixProvider;
        private string _filePath = "C:\\Users\\nazar\\OneDrive\\Рабочий стол\\kamen.txt";

        public AllTransformationTests(IProjectionMatrixProvider projectionMatrixProvider,
                                      ITransformationMatrixProvider transformationMatrixProvider,
                                      IViewMatrixProvider viewMatrixProvider,
                                      IViewportMatrixProvider viewportMatrixProvider)
        {
            _projectionMatrixProvider = projectionMatrixProvider;
            this._transformationMatrixProvider = transformationMatrixProvider;
            this._viewMatrixProvider = viewMatrixProvider;
            this._viewportMatrixProvider = viewportMatrixProvider;
        }

        [Fact]
        public void CreatePerspectiveProjectionMatrix_MatchesMatrix4x4Method()
        {

            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                uint width = 2341;
                uint height = 423;
                var zNear = 10;
                var zFar = 100;

                writer.WriteLine("CreatePerspectiveProjectionMatrix");
                writer.WriteLine($"angle = {45.0f}; width = {width}; height = {height}; zNear = {zNear}; zFar = {zFar};");
                var projectionMatrix = _projectionMatrixProvider.CreatePerspectiveProjectionMatrix(45.0f, (float)width / height, zNear, zFar);
                writeMatrix(writer, projectionMatrix);

                writer.WriteLine("CreatePerspectiveProjectionMatrix");
                writer.WriteLine($"width = {width}; height = {height}; xMIn = {0}; yMin = {0};");
                var viewportMatrix = _viewportMatrixProvider.CreateProjectionToViewportMatrix(width, height, 0, 0);
                writeMatrix(writer, viewportMatrix);


                var eye = new Vector3(10, -19, 10);
                var target = new Vector3(0, 0, 0);
                var up = new Vector3(0, 11, 0);

                writer.WriteLine("WorldToViewMatrix");
                writer.WriteLine($"eye = {eye}; target = {target}; up = {up};");
                var viewMatrix = _viewMatrixProvider.WorldToViewMatrix(eye, target, up);
                writeMatrix(writer, viewMatrix);

                writer.WriteLine("finalMatrix = viewMatrix * projectionMatrix * viewportMatrix");
                var finalMatrix = viewMatrix * projectionMatrix * viewportMatrix;
                writeMatrix(writer, finalMatrix);

                writer.WriteLine("vector4 * finalMatrix");
                Vector4 vector4 = new Vector4(10, -12, 33, 24);
                writer.WriteLine($"vector4 = {vector4}");
                var resultVector = Vector4.Transform(vector4, finalMatrix);
                writer.WriteLine($"resultVector = {resultVector}");
            }

            Assert.True(true);
        }

        private void writeVector(StreamWriter streamWriter, Vector4 vector)
        {
            streamWriter.WriteLine($"Vector: {vector}\n");
        }

        private void writeMatrix(StreamWriter streamWriter, Matrix4x4 matrix)
        {
            streamWriter.WriteLine($"Matrix: \n{matrix}\n");
        }
    }
}
