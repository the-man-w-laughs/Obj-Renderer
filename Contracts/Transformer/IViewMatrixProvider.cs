using System.Numerics;

namespace Contracts.Transformer
{
    public interface IViewMatrixProvider
    {
        Matrix4x4 WorldToViewMatrix(Vector3 eye, Vector3 target, Vector3 up);
    }
}