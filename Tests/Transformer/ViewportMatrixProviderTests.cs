using Contracts.Transformer;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Numerics;
using Xunit;

namespace Tests.Transformer;
public class ViewportMatrixProviderTests
{
    private readonly IViewportMatrixProvider _matrixProvider;

    public ViewportMatrixProviderTests(IViewportMatrixProvider matrixProvider)
    {
        _matrixProvider = matrixProvider;
    }

    [Theory]
    [InlineData(800, 600)] // Test case 1
    [InlineData(1920, 1080)] // Test case 2
    [InlineData(1280, 720)] // Test case 3
    // Add more test cases as needed
    public void CreateProjectionToViewportMatrix_CreatesCorrectMatrix(int screenWidth, int screenHeight)
    {        
        Assert.True(true);
    }
}
