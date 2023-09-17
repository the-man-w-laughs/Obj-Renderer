using Business;
using Business.Contracts;
using Business.Contracts.Transformer.Providers;
using Domain.ObjClass;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SfmlPresentation.Contracts;
using SfmlPresentation.Scene;
using System.Numerics;
using System.Runtime.Intrinsics;
using Transformer.Transpormers;

namespace SfmlPresentation.Utils.ObjDrawers
{
    public class RasterizationObjDrawer : IRasterizationObjDrawer
    {
        private readonly ILineDrawer _drawer;
        private readonly IFaceDrawer _faceDrawer;
        private readonly ITransformationHelper _transformationHelper;        

        public RasterizationObjDrawer(ILineDrawer drawer, IFaceDrawer faceDrawer, ITransformationHelper transformationHelper)
        {
            _drawer = drawer;
            _faceDrawer = faceDrawer;
            _transformationHelper = transformationHelper;
        }

        public void Draw(List<Face> faces, List<Vector3> allVertices, Image image, Vector3 camera, Vector3 light)
        {
            foreach (var face in faces)
            {
                var vertices = new Vector3[3];
                var verticesToDraw = new Vector3[3];
                for (var i = 0; i < 3; i++)
                {
                    vertices[i] = allVertices[face.VertexIndexList[i] - 1];
                    verticesToDraw[i] = _transformationHelper.ConvertTo2DCoordinates(vertices[i], image.Size.X, image.Size.Y, camera);
                }
                if (IsClockwise(verticesToDraw)) continue;

                var color = CalculateLambertianPolygonColor(vertices, light);
                
                _faceDrawer.DrawFace(image, color, verticesToDraw);
            };
        }

        private bool IsClockwise(Vector3[] vertices)
        {
            float sum = 0;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 current = vertices[i];
                Vector3 next = vertices[(i + 1) % vertices.Length];

                sum += (next.X - current.X) * (next.Y + current.Y);
            }

            return sum < 0;
        }

        private Color CalculateLambertianPolygonColor(Vector3[] vertices, Vector3 light)
        {
            if (vertices == null || vertices.Length != 3)
            {
                throw new ArgumentException("Vertices must be an array of length 3.");
            }

            Vector3 edge1 = vertices[0] - vertices[1];
            Vector3 edge2 = vertices[0] - vertices[2];
            Vector3 normal = Vector3.Normalize(Vector3.Cross(edge1, edge2));

            Vector3 toLight = light - CalculateTriangleCentroid(vertices[0], vertices[1], vertices[2]);

            float dotProduct = Vector3.Dot(normal, toLight);

            float magnitudeA = normal.Length();
            float magnitudeB = toLight.Length();

            float cosineTheta = dotProduct / (magnitudeA * magnitudeB);

            float intensity = Math.Max(0.0f, cosineTheta);

            byte finalIntensity = (byte)(intensity * 255);

            return new Color(finalIntensity, finalIntensity, finalIntensity);
        }

        private Vector3 CalculateTriangleCentroid(
            Vector3 point1, Vector3 point2, Vector3 point3)
        {
            Vector3 midPointAB = (point1 + point2) / 2;
            Vector3 midPointBC = (point2 + point3) / 2;
            Vector3 midPointCA = (point3 + point1) / 2;

            Vector3 centroid = (midPointAB + midPointBC + midPointCA) / 3;

            return centroid;
        }
    }
}
