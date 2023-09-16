using SFML.Graphics;
using System.Numerics;

namespace SfmlPresentation.Contracts
{
    public interface ILineDrawer
    {
        void DrawLine(Image bitmap, Color color, int x0, int y0, int x1, int y1);        
    }
}