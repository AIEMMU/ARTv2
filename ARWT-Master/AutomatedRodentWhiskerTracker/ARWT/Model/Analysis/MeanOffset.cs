using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARWT.Model.Analysis
{
    internal class MeanOffset : ModelObjectBase
    {
        public double GetMeanOffset(double[] signal)
        {
            return signal.Average();
        }
    }
}
