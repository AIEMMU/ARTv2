using System;
using System.Windows;
using System.Windows.Media;

namespace ARWT.Extensions
{
    public static class DrawingUtility
    {
        // linear equation solver utility for ai + bj = c and di + ej = f
        private static void Solvexy(double a, double b, double c, double d, double e, double f, out double i, out double j)
        {
            j = (c - a / d * f) / (b - a * e / d);
            i = (c - (b * j)) / a;
        }

        // basis functions
        private static double B0(double t) { return Math.Pow(1 - t, 3); }
        private static double B1(double t) { return t * (1 - t) * (1 - t) * 3; }
        private static double B2(double t) { return (1 - t) * t * t * 3; }
        private static double B3(double t) { return Math.Pow(t, 3); }

        private static void Bez4pts1(double x0, double y0, double x4, double y4, double x5, double y5, double x3, double y3, out double x1, out double y1, out double x2, out double y2)
        {
            // find chord lengths
            double c1 = Math.Sqrt((x4 - x0) * (x4 - x0) + (y4 - y0) * (y4 - y0));
            double c2 = Math.Sqrt((x5 - x4) * (x5 - x4) + (y5 - y4) * (y5 - y4));
            double c3 = Math.Sqrt((x3 - x5) * (x3 - x5) + (y3 - y5) * (y3 - y5));
            // guess "best" t
            double t1 = c1 / (c1 + c2 + c3);
            double t2 = (c1 + c2) / (c1 + c2 + c3);
            // transform x1 and x2
            Solvexy(B1(t1), B2(t1), x4 - (x0 * B0(t1)) - (x3 * B3(t1)), B1(t2), B2(t2), x5 - (x0 * B0(t2)) - (x3 * B3(t2)), out x1, out x2);
            // transform y1 and y2
            Solvexy(B1(t1), B2(t1), y4 - (y0 * B0(t1)) - (y3 * B3(t1)), B1(t2), B2(t2), y5 - (y0 * B0(t2)) - (y3 * B3(t2)), out y1, out y2);
        }

        public static PathFigure BezierFromIntersection(Point startPt, Point int1, Point int2, Point endPt)
        {
            double x1, y1, x2, y2;
            Bez4pts1(startPt.X, startPt.Y, int1.X, int1.Y, int2.X, int2.Y, endPt.X, endPt.Y, out x1, out y1, out x2, out y2);
            PathFigure p = new PathFigure { StartPoint = startPt };
            p.Segments.Add(new BezierSegment { Point1 = new Point(x1, y1), Point2 = new Point(x2, y2), Point3 = endPt });
            return p;
        }

        public static void GetControlPointsForCubicBezier(Point[] points, out Point point1, out Point point2)
        {
            double x1, y1, x2, y2;
            Bez4pts1(points[0].X, points[0].Y, points[1].X, points[1].Y, points[2].X, points[2].Y, points[3].X, points[3].Y, out x1, out y1, out x2, out y2);

            point1 = new Point(x1, y1);
            point2 = new Point(x2, y2);
        }

        public static void GetControlPointsForQuadraticBezier(Point[] points, out Point point1)
        {
            double x1, y1;

            x1 = (2*points[1].X) - (points[0].X/2) - (points[2].X/2);
            y1 = (2*points[1].Y) - (points[0].Y/2) - (points[2].Y/2);
            //x1 = (points[1].X - (Math.Pow(0.5, 2) * points[0].X) + (Math.Pow(0.5, 2) * points[2].X)) / 0.5;
            //y1 = (points[1].Y - (Math.Pow(0.5, 2) * points[0].Y) + (Math.Pow(0.5, 2) * points[2].Y)) / 0.5;

            point1 = new Point(x1, y1);
        }

        public static double GetQuadraticBezierCurvature(Point[] points, double tValue)
        {
            if (points.Length != 3)
            {
                throw new Exception("Can't get quadratic bezier curvature without 3 points");
            }

            if (tValue < 0)
            {
                tValue = 0;
            }
            else if (tValue > 1)
            {
                tValue = 1;
            }

            Point firstOrderPoint = GetFirstOrderQuadraticDifferentialPoint(points, tValue);
            Point secondOrderPoint = GetSecondOrderQuadraticDifferentialPoint(points, tValue);

            double curvature = Math.Abs((firstOrderPoint.X*secondOrderPoint.Y) - (firstOrderPoint.Y*secondOrderPoint.X))/Math.Pow(Math.Pow(firstOrderPoint.X, 2) + Math.Pow(firstOrderPoint.Y, 2), 3/2);

            return curvature;
        }

        public static Point GetFirstOrderQuadraticDifferentialPoint(Point[] points, double tValue)
        {
            if (points.Length != 3)
            {
                throw new Exception("Need 3 points for quadratic beziers!");
            }

            Point p0 = points[0];
            Point p1 = points[1];
            Point p2 = points[2];

            double xFirstOrder = GetFirstOrderQuadraticDifferentialValue(new double[] { p0.X, p1.X, p2.X }, tValue);
            double yFirstOrder = GetFirstOrderQuadraticDifferentialValue(new double[] { p0.Y, p1.Y, p2.Y }, tValue);

            return new Point(xFirstOrder, yFirstOrder);
        }

        private static double GetFirstOrderQuadraticDifferentialValue(double[] points, double tValue)
        {
            double v0 = points[0];
            double v1 = points[1];
            double v2 = points[2];

            return (2*(1 - tValue)*(v1 - v0)) + (2*tValue*(v2 - v1));
        }

