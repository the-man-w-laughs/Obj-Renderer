using System.Numerics;

namespace Business.Contracts.Transformer.Providers
{
    public interface IViewMatrixProvider
    {
        Matrix4x4 WorldToViewMatrix(Vector3 eye, Vector3 target, Vector3 up);
    }
}