using Domain.ObjClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Transformer.Transpormers
{
    public static class CoordinateTransformer
    {
        public static void ApplyTransform(this List<Vector4> vectors, Matrix4x4 transform)
        {
            for (int i = 0; i < vectors.Count; i++)
            {
                vectors[i] = Vector4.Transform(vectors[i], transform);
            }
        }

        public static void ApplyTransformAndDivideByW(this List<Vector4> vectors, Matrix4x4 transform)
        {
            for (int i = 0; i < vectors.Count; i++)
            {
                var transformedVertex = Vector4.Transform(vectors[i], transform);
                vectors[i] = transformedVertex / transformedVertex.W;
            }
        }
        public static List<Vector4> ApplyTransformAndCopy(this List<Vector4> vectors, Matrix4x4 transform)
        {
            return vectors.Select(v => Vector4.Transform(v, transform)).ToList();            
        }

        public static List<Vector4> DivideByW(this List<Vector4> vectors)
        {
            return vectors.Select(v => new Vector4(v.X / v.W, v.Y / v.W, v.Z / v.W, 1)).ToList();
        }
    }
}
