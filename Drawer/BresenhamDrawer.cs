using Business.Contracts.Drawer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer
{
    public class BresenhamDrawer: IDrawer
    {
        public void DrawLine(Bitmap bitmap, Color color, int x0, int y0, int x1, int y1)
        {

            int deltaX = Math.Abs(x1 - x0);
            int deltaY = Math.Abs(y1 - y0);
            int x = x0;
            int y = y0;
            int xIncrement = x0 < x1 ? 1 : -1;
            int yIncrement = y0 < y1 ? 1 : -1;
            int error = deltaX - deltaY;

            bitmap.SetPixel(x, y, color);

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

                bitmap.SetPixel(x, y, color);               
            }
        }
    }
}
