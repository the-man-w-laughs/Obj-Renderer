using System.Numerics;
using Contracts.Transformer;

namespace Transformer.Providers
{
    public class ViewMatrixProvider : IViewMatrixProvider
    {
        public Matrix4x4 WorldToViewMatrix(Vector3 eye, Vector3 target, Vector3 up)
        {
            Vector3 zAxis = Vector3.Normalize(eye - target);
            Vector3 xAxis = Vector3.Normalize(Vector3.Cross(up, zAxis));
            Vector3 yAxis = Vector3.Cross(zAxis, xAxis);

            Matrix4x4 viewMatrix = new Matrix4x4(
                xAxis.X, xAxis.Y, xAxis.Z, -Vector3.Dot(xAxis, eye),
                yAxis.X, yAxis.Y, yAxis.Z, -Vector3.Dot(yAxis, eye),
                zAxis.X, zAxis.Y, zAxis.Z, -Vector3.Dot(zAxis, eye),
                0, 0, 0, 1
            );

            return viewMatrix;
        }
    }
}