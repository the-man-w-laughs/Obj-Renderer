using Business.Contracts.Transformer.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Transformer.Providers
{
    public class TransformationMatrixProvider : ITransformationMatrixProvider
    {
        public Matrix4x4 CreateScaleMatrix(float scaleX, float scaleY, float scaleZ)
        {
            return new Matrix4x4(
                scaleX, 0, 0, 0,
                0, scaleY, 0, 0,
                0, 0, scaleZ, 0,
                0, 0, 0, 1
            );
        }

        public Matrix4x4 CreateTranslationMatrix(float translateX, float translateY, float translateZ)
        {
            return new Matrix4x4(
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                translateX, translateY, translateZ, 1
            );
        }

        public Matrix4x4 CreateRotationMatrix(Vector3 axis, float angleInDegrees)
        {
            // Normalize the axis vector
            axis = Vector3.Normalize(axis);

            // Convert the angle from degrees to radians
            float angleInRadians = angleInDegrees * (float)Math.PI / 180.0f;

            float x = axis.X, y = axis.Y, z = axis.Z;
            float sa = MathF.Sin(angleInRadians), ca = MathF.Cos(angleInRadians);
            float xx = x * x, yy = y * y, zz = z * z;
            float xy = x * y, xz = x * z, yz = y * z;

            Matrix4x4 rotationMatrix = Matrix4x4.Identity;

            rotationMatrix.M11 = xx + ca * (1.0f - xx);
            rotationMatrix.M12 = xy - ca * xy + sa * z;
            rotationMatrix.M13 = xz - ca * xz - sa * y;

            rotationMatrix.M21 = xy - ca * xy - sa * z;
            rotationMatrix.M22 = yy + ca * (1.0f - yy);
            rotationMatrix.M23 = yz - ca * yz + sa * x;

            rotationMatrix.M31 = xz - ca * xz + sa * y;
            rotationMatrix.M32 = yz - ca * yz - sa * x;
            rotationMatrix.M33 = zz + ca * (1.0f - zz);         

            return rotationMatrix;
        }

    }


}
