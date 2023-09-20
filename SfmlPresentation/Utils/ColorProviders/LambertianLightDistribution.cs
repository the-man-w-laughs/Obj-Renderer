using Business.Contracts;
using Business.Contracts.Utils;
using SFML.Graphics;
using SfmlPresentation.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SfmlPresentation.Utils.ColorProviders
{
    public class LambertianLightDistribution : IColorProvider
    {
        private readonly ITransformationHelper _transformationHelper;

        public LambertianLightDistribution(ITransformationHelper transformationHelper)
        {
            this._transformationHelper = transformationHelper;
        }
        public Color GetColor(Vector3[] vertices, Vector3 light, IZBuffer zBuffer)
        {
            if (vertices == null || vertices.Length != 3)
            {
                throw new ArgumentException("Vertices must be an array of length 3.");
            }
            var center = CalculateTriangleCentroid(vertices[0], vertices[1], vertices[2]);

            var centerFromLight = _transformationHelper.ConvertTo2DCoordinates(center, zBuffer.Width, zBuffer.Height, light);

            if (!zBuffer.TryPoint(centerFromLight)) return new Color(0, 0, 0);

            Vector3 edge1 = vertices[0] - vertices[1];
            Vector3 edge2 = vertices[0] - vertices[2];
            Vector3 normal = Vector3.Normalize(Vector3.Cross(edge1, edge2));

            Vector3 toLight = light - center;

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
