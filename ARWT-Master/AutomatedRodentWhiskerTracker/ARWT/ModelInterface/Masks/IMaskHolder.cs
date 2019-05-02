using System.Collections.Generic;
using System.Drawing;

namespace ARWT.ModelInterface.Masks
{
    public interface IMaskHolder : IModelObjectBase
    {
        List<Point[]> LeftPoints
        {
            get;
            set;
        }

        List<Point[]> RightPoints
        {
            get;
            set;
        }

        List<IMask> LeftMasks
        {
            get;
        }

        List<IMask> RightMasks
        {
            get;
        }

        List<ILine> LeftLines
        {
            get;
        }

        List<ILine> RightLines
        {
            get;
        }

        int MaskCount
        {
            get;
        }

        void AddMask(Point[] leftPoints, Point[] rightPoints, double lowerDist, double upperDist);
        void AddLine(Point[] leftPoints, Point[] rightPoints, double dist);
        int GetMaskIndexForDistance(double dist);
    }
}
