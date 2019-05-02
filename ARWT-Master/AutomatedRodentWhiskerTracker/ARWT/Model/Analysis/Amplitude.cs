using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARWT.Model.Analysis
{
    internal class Amplitude : ModelObjectBase
    {
        public double GetMaxAmpitude(double[] signal)
        {
            return signal.Max() - signal.Min();
        }
    }
}
