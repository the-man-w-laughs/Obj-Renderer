using System.Numerics;

namespace Domain.ObjClass
{
    public class Obj
    {
        public List<Vector3> VertexList { get; } = new List<Vector3>();
        public List<Face> FaceList { get; } = new List<Face>();
        public List<Vector2> TextureList { get; } = new List<Vector2>();
        public Extent Size { get; } = new Extent();
        public string Mtl { get; set; }
    }
}
