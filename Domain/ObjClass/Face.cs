using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ObjClass
{
    public class Face
    {
        public string UseMtl { get; set; }
        public int[] VertexIndexList { get; set; }
        public int[] TextureVertexIndexList { get; set; }
    }
}
