using Domain.ObjClass;
using System.Numerics;

namespace Business.Contracts
{
    public interface ITransformationHelper
    {
        List<Vector4> ConvertTo2DCoordinates(List<Vector4> vertices, int width, int height, Vector3 eye);        
        List<Vector4> ConvertToGlobalCoordinates(Obj obj, int scale, Vector3 rotationAxis, int rotationAngleDegrees);
    }
}