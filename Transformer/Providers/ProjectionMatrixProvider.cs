using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Business.Contracts.Transformer.Providers;

namespace Transformer.Providers
{
    public class ProjectionMatrixProvider : IProjectionMatrixProvider
    {
        public Matrix4x4 CreatePerspectiveProjectionMatrix(float fieldOfView, float aspectRatio, float zNear, float zFar)
        {
            float fovRadians = fieldOfView * (MathF.PI / 180f);
            float tanHalfFov = MathF.Tan(fovRadians / 2f);

            Matrix4x4 projectionMatrix = new Matrix4x4(
                1f / (aspectRatio * tanHalfFov), 0f, 0f, 0f,
                0f, 1f / tanHalfFov, 0f, 0f,
                0f, 0f, zFar / (zNear - zFar), -1f,
                0f, 0f, zNear * zFar / (zNear - zFar), 0f
            );

            return projectionMatrix;
        }
    }
}
