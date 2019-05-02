using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Structure;

namespace ARWT.ModelInterface.Whiskers
{
    public interface IWhiskerSegment : IModelObjectBase
    {
        int X
        {
            get;
            set;
        }

        int Y
        {
            get;
            set;
        }

        double Angle
        {
            get;
            set;
        }

        LineSegment2D Line
        {
            get;
            set;
        }

        Color Color
        {
            get;
            set;
        }

        double AvgIntensity
        {
            get;
            set;
        }

        double Distance(IWhiskerSegment whisker);
        double DistanceSquared(IWhiskerSegment whisker);
        double DeltaAngle(IWhiskerSegment whisker);
    }
}
