using System.Drawing;

namespace ARWT.ModelInterface.Bezier
{
    public interface ICubicBezierCurve : IBezierBase
    {
        Point[] GenerateCubicBezierCurve(Point p0, Point p1, Point p2, Point p3);
    }
}
