using Domain.ObjClass;
using System.Numerics;

namespace Business.Contracts
{
    public interface ITransformationHelper
    {
        List<Vector4> ConvertToGlobalCoordinates(Obj obj, int scale);
    }
}