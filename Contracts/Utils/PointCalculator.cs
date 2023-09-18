using Business.Contracts.Utils;
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

            Vector3 vector1To2 = new Vector3(vector2.X - vector1.X, vector2.Y - vector1.Y, vector2.Z - vector1.Z);
            Vector3 vector1To3 = new Vector3(vector3.X - vector1.X, vector3.Y - vector1.Y, vector3.Z - vector1.Z);

            float u = (X - vector1.X) / vector1To2.X;
            float v = (Y - vector1.Y) / vector1To2.Y;

            float interpolatedZ = vector1.Z + u * vector1To2.Z + v * vector1To3.Z;

            return new Vector3(X, Y, interpolatedZ);
        }
    }
}
