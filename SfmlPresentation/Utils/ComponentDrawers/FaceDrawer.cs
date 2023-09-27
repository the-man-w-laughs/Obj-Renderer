using Business.Contracts.Utils;
using SFML.Graphics;
using SfmlPresentation.Contracts;
using SfmlPresentation.Scene;
using SfmlPresentation.Utils.Buffer;
using System.Numerics;

namespace SfmlPresentation.Utils.ComponentDrawers
{
    public class FaceDrawer : IFaceDrawer
    {
        private readonly ILineDrawer lineDrawer;        

        public FaceDrawer(ILineDrawer lineDrawer)
        {
            this.lineDrawer = lineDrawer;
        }

        public void DrawFace(Image image, Vector3[] vertices, IColorProvider colorProvider, IZBuffer zBuffer)
        {
            if (vertices == null || vertices.Length < 3)
            {
                throw new ArgumentException("Vertices must be an array of length 3.");
            }

            var minY = (int)vertices.Min(v => v.Y);            
            var maxY = (int)vertices.Max(v => v.Y);
            if (minY < 0 && maxY > image.Size.Y) return;
            if (maxY > image.Size.Y) 
                maxY = (int)image.Size.Y;
            if (minY < 0) 
                minY = 0;
            if (minY > maxY) return;
            for (var y = minY; y <= maxY; y++)
            {
                List<int> intersections = new List<int>();

                for (int i = 0; i < vertices.Length; i++)
                {
                    int nextIndex = (i + 1) % vertices.Length;
                    int x0 = (int)vertices[i].X;
                    int y0 = (int)vertices[i].Y;
                    int x1 = (int)vertices[nextIndex].X;
                    int y1 = (int)vertices[nextIndex].Y;

                    if (y0 <= y && y1 > y || y1 <= y && y0 > y)
                    {
                        try
                        {
                            int xIntersect = GetIntersect(x0, y0, x1, y1, (int)y);
                            intersections.Add(xIntersect);
                        }
                        catch
                        {

                        }

                    }
                }
                if (intersections.Count() == 2)
                {
                    intersections.Sort();
                    for (int i = 0; i < intersections.Count; i += 2)
                    {
                        int x0 = intersections[i];
                        int x1 = intersections[i + 1];
                        DrawLineIfIntersects(image, x0, (int)y, x1, (int)y, colorProvider, zBuffer);
                    }
                }
            }
        }
        private int GetIntersect(int x0, int y0, int x1, int y1, int y)
        {
            if (y0 == y1)
            {
                throw new InvalidOperationException("Lines are parallel, and there is no intersection.");
            }
            var x = x0 + (x1 - x0) * (y - y0) / (y1 - y0);

            if (x < Math.Min(x0, x1) || x > Math.Max(x0, x1))
            {
                throw new InvalidOperationException("Intersection point is outside the range of x0 and x1.");
            }
            return x;
        }

        private void DrawLineIfIntersects(Image image, int startX, int startY, int endX, int endY, IColorProvider colorProvider, IZBuffer buffer)
        {
            var bitmapWidth = image.Size.X;
            var bitmapHeight = image.Size.Y;

            if (startX < 0) startX = 0;
            if (endX > bitmapWidth) endX = (int)bitmapWidth;

            if (startX >= 0 && startY >= 0 && startX <= bitmapWidth && startY <= bitmapHeight
                && endX >= 0 && endY >= 0 && endX <= bitmapWidth && endY <= bitmapHeight)
            {

                lineDrawer.DrawLine(image, (uint)startX, (uint)startY, (uint)endX, (uint)endY, colorProvider, buffer);
            }
        }
    }
}
