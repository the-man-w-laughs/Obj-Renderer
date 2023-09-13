using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Domain.ObjClass
{
    public class Obj
    {
        public List<Vector4> VertexList { get; } = new List<Vector4>();
        public List<Face> FaceList { get; } = new List<Face>();
        public List<Vector2> TextureList { get; } = new List<Vector2>();
        public string Mtl { get; set; }
        
        public Extent Size
        {
            get
            {
                var size = new Extent();

                if (VertexList.Count > 0)
                {
                    size.XMax = VertexList.Max(v => v.X);
                    size.XMin = VertexList.Min(v => v.X);
                    size.YMax = VertexList.Max(v => v.Y);
                    size.YMin = VertexList.Min(v => v.Y);
                    size.ZMax = VertexList.Max(v => v.Z);
                    size.ZMin = VertexList.Min(v => v.Z);
                }

                return size;
            }
        }
    }
}
