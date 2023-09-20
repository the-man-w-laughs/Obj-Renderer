using Business.Contracts.Utils;
using SFML.Graphics;
using System.Numerics;

namespace SfmlPresentation.Contracts
{
    public interface IColorProvider
    {
        Color GetColor(Vector3[] vertices, Vector3 light, IZBuffer zBuffer);
    }
}