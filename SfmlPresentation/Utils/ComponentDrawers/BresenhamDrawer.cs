using Business.Contracts.Utils;
using SFML.Graphics;
using SFML.Window;
using SfmlPresentation.Contracts;
using SfmlPresentation.Scene;
using System.Numerics;

namespace SfmlPresentation.Utils.ComponentDrawers
{
    public class BresenhamDrawer : ILineDrawer
    {
        public void DrawLine(Image image, Color color, uint x0, uint y0, uint x1, uint y1, IZBuffer zBuffer)
        {
            var deltaX = (uint)Math.Abs(x1 - x0);
            var deltaY = (uint)Math.Abs(y1 - y0);
            int x = (int)x0;
            int y = (int)y0;
            int xIncrement = x0 < x1 ? 1 : -1;
            int yIncrement = y0 < y1 ? 1 : -1;
            var error = deltaX - deltaY;

            SetPixel(image, (uint)x, (uint)y, color, zBuffer);

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

                SetPixel(image, (uint)x, (uint)y, color, zBuffer);
            }
        }

        private void SetPixel(Image image, uint x, uint y, Color color, IZBuffer zBuffer)
        {
            if (zBuffer.SetPoint(x, y))
                image.SetPixel(x, y, color);
        }
    }
}