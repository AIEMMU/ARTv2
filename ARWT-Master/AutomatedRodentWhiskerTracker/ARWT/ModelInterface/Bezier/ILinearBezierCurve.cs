using System.Drawing;

namespace ARWT.ModelInterface.Bezier
{
    public interface ILinearBezierCurve : IBezierBase
    {
        Point[] GenerateLinearBezierCurve(Point p0, Point p1);
    }
}
