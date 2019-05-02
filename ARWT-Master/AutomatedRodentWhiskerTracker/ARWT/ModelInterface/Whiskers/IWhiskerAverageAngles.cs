using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARWT.ModelInterface.Results;

namespace ARWT.ModelInterface.Whiskers
{
    public interface IWhiskerAverageAngles : IModelObjectBase
    {
        int StartFrame
        {
            get;
            set;
        }
        
        int EndFrame
        {
            get;
            set;
        }

        Dictionary<int, double>[] GetWhiskerAngles(Dictionary<int, ISingleFrameExtendedResults> results, bool onlyForInteracting = false);
        double[] GetWhiskerSpread(Dictionary<int, ISingleFrameExtendedResults> results, bool onlyForInteracting = false);
    }
}
