using System.Numerics;

namespace Business.Contracts.Utils
{
    public interface IPointCalculator
    {
        Vector3 CalculatePointOnPlane(float X, float Y);
    }
}