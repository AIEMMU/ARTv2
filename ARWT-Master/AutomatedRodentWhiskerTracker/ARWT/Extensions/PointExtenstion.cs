using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using ArtLibrary.Classes;

namespace ARWT.Extensions
{
    public static class PointExtenstion
    {
        public static Point Center(this Rectangle rect)
        {
            return new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }

        public static int Area(this Rectangle rect)
        {
            return rect.Width * rect.Height;
        }

        public static int AbsoluteDistance(this Point point, Point p)
        {
            return Math.Abs(point.X - p.X) + Math.Abs(point.Y - p.Y);
        }

        public static float Distance(this Point point, Point p)
        {
            float xDiff = Math.Abs(point.X - p.X);
            float yDiff = Math.Abs(point.Y - p.Y);
            return (float)Math.Sqrt((xDiff * xDiff) + (yDiff * yDiff));
        }

        public static float Distance(this Point point, PointF p)
        {
            float xDiff = Math.Abs(point.X - p.X);
            float yDiff = Math.Abs(point.Y - p.Y);
            return (float)Math.Sqrt((xDiff * xDiff) + (yDiff * yDiff));
        }

        public static double Distance(this PointF point, Point p)
        {
            float xDiff = Math.Abs(point.X - p.X);
            float yDiff = Math.Abs(point.Y - p.Y);
            return Math.Sqrt((xDiff * xDiff) + (yDiff * yDiff));
        }

        public static double Distance(this PointF point, PointF p)
        {
            float xDiff = Math.Abs(point.X - p.X);
            float yDiff = Math.Abs(point.Y - p.Y);
            return Math.Sqrt((xDiff * xDiff) + (yDiff * yDiff));
        }

        public static float DistanceSquared(this Point point, Point p)
        {
            float xDiff = Math.Abs(point.X - p.X);
            float yDiff = Math.Abs(point.Y - p.Y);
            return (xDiff * xDiff) + (yDiff * yDiff);
        }

        public static float DistanceSquared(this Point point, PointF p)
        {
            float xDiff = Math.Abs(point.X - p.X);
            float yDiff = Math.Abs(point.Y - p.Y);
            return (xDiff * xDiff) + (yDiff * yDiff);
        }

        public static float DistanceSquared(this PointF point, Point p)
        {
            float xDiff = Math.Abs(point.X - p.X);
            float yDiff = Math.Abs(point.Y - p.Y);
            return (xDiff * xDiff) + (yDiff * yDiff);
        }

        public static float DistanceSquared(this PointF point, PointF p)
        {
            float xDiff = Math.Abs(point.X - p.X);
            float yDiff = Math.Abs(point.Y - p.Y);
            return (xDiff * xDiff) + (yDiff * yDiff);
        }

        public static double DistanceSquared(this System.Windows.Point point, System.Windows.Point p)
        {
            double xDiff = Math.Abs(point.X - p.X);
            double yDiff = Math.Abs(point.Y - p.Y);
            return (xDiff * xDiff) + (yDiff * yDiff);
        }

        public static double DistanceSquared(this System.Windows.Point point, PointF p)
        {
            double xDiff = Math.Abs(point.X - p.X);
            double yDiff = Math.Abs(point.Y - p.Y);
            return (xDiff * xDiff) + (yDiff * yDiff);
        }

        public static void NormalizePoints(Point[] points, Rectangle rectangle)
        {
            if (rectangle.Height == 0 || rectangle.Width == 0)
                return;

            Matrix m = new Matrix();
            m.Translate(rectangle.Center().X, rectangle.Center().Y);

            if (rectangle.Width > rectangle.Height)
                m.Scale(1, 1f * rectangle.Width / rectangle.Height);
            else
                m.Scale(1f * rectangle.Height / rectangle.Width, 1);

            m.Translate(-rectangle.Center().X, -rectangle.Center().Y);
            m.TransformPoints(points);
        }

        public static void NormalizePoints2(Point[] points, Rectangle rectangle, Rectangle needRectangle)
        {
            if (rectangle.Height == 0 || rectangle.Width == 0)
                return;

            float k1 = 1f * needRectangle.Width / rectangle.Width;
            float k2 = 1f * needRectangle.Height / rectangle.Height;
            float k = Math.Min(k1, k2);

            Matrix m = new Matrix();
            m.Scale(k, k);
            m.Translate(needRectangle.X / k - rectangle.X, needRectangle.Y / k - rectangle.Y);
            m.TransformPoints(points);
        }

        public static PointF Offset(this PointF p, float dx, float dy)
        {
            return new PointF(p.X + dx, p.Y + dy);
        }

        public static Point ToPoint(this PointF p)
        {
            return new Point(Round(p.X), Round(p.Y));
        }

        public static int Round(double d)
        {
            if (d < 0)
            {
                return (int)(d - 0.5);
            }
            return (int)(d + 0.5);
        }

        public static Point ToDrawingPoint(this System.Windows.Point point)
        {
            return new Point((int) point.X, (int) point.Y);
        }

        public static PointF ToPointF(this Point p)
        {
            return new PointF(p.X, p.Y);
        }

        public static Point MidPoint(this Point p, Point point)
        {
            return new Point((p.X + point.X) / 2, (p.Y + point.Y) / 2);
        }

        public static PointF MidPoint(this PointF p, PointF point)
        {
            return new PointF((p.X + point.X)/2, (p.Y + point.Y)/2);
        }

        public static Point DistanceFromStart(this StraightLine line, double distance)
        {
            double x = line.StartPoint.X + (distance * line.NormalizedVector).X;
            double y = line.StartPoint.Y + (distance * line.NormalizedVector).Y;

            return new Point((int)x, (int)y);
        }

        public static Point[] OrderPointsByDistance(List<Point> points)
        {
            int count = points.Count;
            Point[] finalPoints = new Point[count];
            double maxDist = -1;
            int startI = -1;
            for (int i = 1; i < count; i++)
            {
                double dist = points[i - 1].DistanceSquared(points[i]);
                if (dist > maxDist)
                {
                    maxDist = dist;
                    startI = i;
                }
            }

            double lastDist = points.First().DistanceSquared(points.Last());

            if (lastDist > maxDist)
            {
                return points.ToArray();
            }

            int j = 0;
            int counter = startI;
            while (true)
            {
                finalPoints[j] = points[counter];
                //orderedPoints.Add(points[counter]);
                counter++;
                j++;

                if (counter >= count)
                {
                    counter = 0;
                }

                if (counter == startI)
                {
                    break;
                }
            }

            return finalPoints;
        }

        public static Point[] ConcatOrderedPoints(Point[] p1, Point[] p2)
        {
            double dist1 = p2.First().Distance(p1.Last());
            double dist2 = p2.Last().Distance(p1.Last());

            if (dist2 < dist1)
            {
                return p1.Concat(p2.Reverse()).ToArray();
            }
            else
            {
                return p1.Concat(p2).ToArray();
            }
        }
    }
}
