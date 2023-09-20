using Business;
using Business.Contracts;
using Business.Contracts.Transformer.Providers;
using Business.Contracts.Utils;
using Domain.ObjClass;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SfmlPresentation.Contracts;
using SfmlPresentation.Scene;
using SfmlPresentation.Utils.Buffer;
using System;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.Intrinsics;
using Transformer.Transpormers;

namespace SfmlPresentation.Utils.ObjDrawers
{
    public class RasterizationObjDrawer : IRasterizationObjDrawer
    {        
        private readonly IFaceDrawer _faceDrawer;
        private readonly ITransformationHelper _transformationHelper;
        private readonly IColorProvider _colorProvider;
        private readonly IFaceShadowMapFiller _faceShadowMapFiller;

        public RasterizationObjDrawer(IFaceDrawer faceDrawer,
                                      ITransformationHelper transformationHelper,
                                      IColorProvider colorProvider,
                                      IFaceShadowMapFiller faceShadowMapFiller)
        {            
            _faceDrawer = faceDrawer;
            _transformationHelper = transformationHelper;
            this._colorProvider = colorProvider;
            this._faceShadowMapFiller = faceShadowMapFiller;
        }

        public void Draw(List<Face> faces, List<Vector3> allVertices, Image image, Vector3 camera, Vector3 light)
        {
            IZBuffer shadowMap = new ZBuffer(image.Size.X, image.Size.Y);

            foreach (var face in faces)
            {
                var verticesToDraw = new Vector3[3];

                for (var i = 0; i < 3; i++)
                {
                    var vertice = allVertices[face.VertexIndexList[i] - 1];
                    verticesToDraw[i] = _transformationHelper.ConvertTo2DCoordinates(vertice, image.Size.X, image.Size.Y, light);
                }
                if (IsClockwise(verticesToDraw))
                    continue;

                shadowMap.PointCalculator = new PointCalculator(verticesToDraw);

                _faceShadowMapFiller.DrawFace(verticesToDraw, shadowMap);
            };

            IZBuffer zBuffer = new ZBuffer(image.Size.X, image.Size.Y);
            foreach (var face in faces)
            {
                var vertices = new Vector3[3];
                var verticesToDraw = new Vector3[3];               

                for (var i = 0; i < 3; i++)
                {
                    vertices[i] = allVertices[face.VertexIndexList[i] - 1];
                    verticesToDraw[i] = _transformationHelper.ConvertTo2DCoordinates(vertices[i], image.Size.X, image.Size.Y, camera);                    
                }

                if (IsClockwise(verticesToDraw)) 
                    continue;

                var color = _colorProvider.GetColor(vertices, light, shadowMap);

                zBuffer.PointCalculator = new PointCalculator(verticesToDraw);
                                
                _faceDrawer.DrawFace(image, color, verticesToDraw, zBuffer);
            };

            //for (var i = 0; i < zBuffer.Width; i++)
            //{
            //    for (var j = 0; j < zBuffer.Height; j++)
            //    {
            //        if (zBuffer.Buffer[i,j] != shadowMap.Buffer[i,j])
            //        {
            //            throw new Exception($"{zBuffer.Buffer[i, j]} != {shadowMap.Buffer[i, j]}");
            //        }
            //    }
            //}
            var a = new Vector3[3];
        }

        private bool IsClockwise(Vector3[] vertices)
        {
            float sum = 0;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 current = vertices[i];
                Vector3 next = vertices[(i + 1) % vertices.Length];

                sum += (next.X - current.X) * (next.Y + current.Y);
            }

            return sum < 0;
        }
    }
}
