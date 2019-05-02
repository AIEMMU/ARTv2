using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using ARWT.Extensions;
using ARWT.ModelInterface.Masks;
using ARWT.Resolver;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace ARWT.Model.Masks
{
    internal class MouseMask : ModelObjectBase, IMouseMask
    {
        public Point[] ExtendPointsOutwards(Point[] points, Vector dir, double distance, int minX, int minY, int maxX, int maxY)
        {
            Point[] newPoints = new Point[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                Point point = points[i];
                int x = (int) (point.X + (dir.X*distance));
                int y = (int) (point.Y + (dir.Y*distance));

                if (x < minX)
                {
                    x = minX;
                }
                else if (x > maxX)
                {
                    x = maxX;
                }

                if (y < minY)
                {
                    y = minY;
                }
                else if (y > maxY)
                {
                    y = maxY;
                }

                newPoints[i] = new Point(x, y);
            }

            return newPoints;
        }

        public Point[] ExtendPointsOutwards(Point[] points, Point center, double distance, int minX, int minY, int maxX, int maxY)
        {
            Point[] newPoints = new Point[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                Point point = points[i];

                Vector dir = new Vector(point.X - center.X, point.Y - center.Y);
                int x = (int)(point.X + (dir.X * distance));
                int y = (int)(point.Y + (dir.Y * distance));

                if (x < minX)
                {
                    x = minX;
                }
                else if (x > maxX)
                {
                    x = maxX;
                }

                if (y < minY)
                {
                    y = minY;
                }
                else if (y > maxY)
                {
                    y = maxY;
                }

                newPoints[i] = new Point(x, y);
            }

            return newPoints;
        }

        public IMaskHolder GetMasks(Point[] body, PointF headPoint, PointF leftPoint, PointF rightPoint, Size maxSize, double headDistanceThreshold, double[] distances, double[] lineDistances, Image<Bgr, byte> debugImg)
        {
            IMaskHolder maskHolder = ModelResolver.Resolve<IMaskHolder>();
            PointF com = leftPoint.MidPoint(rightPoint);
            Vector forwardVec = new Vector(headPoint.X - com.X, headPoint.Y - com.Y);
            Vector leftVec = new Vector(forwardVec.Y, -forwardVec.X);
            Vector rightVec = new Vector(-forwardVec.Y, forwardVec.X);
            leftVec.Normalize();
            rightVec.Normalize();

            List<Point> leftPoints = new List<Point>(), rightPoints = new List<Point>();
            double headValue = MathExtension.GetSide(leftPoint, rightPoint, headPoint);

            foreach (Point point in body)
            {
                //Find if value is on same side as nose
                double pointSide = MathExtension.GetSide(leftPoint, rightPoint, point);

                if ((headValue < 0 && pointSide > 0) || (headValue > 0 && pointSide < 0))
                {
                    continue;
                }

                //Make sure value is far enough from nose tip
                //if (point.Distance(headPoint) < headDistanceThreshold)
                //{
                //    continue;
                //}

                //Find if value is on left or right
                double d = MathExtension.GetSide(com, headPoint, point);

                if (d > 0)
                {
                    leftPoints.Add(point);
                }
                else
                {
                    rightPoints.Add(point);
                }
            }
            
            maskHolder.LeftPoints = new List<Point[]>();
            maskHolder.RightPoints = new List<Point[]>();
            maskHolder.LeftPoints.Add(leftPoints.ToArray());
            maskHolder.RightPoints.Add(rightPoints.ToArray());

            if (leftPoints.Count == 0 || rightPoints.Count == 0)
            {
                return null;
            }
            
            ImageViewer.Show(debugImg);
            //Order them by closest to headPoint
            Point[] orderedLeftPoints = OrderPointsByBiggestGap(leftPoints, headPoint.ToPoint());
            Point[] orderedRightPoints = OrderPointsByBiggestGap(rightPoints, headPoint.ToPoint());
            debugImg.DrawPolyline(orderedRightPoints, false, new Bgr(Color.Red));
            debugImg.DrawPolyline(orderedLeftPoints, false, new Bgr(Color.Red));
            debugImg.Draw(new LineSegment2DF(com, headPoint), new Bgr(Color.Yellow), 2);
            ImageViewer.Show(debugImg);
            //We now have a list of the points that need to go left and points that need to go right
            for (int i = 1; i < distances.Length; i++)
            {
                double d1 = distances[i - 1];
                double d2 = distances[i];

                Point[] extendedLeftPoints1 = ExtendPointsOutwards(orderedLeftPoints, leftVec, d1, 0, 0, maxSize.Width, maxSize.Height).Where(x => x.Distance(headPoint) >= headDistanceThreshold).ToArray();
                Point[] extendedLeftPoints2 = ExtendPointsOutwards(orderedLeftPoints, leftVec, d2, 0, 0, maxSize.Width, maxSize.Height).Where(x => x.Distance(headPoint) >= headDistanceThreshold).ToArray();
                Point[] extendedRightPoints1 = ExtendPointsOutwards(orderedRightPoints, rightVec, d1, 0, 0, maxSize.Width, maxSize.Height).Where(x => x.Distance(headPoint) >= headDistanceThreshold).ToArray();
                Point[] extendedRightPoints2 = ExtendPointsOutwards(orderedRightPoints, rightVec, d2, 0, 0, maxSize.Width, maxSize.Height).Where(x => x.Distance(headPoint) >= headDistanceThreshold).ToArray();
                debugImg.DrawPolyline(extendedRightPoints1, false, new Bgr(Color.Yellow));
                debugImg.DrawPolyline(extendedRightPoints2, false, new Bgr(Color.Red));
                debugImg.DrawPolyline(extendedLeftPoints1, false, new Bgr(Color.Yellow));
                debugImg.DrawPolyline(extendedLeftPoints2, false, new Bgr(Color.Red));
                ImageViewer.Show(debugImg);
                //Generate curved portions
                double startAngle = Math.Atan2(leftVec.Y, leftVec.X);
                Point[] ePoints = GetCircularSegmentPoints(startAngle, Math.PI / 2, d1, 180, headPoint.ToPoint()).Reverse().ToArray();
                extendedLeftPoints1 = extendedLeftPoints1.Concat(ePoints.Reverse()).ToArray();

                ePoints = GetCircularSegmentPoints(startAngle, Math.PI / 2, d2, 180, headPoint.ToPoint()).Reverse().ToArray();
                extendedLeftPoints2 = extendedLeftPoints2.Concat(ePoints.Reverse()).ToArray();

                startAngle = Math.Atan2(rightVec.Y, rightVec.X);

                ePoints = GetCircularSegmentPoints(startAngle, -Math.PI / 2, d1, 180, headPoint.ToPoint()).Reverse().ToArray();
                extendedRightPoints1 = extendedRightPoints1.Concat(ePoints.Reverse()).ToArray();

                ePoints = GetCircularSegmentPoints(startAngle, -Math.PI / 2, d2, 180, headPoint.ToPoint()).Reverse().ToArray();
                extendedRightPoints2 = extendedRightPoints2.Concat(ePoints.Reverse()).ToArray();

                //Reverse the second set of points
                Point[] leftMask = extendedLeftPoints1.Concat(extendedLeftPoints2.Reverse()).ToArray();
                Point[] rightMask = extendedRightPoints1.Concat(extendedRightPoints2.Reverse()).ToArray();

                double lower = d1 < d2 ? d1 : d2;
                double upper = d1 < d2 ? d2 : d1;
                maskHolder.AddMask(leftMask, rightMask, lower, upper);
            }

            //Generate lines
            for (int i = 0; i < lineDistances.Length; i++)
            {
                double dist = lineDistances[i];

                Point[] extendedLeftPoints1 = ExtendPointsOutwards(orderedLeftPoints, leftVec, dist, 0, 0, maxSize.Width, maxSize.Height);
                Point[] extendedRightPoints1 = ExtendPointsOutwards(orderedRightPoints, rightVec, dist, 0, 0, maxSize.Width, maxSize.Height);
                maskHolder.LeftPoints.Add(extendedLeftPoints1);
                maskHolder.RightPoints.Add(extendedRightPoints1);

                //Generate curved portions
                double startAngle = Math.Atan2(leftVec.Y, leftVec.X);
                Point[] ePoints = GetCircularSegmentPoints(startAngle, Math.PI / 2, dist, 180, headPoint.ToPoint()).Reverse().ToArray();
                //maskHolder.LeftPoints.Add(ePoints);
                extendedLeftPoints1 = extendedLeftPoints1.Concat(ePoints.Reverse()).ToArray();

                startAngle = Math.Atan2(rightVec.Y, rightVec.X);

                ePoints = GetCircularSegmentPoints(startAngle, -Math.PI / 2, dist, 180, headPoint.ToPoint()).Reverse().ToArray();
                //maskHolder.RightPoints.Add(ePoints);
                extendedRightPoints1 = extendedRightPoints1.Concat(ePoints.Reverse()).ToArray();

                maskHolder.AddLine(extendedLeftPoints1, extendedRightPoints1, dist);
            }

            return maskHolder;
        }

        public IMaskHolder GetMasks(Point[] body, PointF headPoint, PointF leftPoint, PointF rightPoint, Size maxSize, double headDistanceThreshold, double[] distances, double[] lineDistances)
        {
            IMaskHolder maskHolder = ModelResolver.Resolve<IMaskHolder>();
            PointF com = leftPoint.MidPoint(rightPoint);
            Vector forwardVec = new Vector(headPoint.X - com.X, headPoint.Y - com.Y);
            Vector leftVec = new Vector(forwardVec.Y, -forwardVec.X);
            Vector rightVec = new Vector(-forwardVec.Y, forwardVec.X);
            leftVec.Normalize();
            rightVec.Normalize();

            List<Point> leftPoints = new List<Point>(), rightPoints = new List<Point>();
            double headValue = MathExtension.GetSide(leftPoint, rightPoint, headPoint);

            foreach (Point point in body)
            {
                //Find if value is on same side as nose
                double pointSide = MathExtension.GetSide(leftPoint, rightPoint, point);

                if ((headValue < 0 && pointSide > 0) || (headValue > 0 && pointSide < 0))
                {
                    continue;
                }

                //Make sure value is far enough from nose tip
                //if (point.Distance(headPoint) < headDistanceThreshold)
                //{
                //    continue;
                //}

                //Find if value is on left or right
                double d = MathExtension.GetSide(com, headPoint, point);

                if (d > 0)
                {
                    leftPoints.Add(point);
                }
                else
                {
                    rightPoints.Add(point);
                }
            }

            maskHolder.LeftPoints = new List<Point[]>();
            maskHolder.RightPoints = new List<Point[]>();
            maskHolder.LeftPoints.Add(leftPoints.ToArray());
            maskHolder.RightPoints.Add(rightPoints.ToArray());

            if (leftPoints.Count == 0 || rightPoints.Count == 0)
            {
                return null;
            }
            
            //Order them by closest to headPoint
            Point[] orderedLeftPoints = OrderPointsByBiggestGap(leftPoints, headPoint.ToPoint());
            Point[] orderedRightPoints = OrderPointsByBiggestGap(rightPoints, headPoint.ToPoint());
            
            //We now have a list of the points that need to go left and points that need to go right
            for (int i = 1; i < distances.Length; i++)
            {
                double d1 = distances[i - 1];
                double d2 = distances[i];

                Point[] extendedLeftPoints1 = ExtendPointsOutwards(orderedLeftPoints, leftVec, d1, 0, 0, maxSize.Width, maxSize.Height).Where(x => x.Distance(headPoint) >= headDistanceThreshold).ToArray();
                Point[] extendedLeftPoints2 = ExtendPointsOutwards(orderedLeftPoints, leftVec, d2, 0, 0, maxSize.Width, maxSize.Height).Where(x => x.Distance(headPoint) >= headDistanceThreshold).ToArray();
                Point[] extendedRightPoints1 = ExtendPointsOutwards(orderedRightPoints, rightVec, d1, 0, 0, maxSize.Width, maxSize.Height).Where(x => x.Distance(headPoint) >= headDistanceThreshold).ToArray();
                Point[] extendedRightPoints2 = ExtendPointsOutwards(orderedRightPoints, rightVec, d2, 0, 0, maxSize.Width, maxSize.Height).Where(x => x.Distance(headPoint) >= headDistanceThreshold).ToArray();
                
                //Generate curved portions
                double startAngle = Math.Atan2(leftVec.Y, leftVec.X);
                Point[] ePoints = GetCircularSegmentPoints(startAngle, Math.PI / 2, d1, 180, headPoint.ToPoint()).Reverse().ToArray();
                extendedLeftPoints1 = extendedLeftPoints1.Concat(ePoints.Reverse()).ToArray();

                ePoints = GetCircularSegmentPoints(startAngle, Math.PI / 2, d2, 180, headPoint.ToPoint()).Reverse().ToArray();
                extendedLeftPoints2 = extendedLeftPoints2.Concat(ePoints.Reverse()).ToArray();

                startAngle = Math.Atan2(rightVec.Y, rightVec.X);

                ePoints = GetCircularSegmentPoints(startAngle, -Math.PI / 2, d1, 180, headPoint.ToPoint()).Reverse().ToArray();
                extendedRightPoints1 = extendedRightPoints1.Concat(ePoints.Reverse()).ToArray();

                ePoints = GetCircularSegmentPoints(startAngle, -Math.PI / 2, d2, 180, headPoint.ToPoint()).Reverse().ToArray();
                extendedRightPoints2 = extendedRightPoints2.Concat(ePoints.Reverse()).ToArray();

                //Reverse the second set of points
                Point[] leftMask = extendedLeftPoints1.Concat(extendedLeftPoints2.Reverse()).ToArray();
                Point[] rightMask = extendedRightPoints1.Concat(extendedRightPoints2.Reverse()).ToArray();

                double lower = d1 < d2 ? d1 : d2;
                double upper = d1 < d2 ? d2 : d1;
                maskHolder.AddMask(leftMask, rightMask, lower, upper);
            }

            //Generate lines
            for (int i = 0; i < lineDistances.Length; i++)
            {
                double dist = lineDistances[i];

                Point[] extendedLeftPoints1 = ExtendPointsOutwards(orderedLeftPoints, leftVec, dist, 0, 0, maxSize.Width, maxSize.Height);
                Point[] extendedRightPoints1 = ExtendPointsOutwards(orderedRightPoints, rightVec, dist, 0, 0, maxSize.Width, maxSize.Height);
                maskHolder.LeftPoints.Add(extendedLeftPoints1);
                maskHolder.RightPoints.Add(extendedRightPoints1);

                //Generate curved portions
                double startAngle = Math.Atan2(leftVec.Y, leftVec.X);
                Point[] ePoints = GetCircularSegmentPoints(startAngle, Math.PI / 2, dist, 180, headPoint.ToPoint()).Reverse().ToArray();
                //maskHolder.LeftPoints.Add(ePoints);
                extendedLeftPoints1 = extendedLeftPoints1.Concat(ePoints.Reverse()).ToArray();

                startAngle = Math.Atan2(rightVec.Y, rightVec.X);

                ePoints = GetCircularSegmentPoints(startAngle, -Math.PI / 2, dist, 180, headPoint.ToPoint()).Reverse().ToArray();
                //maskHolder.RightPoints.Add(ePoints);
                extendedRightPoints1 = extendedRightPoints1.Concat(ePoints.Reverse()).ToArray();

                maskHolder.AddLine(extendedLeftPoints1, extendedRightPoints1, dist);
            }

            return maskHolder;
        }

        private Point[] OrderPointsByClosestDistanceToPoint(List<Point> points, PointF targetPoint)
        {
            double dist = double.MaxValue;
            int startIndex = -1;

            for (int i = 0; i < points.Count; i++)
            {
                double cDist = points[i].DistanceSquared(targetPoint);

                if (cDist < dist)
                {
                    dist = cDist;
                    startIndex = i;
                }
            }

            Point[] orderedPoints = new Point[points.Count];
            int counter = 0;
            int pointsCounter = startIndex;
            while (true)
            {
                orderedPoints[counter] = points[pointsCounter];
                counter++;
                pointsCounter++;

                if (pointsCounter >= points.Count)
                {
                    pointsCounter = 0;
                }

                if (pointsCounter == startIndex)
                {
                    break;
                }
            }

            return orderedPoints;
        }

        private Point[] OrderPointsByBiggestGap(List<Point> points, Point targetPoint)
        {
            Point[] finalPoints = new Point[points.Count];

            //Manually handle first and last
            int startIndex = 0;
            double biggestGap = points.First().DistanceSquared(points.Last());

            for (int i = 1; i < points.Count; i++)
            {
                Point prevPoint = points[i - 1];
                Point cPoint = points[i];

                double dist = prevPoint.DistanceSquared(cPoint);

                if (dist > biggestGap)
                {
                    biggestGap = dist;
                    startIndex = i;
                }
            }

            int cCounter = startIndex;
            int fCounter = 0;
            while (true)
            {
                finalPoints[fCounter] = points[cCounter];

                cCounter++;
                fCounter++;

                if (cCounter >= points.Count)
                {
                    cCounter = 0;
                }

                if (fCounter >= points.Count)
                {
                    break;
                }
            }

            if (!targetPoint.IsEmpty)
            {
                double d1 = finalPoints.First().DistanceSquared(targetPoint);
                double d2 = finalPoints.Last().DistanceSquared(targetPoint);

                if (d2 > d1)
                {
                    finalPoints = finalPoints.Reverse().ToArray();
                }
            }

            return finalPoints;
        }

        private Point[] OrderPointsByFurthestDistanceToPoint(List<Point> points, PointF targetPoint)
        {
            double dist = double.MinValue;
            int startIndex = -1;

            for (int i = 0; i < points.Count; i++)
            {
                double cDist = points[i].DistanceSquared(targetPoint);

                if (cDist > dist)
                {
                    dist = cDist;
                    startIndex = i;
                }
            }

            //See which direction we need to go in, check point either side, one should be a lot closer to the original point
            int lowerIndex = startIndex - 1;
            int upperIndex = startIndex + 1;

            if (lowerIndex < 0)
            {
                lowerIndex = points.Count + lowerIndex;
            }

            if (upperIndex >= points.Count)
            {
                upperIndex = upperIndex - points.Count;
            }

            Point lowerPoint = points[lowerIndex];
            Point upperPoint = points[upperIndex];

            double lowerDist = lowerPoint.DistanceSquared(points[startIndex]);
            double upperDist = upperPoint.DistanceSquared(points[startIndex]);

            if (lowerDist < upperDist)
            {
                points.Reverse();
                startIndex = points.Count - startIndex - 1;
            }

            Point[] orderedPoints = new Point[points.Count];
            int counter = 0;
            int pointsCounter = startIndex;
            while (true)
            {
                orderedPoints[counter] = points[pointsCounter];
                counter++;
                pointsCounter++;

                if (pointsCounter >= points.Count)
                {
                    pointsCounter = 0;
                }

                if (pointsCounter == startIndex)
                {
                    break;
                }
            }

            return orderedPoints;
        }

        private Point[] GetCircularSegmentPoints(double startAngle, double deltaAngle, double radius, int samples, Point centerPoint)
        {
            double resolution = deltaAngle / samples;
            int count = (int)Math.Abs(deltaAngle / resolution);
            Point[] points = new Point[count];

            double angle = startAngle;

            for (int i = 0; i < count; i++)
            {
                double x = (radius * Math.Cos(angle)) + centerPoint.X;
                double y = (radius * Math.Sin(angle)) + centerPoint.Y;
                points[i] = new Point((int)x, (int)y);

                angle += resolution;

                if (angle > Math.PI)
                {
                    angle -= 2 * Math.PI;
                }
            }

            return points.Distinct().ToArray();
        }
    }
}
