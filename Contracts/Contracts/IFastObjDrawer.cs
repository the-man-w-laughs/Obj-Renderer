using Domain.ObjClass;
using System.Drawing;
using System.Numerics;

namespace Business.Contracts
{
    public interface IFastObjDrawer
    {
        void Draw(List<Face> faces, List<Vector4> vertices, Bitmap bitmap, Vector3 eye);
    }
}