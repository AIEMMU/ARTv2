using ARWT.ModelInterface.Feet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace ARWT.Foot.video_processing
{
    public class HelperFunctions
    {
        public static double getDistance(Point point1, Point point2)
        {
            double dx = Math.Pow((point2.X - point1.X), 2);
            double dy = Math.Pow((point2.Y - point1.Y), 2);

            return Math.Sqrt(dx + dy);
        }

        public static double getDistance(Point point1)
        {
            double dx = Math.Pow((point1.X), 2);
            double dy = Math.Pow((point1.Y), 2);

            return Math.Sqrt(dx + dy);
        }
        public Point checkLine(int x , int y, int cx, int cy, int width, int height)
        {
            if (x > width)
            {
                return lineIntersection(new Point(x, y), new Point(cx, cy), new Point(width, 0), new Point(width, height));
            }else if(x<0)
            {
                return lineIntersection(new Point(x, y), new Point(cx, cy), new Point(0, 0), new Point(0, height));
            }
            else if (y > height)
            {
                return lineIntersection(new Point(x, y), new Point(cx, cy), new Point(0, height), new Point(width, height));
            }
            else if (y< 0)
            {
                return lineIntersection(new Point(x, y), new Point(cx, cy), new Point(0, 0), new Point(width, 0));
            }
            else
            {
                return new Point(x, y);
            }
        }
        public static Point lineIntersection(Point lineA, Point LineB, Point line2A, Point Line2B)
        {
            Point xdiff = new Point((lineA.X - LineB.X), (line2A.X - Line2B.X));
            Point ydiff = new Point((lineA.Y - LineB.Y), (line2A.Y - Line2B.Y));

            double div = det(xdiff, ydiff);
            if (div == 0){
                return new Point(0,0); //might be wrong
            }
            Point d = new Point(det(lineA, LineB), det(line2A, Line2B));
            double x = det(d, xdiff) / div;
            double y = det(d,ydiff) / div;
            return new Point(Convert.ToInt32(x), Convert.ToInt32(y));
        }

        public static double getDistanceFromContour(Point footPoint, Point[] contour)
        {
            List<double> dist = new List<double>();
            foreach(var point in contour)
            {
                dist.Add(Math.Sqrt(Math.Pow((point.X - footPoint.X), 2)+ Math.Pow((point.Y - footPoint.Y), 2)));
            }
            return dist.Min();
        }
        public static List<List<double>> cDist(IFootPlacement[] prevFootPlacements, IFootPlacement[] footPlacements)
        {
            List<List<double>> distances = new List<List<double>>();
            for(int i = 0; i < prevFootPlacements.Length; i++)
            {
                List<double> dist = new List<double>();
                for(int j=0; j<footPlacements.Length; j++)
                {
                    dist.Add(Math.Sqrt(getCDistance(prevFootPlacements[i], footPlacements[j])));
                }
                distances.Add(dist);
            }
            return distances;
        }

        private static double getCDistance(IFootPlacement prevPlacement, IFootPlacement placement)
        {
            double deltaCentroidX = Math.Pow(prevPlacement.centroidX - placement.centroidX, 2);
            double deltaCentroidY = Math.Pow(prevPlacement.centroidY - placement.centroidY, 2);
            double deltaMinX = Math.Pow(prevPlacement.minX - placement.minX, 2);
            double deltaMinY = Math.Pow(prevPlacement.minY - placement.minY, 2);
            double deltaMaxX = Math.Pow(prevPlacement.maxX - placement.maxX, 2);
            double deltaMaxY = Math.Pow(prevPlacement.maxY - placement.maxY, 2);
            return deltaCentroidX + deltaCentroidY + deltaMinX + deltaMinY + deltaMaxX + deltaMaxY;
        }

        public static PointF getDistanceFromLine(Point footPoints, Point centroidPoint, Point headPoint)
        {
            Point a = new Point((headPoint.X - centroidPoint.X), (headPoint.Y - centroidPoint.Y));
            Point b = new Point((footPoints.X - centroidPoint.X), (footPoints.Y - centroidPoint.Y));
            double distancePerp = CrossProduct(a, b, new Point(0, 0)) / getDistance(a);
            double distPar = DotProduct(a,b) / getDistance(a);
            return new PointF(Convert.ToSingle(distancePerp), (Convert.ToSingle(distPar)));
        }

        private static double CrossProduct(Point p1, Point p2, Point p3)
        {
            return (p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y);
        }

        //need to check this
        private static int det(Point p1, Point p2)
        {
            return p1.X * p2.Y - p1.Y * p2.X;
        }
        private static double DotProduct(Point p1, Point p2)
        {
            return (p2.X * p1.X) + (p2.Y * p1.Y);
        }

        public  static int[] sortRows(List<List<double>> array)
        {
            List<int> idx = new List<int>();
            foreach(var item in array)
            {
                idx.Add(item.IndexOf(item.Min()));
            }
            return sortCols(array, idx.ToArray());
        }
        public static int[] sortIndexs(List<List<double>> array, int[] rows)
        {
            List<int> idx = new List<int>();
            for(int i =0; i< rows.Length; i++)
            {
                idx.Add(array[rows[i]].IndexOf(array[rows[i]].Min()));
            }

            return idx.ToArray();
        }
        public static int[] sortCols(List<List<double>> array, int[] rows)
        {
            List<double> values = new List<double>();
            for(int i =0; i < rows.Length; i++)
            {
                values.Add(array[i][rows[i]]);
            }
            var sorted = values
                .Select((x, i) => new KeyValuePair<double, int>(x, i))
                .OrderBy(x => x.Key)
                .ToList();
            return sorted.Select(x => x.Value).ToArray();

        }
    }
}
