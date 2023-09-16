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

namespace SfmlPresentation.Utils
{
    public class RasterizationObjDrawer : IRasterizationObjDrawer
    {
        private readonly ILineDrawer _drawer;
        private readonly IFaceDrawer _faceDrawer;

        public Camera Light { get; set; } = new Camera(Math.PI / 2, 0, 10);

        public RasterizationObjDrawer(ILineDrawer drawer, IFaceDrawer faceDrawer)
        {            
            _drawer = drawer;
            this._faceDrawer = faceDrawer;
        }

        public void Draw(List<Face> faces, List<Vector3> verticesToDraw, Image image, Camera light)
        {        
            foreach (var face in faces)
            {                
                var vertices = new Vector3[3];
                for (var i = 0; i < 3; i++)
                {
                    vertices[i] = verticesToDraw[face.VertexIndexList[i] - 1];
                }                

                if (IsClockwise(vertices)) continue;

                //var color = Color.White;
                var color = CalculateLambertianPolygonColor(vertices, light);

                _faceDrawer.DrawFace(image, color, vertices);
            };                    
        }

        private Color CalculateLambertianPolygonColor(Vector3[] vertices, Camera light)
        {
            if (vertices == null || vertices.Length != 3)
            {
                throw new ArgumentException("Vertices must be an array of length 3.");
            }
            
            Vector3 edge1 = vertices[0] - vertices[1]; 
            Vector3 edge2 = vertices[0] - vertices[2]; 
            Vector3 normal = Vector3.Normalize(Vector3.Cross(edge1, edge2));

            var eye = light.Eye;

            Vector3 toLight = vertices[0] - eye;
            
            float dotProduct = Vector3.Dot(normal, toLight);            
            
            float magnitudeA = normal.Length();
            float magnitudeB = toLight.Length();
            
            float cosineTheta = dotProduct / (magnitudeA * magnitudeB);

            float intensity = Math.Max(0.0f, cosineTheta);            

            byte finalIntensity = (byte)(intensity * 255);

            return new Color(finalIntensity, finalIntensity, finalIntensity);
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

        private void DrawLineIfIntersects(Image image, Color color, int startX, int startY, int endX, int endY)
        {
            var bitmapWidth = image.Size.X;
            var bitmapHeight = image.Size.Y;

            if (startX >= 0 && startY >= 0 && startX < bitmapWidth && startY < bitmapHeight
                && endX >= 0 && endY >= 0 && endX < bitmapWidth && endY < bitmapHeight)
            {
                _drawer.DrawLine(image, color, startX, startY, endX, endY);
            }
        }        
    }
}
