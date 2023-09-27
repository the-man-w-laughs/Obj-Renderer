using Business.Contracts.Utils;
using SFML.Graphics;
using System.Numerics;

namespace SfmlPresentation.Contracts
{
    public interface IColorProvider
    {
        Vector3 Normal { set; }
        void SetNormal(Vector3 normal);
        void SetNormal(Vector3[] vertices);
        Color GetColor(Vector3 vector);
    }
}