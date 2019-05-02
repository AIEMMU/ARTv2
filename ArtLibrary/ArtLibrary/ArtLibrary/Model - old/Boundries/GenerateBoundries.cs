﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.Model;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ArtLibrary.ModelInterface.Boundries;

namespace ArtLibrary.Model.Boundries
{
    internal class GenerateBoundries : ModelObjectBase, IGenerateBoundries
    {
        public void GetBoundries(Image<Gray, Byte> binaryBackground, out List<Point[]> boundries, out List<Point[]> artefacts, out List<RotatedRect> boxes)
        {
            //Find outer boundries
            double minimumContourArea = 250;
            double minimumBoundryArea = 1000;
            //double approximationFactor = 0.001;
            List<Point[]> allBoundries = new List<Point[]>();
            List<Point[]> allObjects = new List<Point[]>();
            List<RotatedRect> boxList = new List<RotatedRect>();
            using (Image<Gray, Byte> filteredBinary = binaryBackground.SmoothMedian(7))
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            using (Mat hierarchy = new Mat())
            {
                CvInvoke.FindContours(filteredBinary, contours, hierarchy, RetrType.Tree, ChainApproxMethod.ChainApproxNone);
                var temp = hierarchy.ToImage<Bgra, Byte>();

                int count = contours.Size;
                List<int> boundryIds = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    {
                        double contourArea = CvInvoke.ContourArea(contour);

                        if (contourArea >= minimumBoundryArea)
                        {
                            Bgra currentContour = temp[0, i];
                            if (currentContour.Alpha == 0)
                            {
                                allBoundries.Add(contour.ToArray());
                                boundryIds.Add(i);
                            }
                        }
                    }
                }

                for (int i = 0; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    {
                        double contourArea = CvInvoke.ContourArea(contour);

                        if (contourArea >= minimumContourArea)
                        {
                            Bgra currentContour = temp[0, i];

                            if (!boundryIds.Contains(i) && boundryIds.Contains((int)currentContour.Alpha))
                            {
                                bool isRectangle = true;
                                bool isCircle = false;
                                //Can the object be approximated as a circle or rectangle?
                                using (VectorOfPoint apxContour = new VectorOfPoint())
                                {
                                    double epsilon = CvInvoke.ArcLength(contour, true) * 0.05;
                                    CvInvoke.ApproxPolyDP(contour, apxContour, epsilon, true);

                                    if (apxContour.Size == 4) //The contour has 4 vertices.
                                    {

                                        Point[] pts = apxContour.ToArray();
                                        LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                                        for (int j = 0; j < edges.Length; j++)
                                        {
                                            double angle = Math.Abs(edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
                                            if (angle < 70 || angle > 110)
                                            {
                                                isRectangle = false;
                                                break;
                                            }
                                        }

                                        if (isRectangle) boxList.Add(CvInvoke.MinAreaRect(apxContour));
                                    }
                                    else
                                    {
                                        isRectangle = false;
                                    }
                                }

                                if (!isRectangle && !isCircle)
                                {
                                    allObjects.Add(contour.ToArray());
                                }
                            }
                        }
                    }
                }
            }

            //Find mouse
            //mousePoints = null;
            //using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            //{
            //    CvInvoke.FindContours(binaryMouse, contours, null, RetrType.External, ChainApproxMethod.ChainApproxNone);

            //    int count = contours.Size;
            //    double maxArea = 0;
            //    for (int j = 0; j < count; j++)
            //    {
            //        using (VectorOfPoint contour = contours[j])
            //        {
            //            double contourArea = CvInvoke.ContourArea(contour);
            //            if (contourArea >= maxArea)
            //            {
            //                maxArea = contourArea;
            //                mousePoints = contour.ToArray();
            //            }
            //        }
            //    }
            //}

            boundries = allBoundries;
            artefacts = allObjects;
            boxes = boxList;
            //Check if any contours can be approximated as shapes


            //We now have a list of boundries, if there's more than one it means something is sticking across the screen
            if (allBoundries.Count > 1)
            {
                //Need to find points from all boundries that are effectively parallel

            }

            //Image<Bgr, Byte> allContourImage = FirstFrame.Clone();

            //allContourImage.DrawPolyline(mousePoints, true, new Bgr(Color.Yellow), 2);
            //allContourImage.DrawPolyline(allBoundries.ToArray(), true, new Bgr(Color.Red), 2);
            //allContourImage.DrawPolyline(allObjects.ToArray(), true, new Bgr(Color.LightGreen), 2);
            //foreach (var box in boxList)
            //{
            //    allContourImage.Draw(box.GetVertices().Select(x => new Point((int)x.X, (int)x.Y)).ToArray(), new Bgr(Color.Aqua), 2);
            //}
        }
    }
}
