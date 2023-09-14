using Business.Contracts;
using Business.Contracts.Transformer.Providers;
using Domain.ObjClass;
using FormsPresentation.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;
using Transformer.Transpormers;

namespace FormsPresentation.Utils
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

        public void Draw(List<Face> faces, List<Vector4> verticesToDraw, Bitmap bitmap)
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

        private void DrawLineIfIntersects(Bitmap bitmap, int startX, int startY, int endX, int endY)
        {
            lock (bitmap)
            {
                int bitmapWidth = bitmap.Width;
                int bitmapHeight = bitmap.Height;

                if (startX >= 0 && startY >= 0 && startX < bitmapWidth && startY < bitmapHeight
                    && endX >= 0 && endY >= 0 && endX < bitmapWidth && endY < bitmapHeight)
                {
                    _drawer.DrawLine(bitmap, Color.Black, startX, startY, endX, endY);
                }
            }
        }
    }
}
