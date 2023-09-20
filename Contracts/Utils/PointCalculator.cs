using Business.Contracts.Utils;
using System.Drawing.Drawing2D;
using System.Numerics;

namespace SfmlPresentation.Utils
{
    public class PointCalculator : IPointCalculator
    {
        private readonly Vector3[] vertices;
        private (float A, float B, float C, float D) planeParameters;

        public PointCalculator(Vector3[] vertices)
        {
            if (vertices.Length != 3)
            {
                throw new ArgumentException("The 'vectors' array must contain exactly 3 Vector3 objects.");
            }
            planeParameters = GetPlaneParameters(vertices);
            this.vertices = vertices;
        }

        public Vector3 CalculatePointOnPlane(float X, float Y)
        {
            if (Math.Abs(planeParameters.C) < float.Epsilon)
                return new Vector3(X, Y, 0);
            var Z = (planeParameters.A * X + planeParameters.B * Y + planeParameters.D) / - planeParameters.C;            
            return new Vector3(X, Y, Z);
        }

        private (float A, float B, float C, float D) GetPlaneParameters(Vector3[] points)
        {
            if (points.Length < 3)
            {
                throw new ArgumentException("At least 3 non-collinear points are required to define a plane.", nameof(points));
            }

            Vector3 p1 = points[0];
            Vector3 p2 = points[1];
            Vector3 p3 = points[2];
            
            Vector3 vector1 = p2 - p1;
            Vector3 vector2 = p3 - p1;
            
            Vector3 normal = Vector3.Cross(vector1, vector2);
            
            normal = Vector3.Normalize(normal);
            
            float A = normal.X;
            float B = normal.Y;
            float C = normal.Z;
            
            float D = -(A * p1.X + B * p1.Y + C * p1.Z);

            return (A, B, C, D);
        }


    }
}
