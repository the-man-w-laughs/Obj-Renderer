using SFML.Graphics;
using SfmlPresentation.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SfmlPresentation.Utils
{
    public class FaceDrawer : IFaceDrawer
    {
        private readonly ILineDrawer lineDrawer;

        public FaceDrawer(ILineDrawer lineDrawer)
        {
            this.lineDrawer = lineDrawer;
        }

        public void DrawFace(Image image, Color color, Vector3[] vertices)
        {
            if (vertices == null || vertices.Length < 3)
            {
                throw new ArgumentException("Vertices must be an array of length 3.");
            }
            
            int minY = (int)vertices.Min(v => v.Y);            
            int maxY = (int)vertices.Max(v => v.Y);
            if (minY < 0 || maxY > image.Size.Y) return;

            for (int y = minY; y <= maxY; y++)
            {
                List<int> intersections = new List<int>();
                
                for (int i = 0; i < vertices.Length; i++)
                {
                    int nextIndex = (i + 1) % vertices.Length;
                    int x0 = (int)vertices[i].X;
                    int y0 = (int)vertices[i].Y;
                    int x1 = (int)vertices[nextIndex].X;
                    int y1 = (int)vertices[nextIndex].Y;
                    
                    if ((y0 <= y && y1 > y) || (y1 <= y && y0 > y))
                    {
                        try
                        {
                            int xIntersect = GetIntersect(x0, y0, x1, y1, y);
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
                        DrawLineIfIntersects(image, color, x0, y, x1, y);
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
            int x = x0 + (x1 - x0) * (y - y0) / (y1 - y0);

            if (x < Math.Min(x0, x1) || x > Math.Max(x0, x1))
            {
                throw new InvalidOperationException("Intersection point is outside the range of x0 and x1.");
            }
            return x;
        }

        private void DrawLineIfIntersects(Image image, Color color, int startX, int startY, int endX, int endY)
        {
            var bitmapWidth = image.Size.X;
            var bitmapHeight = image.Size.Y;

            if (startX >= 0 && startY >= 0 && startX < bitmapWidth && startY < bitmapHeight
                && endX >= 0 && endY >= 0 && endX < bitmapWidth && endY < bitmapHeight)
            {
                lineDrawer.DrawLine(image, color, startX, startY, endX, endY);
            }
        }

    }
}
