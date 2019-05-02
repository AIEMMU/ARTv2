using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace ARWT.Extensions
{
    public static class ImageExtension
    {
        public static List<VectorOfPoint> GetContours(Image<Gray, Byte> image, ChainApproxMethod apxMethod = ChainApproxMethod.ChainApproxSimple, RetrType retrievalType = RetrType.List, double accuracy = 0.001d, double minimumArea = 10)
        {
            List<VectorOfPoint> convertedContours = new List<VectorOfPoint>();

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                using (Image<Gray, Byte> tempImage = image.Copy())
                {
                    CvInvoke.FindContours(tempImage, contours, null, retrievalType, apxMethod);
                }

                int count = contours.Size;
                for (int i = 0; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    {
                        VectorOfPoint approxContour = new VectorOfPoint();
                        CvInvoke.ApproxPolyDP(contour, approxContour, accuracy, false);
                        if (CvInvoke.ContourArea(approxContour, false) > minimumArea)
                        {
                            convertedContours.Add(approxContour);
                        }
                    }
                }
            }

            return convertedContours;
        }

        public static Image<Bgr, Byte> DrawContours(Image<Gray, Byte> image)
        {
            double something = 30;
            List<VectorOfPoint> convertedContours = GetContours(image, ChainApproxMethod.ChainApproxSimple, RetrType.List, 0.001, something);

            Image<Bgr, Byte> result = new Image<Bgr, Byte>(image.Width, image.Height, new Bgr(0, 0, 0));

            int counter = 1;
            Random randomGen = new Random();
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            
            foreach (VectorOfPoint points in convertedContours)
            {
                //KnownColor randomColorName = names[randomGen.Next(names.Length)];
                //Color randomColor = Color.FromKnownColor(randomColorName);
                Color randomColor = Color.Blue;
                Bgr color = new Bgr(randomColor);
                
                var temp = points.ToArray();
                result.Draw(temp, color, 2);

                counter++;
            }

            return result;
        }

        public static Image<Bgr, Byte> DrawContours(Image<Gray, Byte> image, Color color)
        {
            double something = 30;
            List<VectorOfPoint> convertedContours = GetContours(image, ChainApproxMethod.ChainApproxSimple, RetrType.List, 0.001, something);

            Image<Bgr, Byte> result = new Image<Bgr, Byte>(image.Width, image.Height, new Bgr(0, 0, 0));
            
            foreach (VectorOfPoint points in convertedContours)
            {
                Bgr bgrColor = new Bgr(color);

                var temp = points.ToArray();
                result.Draw(temp, bgrColor, 2);
            }

            return result;
        }

        public static Image<Bgr, Byte> DrawPoints(IEnumerable<PointF> points, Image<Bgr, Byte> overlay, float radius, bool copy = true)
        {
            Image<Bgr, Byte> result;
            if (copy)
            {
                result = overlay.Copy();
            }
            else
            {
                result = overlay;
            }

            foreach (var point in points)
            {
                result.Draw(new CircleF(new PointF(point.X, point.Y), radius), new Bgr(Color.Yellow), 1);
            }

            return result;
        }
        public static Image<Bgr, Byte> DrawPoints(IEnumerable<Point> points, Image<Bgr, Byte> overlay, bool copy = true)
        {
            Image<Bgr, Byte> result;
            if (copy)
            {
                result = overlay.Copy();
            }
            else
            {
                result = overlay;
            }

            foreach (var point in points)
            {
                result.Draw(new CircleF(new PointF(point.X, point.Y), 5.0f), new Bgr(Color.Yellow), 1);
            }

            return result;
        }

        public static Image<Bgr, Byte> DrawPoints(IEnumerable<PointF> points, Image<Bgr, Byte> overlay, bool copy = true)
        {
            Image<Bgr, Byte> result;
            if (copy)
            {
                result = overlay.Copy();
            }
            else
            {
                result = overlay;
            }

            foreach (var point in points)
            {
                result.Draw(new CircleF(new PointF(point.X, point.Y), 5.0f), new Bgr(Color.Yellow), 4);
            }

            return result;
        }

        public static List<Point> FindBoundingContourPoints(IEnumerable<Point> contours, Point[] boundingPoints)
        {
            if (boundingPoints.Length != 2)
            {
                return null;
            }

            List<Point> contourPoints = contours.ToList();
            List<Point> actualContourPointsList = new List<Point>();

            foreach (Point point in contourPoints)
            {
                if (!actualContourPointsList.Contains(point))
                {
                    actualContourPointsList.Add(point);
                }
                else
                {
                    break;
                }
            }

            Point firstPoint = boundingPoints[0];
            Point lastPoint = boundingPoints[1];
            
            int firstIndex = -1;
            int lastIndex  = -1;
            float p1Dist = 10000;
            float p2Dist = 10000;

            //Loop through all the points and find the closest points to the bounds
            for (int i = 0; i < actualContourPointsList.Count; i++)
            {
                float dist1 = firstPoint.Distance(actualContourPointsList[i]);
                float dist2 = lastPoint.Distance(actualContourPointsList[i]);

                if (dist1 < p1Dist)
                {
                    p1Dist = dist1;
                    //closestFirstPoint = contourPoints[i];
                    firstIndex = i;
                }

                if (dist2 < p2Dist)
                {
                    p2Dist = dist2;
                    //closestLastPoint = contourPoints[i];
                    lastIndex = i;
                }
            }

            List<Point> result = new List<Point>();

            //Return all the points that lie between first index and last index
            int start = firstIndex <= lastIndex ? firstIndex : lastIndex;
            int end = lastIndex >= firstIndex ? lastIndex : firstIndex;
            for (int j = start; j <= end; j++)
            {
                result.Add(actualContourPointsList[j]);
            }

            return result;
        }

        public static List<Point> FindBoundingContourPoints(IEnumerable<Point> contours, PointF[] boundingPoints)
        {
            if (boundingPoints.Length != 2)
            {
                return null;
            }

            List<Point> contourPoints = contours.ToList();
            List<Point> actualContourPointsList = new List<Point>();

            foreach (Point point in contourPoints)
            {
                if (!actualContourPointsList.Contains(point))
                {
                    actualContourPointsList.Add(point);
                }
                else
                {
                    break;
                }
            }

            PointF firstPoint = boundingPoints[0];
            PointF lastPoint = boundingPoints[1];

            int firstIndex = -1;
            int lastIndex = -1;
            double p1Dist = 10000;
            double p2Dist = 10000;

            //Loop through all the points and find the closest points to the bounds
            for (int i = 0; i < actualContourPointsList.Count; i++)
            {
                double dist1 = firstPoint.Distance(actualContourPointsList[i]);
                double dist2 = lastPoint.Distance(actualContourPointsList[i]);

                if (dist1 < p1Dist)
                {
                    p1Dist = dist1;
                    //closestFirstPoint = contourPoints[i];
                    firstIndex = i;
                }

                if (dist2 < p2Dist)
                {
                    p2Dist = dist2;
                    //closestLastPoint = contourPoints[i];
                    lastIndex = i;
                }
            }

            List<Point> result = new List<Point>();

            //Return all the points that lie between first index and last index
            int start = firstIndex <= lastIndex ? firstIndex : lastIndex;
            int end = lastIndex >= firstIndex ? lastIndex : firstIndex;
            for (int j = start; j <= end; j++)
            {
                result.Add(actualContourPointsList[j]);
            }

            return result;
        }
    }
}
