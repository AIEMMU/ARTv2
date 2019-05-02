using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARWT.ModelInterface.Whiskers
{
    public interface IWhiskerCollection : IModelObjectBase
    {
        IWhiskerSegment[] LeftWhiskers
        {
            get;
            set;
        }
        IWhiskerSegment[] RightWhiskers
        {
            get;
            set;
        }
    }
}
