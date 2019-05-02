using System;
using System.Collections.Generic;
using System.Drawing;
using ARWT.Extensions;
using ARWT.ModelInterface.Bezier;

namespace ARWT.Model.Bezier
{
    internal abstract class BezierBase : ModelObjectBase, IBezierBase
    {
        public double ShortestDistanceFromBezierCurve(IEnumerable<Point> bezierPoints, Point currentPoint)
        {
            double minDist = double.MaxValue;

            foreach (Point point in bezierPoints)
            {
                double dist = point.DistanceSquared(currentPoint);
                if (dist < minDist)
                {
                    minDist = dist;
                }
            }

            return Math.Sqrt(minDist);
        }

        public abstract Point[] GenerateBezierCurve(params Point[] points);
    }
}
