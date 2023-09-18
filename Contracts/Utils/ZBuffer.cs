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
        private uint width;
        private uint height;

        private IPointCalculator _pointCalculator;
        public IPointCalculator PointCalculator { set => _pointCalculator = value; }
        public ZBuffer(uint width, uint height)
        {
            this.width = width;
            this.height = height;
            this.buffer = new float[width, height];

            InitializeBuffer();
        }

        private void InitializeBuffer()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    buffer[x, y] = float.MaxValue;
                }
            }
        }

        public bool SetPoint(uint x, uint y)
        {
            var z = _pointCalculator.CalculatePointOnPlane(x, y).Z;
            if (x >= 0 && x < width && y >= 0 && y < height && z < buffer[x, y])
            {
                buffer[x, y] = z;
                return true;
            }
            return false;
        }
    }
}
