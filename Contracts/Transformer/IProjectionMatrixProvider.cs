using System.Numerics;

namespace Contracts.Transformer
{
    public interface IProjectionMatrixProvider
    {
        Matrix4x4 CreatePerspectiveProjectionMatrix(float fieldOfView, float aspectRatio, float zNear, float zFar);
    }
}