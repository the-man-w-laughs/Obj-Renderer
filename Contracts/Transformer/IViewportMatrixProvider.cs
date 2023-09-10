using System.Numerics;

namespace Contracts.Transformer
{
    public interface IViewportMatrixProvider
    {
        Matrix4x4 CreateProjectionToViewportMatrix(int screenWidth, int screenHeight, float xMin, float yMin);
    }
}