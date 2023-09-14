using Domain.ObjClass;
using System.Numerics;

namespace FormsPresentation.Contracts
{
    public interface IFastObjDrawer
    {
        void Draw(List<Face> faces, List<Vector4> verticesToDraw, Bitmap bitmap);
    }
}