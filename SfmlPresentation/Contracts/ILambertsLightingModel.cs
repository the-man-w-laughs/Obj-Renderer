using Domain.ObjClass;
using SFML.Graphics;
using System.Numerics;

namespace SfmlPresentation.Contracts
{
    public interface ILambertsLightingModel
    {
        List<Color> GetColors(List<Face> faces, List<Vector3> vectors, Vector3 light);
    }
}