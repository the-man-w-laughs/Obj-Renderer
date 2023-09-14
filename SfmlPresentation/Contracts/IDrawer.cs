using SFML.Graphics;

namespace SfmlPresentation.Contracts
{
    public interface IDrawer
    {
        void DrawLine(Image bitmap, Color color, int x0, int y0, int x1, int y1);
    }
}