using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SfmlPresentation.Contracts
{
    public interface IFaceDrawer
    {
        void DrawFace(Image image, Color color, Vector3[] vertices);
    }
}
