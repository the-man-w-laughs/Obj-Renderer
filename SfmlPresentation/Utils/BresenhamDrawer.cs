using SFML.Graphics;
using SFML.Window;
using SfmlPresentation.Contracts;
using SfmlPresentation.Scene;

namespace SfmlPresentation.Utils
{
    public class BresenhamDrawer : ILineDrawer
    {
        public void DrawLine(Image image, Color color, int x0, int y0, int x1, int y1)
        {
            int deltaX = Math.Abs(x1 - x0);
            int deltaY = Math.Abs(y1 - y0);
            int x = x0;
            int y = y0;
            int xIncrement = x0 < x1 ? 1 : -1;
            int yIncrement = y0 < y1 ? 1 : -1;
            int error = deltaX - deltaY;

            image.SetPixel((uint)x, (uint)y, color);

            while (x != x1 || y != y1)
            {
                int error2 = error * 2;

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

                image.SetPixel((uint)x, (uint)y, color);
            }
        }
    }
}
