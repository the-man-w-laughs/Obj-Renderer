using Business.Contracts.Utils;
using SFML.Graphics;
using SfmlPresentation.Scene;
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
        void DrawFace(Image image, Vector3[] vertices, IColorProvider colorProvider, IZBuffer zBuffer);
    }
}
