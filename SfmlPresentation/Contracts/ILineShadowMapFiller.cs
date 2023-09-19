using Business.Contracts.Utils;

namespace SfmlPresentation.Contracts
{
    public interface ILineShadowMapFiller
    {
        void DrawLine(uint x0, uint y0, uint x1, uint y1, IZBuffer zBuffer);
    }
}