using Business.Contracts.Utils;
using System.Drawing.Drawing2D;
using System.Numerics;

namespace SfmlPresentation.Utils
{
    public class PointCalculator : IPointCalculator
    {
        private readonly Vector3[] vertices;
        public PointCalculator(Vector3[] vertices)
        {
            if (vertices.Length != 3)
            {
                throw new ArgumentException("The 'vectors' array must contain exactly 3 Vector3 objects.");
            }
            this.vertices = vertices;
        }
        public Vector3 CalculatePointOnPlane(float X, float Y)
        {
            Vector3 vector1 = vertices[0];
            Vector3 vector2 = vertices[1];
            Vector3 vector3 = vertices[2];

            Vector3 normal = Vector3.Cross(vector2 - vector1, vector3 - vector1);

            if (Math.Abs(normal.X) < float.Epsilon && Math.Abs(normal.Y) < float.Epsilon)
            {                
                return Vector3.Zero;
            }            
            float u = ((X - vector1.X) * normal.X + (Y - vector1.Y) * normal.Y - vector1.Z * normal.Z) /
                      (normal.X * vector2.X + normal.Y * vector2.Y - normal.Z * vector2.Z);
            float v = (X - vector1.X - u * vector2.X) / vector3.X;

            float interpolatedZ = vector1.Z + u * vector2.Z + v * vector3.Z;

            return new Vector3(X, Y, interpolatedZ);
        }
    }
}
