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
        // Add more test cases as needed
        public void TransformVertexWithProjectionMatrix_ReturnsCorrectResult(float fieldOfView, float aspectRatio, float zNear, float zFar)
        {
            // Arrange
            var vertex = new Vector3(1.0f, 2.0f, 3.0f);
            var projectionMatrix = _projectionMatrixProvider.CreatePerspectiveProjectionMatrix(fieldOfView, aspectRatio, zNear, zFar);

            // Act
            var transformedVertex = Vector3.Transform(vertex, projectionMatrix);

            // Assert
            var expectedX = vertex.X / vertex.Z;
            var expectedY = vertex.Y / vertex.Z;
            var expectedZ = 1.0f;

            Assert.Equal(expectedX, transformedVertex.X, precision: 6);
            Assert.Equal(expectedY, transformedVertex.Y, precision: 6);
            Assert.Equal(expectedZ, transformedVertex.Z, precision: 6);
        }
    }
}
