using Contracts.Transformer;
using System;
using System.Numerics;
using Xunit;

namespace Tests.Transformer;
public class TransformationMatrixProviderTests
{
    private readonly ITransformationMatrixProvider _matrixProvider;

    public TransformationMatrixProviderTests(ITransformationMatrixProvider matrixProvider)
    {
        _matrixProvider = matrixProvider;
    }

    [Theory]
    [InlineData(2.0f, 2.0f, 2.0f)]
    [InlineData(1.0f, 1.0f, 1.0f)]
    [InlineData(0.5f, 0.5f, 0.5f)]
    public void CreateScaleMatrix_MatchesMatrix4x4(float scaleX, float scaleY, float scaleZ)
    {
        // Arrange - Create a scale matrix using ITransformationMatrixProvider
        var scaleMatrixToTest = _matrixProvider.CreateScaleMatrix(scaleX, scaleY, scaleZ);

        // Arrange - Create a scale matrix using Matrix4x4
        var expectedScaleMatrix = Matrix4x4.CreateScale(scaleX, scaleY, scaleZ);

        // Assert - Compare the two matrices
        Assert.Equal(expectedScaleMatrix, scaleMatrixToTest);
    }

    [Theory]
    [InlineData(2.0f, 2.0f, 2.0f)]
    [InlineData(1.0f, 1.0f, 1.0f)]
    [InlineData(0.5f, 0.5f, 0.5f)]
    public void CreateTranslationMatrix_MatchesMatrix4x4(float translateX, float translateY, float translateZ)
    {
        // Arrange - Create a translation matrix using ITransformationMatrixProvider
        var translationMatrixToTest = _matrixProvider.CreateTranslationMatrix(translateX, translateY, translateZ);

        // Arrange - Create a translation matrix using Matrix4x4
        var expectedTranslationMatrix = Matrix4x4.CreateTranslation(translateX, translateY, translateZ);

        // Assert - Compare the two matrices
        Assert.Equal(expectedTranslationMatrix, translationMatrixToTest);
    }

    [Theory]
    [InlineData(1.0f, 0.0f, 0.0f, 45.0f)]
    [InlineData(0.0f, 1.0f, 0.0f, 90.0f)]
    [InlineData(0.0f, 0.0f, 1.0f, 180.0f)]
    public void CreateRotationMatrix_MatchesMatrix4x4(float axisX, float axisY, float axisZ, float angleInDegrees)
    {
        var axis = new Vector3(axisX, axisY, axisZ);

        // Arrange - Create a rotation matrix using ITransformationMatrixProvider
        var rotationMatrixToTest = _matrixProvider.CreateRotationMatrix(axis, angleInDegrees);

        // Arrange - Create a rotation matrix using Matrix4x4
        var expectedRotationMatrix = Matrix4x4.CreateFromAxisAngle(axis, angleInDegrees * (float)Math.PI / 180.0f);

        // Assert - Compare the two matrices
        Assert.Equal(expectedRotationMatrix, rotationMatrixToTest);
    }
}
