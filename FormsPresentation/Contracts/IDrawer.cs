namespace FormsPresentation.Contracts
{
    public interface IDrawer
    {
        void DrawLine(Bitmap bitmap, Color color, int x0, int y0, int x1, int y1);
    }
}