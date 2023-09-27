using Business.Contracts.Utils;
using SFML.Graphics;
using SfmlPresentation.Scene;
using System.Numerics;

namespace SfmlPresentation.Contracts
{
    public interface ILineDrawer
    {
        public void DrawLine(Image image, uint x0, uint y0, uint x1, uint y1, IColorProvider colorProvider, IZBuffer zBuffer);
    }
}