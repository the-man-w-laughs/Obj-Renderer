using Business.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SfmlPresentation.Utils.Buffer
{
    public class ZBuffer : IZBuffer
    {
        private float[,] buffer;
        public uint Width { get; }
        public uint Height { get; }

        private IPointCalculator _pointCalculator;
        public IPointCalculator PointCalculator { set => _pointCalculator = value; }
        public float[,] Buffer { get => buffer; set => buffer = value; }

        public ZBuffer(uint width, uint height)
        {
            this.Width = width;
            this.Height = height;
            this.Buffer = new float[width, height];

            InitializeBuffer();
        }

        private void InitializeBuffer()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Buffer[x, y] = float.MaxValue;
                }
            }
        }

        public bool SetPoint(uint x, uint y)
        {
            var z = _pointCalculator.CalculatePointOnPlane(x, y).Z;            
            if (x >= 0 && x < Width && y >= 0 && y < Height && z <= Buffer[x, y])
            {
                Buffer[x, y] = z;
                return true;
            }
            return false;
        }

        public bool TryPoint(Vector3 vector)
        {           
            if (vector.X >= 0 && vector.X < Width && vector.Y >= 0 && vector.Y < Height)
            {
                var bufferValue = Buffer[(uint)vector.X, (uint)vector.Y];
                float epsilon = 0.001f;                
                return vector.Z <= (bufferValue + epsilon);                
            }
            else
            {
                return true;
            }
        }
    }
}
