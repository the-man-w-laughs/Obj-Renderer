﻿using System.Numerics;

namespace Business.Contracts.Utils
{
    public interface IZBuffer
    {
        IPointCalculator PointCalculator { set; }
        uint Width { get; }
        uint Height { get; }
        float[,] Buffer { get; set; }

        bool TryPoint(Vector3 vector);
        bool SetPoint(uint x, uint y);
    }
}