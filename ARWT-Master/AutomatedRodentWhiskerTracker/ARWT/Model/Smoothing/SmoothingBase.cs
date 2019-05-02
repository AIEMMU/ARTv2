using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARWT.ModelInterface.Smoothing;

namespace ARWT.Model.Smoothing
{
    internal abstract class SmoothingBase : ModelObjectBase, ISmoothingBase
    {
        public abstract string Name
        {
            get;
        }

        public abstract double[] Smooth(double[] orignalvalues);
    }
}
