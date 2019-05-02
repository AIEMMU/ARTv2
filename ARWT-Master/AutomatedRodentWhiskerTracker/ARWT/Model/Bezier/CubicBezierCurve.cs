using System;
using System.Collections.Generic;
using System.Drawing;
using ARWT.Extensions;
using ARWT.ModelInterface.Bezier;

namespace ARWT.Model.Bezier
{
    internal class CubicBezierCurve : BezierBase, ICubicBezierCurve
    {
        public Point[] GenerateCubicBezierCurve(Point p0, Point p1, Point p2, Point p3)
        {
            List<Point> bezierPoints = new List<Point>();

            //Generate apx value for delta t
            double dist = p0.Distance(p1);
            dist += p1.Distance(p2);
            dist += p2.Distance(p3);
            double deltaT = 1 / dist;

            for (double t = 0; t <= 1; t += deltaT)
            {
                System.Windows.Point bezierPoint = BezierPoint(p0, p1, p2, p3, t);
                Point intPoint = bezierPoint.ToDrawingPoint();

                if (!bezierPoints.Contains(intPoint))
                {
                    bezierPoints.Add(intPoint);
                }
            }

            return bezierPoints.ToArray();
        }

        private System.Windows.Point BezierPoint(Point p0, Point p1, Point p2, Point p3, double t)
        {
            double x = (Math.Pow(1 - t, 3) * p0.X) + (3 * Math.Pow(1 - t, 2) * t * p1.X) + (3 * (1 - t) * (Math.Pow(t, 2) * p2.X)) + (Math.Pow(t, 3) * p3.X);
            double y = (Math.Pow(1 - t, 3) * p0.Y) + (3 * Math.Pow(1 - t, 2) * t * p1.Y) + (3 * (1 - t) * (Math.Pow(t, 2) * p2.Y)) + (Math.Pow(t, 3) * p3.Y);

            return new System.Windows.Point(x, y);
        }

        public override Point[] GenerateBezierCurve(params Point[] points)
        {
            return GenerateCubicBezierCurve(points[0], points[1], points[2], points[3]);
        }
    }
}
