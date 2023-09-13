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
        public void ApplyTransform(List<Vector4> vectors, Matrix4x4 transform)
        {
            for (int i = 0; i < vectors.Count; i++)
            {
                vectors[i] = Vector4.Transform(vectors[i], transform);
            }
        }

        public List<Vector4> ApplyTransformAndDivideByWAndCopy(List<Vector4> vectors, Matrix4x4 transform)
        {
            //List<Vector4> transformedVectors = new List<Vector4>();
            //object lockObject = new object(); // Create a lock object

            //Parallel.For(0, vectors.Count, i =>
            //{
            //    var transformedVertex = Vector4.Transform(vectors[i], transform);
            //    var transformedVector = transformedVertex / transformedVertex.W;

            //    // Safely add the transformed vector to the list using a lock
            //    lock (lockObject)
            //    {
            //        transformedVectors.Add(transformedVector);
            //    }
            //});

            List<Vector4> transformedVectors = new List<Vector4>();

            for (int i = 0; i < vectors.Count; i++)
            {
                var transformedVertex = Vector4.Transform(vectors[i], transform);
                var transformedVector = transformedVertex / transformedVertex.W;

                transformedVectors.Add(transformedVector);
            }
            return transformedVectors;
        }
        public List<Vector4> ApplyTransformAndCopy(List<Vector4> vectors, Matrix4x4 transform)
        {
            return vectors.Select(v => Vector4.Transform(v, transform)).ToList();            
        }

        public List<Vector4> DivideByW(List<Vector4> vectors)
        {
            return vectors.Select(v => new Vector4(v.X / v.W, v.Y / v.W, v.Z / v.W, 1)).ToList();
        }
    }
}
