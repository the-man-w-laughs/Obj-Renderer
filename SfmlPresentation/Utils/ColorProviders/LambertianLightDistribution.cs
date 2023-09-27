using Business.Contracts;
using Business.Contracts.Utils;
using SFML.Graphics;
using SfmlPresentation.Contracts;
using SfmlPresentation.Scene;
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
        private readonly Vector3 camera;
        private readonly Vector3 light;
        private readonly Matrix4x4 inversedMatrix;
        private Vector3 _normal;

        public LambertianLightDistribution(Vector3 camera, Vector3 light, Matrix4x4 finalMatrix)
        {
            this.camera = camera;
            this.light = light;
            Matrix4x4.Invert(finalMatrix, out inversedMatrix);
        }

        public Vector3 Normal
        {
            set
            {
                _normal = Vector3.Normalize(value);
            }
        }
        public void SetNormal(Vector3 normal)
        {
            _normal = Vector3.Normalize(normal);
        }

        public void SetNormal(Vector3[] vertices)
        {
            Vector3 edge1 = vertices[0] - vertices[1];
            Vector3 edge2 = vertices[0] - vertices[2];
            _normal = Vector3.Normalize(Vector3.Cross(edge1, edge2));
        }

        public Color GetColor(Vector3 point)
        {
            Vector3 toLight = light - _normal;

            float dotProduct = Vector3.Dot(_normal, toLight);
            
            float magnitudeB = toLight.Length();

            float cosineTheta = dotProduct / (magnitudeB);

            float intensity = Math.Max(0.0f, cosineTheta);

            byte finalIntensity = (byte)(intensity * 255);

            return new Color(finalIntensity, finalIntensity, finalIntensity);
        }

    }
}
