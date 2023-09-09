using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Contracts.Transformer;

namespace Transformer.Providers
{
    public class ViewportMatrixProvider : IViewportMatrixProvider
    {
        public Matrix4x4 CreateProjectionToViewportMatrix(int screenWidth, int screenHeight)
        {
            float xMin = 0;
            float xMax = screenWidth;
            float yMin = 0;
            float yMax = screenHeight;

            Matrix4x4 viewportMatrix = new Matrix4x4(
                xMax / 2, 0, 0, (xMax + xMin) / 2,
                0, -yMax / 2, 0, (yMax + yMin) / 2,
                0, 0, 1, 0,
                0, 0, 0, 1
            );

            return viewportMatrix;
        }
    }
}
