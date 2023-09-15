using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SfmlPresentation.Scene
{
    public class Camera
    {
        private double _alpha;
        private double _beta;
        private double _r;

        public Camera(double alpha, double beta, double r)
        {
            _alpha = alpha;
            _beta = beta;
            _r = r;
        }

        public Vector3 Eye
        {
            get
            {
                double x = R * Math.Sin(Alpha) * Math.Sin(Beta);
                double y = R * Math.Cos(Alpha);
                double z = R * Math.Sin(Alpha) * Math.Cos(Beta);

                return new Vector3((float)x, (float)y, (float)z);
            }
        }
        public double R
        {
            get { return _r; }
            set
            {
                if (value > 0 && value < 100)
                {
                    _r = value;
                }
            }
        }

        public double Alpha
        {
            get { return _alpha; }
            set
            {
                if (value >= 0 && value <= Math.PI)
                {
                    _alpha = value;
                }
            }
        }

        public double Beta
        {
            get { return _beta; }
            set
            {
                if (value >= 0 && value <= 2 * Math.PI)
                {
                    _beta = value;
                }
            }
        }

        public void ChangeAlpha(double delta)
        {
            Alpha += delta;
        }
        public void ChangeBeta(double delta)
        {
            _beta = (_beta + delta) % (2 * Math.PI);
            if (_beta < 0)
            {
                _beta += 2 * Math.PI;
            }
        }
    }
}
