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
    public class FastObjDrawer : IFastObjDrawer
    {
        private readonly IDrawer _drawer;
        private readonly IViewMatrixProvider _viewMatrixProvider;
        private readonly ICoordinateTransformer _coordinateTransformer;
        private readonly IProjectionMatrixProvider _projectionMatrixProvider;
        private readonly IViewportMatrixProvider _viewportMatrixProvider;

        public FastObjDrawer(IDrawer drawer,
                             IViewMatrixProvider viewMatrixProvider,
                             ICoordinateTransformer coordinateTransformer,
                             IProjectionMatrixProvider projectionMatrixProvider,
                             IViewportMatrixProvider viewportMatrixProvider)
        {
            _drawer = drawer;
            _viewMatrixProvider = viewMatrixProvider;
            _coordinateTransformer = coordinateTransformer;
            _projectionMatrixProvider = projectionMatrixProvider;
            _viewportMatrixProvider = viewportMatrixProvider;
        }

        public void Draw(List<Face> faces, List<Vector4> verticesToDraw, Image bitmap)
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

                    DrawLineIfIntersects(bitmap, startX, startY, endX, endY);
                }

                var lastPoint = verticesToDraw[face.VertexIndexList.Last() - 1];
                var firstPoint = verticesToDraw[face.VertexIndexList.First() - 1];

                int lastX = (int)lastPoint.X;
                int lastY = (int)lastPoint.Y;
                int firstX = (int)firstPoint.X;
                int firstY = (int)firstPoint.Y;

                DrawLineIfIntersects(bitmap, lastX, lastY, firstX, firstY);
            };
        }

        private void DrawLineIfIntersects(Image image, int startX, int startY, int endX, int endY)
        {
            var bitmapWidth = image.Size.X;
            var bitmapHeight = image.Size.Y;

            if (startX >= 0 && startY >= 0 && startX < bitmapWidth && startY < bitmapHeight
                && endX >= 0 && endY >= 0 && endX < bitmapWidth && endY < bitmapHeight)
            {
                _drawer.DrawLine(image, Color.White, startX, startY, endX, endY);
            }
        }
    }
}
