using System;
using System.Collections.Generic;
using System.Drawing;
using ARWT.Extensions;
using ARWT.ModelInterface.Bezier;

namespace ARWT.Model.Bezier
{
    internal class QuadraticBezierCurve : BezierBase, IQuadraticBezierCurve
    {
        public Point[] GenerateQuadraticBezierCurve(Point p0, Point p1, Point p2)
        {
            List<Point> bezierPoints = new List<Point>();

            //Generate apx value for delta t
            double dist = p0.Distance(p1);
            dist += p1.Distance(p2);
            double deltaT = 1/dist;

            for (double t = 0; t <= 1; t += deltaT)
            {
                System.Windows.Point bezierPoint = BezierPoint(p0, p1, p2, t);
                Point intPoint = bezierPoint.ToDrawingPoint();

                if (!bezierPoints.Contains(intPoint))
                {
                    bezierPoints.Add(intPoint);
                }
            }

            return bezierPoints.ToArray();
        }

        private System.Windows.Point BezierPoint(Point p0, Point p1, Point p2, double t)
        {
            double x = (Math.Pow(1 - t, 2) * p0.X) + (2 * (1 - t) * t * p1.X) + (Math.Pow(t, 2) * p2.X);
            double y = (Math.Pow(1 - t, 2) * p0.Y) + (2 * (1 - t) * t * p1.Y) + (Math.Pow(t, 2) * p2.Y);

            return new System.Windows.Point(x, y);
        }

        public override Point[] GenerateBezierCurve(params Point[] points)
        {
            return GenerateQuadraticBezierCurve(points[0], points[1], points[2]);
        }
    }
}
