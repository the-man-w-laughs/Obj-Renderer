using System.Numerics;
using System.Security.Principal;
using Business.Contracts.Transformer.Providers;

namespace Transformer.Providers
{
    public class ViewMatrixProvider : IViewMatrixProvider
    {
        public Matrix4x4 WorldToViewMatrix(Vector3 eye, Vector3 target, Vector3 up)
        {
            Vector3 zaxis = Vector3.Normalize(eye - target);
            Vector3 xaxis = Vector3.Normalize(Vector3.Cross(up, zaxis));
            Vector3 yaxis = Vector3.Cross(zaxis, xaxis);

            Matrix4x4 result = Matrix4x4.Identity;

            result.M11 = xaxis.X;
            result.M12 = yaxis.X;
            result.M13 = zaxis.X;

            result.M21 = xaxis.Y;
            result.M22 = yaxis.Y;
            result.M23 = zaxis.Y;

            result.M31 = xaxis.Z;
            result.M32 = yaxis.Z;
            result.M33 = zaxis.Z;

            result.M41 = -Vector3.Dot(xaxis, eye);
            result.M42 = -Vector3.Dot(yaxis, eye);
            result.M43 = -Vector3.Dot(zaxis, eye);

            return result;
        }
    }
}