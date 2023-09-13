using System.Numerics;

namespace Business.Contracts.Transformer.Providers
{
    public interface IProjectionMatrixProvider
    {
        Matrix4x4 CreatePerspectiveProjectionMatrix(float fieldOfView, float aspectRatio, float zNear, float zFar);
    }
}