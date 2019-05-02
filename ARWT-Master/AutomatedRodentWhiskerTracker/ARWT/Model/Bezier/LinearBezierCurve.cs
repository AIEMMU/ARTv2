using System.Collections.Generic;
using System.Drawing;
using ARWT.Extensions;
using ARWT.ModelInterface.Bezier;

namespace ARWT.Model.Bezier
{
    internal class LinearBezierCurve : BezierBase, ILinearBezierCurve
    {
        public Point[] GenerateLinearBezierCurve(Point p0, Point p1)
        {
            List<Point> bezierPoints = new List<Point>();

            //Generate apx value for delta t
            double dist = p0.Distance(p1);
            double deltaT = 1 / dist;

            for (double t = 0; t <= 1; t += deltaT)
            {
                System.Windows.Point bezierPoint = BezierPoint(p0, p1, t);
                Point intPoint = bezierPoint.ToDrawingPoint();

                if (!bezierPoints.Contains(intPoint))
                {
                    bezierPoints.Add(intPoint);
                }
            }

            return bezierPoints.ToArray();
        }

        private System.Windows.Point BezierPoint(Point p0, Point p1, double t)
        {
            double x = ((1 - t) * p0.X) + (t * p1.X);
            double y = ((1 - t) * p0.Y) + (t * p1.Y);

            return new System.Windows.Point(x, y);
        }

        public override Point[] GenerateBezierCurve(params Point[] points)
        {
            return GenerateLinearBezierCurve(points[0], points[1]);
        }
    }
}
