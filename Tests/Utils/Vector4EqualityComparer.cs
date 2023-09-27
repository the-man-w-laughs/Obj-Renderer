using System;
using System.Collections.Generic;
using System.Numerics; // Import the Vector4 type if it's not already available

public class Vector4EqualityComparer : IEqualityComparer<Vector4>
{
    private readonly float _tolerance;

    public Vector4EqualityComparer(float tolerance = 1e-5f)
    {
        _tolerance = tolerance;
    }

    public bool Equals(Vector4 x, Vector4 y)
    {
        return
            FloatEqual(x.X, y.X, _tolerance) &&
            FloatEqual(x.Y, y.Y, _tolerance) &&
            FloatEqual(x.Z, y.Z, _tolerance) &&
            FloatEqual(x.W, y.W, _tolerance);
    }

    public int GetHashCode(Vector4 vector)
    {        
        return vector.X.GetHashCode() ^ vector.Y.GetHashCode() ^ vector.Z.GetHashCode() ^ vector.W.GetHashCode();
    }

    private bool FloatEqual(float a, float b, float tolerance)
    {
        return Math.Abs(a - b) < tolerance;
    }
}
