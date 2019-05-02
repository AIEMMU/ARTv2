using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARWT.ModelInterface.Whiskers
{
    public interface IWhiskerAllocator : IModelObjectBase
    {
        ITrackSingleWhisker[] InitialiseWhiskers(IEnumerable<IWhiskerSegment> currentWhiskers, PointF nosePoint, PointF midPoint);
        Dictionary<IWhiskerSegment, ITrackSingleWhisker> AllocateWhiskers(int frameNumber, IEnumerable<ITrackSingleWhisker> singleWhiskers, IEnumerable<IWhiskerSegment> currentWhiskers, PointF nosePoint, PointF midPoint);
    }
}
