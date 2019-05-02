using System.Drawing;

namespace ARWT.ModelInterface.Bezier
{
    public interface IQuadraticBezierCurve : IBezierBase
    {
        Point[] GenerateQuadraticBezierCurve(Point p0, Point p1, Point p2);
    }
}
