using System.Numerics;

namespace Business.Contracts.Transformer.Providers
{
    public interface IViewportMatrixProvider
    {
        Matrix4x4 CreateProjectionToViewportMatrix(uint screenWidth, uint screenHeight, float xMin, float yMin);
    }
}