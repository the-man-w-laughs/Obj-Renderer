using Domain.ObjClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Domain.Scene
{
    public class ModelToDraw
    {
        public ModelToDraw(List<Face> faces, List<Vector3> allVertices)
        {
            Faces = new(allVertices.Count);            
            foreach (var face in faces)
            {                                
                var points = new Point[3];

                for (var i = 0; i < 3; i++)
                {                                        
                    var point = new Point();
                    point.Position = allVertices[face.VertexIndexList[i] - 1];
                    points[i] = point;
                }                
                Faces[] = points;
            };
        }
        public List<Point[]> Faces;
    }
}
