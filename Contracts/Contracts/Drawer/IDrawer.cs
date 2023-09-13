using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contracts.Drawer
{
    public interface IDrawer
    {
        public void DrawLine(Bitmap bitmap, Color color, int x0, int y0, int x1, int y1);
    }
}