        private static Point GetSecondOrderQuadraticDifferentialPoint(Point[] points, double tValue)
        {
            if (points.Length != 3)
            {
                throw new Exception("Need 3 points for cubic beziers!");
            }

            Point p0 = points[0];
            Point p1 = points[1];
            Point p2 = points[2];

            double xSecondOrder = GetSecondOrderQuadraticDifferentialValue(new double[] { p0.X, p1.X, p2.X }, tValue);
            double ySecondOrder = GetSecondOrderQuadraticDifferentialValue(new double[] { p0.Y, p1.Y, p2.Y }, tValue);

            return new Point(xSecondOrder, ySecondOrder);
        }

        private static double GetSecondOrderQuadraticDifferentialValue(double[] points, double tValue)
        {
            double v0 = points[0];
            double v1 = points[1];
            double v2 = points[2];

            return 2*(v2 - (2*v1) + v0);
        }

        public static double GetCubicBezierCurvature(Point[] points, double tValue)
        {
            if (points.Length != 4)
            {
                throw new Exception("Can't get cubic bezier curvature without 4 points");
            }

            if (tValue < 0)
            {
                tValue = 0;
            }
            else if (tValue > 1)
            {
                tValue = 1;
            }

            Point firstOrderPoint = GetFirstOrderCubicDifferentialPoint(points, tValue);
            Point secondOrderPoint = GetSecondOrderCubicDifferentialPoint(points, tValue);

            double curvature = Math.Abs((firstOrderPoint.X*secondOrderPoint.Y) - (firstOrderPoint.Y*secondOrderPoint.X))/Math.Pow(Math.Pow(firstOrderPoint.X, 2) + Math.Pow(firstOrderPoint.Y, 2), 3/2);

            return curvature;
        }

        public static Point GetFirstOrderCubicDifferentialPoint(Point[] points, double tValue)
        {
            if (points.Length != 4)
            {
                throw new Exception("Need 4 points for cubic beziers!");
            }

            Point p0 = points[0];
            Point p1 = points[1];
            Point p2 = points[2];
            Point p3 = points[3];

            double xFirstOrder = GetFirstOrderCubicDifferentialValue(new double[] { p0.X, p1.X, p2.X, p3.X }, tValue);
            double yFirstOrder = GetFirstOrderCubicDifferentialValue(new double[] { p0.Y, p1.Y, p2.Y, p3.Y }, tValue);

            return new Point(xFirstOrder, yFirstOrder);
        }

        private static double GetFirstOrderCubicDifferentialValue(double[] points, double tValue)
        {
            double v0 = points[0];
            double v1 = points[1];
            double v2 = points[2];
            double v3 = points[3];

            return (3 * Math.Pow(1 - tValue, 2) * (v1 - v0)) + (6 * (1 - tValue) * tValue * (v2 - v1)) + (3 * Math.Pow(tValue, 2) * (v3 - v2));
        }

        private static Point GetSecondOrderCubicDifferentialPoint(Point[] points, double tValue)
        {
            if (points.Length != 4)
            {
                throw new Exception("Need 4 points for cubic beziers!");
            }

            Point p0 = points[0];
            Point p1 = points[1];
            Point p2 = points[2];
            Point p3 = points[3];

            double xSecondOrder = GetSecondOrderCubicDifferentialValue(new double[] { p0.X, p1.X, p2.X, p3.X }, tValue);
            double ySecondOrder = GetSecondOrderCubicDifferentialValue(new double[] { p0.Y, p1.Y, p2.Y, p3.Y }, tValue);

            return new Point(xSecondOrder, ySecondOrder);
        }

        private static double GetSecondOrderCubicDifferentialValue(double[] points, double tValue)
        {
            double v0 = points[0];
            double v1 = points[1];
            double v2 = points[2];
            double v3 = points[3];

            return (6 * (1 - tValue) * (v2 - (2 * v1) + v0)) + (6 * tValue * (v3 - (2 * v2) + v1));
        }

        public static PathGeometry GenerateBezierCurve(Point[] points)
        {
            PathFigure pathFigure = new PathFigure();
            PointCollection pointCollection = new PointCollection(points.Length);
            pathFigure.StartPoint = new Point(points[0].X, points[0].Y);

            PathGeometry myPathGeometry = new PathGeometry();
            PathSegment pathSegment;

            if (points.Length == 2)
            {
                pathSegment = new LineSegment();
                ((LineSegment)pathSegment).Point = new Point(points[1].X, points[1].Y);
            }
            else if (points.Length == 3)
            {
                pathSegment = new QuadraticBezierSegment();
                ((QuadraticBezierSegment)pathSegment).Point1 = new Point(points[1].X, points[1].Y);
                ((QuadraticBezierSegment)pathSegment).Point2 = new Point(points[2].X, points[2].Y);
            }
            else if (points.Length == 4)
            {
                for (int i = 1; i < points.Length; i++)
                {
                    pointCollection.Add(points[i]);
                }

                pathSegment = new PolyBezierSegment();
                ((PolyBezierSegment)pathSegment).Points = pointCollection;
            }
            else
            {
                return null;
            }

            PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
            pathSegmentCollection.Add(pathSegment);

            pathFigure.Segments = pathSegmentCollection;

            PathFigureCollection pathFigureCollection = new PathFigureCollection();
            pathFigureCollection.Add(pathFigure);


            myPathGeometry.Figures = pathFigureCollection;

            return myPathGeometry;
        }
    }
}
