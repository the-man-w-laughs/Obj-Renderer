using Domain.ObjClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Transformer.Transpormers
{
    public class CoordinateTransformer: ICoordinateTransformer
    {
        public void ApplyTransform(List<Vector3> vectors, Matrix4x4 transform)
        {
            for (int i = 0; i < vectors.Count; i++)
            {
                vectors[i] = Vector3.Transform(vectors[i], transform);
            }
        }

        public void ApplyTransformAndDivideByW(List<Vector3> vectors, Matrix4x4 transform)
        {           
            for (int i = 0; i < vectors.Count; i++)
            {
                vectors[i] = ApplyTransformAndDivideByW(vectors[i], transform);
            }            
        }

        public Vector3 ApplyTransformAndDivideByW(Vector3 vector, Matrix4x4 transform)
        {
            var transformedVertex = Vector4.Transform(vector, transform);
            var transformedVector = transformedVertex / transformedVertex.W;

            return new Vector3(transformedVector.X, transformedVector.Y, transformedVector.Z);
        }
    }
}
