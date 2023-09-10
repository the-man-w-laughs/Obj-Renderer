using Contracts.Transformer;
using System;
using System.Numerics;
using Xunit;

namespace Tests.Transformer;
public class ViewMatrixProviderTests
{
    private readonly IViewMatrixProvider _matrixProvider;

    public ViewMatrixProviderTests(IViewMatrixProvider matrixProvider)
    {
        _matrixProvider = matrixProvider;
    }

    [Theory]
    [InlineData(1.0f, 2.0f, 3.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f)] // Test case 1
    [InlineData(2.0f, 3.0f, 4.0f, 1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f)] // Test case 2
    // Add more test cases as needed
    public void WorldToViewMatrix_CreatesCorrectViewMatrix(
        float eyeX, float eyeY, float eyeZ,
        float targetX, float targetY, float targetZ,
        float upX, float upY, float upZ)
    {
        // Arrange
        var eye = new Vector3(eyeX, eyeY, eyeZ);
        var target = new Vector3(targetX, targetY, targetZ);
        var up = new Vector3(upX, upY, upZ);

        // Calculate the expected view matrix manually (you should use your method here)
        var expectedViewMatrix = Matrix4x4.CreateLookAt(eye, target, up);

        // Act: Call your WorldToViewMatrix method to get the actual view matrix
        var actualViewMatrix = _matrixProvider.WorldToViewMatrix(eye, target, up); // Replace YourClass with your actual class name

        // Assert: Compare the expected and actual matrices
        Assert.Equal(expectedViewMatrix, actualViewMatrix);
    }
}
