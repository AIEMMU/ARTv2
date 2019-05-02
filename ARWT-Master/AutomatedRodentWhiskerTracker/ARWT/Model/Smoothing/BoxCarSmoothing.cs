using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARWT.ModelInterface.Smoothing;

namespace ARWT.Model.Smoothing
{
    internal class BoxCarSmoothing : SmoothingBase, IBoxCarSmoothing
    {
        public override string Name
        {
            get
            {
                return "Box Car";
            }
        }

        public override double[] Smooth(double[] orignalvalues)
        {
            double[] filter = new double[] { 0.2, 0.2, 0.2, 0.2, 0.2 };
            double[] result = new double[orignalvalues.Length];
            double[] borderedSignal = new double[orignalvalues.Length + 4];

            Console.Write(orignalvalues.Length);
            for (int i = -2; i < orignalvalues.Length + 2; i++)
            {
                if (i < 0)
                {
                    borderedSignal[i + 2] = orignalvalues[0];
                }
                else if (i >= orignalvalues.Length)
                {
                    borderedSignal[i + 2] = orignalvalues.Last();
                }
                else
                {
                    borderedSignal[i + 2] = orignalvalues[i];
                }
            }

            for (int i = 2; i < borderedSignal.Length - 2; i++)
            {
                //Start at 2, finish at length - 2
                result[i - 2] = (borderedSignal[i - 2] * filter[0]) + (borderedSignal[i - 1] * filter[1]) + (borderedSignal[i] * filter[2]) + (borderedSignal[i + 1] * filter[3]) + (borderedSignal[i + 2] * filter[4]);
            }

            return result;
        }
    }
}
