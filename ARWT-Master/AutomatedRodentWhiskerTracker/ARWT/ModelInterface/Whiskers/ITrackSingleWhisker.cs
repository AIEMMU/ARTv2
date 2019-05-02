using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARWT.ModelInterface.Whiskers
{
    public interface ITrackSingleWhisker : IModelObjectBase
    {
        Point Center
        {
            get;
        }
        
        double Angle
        {
            get;
        }

        Color Color
        {
            get;
            set;
        }

        IWhiskerSegment CurrentWhisker
        {
            get;
            set;
        }

        Dictionary<int, IWhiskerSegment> WhiskerList
        {
            get;
            set;
        }
        int MissingFrameCount
        {
            get;
            set;
        }

        int PositionId
        {
            get;
            set;
        }

        void Initialise(IWhiskerSegment whisker);

        void FindPotentialWhisker(int frameNumber, IEnumerable<IWhiskerSegment> whiskers);

        Dictionary<int, IWhiskerSegment> GetWhiskers();

        void ClearLists();
    }
}
