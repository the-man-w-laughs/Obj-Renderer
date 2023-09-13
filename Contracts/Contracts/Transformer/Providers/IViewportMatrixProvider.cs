using System.Numerics;

namespace Business.Contracts.Transformer.Providers
{
    public interface IViewportMatrixProvider
    {
        Matrix4x4 CreateProjectionToViewportMatrix(int screenWidth, int screenHeight, float xMin, float yMin);
    }
}