using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARWT.ModelInterface.Analysis
{
    public interface IFrequency : IModelObjectBase
    {
        double[] Signal
        {
            get;
            set;
        }

        
        double FrameRate
        {
            get;
            set;
        }
        
        bool UseDft
        {
            get;
            set;
        }

        double GetFrequency(out double[] zeroed);
    }
}
