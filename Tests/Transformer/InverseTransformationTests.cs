using System;
using System.Numerics;
using Business.Contracts.Transformer.Providers;
using Xunit;

namespace Tests.Transformer
{
    public class InverseTransformationTests
    {
        private readonly IProjectionMatrixProvider _projectionMatrixProvider;        
        private readonly IViewMatrixProvider _viewMatrixProvider;
        private readonly IViewportMatrixProvider _viewportMatrixProvider;        

        public InverseTransformationTests(IProjectionMatrixProvider projectionMatrixProvider,
                                      IViewMatrixProvider viewMatrixProvider,
                                      IViewportMatrixProvider viewportMatrixProvider)
        {
            _projectionMatrixProvider = projectionMatrixProvider;            
            this._viewMatrixProvider = viewMatrixProvider;
            this._viewportMatrixProvider = viewportMatrixProvider;
        }

        [Fact]
        public void CreatePerspectiveProjectionMatrix_MatchesMatrix4x4Method()
        {                        
            uint width = 2341;
            uint height = 423;
            var zNear = 10;
            var zFar = 100;
            var projectionMatrix = _projectionMatrixProvider.CreatePerspectiveProjectionMatrix(45.0f, (float)width / height, zNear, zFar);
            var viewportMatrix = _viewportMatrixProvider.CreateProjectionToViewportMatrix(width, height, 0, 0);               
            
            var eye = new Vector3(10, -19, 10);
            var target = new Vector3(0, 0, 0);
            var up = new Vector3(0, 11, 0);                
            var viewMatrix = _viewMatrixProvider.WorldToViewMatrix(eye, target, up);                
            
            var finalMatrix = viewMatrix * projectionMatrix * viewportMatrix;                                
            Vector4 vector4 = new Vector4(10, -12, 33, 24);                
            var resultVector = Vector4.Transform(vector4, finalMatrix);
            Matrix4x4 inverseMatrix;
            Matrix4x4.Invert(finalMatrix, out inverseMatrix);
            var originalVector = Vector4.Transform(resultVector, inverseMatrix);
            Assert.Equal(vector4, originalVector, new Vector4EqualityComparer(10e-4f));                        
        }
    }
}
