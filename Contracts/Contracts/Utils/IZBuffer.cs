namespace Business.Contracts.Utils
{
    public interface IZBuffer
    {
        IPointCalculator PointCalculator { set; }

        bool SetPoint(uint x, uint y);
    }
}