using Domain.ObjClass;
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
    public interface IRasterizationObjDrawer
    {
        Camera Light { get; set; }
        void Draw(List<Face> faces, List<Vector3> verticesToDraw, Image bitmap, Camera light);
    }
}
