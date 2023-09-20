using System.Numerics;

namespace Transformer.Transpormers
{
    public interface ICoordinateTransformer
    {
        void ApplyTransform(List<Vector3> vectors, Matrix4x4 transform);        
        void ApplyTransformAndDivideByW(List<Vector3> vectors, Matrix4x4 transform);
        Vector3 ApplyTransformAndDivideByW(Vector3 vectors, Matrix4x4 transform);
    }
}