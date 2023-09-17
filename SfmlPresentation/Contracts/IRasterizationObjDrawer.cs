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
        void Draw(List<Face> faces, List<Vector3> vertices, Image bitmap, Vector3 camera, Vector3 light);
    }
}
