using Domain.ObjClass;
using System.Numerics;

namespace Business.Contracts
{
    public interface ITransformationHelper
    {
        List<Vector3> ConvertTo2DCoordinates(List<Vector3> vertices, uint width, uint height, Vector3 camera);
        Vector3 ConvertTo2DCoordinates(Vector3 target, uint width, uint height, Vector3 camera);
        List<Vector3> ConvertToGlobalCoordinates(Obj obj, int scale, Vector3 rotationAxis, int rotationAngleDegrees);
    }
}