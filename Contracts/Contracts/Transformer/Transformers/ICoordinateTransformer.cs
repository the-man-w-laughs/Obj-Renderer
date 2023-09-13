using System.Numerics;

namespace Transformer.Transpormers
{
    public interface ICoordinateTransformer
    {
        void ApplyTransform(List<Vector4> vectors, Matrix4x4 transform);
        List<Vector4> ApplyTransformAndCopy(List<Vector4> vectors, Matrix4x4 transform);
        List<Vector4> ApplyTransformAndDivideByWAndCopy(List<Vector4> vectors, Matrix4x4 transform);
        List<Vector4> DivideByW(List<Vector4> vectors);
    }
}