using Business.Contracts.Transformer.Providers;
using Domain.ObjClass;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SfmlPresentation.Contracts;
using System.Numerics;
using Transformer.Transpormers;

namespace SfmlPresentation.Utils
{
    public class PolygonObjDrawer : IPolygonObjDrawer
    {
        private readonly ILineDrawer _drawer;

        public PolygonObjDrawer(ILineDrawer drawer)
        {
            _drawer = drawer;            
        }

        public void Draw(List<Face> faces, List<Vector4> verticesToDraw, Image bitmap, Color color)
        {
            foreach (var face in faces)
            {
                for (int i = 0; i < face.VertexIndexList.Count() - 1; i++)
                {
                    var startPoint = verticesToDraw[face.VertexIndexList[i] - 1];
                    var endPoint = verticesToDraw[face.VertexIndexList[i + 1] - 1];

                    int startX = (int)startPoint.X;
                    int startY = (int)startPoint.Y;
                    int endX = (int)endPoint.X;
                    int endY = (int)endPoint.Y;

                    DrawLineIfIntersects(bitmap, color, startX, startY, endX, endY);
                }

                var lastPoint = verticesToDraw[face.VertexIndexList.Last() - 1];
                var firstPoint = verticesToDraw[face.VertexIndexList.First() - 1];

                int lastX = (int)lastPoint.X;
                int lastY = (int)lastPoint.Y;
                int firstX = (int)firstPoint.X;
                int firstY = (int)firstPoint.Y;

                DrawLineIfIntersects(bitmap, color, lastX, lastY, firstX, firstY);
            };
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
