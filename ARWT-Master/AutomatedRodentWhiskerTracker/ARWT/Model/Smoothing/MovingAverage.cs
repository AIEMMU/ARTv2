using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARWT.ModelInterface.Smoothing;

namespace ARWT.Model.Smoothing
{
    internal class MovingAverage: SmoothingBase, iMovingAverage
    {
        public override string Name
        {
            get
            {
                return "Moving Average 5";
            }
        }

        public override double[] Smooth(double[] x)
        {
            int windowSize = 5;
            double[] ma = new double[x.Length];
            int period = windowSize;
            ma[0] = x[0];
            ma[x.Length - 1] = x.Last();
            for (int i = 1; i < x.Length - 1; i++)
            {
                if (i - (period / 2) < 0 || (i + (period / 2) > x.Length - 1))
                {
                    period = windowSize - 2;
                    if (i - (period / 2) < 0 || (i + (period / 2) > x.Length - 1))
                    {
                        period -= 2;
                        if (i - (period / 2) < 0 || (i + (period / 2) > x.Length - 1))
                        {
                            period -= 2;
                        }
                    }
                }
                double total = 0.0;
                for (int j = 0; j < period; j++)
                {
                    int meow = Convert.ToInt32(j - (period / 2));
                    total += x[i + (j - period / 2)];
                }
                ma[i] = total / period;
                period = windowSize;
            }

            return ma;

        }
    }

    internal class MovingAverage2 : SmoothingBase, iMovingAverage2
    {
        public override string Name
        {
            get
            {
                return "Moving Average 9";
            }
        }
        public override double[] Smooth(double[] x)
        {
            if (x.Length == 0)
            {
                return x;
            }
            int windowSize = 9;
            double[] ma = new double[x.Length];
            int period = windowSize;
            ma[0] = x[0];
            ma[x.Length - 1] = x.Last();
            for (int i = 1; i < x.Length - 1; i++)
            {
                if (i - (period / 2) < 0 || (i + (period / 2) > x.Length - 1))
                {
                    period = windowSize - 2;
                    if (i - (period / 2) < 0 || (i + (period / 2) > x.Length - 1))
                    {
                        period -= 2;
                        if (i - (period / 2) < 0 || (i + (period / 2) > x.Length - 1))
                        {
                            period -= 2;
                        }
                    }
                }
                double total = 0.0;
                for (int j = 0; j < period; j++)
                {
                    int meow = Convert.ToInt32(j - (period / 2));
                    total += x[i + (j - period / 2)];
                }
                ma[i] = total / period;
                period = windowSize;
            }

            return ma;
        }
    }
}
