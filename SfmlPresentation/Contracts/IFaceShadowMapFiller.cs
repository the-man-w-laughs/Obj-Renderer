using Business.Contracts.Utils;
using System.Numerics;

namespace SfmlPresentation.Contracts
{
    public interface IFaceShadowMapFiller
    {
        void DrawFace(Vector3[] vertices, IZBuffer zBuffer);
    }
}