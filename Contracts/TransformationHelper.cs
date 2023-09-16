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
        private readonly IViewMatrixProvider _viewMatrixProvider;
        private readonly IProjectionMatrixProvider _projectionMatrixProvider;
        private readonly IViewportMatrixProvider _viewportMatrixProvider;

        public TransformationHelper(ICoordinateTransformer coordinateTransformer,
                                    ITransformationMatrixProvider transformationMatrixProvider,
                                    IViewMatrixProvider viewMatrixProvider,                             
                                    IProjectionMatrixProvider projectionMatrixProvider,
                                    IViewportMatrixProvider viewportMatrixProvider)
        {
            _coordinateTransformer = coordinateTransformer;
            _transformationMatrixProvider = transformationMatrixProvider;
            _viewMatrixProvider = viewMatrixProvider;
            _projectionMatrixProvider = projectionMatrixProvider;
            _viewportMatrixProvider = viewportMatrixProvider;
        }

        public List<Vector4> ConvertToGlobalCoordinates(Obj obj, int scale, Vector3 rotationAxis, int rotationAngleDegrees)
        {
            // Define transformation parameters            
            var scaleVector = scale * new Vector3(1, 1, 1);
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

        public List<Vector3> ConvertTo2DCoordinates(List<Vector4> vertices, int width, int height, Vector3 eye)
        {
            var projectionMatrix = _projectionMatrixProvider.CreatePerspectiveProjectionMatrix(45.0f, (float)width / height, 1.0f, 100.0f);
            var viewportMatrix = _viewportMatrixProvider.CreateProjectionToViewportMatrix(width, height, 0, 0);

            var target = new Vector3(0, 0, 0);
            var up = new Vector3(0, 1, 0);
            var viewMatrix = _viewMatrixProvider.WorldToViewMatrix(eye, target, up);

            var finalMatrix = viewMatrix * projectionMatrix * viewportMatrix;

            return _coordinateTransformer.ApplyTransformAndDivideByWAndCopy(vertices, finalMatrix);
        }
    }
}
