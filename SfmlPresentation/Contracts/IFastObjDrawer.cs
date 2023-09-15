using Domain.ObjClass;
using SFML.Graphics;
using System.Numerics;

namespace SfmlPresentation.Contracts
{
    public interface IFastObjDrawer
    {
        void Draw(List<Face> faces, List<Vector4> verticesToDraw, Image bitmap, Color color);
    }
}