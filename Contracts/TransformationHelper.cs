using Domain.ObjClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using Transformer.Transpormers;
using System.Transactions;
using Business.Contracts.Transformer.Providers;
using Business.Contracts;

namespace Business
{
    public class TransformationHelper : ITransformationHelper
    {
        private readonly ICoordinateTransformer _coordinateTransformer;
        private readonly ITransformationMatrixProvider _transformationMatrixProvider;

        public TransformationHelper(ICoordinateTransformer coordinateTransformer, ITransformationMatrixProvider transformationMatrixProvider)
        {
            _coordinateTransformer = coordinateTransformer;
            _transformationMatrixProvider = transformationMatrixProvider;
        }

        public List<Vector4> ConvertToGlobalCoordinates(Obj obj, int scale)
        {
            // Define transformation parameters            
            var scaleVector = scale * new Vector3(1, 1, 1);
            var rotationAxis = new Vector3(1.0f, 0.0f, 0.0f);
            var rotationAngleDegrees = 0;
            var size = obj.Size;
            var translation = new Vector3((size.XMax + size.XMin) / 2, (size.YMax + size.YMin) / 2, (size.ZMax + size.ZMin) / 2);

            // Compute transformation matrices
            var translationMatrix = _transformationMatrixProvider.CreateTranslationMatrix(-translation.X, -translation.Y, -translation.Z);
            var scaleMatrix = _transformationMatrixProvider.CreateScaleMatrix(scaleVector.X, scaleVector.Y, scaleVector.Z);
            var rotationMatrix = _transformationMatrixProvider.CreateRotationMatrix(rotationAxis, rotationAngleDegrees);
            var modelMatrix = translationMatrix * rotationMatrix * scaleMatrix;
            var vertices = obj.VertexList.Select(v => new Vector4(v.X, v.Y, v.Z, 1)).ToList();
            _coordinateTransformer.ApplyTransform(vertices, modelMatrix);
            return vertices;
        }
    }
}
