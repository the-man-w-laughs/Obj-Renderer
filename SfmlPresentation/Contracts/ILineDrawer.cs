using Business.Contracts.Utils;
using SFML.Graphics;
using System.Numerics;

namespace SfmlPresentation.Contracts
{
    public interface ILineDrawer
    {
        public void DrawLine(Image image, Color color, uint x0, uint y0, uint x1, uint y1, IZBuffer zBuffer);
    }
}