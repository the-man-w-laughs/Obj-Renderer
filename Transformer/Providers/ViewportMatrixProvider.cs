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
        public Matrix4x4 CreateProjectionToViewportMatrix(int screenWidth, int screenHeight, float xMin, float yMin)
        {            
            Matrix4x4 viewportMatrix = new Matrix4x4(
                screenWidth / 2, 0, 0, xMin + screenWidth / 2,
                0, -screenHeight / 2, 0, yMin + screenHeight / 2,
                0, 0, 1, 0,
                0, 0, 0, 1
            );

            return viewportMatrix;
        }
    }
}
