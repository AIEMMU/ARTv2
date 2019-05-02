using System.Collections.Generic;
using System.Drawing;

namespace ARWT.ModelInterface.Bezier
{
    public interface IBezierBase : IModelObjectBase
    {
        double ShortestDistanceFromBezierCurve(IEnumerable<Point> bezierPoints, Point currentPoint);
        Point[] GenerateBezierCurve(params Point[] points);
    }
}
