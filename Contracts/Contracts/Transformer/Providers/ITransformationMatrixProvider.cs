using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contracts.Transformer.Providers
{
    public interface ITransformationMatrixProvider
    {
        public Matrix4x4 CreateScaleMatrix(float scaleX, float scaleY, float scaleZ);

        public Matrix4x4 CreateTranslationMatrix(float translateX, float translateY, float translateZ);

        public Matrix4x4 CreateRotationMatrix(Vector3 axis, float angleInDegrees);
    }
}
