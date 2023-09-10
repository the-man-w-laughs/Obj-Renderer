namespace Domain.ObjClass
{
    public class Extent
    {
        public float XMax { get; set; }
        public float XMin { get; set; }
        public float YMax { get; set; }
        public float YMin { get; set; }
        public float ZMax { get; set; }
        public float ZMin { get; set; }

        public float XSize { get { return XMax - XMin; } }
        public float YSize { get { return YMax - YMin; } }
        public float ZSize { get { return ZMax - ZMin; } }
    }
}
