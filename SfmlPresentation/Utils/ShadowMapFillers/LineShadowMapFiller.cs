using Business.Contracts.Utils;
using SFML.Graphics;
using SFML.Window;
using SfmlPresentation.Contracts;
using SfmlPresentation.Scene;
using System.Numerics;

namespace SfmlPresentation.Utils.ComponentDrawers
{
    public class LineShadowMapFiller : ILineShadowMapFiller
    {
        public void DrawLine(uint x0, uint y0, uint x1, uint y1, IZBuffer zBuffer)
        {
            var deltaX = (uint)Math.Abs(x1 - x0);
            var deltaY = (uint)Math.Abs(y1 - y0);
            int x = (int)x0;
            int y = (int)y0;
            int xIncrement = x0 < x1 ? 1 : -1;
            int yIncrement = y0 < y1 ? 1 : -1;
            var error = deltaX - deltaY;

            zBuffer.SetPoint((uint)x, (uint)y);

            while (x != x1 || y != y1)
            {
                var error2 = error * 2;

                if (error2 > -deltaY)
                {
                    error -= deltaY;
                    x += xIncrement;
                }

                if (error2 < deltaX)
                {
                    error += deltaX;
                    y += yIncrement;
                }

                zBuffer.SetPoint((uint)x, (uint)y);
            }
        }
    }
}