using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARWT.ModelInterface.Smoothing;

namespace ARWT.Model.Smoothing
{
    internal class GaussianSmoothing : SmoothingBase, IGaussianSmoothing
    {
        private double[] _GaussianKernal = new double[] {0.006, 0.061, 0.242, 0.383, 0.242, 0.061, 0.006};
        
        private double[] GaussianKernal
        {
            get
            {
                return _GaussianKernal;
            }
        }
        public override string Name
        {
            get
            {
                return "Gaussian";
            }
        }

        public override double[] Smooth(double[] orignalvalues)
        {
            if (orignalvalues == null || !orignalvalues.Any())
            {
                return new double[] {};
            }

            //Extend values by original
            int halfKernal = (_GaussianKernal.Length - 1)/2;

            List<double> newValues = new List<double>();

            for (int i = 0; i < halfKernal; i++)
            {
                newValues.Add(orignalvalues[0]);
            }

            newValues.AddRange(orignalvalues);

            for (int i = 0; i < halfKernal; i++)
            {
                newValues.Add(orignalvalues.Last());
            }

            List<double> values = new List<double>();

            for (int i = halfKernal; i < newValues.Count - halfKernal; i++)
            {
                double value = (newValues[i - 3]*_GaussianKernal[0]) + (newValues[i - 2]*_GaussianKernal[1]) + (newValues[i - 1]*_GaussianKernal[2]) + (newValues[i]*_GaussianKernal[3]) + (newValues[i + 1]*_GaussianKernal[4]) + (newValues[i + 2]*_GaussianKernal[5]) + (newValues[i + 3]*_GaussianKernal[6]);
                values.Add(value);
            }

            return values.ToArray();
        }
    }
}
