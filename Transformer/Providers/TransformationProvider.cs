using Contracts.Transformer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Transformer.Providers
{
    public class TransformationProvider : ITransformationProvider
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
            // Convert the angle from degrees to radians
            float angleInRadians = MathF.PI * angleInDegrees / 180.0f;

            float cos = (float)Math.Cos(angleInRadians);
            float sin = (float)Math.Sin(angleInRadians);
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;

            return new Matrix4x4(
                cos + (1 - cos) * x * x, (1 - cos) * x * y - sin * z, (1 - cos) * x * z + sin * y, 0,
                (1 - cos) * x * y + sin * z, cos + (1 - cos) * y * y, (1 - cos) * y * z - sin * x, 0,
                (1 - cos) * x * z - sin * y, (1 - cos) * y * z + sin * x, cos + (1 - cos) * z * z, 0,
                0, 0, 0, 1
            );
        }
    }


}
