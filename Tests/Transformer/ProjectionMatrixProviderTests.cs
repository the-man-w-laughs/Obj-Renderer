using System;
using System.Numerics;
using Contracts.Transformer;
using Xunit;

namespace Tests.Transformer
{
    public class ProjectionMatrixProviderTests
    {
        private readonly IProjectionMatrixProvider _projectionMatrixProvider;

        public ProjectionMatrixProviderTests(IProjectionMatrixProvider projectionMatrixProvider)
        {
            _projectionMatrixProvider = projectionMatrixProvider;
        }

        [Theory]
        [InlineData(60.0f, 16.0f / 9.0f, 0.1f, 100.0f)]
        public void CreatePerspectiveProjectionMatrix_MatchesMatrix4x4Method(float fieldOfView, float aspectRatio, float zNear, float zFar)
        {
            // Arrange - create the matrices with mock data
            var projectionMatrixToTest = _projectionMatrixProvider.CreatePerspectiveProjectionMatrix(fieldOfView, aspectRatio, zNear, zFar);

            var expectedProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(
                (float)Math.PI * fieldOfView / 180.0f,
                aspectRatio,
                zNear,
                zFar
            );

            Assert.Equal(expectedProjectionMatrix, projectionMatrixToTest);
        }
    }
}
