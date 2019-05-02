using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ArtLibrary.Classes;
using ArtLibrary.Extensions;
using ArtLibrary.Model.Events;
using ArtLibrary.ModelInterface.Results;
using ArtLibrary.Services.Mouse;
using ArtLibrary.Services.RBSK;
using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using ARWT.ModelInterface.Masks;
using ARWT.ModelInterface.NonMaxSuppression;
using ARWT.ModelInterface.RBSK2;
using ARWT.ModelInterface.Results;
using ARWT.ModelInterface.Whiskers;
using ARWT.Resolver;
using ARWT.View.Image;
using Point = System.Drawing.Point;

namespace ARWT.Model.Whiskers
{
    internal class WhiskerDetector : ModelObjectBase, IWhiskerDetector
    {
        //private IEnumerable<IWhiskerSegment> OpenImage3(string imageLoc, string binImageLoc, string frangiImageLoc, out double jsi, out double tp, string mwaFile2 = "", string externalImage = "", string externalImage2 = "", double scaleFactor = 1)
        //private double m_LineLength;
        //public double LineLength
        //{
        //    get
        //    {
        //        return m_LineLength;
        //    }
        //    set
        //    {
        //        if (Equals(m_LineLength, value))
        //        {
        //            return;
        //        }

        //        m_LineLength = value;

        //        MarkAsDirty();
        //        CreateLineDictionary();
        //    }
        //}

        private double m_OrientationResolution;
        public double OrientationResolution
        {
            get
            {
                return m_OrientationResolution;
            }
            set
            {
                if (Equals(m_OrientationResolution, value))
                {
                    return;
                }

                m_OrientationResolution = value;

                MarkAsDirty();
                CreateLineDictionary();
            }
        }


        private static Dictionary<double, Vector> m_OrientationTable;
        public static Dictionary<double, Vector> OrientationTable
        {
            get
            {
                return m_OrientationTable;
            }
            private set
            {
                if (Equals(m_OrientationTable, value))
                {
                    return;
                }

                m_OrientationTable = value;

                //MarkAsDirty();
            }
        }

        private void CreateLineDictionary()
        {
            if (OrientationResolution == 0 || SettingLineLength == 0)
            {
                return;
            }

            Dictionary<double, Vector> orientationTable = new Dictionary<double, Vector>();
            for (double orientation = 0; orientation < 180; orientation += OrientationResolution)
            {
                double x = SettingLineLength * Math.Cos(orientation * MathExtension.Deg2Rad);
                double y = SettingLineLength * Math.Sin(orientation * MathExtension.Deg2Rad);
                orientationTable.Add(orientation, new Vector(x, y));
            }

            OrientationTable = orientationTable;
        }

        int lowerLim = 7;
        int upperLim = 50;
        private int dilationLevel = 5;

        //double[] lineDistances = new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20};

        int SettingNonMaxSuprressionIntensity = 120;
        int SettingNonMaxSuprKernalSize = 1;
        //int SettingLineAverageIntensity = 150;
        int SettingLineLength = 12;
        int SettingLineThickness = 1;
        private int SettingMinBlobIntensity = 25;
        private int MinBlobArea = 3;
        //const double SettingOrientationResolution = 1;
        //int SettingLineAverageIntensityMax = 180;
        //int L1LowerBound = 7;
        //int L1UpperBound = 10;
        //int L2LowerBound = 36;
        //int L2UpperBound = 39;
        //int L3LowerBound = 56;
        //int L3UpperBound = 59;

        public List<IWhiskerSegment>[] FindWhiskers(Image<Gray, byte> grayImage, Image<Gray, byte> binaryBackground, PointF[] headPoints, Point[] bodyContour, int sizeRatio, ISingleFrameExtendedResults results, IWhiskerVideoSettings settings)
        {
            using (Image<Gray, byte> backgroundNot = binaryBackground.Not())
            using (Image<Gray, byte> dilateImg = grayImage.Dilate(dilationLevel))
            using (Image<Gray, byte> imgNot = grayImage.Not())
            using (Image<Gray, byte> dilateNot = dilateImg.Not())
            using (Image<Gray, Byte> finalImg = imgNot.Sub(dilateNot))
            using (Image<Gray, Byte> dest = new Image<Gray, byte>(finalImg.Size))
            {
                Image<Gray, byte> backgroundSub = null;

                using (Image<Gray, byte> bodyImage = backgroundNot.CopyBlank())
                {
                    bodyImage.Draw(bodyContour, new Gray(255), -1);

                    using (Image<Gray, byte> bodyDilate = bodyImage.Dilate(5))
                    using (Image<Gray, byte> t3 = backgroundNot.Add(bodyDilate))
                    {
                        backgroundSub = t3.Dilate(3);
                    }
                }

                double[] min, max;
                Point[] minLoc, maxLoc;
                finalImg.MinMax(out min, out max, out minLoc, out maxLoc);

                finalImg.SetValue(min[0], backgroundSub);
                CvInvoke.Normalize(finalImg, dest, 0, 255, NormType.MinMax);
                //ImageShower.ShowImage(dest);
                using (Image<Gray, float> floatImage = dest.Convert<Gray, float>())
                using (Image<Gray, float> frangiImage = floatImage.Frangi(1.37, 1.371, 0.5, 2.85, 3.67, false))
                using (Image<Gray, byte> frangiTz = frangiImage.Convert<Gray, byte>())
                using (Image<Gray, byte> frangiByte = frangiTz.ThresholdToZero(new Gray(150)))
                {
                    using (Image<Gray, byte> subImage = dest)
                    using (Image<Gray, byte> bSubImage = subImage.ThresholdBinary(new Gray(20), new Gray(255)))
                    using (Image<Gray, byte> bNotSubImage = bSubImage.Not())
                    using (Image<Gray, byte> maskAllLeft = new Image<Gray, byte>(bNotSubImage.Size))
                    using (Image<Gray, byte> maskAllRight = new Image<Gray, byte>(bNotSubImage.Size))
                    {
                        double headDistanceThreshold = 10 * sizeRatio;
                        int lBound = lowerLim * sizeRatio;
                        int uBound = upperLim * sizeRatio;
                        double[] maskDistances = new double[] { lBound, uBound };

                        List<double> lineDist = new List<double>();

                        for (int i = lBound; i < uBound; i += 1)
                        {
                            lineDist.Add(i);
                        }
                        
                        double[] lineDistances = lineDist.ToArray();

                        IMouseMask mouseMask = ModelResolver.Resolve<IMouseMask>();
                        
                        IMaskHolder maskHolder = mouseMask.GetMasks(bodyContour, headPoints[2], headPoints[0], headPoints[4], new System.Drawing.Size(grayImage.Width, grayImage.Height), headDistanceThreshold, maskDistances, lineDistances);
                        
                        if (maskHolder == null)
                        {
                            return null;
                        }
                        
                        List<ILine> gaborListLeft = new List<ILine>();
                        List<ILine> gaborListRight = new List<ILine>();

                        int count = maskHolder.LeftLines.Count;

                        for (int i = 0; i < count; i++)
                        {
                            ILine lineLeft;
                            ILine lineRight;

                            lineLeft = maskHolder.LeftLines[i];
                            lineRight = maskHolder.RightLines[i];

                            Image<Gray, byte> lineMaskLeft = bNotSubImage.CopyBlank();
                            Image<Gray, byte> lineMaskRight = bNotSubImage.CopyBlank();

                            lineMaskLeft.DrawPolyline(lineLeft.LinePoints, false, new Gray(255));
                            lineMaskRight.DrawPolyline(lineRight.LinePoints, false, new Gray(255));

                            lineLeft.MaskImage = lineMaskLeft;
                            lineRight.MaskImage = lineMaskRight;

                            lineLeft.Image = GetMask(frangiByte, lineLeft.MaskImage, backgroundSub);
                            lineRight.Image = GetMask(frangiByte, lineRight.MaskImage, backgroundSub);

                            gaborListLeft.Add(lineLeft);
                            gaborListRight.Add(lineRight);
                        }
                        
                        count = maskHolder.LeftMasks.Count;

                        for (int i = 0; i < count; i++)
                        {
                            Image<Gray, byte> leftMask = bNotSubImage.CopyBlank();
                            Image<Gray, byte> rightMask = bNotSubImage.CopyBlank();

                            IEnumerable<Point> lp1 = maskHolder.LeftMasks[i].MaskPoints;
                            IEnumerable<Point> rp1 = maskHolder.RightMasks[i].MaskPoints;

                            //Draw mask
                            leftMask.Draw(lp1.ToArray(), new Gray(255), -1);
                            rightMask.Draw(rp1.ToArray(), new Gray(255), -1);

                            maskHolder.LeftMasks[i].MaskImage = leftMask;
                            maskHolder.RightMasks[i].MaskImage = rightMask;

                            maskHolder.LeftMasks[i].Image = GetMask(frangiByte, maskHolder.LeftMasks[i].MaskImage, backgroundSub);
                            maskHolder.RightMasks[i].Image = GetMask(frangiByte, maskHolder.RightMasks[i].MaskImage, backgroundSub);
                        }
                        
                        //Draw mask
                        IEnumerable<Point> lp = maskHolder.LeftMasks.Last().MaskPoints;
                        IEnumerable<Point> rp = maskHolder.RightMasks.Last().MaskPoints;
                        
                        maskAllLeft.Draw(lp.ToArray(), new Gray(255), -1);
                        maskAllRight.Draw(rp.ToArray(), new Gray(255), -1);
                        Image<Gray, byte> imageAllLeft = GetMask(frangiByte, maskAllLeft, backgroundSub);
                        Image<Gray, byte> imageAllRight = GetMask(frangiByte, maskAllRight, backgroundSub);
                        
                        using (Image<Gray, byte> gfLeft = imageAllLeft.CopyBlank())
                        using (Image<Gray, byte> gfRight = imageAllLeft.CopyBlank())
                        {
                            object lockObject = new object();
                            
                            Parallel.ForEach(gaborListLeft, new ParallelOptions() { MaxDegreeOfParallelism = 8 }, lineLeft =>
                            {
                                using (Image<Gray, byte> gPointImg = GetGoodPoints(lineLeft, SettingNonMaxSuprressionIntensity, SettingNonMaxSuprKernalSize * sizeRatio, imageAllLeft))
                                {
                                    lock (lockObject)
                                    {
                                        gfLeft.SetValue(new Gray(255), gPointImg);
                                    }
                                }
                            });
                            
                            Parallel.ForEach(gaborListRight, new ParallelOptions() { MaxDegreeOfParallelism = 8 }, lineRight =>
                              {
                                  using (Image<Gray, byte> gPointImg = GetGoodPoints(lineRight, SettingNonMaxSuprressionIntensity, SettingNonMaxSuprKernalSize * sizeRatio, imageAllRight))
                                  {
                                      lock (lockObject)
                                      {
                                          gfRight.SetValue(new Gray(255), gPointImg);
                                      }
                                  }
                            });
                            
                            List<Point> seedPointsLeft = new List<Point>();
                            List<Point> seedPointsRight = new List<Point>();

                            List<Image<Gray, byte>> lineMasksLeft = new List<Image<Gray, byte>>();
                            List<Image<Gray, byte>> lineMasksRight = new List<Image<Gray, byte>>();

                            List<Point> innerListLeft = new List<Point>();

                            List<Point> innerListRight = new List<Point>();

                            List<Image<Gray, byte>> iListLeft = new List<Image<Gray, byte>>();

                            List<Image<Gray, byte>> iListRight = new List<Image<Gray, byte>>();

                            int lb1 = settings.LowerBound * sizeRatio;
                            int ub1 = settings.UpperBound * sizeRatio;
                            AddDistanceImages(lb1, ub1, iListLeft, gaborListLeft, gfLeft, iListRight, gaborListRight, gfRight, innerListLeft, innerListRight);
                            
                            using (Image<Gray, byte> iImageLeft = subImage.CopyBlank())
                            using (Image<Gray, byte> iImageRight = subImage.CopyBlank())
                            {
                                SetPoints(innerListLeft, iImageLeft);

                                SetPoints(innerListRight, iImageRight);

                                List<CvBlob> blobsL1 = new List<CvBlob>();
                                CvBlobs blobsL2 = new CvBlobs();
                                List<CvBlob> blobsR1 = new List<CvBlob>();
                                CvBlobs blobsR2 = new CvBlobs();

                                CvBlobDetector blobDetector = new CvBlobDetector();

                                blobDetector.Detect(iImageLeft, blobsL2);

                                blobDetector.Detect(iImageRight, blobsR2);

                                blobsL2.FilterByArea(MinBlobArea, int.MaxValue);

                                blobsR2.FilterByArea(MinBlobArea, int.MaxValue);

                                foreach (var blob in blobsL2)
                                {
                                    using (Image<Gray, byte> blobMask = dest.CopyBlank())
                                    {
                                        blobMask.Draw(blob.Value.GetContour(), new Gray(255), -1);
                                        Gray avg = dest.GetAverage(blobMask);

                                        if (avg.Intensity > SettingMinBlobIntensity)
                                        {
                                            blobsL1.Add(blob.Value);
                                        }
                                    }
                                }

                                foreach (var blob in blobsR2)
                                {
                                    using (Image<Gray, byte> blobMask = dest.CopyBlank())
                                    {
                                        blobMask.Draw(blob.Value.GetContour(), new Gray(255), -1);
                                        Gray avg = dest.GetAverage(blobMask);

                                        if (avg.Intensity > SettingMinBlobIntensity)
                                        {
                                            blobsR1.Add(blob.Value);
                                        }
                                    }
                                }

                                seedPointsLeft.AddRange(blobsL1.Select(blob => blob.BoundingBox.Center()));
                                seedPointsRight.AddRange(blobsR1.Select(blob => blob.BoundingBox.Center()));
                                
                                Tuple<LineSegment2D, double, double>[,] allLines = new Tuple<LineSegment2D, double, double>[imageAllLeft.Width, imageAllLeft.Height];
                                
                                List<IWhiskerSegment> leftWhiskers = FindBestResponseLines(seedPointsLeft, SettingLineThickness, imageAllLeft.Add(iImageLeft), settings.LineMinIntensity, allLines);
                                List<IWhiskerSegment> rightWhiskers = FindBestResponseLines(seedPointsRight, SettingLineThickness, imageAllRight.Add(iImageRight), settings.LineMinIntensity, allLines);
                                
                                foreach (var line in lineMasksLeft)
                                {
                                    line.Dispose();
                                }

                                foreach (var line in lineMasksRight)
                                {
                                    line.Dispose();
                                }

                                return new List<IWhiskerSegment>[] { leftWhiskers, rightWhiskers };
                            }
                        }
                    }
                }
            }
        }

        public List<IWhiskerSegment>[] FindWhiskers(Image<Gray, byte> grayImage, Image<Gray, byte> binaryBackground, PointF[] headPoints, Point[] bodyContour, int sizeRatio, ISingleFrameExtendedResults results, IWhiskerVideoSettings settings, IWhiskerSegment leftSeg, IWhiskerSegment rightSeg)
        {
            using (Image<Gray, byte> backgroundNot = binaryBackground.Not())
            using (Image<Gray, byte> dilateImg = grayImage.Dilate(dilationLevel))
            using (Image<Gray, byte> imgNot = grayImage.Not())
            using (Image<Gray, byte> dilateNot = dilateImg.Not())
            using (Image<Gray, Byte> finalImg = imgNot.Sub(dilateNot))
            using (Image<Gray, Byte> dest = new Image<Gray, byte>(finalImg.Size))
            {
                Image<Gray, byte> backgroundSub = null;

                using (Image<Gray, byte> bodyImage = backgroundNot.CopyBlank())
                {
                    bodyImage.Draw(bodyContour, new Gray(255), -1);

                    using (Image<Gray, byte> bodyDilate = bodyImage.Dilate(5))
                    using (Image<Gray, byte> t3 = backgroundNot.Add(bodyDilate))
                    {
                        backgroundSub = t3.Dilate(3);
                    }
                }

                double[] min, max;
                Point[] minLoc, maxLoc;
                finalImg.MinMax(out min, out max, out minLoc, out maxLoc);

                finalImg.SetValue(min[0], backgroundSub);
                CvInvoke.Normalize(finalImg, dest, 0, 255, NormType.MinMax);
                //ImageShower.ShowImage(dest);
                using (Image<Gray, float> floatImage = dest.Convert<Gray, float>())
                using (Image<Gray, float> frangiImage = floatImage.Frangi(1.37, 1.371, 0.5, 2.85, 3.67, false))
                using (Image<Gray, byte> frangiTz = frangiImage.Convert<Gray, byte>())
                using (Image<Gray, byte> frangiByte = frangiTz.ThresholdToZero(new Gray(150)))
                {
                    using (Image<Gray, byte> subImage = dest)
                    using (Image<Gray, byte> bSubImage = subImage.ThresholdBinary(new Gray(20), new Gray(255)))
                    using (Image<Gray, byte> bNotSubImage = bSubImage.Not())
                    using (Image<Gray, byte> maskAllLeft = new Image<Gray, byte>(bNotSubImage.Size))
                    using (Image<Gray, byte> maskAllRight = new Image<Gray, byte>(bNotSubImage.Size))
                    {
                        double headDistanceThreshold = 10 * sizeRatio;
                        int lBound = lowerLim * sizeRatio;
                        int uBound = upperLim * sizeRatio;
                        double[] maskDistances = new double[] { lBound, uBound };

                        List<double> lineDist = new List<double>();

                        for (int i = lBound; i < uBound; i += 1)
                        {
                            lineDist.Add(i);
                        }

                        double[] lineDistances = lineDist.ToArray();

                        IMouseMask mouseMask = ModelResolver.Resolve<IMouseMask>();

                        IMaskHolder maskHolder = mouseMask.GetMasks(bodyContour, headPoints[2], headPoints[0], headPoints[4], new System.Drawing.Size(grayImage.Width, grayImage.Height), headDistanceThreshold, maskDistances, lineDistances);

                        if (maskHolder == null)
                        {
                            return null;
                        }

                        List<ILine> gaborListLeft = new List<ILine>();
                        List<ILine> gaborListRight = new List<ILine>();

                        int count = maskHolder.LeftLines.Count;

                        for (int i = 0; i < count; i++)
                        {
                            ILine lineLeft;
                            ILine lineRight;

                            lineLeft = maskHolder.LeftLines[i];
                            lineRight = maskHolder.RightLines[i];

                            Image<Gray, byte> lineMaskLeft = bNotSubImage.CopyBlank();
                            Image<Gray, byte> lineMaskRight = bNotSubImage.CopyBlank();

                            lineMaskLeft.DrawPolyline(lineLeft.LinePoints, false, new Gray(255));
                            lineMaskRight.DrawPolyline(lineRight.LinePoints, false, new Gray(255));

                            lineLeft.MaskImage = lineMaskLeft;
                            lineRight.MaskImage = lineMaskRight;

                            lineLeft.Image = GetMask(frangiByte, lineLeft.MaskImage, backgroundSub);
                            lineRight.Image = GetMask(frangiByte, lineRight.MaskImage, backgroundSub);

                            gaborListLeft.Add(lineLeft);
                            gaborListRight.Add(lineRight);
                        }

                        count = maskHolder.LeftMasks.Count;

                        for (int i = 0; i < count; i++)
                        {
                            Image<Gray, byte> leftMask = bNotSubImage.CopyBlank();
                            Image<Gray, byte> rightMask = bNotSubImage.CopyBlank();

                            IEnumerable<Point> lp1 = maskHolder.LeftMasks[i].MaskPoints;
                            IEnumerable<Point> rp1 = maskHolder.RightMasks[i].MaskPoints;

                            //Draw mask
                            leftMask.Draw(lp1.ToArray(), new Gray(255), -1);
                            rightMask.Draw(rp1.ToArray(), new Gray(255), -1);

                            maskHolder.LeftMasks[i].MaskImage = leftMask;
                            maskHolder.RightMasks[i].MaskImage = rightMask;

                            maskHolder.LeftMasks[i].Image = GetMask(frangiByte, maskHolder.LeftMasks[i].MaskImage, backgroundSub);
                            maskHolder.RightMasks[i].Image = GetMask(frangiByte, maskHolder.RightMasks[i].MaskImage, backgroundSub);
                        }

                        //Draw mask
                        IEnumerable<Point> lp = maskHolder.LeftMasks.Last().MaskPoints;
                        IEnumerable<Point> rp = maskHolder.RightMasks.Last().MaskPoints;

                        maskAllLeft.Draw(lp.ToArray(), new Gray(255), -1);
                        maskAllRight.Draw(rp.ToArray(), new Gray(255), -1);
                        Image<Gray, byte> imageAllLeft = GetMask(frangiByte, maskAllLeft, backgroundSub);
                        Image<Gray, byte> imageAllRight = GetMask(frangiByte, maskAllRight, backgroundSub);

                        using (Image<Gray, byte> gfLeft = imageAllLeft.CopyBlank())
                        using (Image<Gray, byte> gfRight = imageAllLeft.CopyBlank())
                        {
                            object lockObject = new object();

                            Parallel.ForEach(gaborListLeft, new ParallelOptions() { MaxDegreeOfParallelism = 8 }, lineLeft =>
                            {
                                using (Image<Gray, byte> gPointImg = GetGoodPoints(lineLeft, SettingNonMaxSuprressionIntensity, SettingNonMaxSuprKernalSize * sizeRatio, imageAllLeft))
                                {
                                    lock (lockObject)
                                    {
                                        gfLeft.SetValue(new Gray(255), gPointImg);
                                    }
                                }
                            });

                            Parallel.ForEach(gaborListRight, new ParallelOptions() { MaxDegreeOfParallelism = 8 }, lineRight =>
                            {
                                using (Image<Gray, byte> gPointImg = GetGoodPoints(lineRight, SettingNonMaxSuprressionIntensity, SettingNonMaxSuprKernalSize * sizeRatio, imageAllRight))
                                {
                                    lock (lockObject)
                                    {
                                        gfRight.SetValue(new Gray(255), gPointImg);
                                    }
                                }
                            });

                            List<Point> seedPointsLeft = new List<Point>();
                            List<Point> seedPointsRight = new List<Point>();

                            List<Image<Gray, byte>> lineMasksLeft = new List<Image<Gray, byte>>();
                            List<Image<Gray, byte>> lineMasksRight = new List<Image<Gray, byte>>();

                            List<Point> innerListLeft = new List<Point>();

                            List<Point> innerListRight = new List<Point>();

                            List<Image<Gray, byte>> iListLeft = new List<Image<Gray, byte>>();

                            List<Image<Gray, byte>> iListRight = new List<Image<Gray, byte>>();

                            int lb1 = settings.LowerBound * sizeRatio;
                            int ub1 = settings.UpperBound * sizeRatio;
                            AddDistanceImages(lb1, ub1, iListLeft, gaborListLeft, gfLeft, iListRight, gaborListRight, gfRight, innerListLeft, innerListRight);

                            using (Image<Gray, byte> iImageLeft = subImage.CopyBlank())
                            using (Image<Gray, byte> iImageRight = subImage.CopyBlank())
                            {
                                SetPoints(innerListLeft, iImageLeft);

                                SetPoints(innerListRight, iImageRight);

                                List<CvBlob> blobsL1 = new List<CvBlob>();
                                CvBlobs blobsL2 = new CvBlobs();
                                List<CvBlob> blobsR1 = new List<CvBlob>();
                                CvBlobs blobsR2 = new CvBlobs();

                                CvBlobDetector blobDetector = new CvBlobDetector();

                                blobDetector.Detect(iImageLeft, blobsL2);

                                blobDetector.Detect(iImageRight, blobsR2);

                                blobsL2.FilterByArea(MinBlobArea, int.MaxValue);

                                blobsR2.FilterByArea(MinBlobArea, int.MaxValue);

                                foreach (var blob in blobsL2)
                                {
                                    using (Image<Gray, byte> blobMask = dest.CopyBlank())
                                    {
                                        blobMask.Draw(blob.Value.GetContour(), new Gray(255), -1);
                                        Gray avg = dest.GetAverage(blobMask);

                                        if (avg.Intensity > SettingMinBlobIntensity)
                                        {
                                            blobsL1.Add(blob.Value);
                                        }
                                    }
                                }

                                foreach (var blob in blobsR2)
                                {
                                    using (Image<Gray, byte> blobMask = dest.CopyBlank())
                                    {
                                        blobMask.Draw(blob.Value.GetContour(), new Gray(255), -1);
                                        Gray avg = dest.GetAverage(blobMask);

                                        if (avg.Intensity > SettingMinBlobIntensity)
                                        {
                                            blobsR1.Add(blob.Value);
                                        }
                                    }
                                }

                                seedPointsLeft.AddRange(blobsL1.Select(blob => blob.BoundingBox.Center()));
                                seedPointsRight.AddRange(blobsR1.Select(blob => blob.BoundingBox.Center()));

                                Tuple<LineSegment2D, double, double>[,] allLines = new Tuple<LineSegment2D, double, double>[imageAllLeft.Width, imageAllLeft.Height];

                                List<IWhiskerSegment> leftWhiskers = FindBestResponseLines(seedPointsLeft, SettingLineThickness, imageAllLeft.Add(iImageLeft), settings.LineMinIntensity, allLines, leftSeg);
                                List<IWhiskerSegment> rightWhiskers = FindBestResponseLines(seedPointsRight, SettingLineThickness, imageAllRight.Add(iImageRight), settings.LineMinIntensity, allLines, rightSeg);

                                foreach (var line in lineMasksLeft)
                                {
                                    line.Dispose();
                                }

                                foreach (var line in lineMasksRight)
                                {
                                    line.Dispose();
                                }

                                return new List<IWhiskerSegment>[] { leftWhiskers, rightWhiskers };
                            }
                        }
                    }
                }
            }
        }

        public void Debug(Image<Gray, byte> frame, Image<Gray, byte> backgroundImage, int videoWidth, int videoHeight, ISingleFrameExtendedResults currentFrame, string folderLoc, IWhiskerVideoSettings settings)
        {
            int newWidth = videoWidth / 8;
            int newHeight = videoHeight / 8;

            IWhiskerDetector whiskerDetector = ModelResolver.Resolve<IWhiskerDetector>();
            float scaleRatio = 2;
            //whiskerDetector.LineLength = SettingLineLength * (int) scaleRatio;
            whiskerDetector.OrientationResolution = 1;

            int x = (int) currentFrame.HeadPoints[2].X - (newWidth / 2);
            int y = (int) currentFrame.HeadPoints[2].Y - (newHeight / 2);
            int left = x + newWidth;
            int top = y + newHeight;

            if (x < 0)
            {
                x = 0;
            }

            if (y < 0)
            {
                y = 0;
            }

            if (left > videoWidth)
            {
                left = videoWidth;
            }

            if (top > videoHeight)
            {
                top = videoHeight;
            }

            Rectangle zoomRect = new Rectangle(x, y, left - x, top - y);
            

            Inter interpolationType = Inter.Area;
            
            frame.ROI = zoomRect;
            backgroundImage.ROI = zoomRect;
            

            using (Image<Gray, byte> subImage = frame.Copy())
            using (Image<Gray, byte> subBinary = backgroundImage.Copy())
            using (Image<Gray, byte> zoomedImage = subImage.Resize(scaleRatio, interpolationType))
            using (Image<Gray, byte> zoomedBinary = subBinary.Resize(scaleRatio, interpolationType))
            {
                PointF[] headPointsCorrected = new PointF[5];

                int bodyContourLength = currentFrame.BodyContour.Length;
                Point[] bodyContourCorrected = new Point[bodyContourLength];

                for (int j = 0; j < 5; j++)
                {
                    headPointsCorrected[j] = new PointF((currentFrame.HeadPoints[j].X - zoomRect.X) * scaleRatio, (currentFrame.HeadPoints[j].Y - zoomRect.Y) * scaleRatio);
                }

                for (int j = 0; j < bodyContourLength; j++)
                {
                    bodyContourCorrected[j] = new Point((int) ((currentFrame.BodyContour[j].X - zoomRect.X) * scaleRatio), (int) ((currentFrame.BodyContour[j].Y - zoomRect.Y) * scaleRatio));
                }

                ISingleFrameExtendedResults cFrame = ModelResolver.Resolve<ISingleFrameExtendedResults>();
                cFrame.Orientation = currentFrame.Orientation;//new Vector((currentFrame.Orientation.X - zoomRect.X) * scaleRatio, (currentFrame.Orientation.Y - zoomRect.Y) * scaleRatio);
                cFrame.MidPoint = new PointF((currentFrame.MidPoint.X - zoomRect.X) * scaleRatio, (currentFrame.MidPoint.Y - zoomRect.Y) * scaleRatio);

                whiskerDetector.FindWhiskersDebug(zoomedImage, zoomedBinary, headPointsCorrected, bodyContourCorrected, (int) scaleRatio, folderLoc, cFrame, settings, true);
            }
        }

        public List<IWhiskerSegment>[] FindWhiskersDebug(Image<Gray, byte> grayImage, Image<Gray, byte> binaryBackground, PointF[] headPoints, Point[] bodyContour, int sizeRatio, string folderLoc, ISingleFrameExtendedResults results, IWhiskerVideoSettings settings, bool showImages = true)
        {
            string newFolderLoc = folderLoc + "\\";
            bool timer = false;
            Stopwatch sw = new Stopwatch();

            using (Image<Gray, float> xS = grayImage.Sobel(1, 0, 5))
            using (Image<Gray, float> yS = grayImage.Sobel(0, 1, 5))
            using (Image<Gray, float> xSquared = xS.Mul(xS))
            using (Image<Gray, float> ySquared = yS.Mul(yS))
            using (Image<Gray, float> added = xSquared.Add(ySquared))
            using (Image<Gray, float> rooted = added.CopyBlank())
            using (Image<Gray, byte> backgroundNot = binaryBackground.Not())
            using (Image<Gray, byte> dilateImg = grayImage.Dilate(dilationLevel))
            using (Image<Gray, byte> imgNot = grayImage.Not())
            using (Image<Gray, byte> dilateNot = dilateImg.Not())
            using (Image<Gray, Byte> finalImg = imgNot.Sub(dilateNot))
            using (Image<Gray, Byte> dest2 = new Image<Gray, byte>(finalImg.Size))
            {
                Image<Gray, byte> dest = new Image<Gray, byte>(finalImg.Size);
                if (timer)
                {
                    sw.Stop();
                    Console.WriteLine("Initialisations: " + sw.ElapsedMilliseconds);
                    sw.Restart();
                }

                Image<Gray, byte> backgroundSub = null;

                using (Image<Gray, byte> bodyImage = backgroundNot.CopyBlank())
                {
                    bodyImage.Draw(bodyContour, new Gray(255), -1);

                    using (Image<Gray, byte> bodyDilate = bodyImage.Dilate(5))
                    using (Image<Gray, byte> t3 = backgroundNot.Add(bodyDilate))
                    {
                        backgroundSub = t3.Dilate(3);
                    }
                }

                //ImageViewer.Show(backgroundSub);

                grayImage.Save(newFolderLoc + "1-Gray.png");
                //grayImage.Not().Save(newFolderLoc + "1-Gray-Not.png");
                if (showImages)
                    ImageViewer.Show(dilateImg);
                dilateImg.Save(newFolderLoc + "2-Dilate.png");

                double[] min, max;
                Point[] minLoc, maxLoc;
                finalImg.MinMax(out min, out max, out minLoc, out maxLoc);

                finalImg.SetValue(min[0], backgroundSub);
                CvInvoke.Sqrt(added, rooted);
                CvInvoke.Normalize(finalImg, dest2, 0, 255, NormType.MinMax);
                //dest = dest.SmoothGaussian(3);

                

                if (showImages)
                    ImageViewer.Show(dest2);


                dest2.Save(newFolderLoc + "3-DilatedSubbed.png");
                using (Image<Gray, byte> sobel = rooted.Convert<Gray, byte>())
                using (Image<Gray, byte> sub = dest2.Sub(sobel))
                using (Image<Gray, byte> subTest = sub.CopyBlank())
                {
                    CvInvoke.Normalize(sub, subTest, 0, 255, NormType.MinMax);
                    //ImageViewer.Show(sobel);
                    //ImageViewer.Show(subTest);
                    dest = subTest.Clone();
                }



                if (timer)
                {
                    sw.Stop();
                    Console.WriteLine("CvInvoke: " + sw.ElapsedMilliseconds);
                    sw.Restart();
                }

                using (Image<Gray, float> floatImage = dest.Convert<Gray, float>())
                using (Image<Gray, float> frangiImage = floatImage.Frangi(1.37, 1.371, 0.5, 2.85, 3.67, false))
                using (Image<Gray, byte> frangiTz = frangiImage.Convert<Gray, byte>())
                using (Image<Gray, byte> frangiByte = frangiTz.ThresholdToZero(new Gray(150)))
                {
                    if (showImages)
                        ImageViewer.Show(frangiByte);
                    frangiByte.Save(newFolderLoc + "4-Frangi.png");
                    
                    using (Image<Gray, byte> subImage = dest)
                    using (Image<Gray, byte> bSubImage = subImage.ThresholdBinary(new Gray(20), new Gray(255)))
                    using (Image<Gray, byte> bNotSubImage = bSubImage.Not())
                    using (Image<Gray, byte> maskAll = new Image<Gray, byte>(bNotSubImage.Size))
                    using (Image<Gray, byte> maskAllLeft = new Image<Gray, byte>(bNotSubImage.Size))
                    using (Image<Gray, byte> maskAllRight = new Image<Gray, byte>(bNotSubImage.Size))
                    {
                        if (timer)
                        {
                            sw.Stop();
                            Console.WriteLine("Next usings: " + sw.ElapsedMilliseconds);
                            sw.Restart();
                        }


                        double headDistanceThreshold = 10 * sizeRatio;
                        int lBound = lowerLim * sizeRatio;
                        int uBound = upperLim * sizeRatio;
                        double[] maskDistances = new double[] { lBound, uBound };

                        List<double> lineDist = new List<double>();
                        //int lowerLim = 10*sizeRatio;
                        //int upperLim = 100*sizeRatio;

                        for (int i = lBound; i < uBound; i += 1)
                        {
                            lineDist.Add(i);
                        }

                        //double[] lineDistances = new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20};
                        double[] lineDistances = lineDist.ToArray();

                        IMouseMask mouseMask = ModelResolver.Resolve<IMouseMask>();
                        //Image<Bgr, byte> debugImg = subImage.Convert<Bgr, byte>();
                        //using (Image<Bgr, byte> tempImage = grayImage.Convert<Bgr, byte>())
                        //{
                        //    tempImage.DrawPolyline(bodyContour, true, new Bgr(Color.Yellow), 1);
                        //    tempImage.Draw(new CircleF(headPoints[2], 3), new Bgr(Color.Red), 2);
                        //    ImageViewer.Show(tempImage);
                        //}
                        if (timer)
                        {
                            sw.Stop();
                            Console.WriteLine("general: " + sw.ElapsedMilliseconds);
                            sw.Restart();
                        }
                        IMaskHolder maskHolder = mouseMask.GetMasks(bodyContour, headPoints[2], headPoints[0], headPoints[4], new System.Drawing.Size(grayImage.Width, grayImage.Height), headDistanceThreshold, maskDistances, lineDistances);
                        if (timer)
                        {
                            sw.Stop();
                            Console.WriteLine("mask holder: " + sw.ElapsedMilliseconds);
                            sw.Restart();
                        }
                        if (maskHolder == null)
                        {
                            return null;
                        }

                        //List<ILine> gaborList = new List<ILine>();
                        List<ILine> gaborListLeft = new List<ILine>();
                        List<ILine> gaborListRight = new List<ILine>();

                        int count = maskHolder.LeftLines.Count;

                        for (int i = 0; i < count; i++)
                        {
                            //ILine line = ModelResolver.Resolve<ILine>();
                            //line.LinePoints = maskHolder.LeftLines[i].LinePoints.Concat(maskHolder.RightLines[i].LinePoints.Reverse()).ToArray();
                            //line.Distance = maskHolder.LeftLines[i].Distance;

                            //Image<Gray, byte> lineMask = bNotSubImage.CopyBlank();
                            //lineMask.DrawPolyline(line.LinePoints, false, new Gray(255));

                            //line.MaskImage = lineMask;
                            //line.Image = GetMask(frangiByte, line.MaskImage, backgroundSub);
                            //gaborList.Add(line);

                            ILine lineLeft;
                            ILine lineRight;

                            lineLeft = maskHolder.LeftLines[i];
                            lineRight = maskHolder.RightLines[i];

                            Image<Gray, byte> lineMaskLeft = bNotSubImage.CopyBlank();
                            Image<Gray, byte> lineMaskRight = bNotSubImage.CopyBlank();

                            lineMaskLeft.DrawPolyline(lineLeft.LinePoints, false, new Gray(255));
                            lineMaskRight.DrawPolyline(lineRight.LinePoints, false, new Gray(255));

                            lineLeft.MaskImage = lineMaskLeft;
                            lineRight.MaskImage = lineMaskRight;

                            lineLeft.Image = GetMask(frangiByte, lineLeft.MaskImage, backgroundSub);
                            lineRight.Image = GetMask(frangiByte, lineRight.MaskImage, backgroundSub);

                            gaborListLeft.Add(lineLeft);
                            gaborListRight.Add(lineRight);
                        }

                        if (timer)
                        {
                            sw.Stop();
                            Console.WriteLine("mask stuff 1: " + sw.ElapsedMilliseconds);
                            sw.Restart();
                        }

                        count = maskHolder.LeftMasks.Count;

                        for (int i = 0; i < count; i++)
                        {
                            Image<Gray, byte> leftMask = bNotSubImage.CopyBlank();
                            Image<Gray, byte> rightMask = bNotSubImage.CopyBlank();

                            IEnumerable<Point> lp1 = maskHolder.LeftMasks[i].MaskPoints;
                            IEnumerable<Point> rp1 = maskHolder.RightMasks[i].MaskPoints;

                            //Draw mask
                            leftMask.Draw(lp1.ToArray(), new Gray(255), -1);
                            rightMask.Draw(rp1.ToArray(), new Gray(255), -1);

                            maskHolder.LeftMasks[i].MaskImage = leftMask;
                            maskHolder.RightMasks[i].MaskImage = rightMask;

                            maskHolder.LeftMasks[i].Image = GetMask(frangiByte, maskHolder.LeftMasks[i].MaskImage, backgroundSub);
                            maskHolder.RightMasks[i].Image = GetMask(frangiByte, maskHolder.RightMasks[i].MaskImage, backgroundSub);
                        }

                        if (timer)
                        {
                            sw.Stop();
                            Console.WriteLine("mask stuff 2: " + sw.ElapsedMilliseconds);
                            sw.Restart();
                        }

                        //Draw full mask
                        IEnumerable<Point> lp = maskHolder.LeftMasks.Last().MaskPoints;
                        IEnumerable<Point> rp = maskHolder.RightMasks.Last().MaskPoints;

                        maskAll.Draw(lp.ToArray(), new Gray(255), -1);
                        maskAll.Draw(rp.ToArray(), new Gray(255), -1);

                        using (Image<Gray, byte> gImage2 = grayImage.Add(maskAll))
                        {
                            gImage2.Save(newFolderLoc + "5-MaskAll.png");
                        }

                        maskAllLeft.Draw(lp.ToArray(), new Gray(255), -1);
                        maskAllRight.Draw(rp.ToArray(), new Gray(255), -1);

                        //Image<Gray, byte> imageAll = GetMask(frangiByte, maskAll, backgroundSub);
                        Image<Gray, byte> imageAllLeft = GetMask(frangiByte, maskAllLeft, backgroundSub);
                        Image<Gray, byte> imageAllRight = GetMask(frangiByte, maskAllRight, backgroundSub);

                        using (Image<Gray, byte> imageAll = imageAllLeft.Add(imageAllRight))
                        {
                            imageAll.Save(newFolderLoc + "6-FrangiMask.png");
                            if (showImages)
                                ImageViewer.Show(imageAll);
                        }

                        if (timer)
                        {
                            sw.Stop();
                            Console.WriteLine("mask stuff 3: " + sw.ElapsedMilliseconds);
                            sw.Restart();
                        }

                        //using (Image<Gray, byte> gf = imageAll.CopyBlank())
                        using (Image<Gray, byte> gfLeft = imageAllLeft.CopyBlank())
                        using (Image<Gray, byte> gfRight = imageAllLeft.CopyBlank())
                        {
                            //foreach (ILine line in gaborList)
                            //{
                            //    GetGoodPoints(line, SettingNonMaxSuprressionIntensity, SettingNonMaxSuprKernalSize, imageAll, gf);
                            //}

                            if (timer)
                            {
                                sw.Stop();
                                Console.WriteLine("using stuff 1: " + sw.ElapsedMilliseconds);
                                sw.Restart();
                            }

                            object lockObject = new object();
                            //Image<Gray, byte> maskImage = gfLeft.CopyBlank();
                            //Image<Gray, byte> tLeft = gfLeft.CopyBlank();
                            //foreach (ILine lineLeft in gaborListLeft)
                            Parallel.ForEach(gaborListLeft, new ParallelOptions() { MaxDegreeOfParallelism = 8 }, lineLeft =>
                            {
                                using (Image<Gray, byte> gPointImg = GetGoodPoints(lineLeft, SettingNonMaxSuprressionIntensity, SettingNonMaxSuprKernalSize, imageAllLeft))
                                {
                                    lock (lockObject)
                                    {
                                        gfLeft.SetValue(new Gray(255), gPointImg);
                                    }
                                }
                            });

                            //gfLeft.Add(tLeft);


                            //Image<Gray, byte> tRight = gfLeft.CopyBlank();
                            //foreach (ILine lineRight in gaborListRight)
                            Parallel.ForEach(gaborListRight, new ParallelOptions() { MaxDegreeOfParallelism = 8 }, lineRight =>
                            {
                                using (Image<Gray, byte> gPointImg = GetGoodPoints(lineRight, SettingNonMaxSuprressionIntensity, SettingNonMaxSuprKernalSize, imageAllRight))
                                {
                                    lock (lockObject)
                                    {
                                        gfRight.SetValue(new Gray(255), gPointImg);
                                    }
                                }
                                //GetGoodPoints(lineRight, SettingNonMaxSuprressionIntensity, SettingNonMaxSuprKernalSize, imageAllRight, gfRight);
                            });

                            //gfRight.Add(tRight);

                            if (timer)
                            {
                                sw.Stop();
                                Console.WriteLine("good points 1: " + sw.ElapsedMilliseconds);
                                sw.Restart();
                            }

                            List<Point> seedPointsLeft = new List<Point>();
                            List<Point> seedPointsRight = new List<Point>();

                            List<Image<Gray, byte>> lineMasksLeft = new List<Image<Gray, byte>>();
                            List<Image<Gray, byte>> lineMasksRight = new List<Image<Gray, byte>>();

                            List<Point> innerListLeft = new List<Point>();
                            //List<Point> outerListLeft = new List<Point>();
                            //List<Point> midListLeft = new List<Point>();

                            List<Point> innerListRight = new List<Point>();
                            //List<Point> outerListRight = new List<Point>();
                            //List<Point> midListRight = new List<Point>();

                            List<Image<Gray, byte>> iListLeft = new List<Image<Gray, byte>>();
                            //List<Image<Gray, byte>> mListLeft = new List<Image<Gray, byte>>();
                            //List<Image<Gray, byte>> oListLeft = new List<Image<Gray, byte>>();

                            List<Image<Gray, byte>> iListRight = new List<Image<Gray, byte>>();
                            //List<Image<Gray, byte>> mListRight = new List<Image<Gray, byte>>();
                            //List<Image<Gray, byte>> oListRight = new List<Image<Gray, byte>>();


                            int lb1 = settings.LowerBound * sizeRatio;
                            int ub1 = settings.UpperBound * sizeRatio;

                            using (Image<Gray, byte> imageAll = imageAllLeft.Add(imageAllRight))
                            {
                                imageAll.Save(newFolderLoc + "6-FrangiMask.png");
                                if (showImages)
                                    ImageViewer.Show(imageAll);

                                using (Image<Bgr, byte> gImage = imageAll.Convert<Bgr, byte>())
                                {

                                    for (int i = lb1; i < ub1; i++)
                                    {
                                        using (Image<Bgr, byte> gImage2 = imageAll.Convert<Bgr, byte>())
                                        {
                                            //ImageViewer.Show(gImage2);
                                            gImage2.SetValue(new Bgr(Color.Red), gaborListLeft[i].MaskImage);
                                            gImage2.SetValue(new Bgr(Color.Red), gaborListRight[i].MaskImage);
                                            //ImageViewer.Show(gImage2);
                                            //gImage2.Save(newFolderLoc + $"6(3)-{i}-Mask.png");

                                            gImage.SetValue(new Bgr(Color.Red), gaborListLeft[i].MaskImage);
                                            gImage.SetValue(new Bgr(Color.Red), gaborListRight[i].MaskImage);

                                        }
                                    }

                                    gImage.Save(newFolderLoc + "6(3)-Mask.png");
                                }
                            }

                            AddDistanceImages(lb1, ub1, iListLeft, gaborListLeft, gfLeft, iListRight, gaborListRight, gfRight, innerListLeft, innerListRight);

                            if (timer)
                            {
                                sw.Stop();
                                Console.WriteLine("add distance stuff 1: " + sw.ElapsedMilliseconds);
                                sw.Restart();
                            }

                            //lb1 = L2LowerBound*sizeRatio;
                            //ub1 = L2UpperBound*sizeRatio;
                            //AddDistanceImages(lb1, ub1, mListLeft, gaborListLeft, gfLeft, mListRight, gaborListRight, gfRight, midListLeft, midListRight);

                            //lb1 = L3LowerBound*sizeRatio;
                            //ub1 = L3UpperBound*sizeRatio;
                            //AddDistanceImages(lb1, ub1, oListLeft, gaborListLeft, gfLeft, oListRight, gaborListRight, gfRight, outerListLeft, outerListRight);

                            using (Image<Gray, byte> iImageLeft = subImage.CopyBlank())
                            //using (Image<Gray, byte> mImageLeft = subImage.CopyBlank())
                            //using (Image<Gray, byte> oImageLeft = subImage.CopyBlank())
                            using (Image<Gray, byte> iImageRight = subImage.CopyBlank())
                            //using (Image<Gray, byte> mImageRight = subImage.CopyBlank())
                            //using (Image<Gray, byte> oImageRight = subImage.CopyBlank())
                            using (Image<Gray, byte> tImage5 = frangiByte.Clone())
                            //using (Image<Bgr, byte> tImage4 = tImage5.Convert<Bgr, byte>())
                            {
                                SetPoints(innerListLeft, iImageLeft);
                                //SetPoints(midListLeft, mImageLeft);
                                //SetPoints(outerListLeft, oImageLeft);

                                SetPoints(innerListRight, iImageRight);
                                //SetPoints(midListRight, mImageRight);
                                //SetPoints(outerListRight, oImageRight);

                                using (Image<Gray, byte> iImage = iImageLeft.Add(iImageRight))
                                {
                                    iImage.Save(newFolderLoc + "7-iImage.png");
                                    if (showImages)
                                        ImageViewer.Show(iImage);
                                }

                                List<CvBlob> blobsL1 = new List<CvBlob>();
                                CvBlobs blobsL2 = new CvBlobs();
                                //CvBlobs blobsL3 = new CvBlobs();
                                List<CvBlob> blobsR1 = new List<CvBlob>();
                                CvBlobs blobsR2 = new CvBlobs();
                                //CvBlobs blobsR3 = new CvBlobs();

                                CvBlobDetector blobDetector = new CvBlobDetector();

                                blobDetector.Detect(iImageLeft, blobsL2);
                                //blobDetector.Detect(mImageLeft, blobsL2);
                                //blobDetector.Detect(oImageLeft, blobsL3);

                                blobDetector.Detect(iImageRight, blobsR2);
                                //blobDetector.Detect(mImageRight, blobsR2);
                                //blobDetector.Detect(oImageRight, blobsR3);

                                blobsL2.FilterByArea(MinBlobArea, int.MaxValue);
                                //blobsL2.FilterByArea(MinBlobArea, int.MaxValue);
                                //blobsL3.FilterByArea(MinBlobArea, int.MaxValue);

                                blobsR2.FilterByArea(MinBlobArea, int.MaxValue);
                                //blobsR2.FilterByArea(MinBlobArea, int.MaxValue);
                                //blobsR3.FilterByArea(MinBlobArea, int.MaxValue);

                                //byte[,,] destData = dest.Data;

                                foreach (var blob in blobsL2)
                                {
                                    using (Image<Gray, byte> blobMask = dest.CopyBlank())
                                    {
                                        blobMask.Draw(blob.Value.GetContour(), new Gray(255), -1);
                                        Gray avg = dest.GetAverage(blobMask);

                                        if (avg.Intensity > 25)
                                        {
                                            blobsL1.Add(blob.Value);
                                        }
                                    }
                                }

                                foreach (var blob in blobsR2)
                                {
                                    using (Image<Gray, byte> blobMask = dest.CopyBlank())
                                    {
                                        blobMask.Draw(blob.Value.GetContour(), new Gray(255), -1);
                                        Gray avg = dest.GetAverage(blobMask);

                                        if (avg.Intensity > SettingMinBlobIntensity)
                                        {
                                            blobsR1.Add(blob.Value);
                                        }
                                    }
                                }

                                seedPointsLeft.AddRange(blobsL1.Select(blob => blob.BoundingBox.Center()));
                                seedPointsRight.AddRange(blobsR1.Select(blob => blob.BoundingBox.Center()));


                                if (timer)
                                {
                                    sw.Stop();
                                    Console.WriteLine("blob stuff 1: " + sw.ElapsedMilliseconds);
                                    sw.Restart();
                                }

                                Tuple<LineSegment2D, double, double>[,] allLines = new Tuple<LineSegment2D, double, double>[imageAllLeft.Width, imageAllLeft.Height];
                                using (Image<Gray, byte> iImageAl = iImageRight.Add(iImageLeft))
                                using (Image<Bgr, byte> iImageAlC = iImageAl.Convert<Bgr, byte>())
                                using (Image<Gray, byte> imageAllGray = imageAllLeft.Add(imageAllRight))
                                using (Image<Bgr, byte> imageAll = imageAllGray.Convert<Bgr, byte>())
                                using (Image<Bgr, byte> destC = dest.Convert<Bgr, byte>())
                                using (Image<Bgr, byte> gC = grayImage.Convert<Bgr, byte>())
                                {
                                    if (showImages)
                                        ImageViewer.Show(imageAllGray.ThresholdToZero(new Gray(60)));
                                    foreach (var blob in blobsL1)
                                    {
                                        imageAll.Draw(blob.GetContour(), new Bgr(Color.Red));
                                        imageAll.Draw(new CircleF(blob.Centroid, 2), new Bgr(Color.Blue));
                                        iImageAlC.Draw(blob.GetContour(), new Bgr(Color.Red));
                                        iImageAlC.Draw(new CircleF(blob.Centroid, 2), new Bgr(Color.Blue));
                                        destC.Draw(new CircleF(blob.Centroid, 2), new Bgr(Color.Blue));
                                        gC.Draw(new CircleF(blob.Centroid, 2), new Bgr(Color.Blue));
                                    }

                                    foreach (var blob in blobsR1)
                                    {
                                        imageAll.Draw(blob.GetContour(), new Bgr(Color.Red));
                                        imageAll.Draw(new CircleF(blob.Centroid, 2), new Bgr(Color.Blue));
                                        iImageAlC.Draw(blob.GetContour(), new Bgr(Color.Red));
                                        iImageAlC.Draw(new CircleF(blob.Centroid, 2), new Bgr(Color.Blue));
                                        destC.Draw(new CircleF(blob.Centroid, 2), new Bgr(Color.Blue));
                                        gC.Draw(new CircleF(blob.Centroid, 2), new Bgr(Color.Blue));
                                    }
                                    imageAll.Save(newFolderLoc + "8-ImageAll.png");
                                    iImageAlC.Save(newFolderLoc + "9-ImageAlc.png");
                                    destC.Save(newFolderLoc + "10-destC.png");
                                    gC.Save(newFolderLoc + "11-gc.png");
                                    if (showImages)
                                    {
                                        ImageViewer.Show(imageAll);
                                        ImageViewer.Show(iImageAlC);
                                        ImageViewer.Show(destC);
                                        ImageViewer.Show(gC);
                                    }

                                }
                                if (showImages)
                                    ImageViewer.Show(imageAllRight.Add(iImageRight));
                                imageAllRight.Add(iImageRight).Save(newFolderLoc + "12-imageRightAll.png");
                                List<IWhiskerSegment> leftWhiskers = FindBestResponseLines(seedPointsLeft, SettingLineThickness, imageAllLeft, settings.LineMinIntensity, allLines);
                                List<IWhiskerSegment> rightWhiskers = FindBestResponseLines(seedPointsRight, SettingLineThickness, imageAllRight.Add(iImageRight), settings.LineMinIntensity, allLines);

                                if (timer)
                                {
                                    sw.Stop();
                                    Console.WriteLine("response stuff 1: " + sw.ElapsedMilliseconds);
                                    //sw.Restart();
                                }

                                foreach (var line in lineMasksLeft)
                                {
                                    line.Dispose();
                                }

                                foreach (var line in lineMasksRight)
                                {
                                    line.Dispose();
                                }

                                PointF midPoint = headPoints[1].MidPoint(headPoints[3]);

                                using (Image<Gray, byte> tIm = grayImage.Clone())
                                //using (Image<Bgr, byte> tIm2 = frangiByte.Convert<Bgr, byte>())
                                using (Image<Gray, byte> tIm3 = imageAllLeft.Add(imageAllRight))
                                using (Image<Bgr, byte> tIm2 = tIm3.Convert<Bgr, byte>())
                                {
                                    foreach (var whisker in leftWhiskers)
                                    {
                                        tIm.Draw(whisker.Line, new Gray(255), 1);
                                        tIm2.Draw(whisker.Line, new Bgr(Color.Red), SettingLineThickness);
                                    }

                                    foreach (var whisker in rightWhiskers)
                                    {
                                        tIm.Draw(whisker.Line, new Gray(255), 1);
                                        tIm2.Draw(whisker.Line, new Bgr(Color.Red), SettingLineThickness);
                                    }
                                    tIm.Draw(new CircleF(midPoint, 2), new Gray(255));
                                    tIm.Save(newFolderLoc + "13-tIm.png");
                                    tIm2.Save(newFolderLoc + "13-tIm(2).png");
                                    if (showImages)
                                        ImageViewer.Show(tIm);
                                }

                                RemoveDudWhiskers(results.MidPoint, leftWhiskers, results.Orientation, 15, true);
                                RemoveDudWhiskers(results.MidPoint, rightWhiskers, results.Orientation, 15, false);

                                using (Image<Gray, byte> tIm = grayImage.Clone())
                                //using (Image<Bgr, byte> tIm2 = frangiByte.Convert<Bgr, byte>())
                                using (Image<Gray, byte> tIm3 = imageAllLeft.Add(imageAllRight))
                                using (Image<Bgr, byte> tIm2 = tIm3.Convert<Bgr, byte>())
                                {
                                    foreach (var whisker in leftWhiskers)
                                    {
                                        tIm.Draw(whisker.Line, new Gray(255), 1);
                                        tIm2.Draw(whisker.Line, new Bgr(Color.Red), SettingLineThickness);
                                    }

                                    foreach (var whisker in rightWhiskers)
                                    {
                                        tIm.Draw(whisker.Line, new Gray(255), 1);
                                        tIm2.Draw(whisker.Line, new Bgr(Color.Red), SettingLineThickness);
                                    }
                                    tIm.Draw(new CircleF(midPoint, 2), new Gray(255));
                                    tIm.Save(newFolderLoc + "14-tIm.png");
                                    tIm2.Save(newFolderLoc + "14-tIm(2).png");
                                    if (showImages)
                                        ImageViewer.Show(tIm);
                                }

                                return new List<IWhiskerSegment>[] { leftWhiskers, rightWhiskers };
                            }
                        }
                    }
                }
            }
        }

        private static void SetPoints(IEnumerable<Point> list, Image<Gray, byte> image)
        {
            foreach (var point in list)
            {
                image[point] = new Gray(255);
            }
        }

        private Image<Gray, byte> GetMask(Image<Gray, byte> img, Image<Gray, byte> mask, Image<Gray, byte> b)
        {
            Image<Gray, byte> dest = img.CopyBlank();
            img.Copy(dest, mask);
            return dest;
        }

        private Image<Gray, byte> GetGoodPoints(ILine line, int settingNonMaxSuprressionIntensity, int settingNonMaxSuprKernalSize, Image<Gray, byte> imageAll)
        {
            //using (Image<Gray, byte> gtest1 = line.Image.Clone())
            Image<Gray, byte> gtest1 = line.Image.Clone();
            using (VectorOfPoint goodPoints = new VectorOfPoint())
            {
                CvInvoke.FindNonZero(gtest1, goodPoints);
                Point[] gPoints = goodPoints.ToArray();
                byte[,,] data = gtest1.Data;
                foreach (Point po in gPoints)
                {
                    if (data[po.Y, po.X, 0] < settingNonMaxSuprressionIntensity)
                    {
                        data[po.Y, po.X, 0] = 0;
                        continue;
                    }

                    int xStart = po.X - settingNonMaxSuprKernalSize;
                    int yStart = po.Y - settingNonMaxSuprKernalSize;
                    int xEnd = po.X + settingNonMaxSuprKernalSize;
                    int yEnd = po.Y + settingNonMaxSuprKernalSize;

                    Rectangle rect2 = new Rectangle(xStart, yStart, xEnd - xStart + 1, yEnd - yStart + 1);
                    if (rect2.X < 0)
                    {
                        rect2.X = 0;
                    }

                    if (rect2.Y < 0)
                    {
                        rect2.Y = 0;
                    }

                    int imgWidth = imageAll.Width;
                    if (rect2.Right >= imgWidth)
                    {
                        rect2.Width = imgWidth - rect2.X - 1;
                    }

                    int imageHeight = imageAll.Height;
                    if (rect2.Bottom >= imageHeight)
                    {
                        rect2.Height = imageHeight - rect2.Y - 1;
                    }

                    Image<Gray, byte> nmG = gtest1.GetSubRect(rect2);
                    //ImageViewer.Show(nmG);
                    INonMaximaSuppression nm = ModelResolver.Resolve<INonMaximaSuppression>();
                    nm.Apply(nmG, 2);
                    //ImageViewer.Show(nmG);
                }

                //gtest1 = gtest1.ThresholdBinary(new Gray(130), new Gray(255));
                //gf.SetValue(new Bgr(Color.Aqua), line.MaskImage);
                //using (Image<Gray, byte> gtest2 = gtest1.ThresholdBinary(new Gray(130), new Gray(255)))
                //{
                //gf.SetValue(new Bgr(Color.Red), gtest1);
                //gf.SetValue(new Gray(255), gtest1);
                return gtest1;
                //}
            }
        }

        private IEnumerable<Point> FindNonZero(IInputArray src)
        {
            using (VectorOfPoint goodPoints = new VectorOfPoint())
            {
                CvInvoke.FindNonZero(src, goodPoints);
                return goodPoints.ToArray().Distinct();
            }
        }

        private void AddDistanceImages(int lowerBound, int upperBound, List<Image<Gray, byte>> listLeft, List<ILine> gaborListLeft, Image<Gray, byte> gfLeft, List<Image<Gray, byte>> listRight, List<ILine> gaborListRight, Image<Gray, byte> gfRight, List<Point> midListLeft, List<Point> midListRight)
        {
            for (int i = lowerBound; i < upperBound; i++)
            {
                listLeft.Add(gaborListLeft[i].MaskImage.And(gfLeft));
                listRight.Add(gaborListRight[i].MaskImage.And(gfRight));
            }

            foreach (var line in listLeft)
            {
                midListLeft.AddRange(FindNonZero(line));
            }

            foreach (var line in listRight)
            {
                midListRight.AddRange(FindNonZero(line));
            }
        }

        private List<IWhiskerSegment> FindBestResponseLines(IEnumerable<Point> seedPoints, int lineThickness, Image<Gray, byte> image, double minimumAverageIntensity, Tuple<LineSegment2D, double, double>[,] output)
        {
            ConcurrentBag<IWhiskerSegment> segments = new ConcurrentBag<IWhiskerSegment>();
            foreach (Point seed in seedPoints)
            //Parallel.ForEach(seedPoints, new ParallelOptions() {MaxDegreeOfParallelism = 8}, seed =>
            {
                double bestAngle = -1;
                double bestAverage = -1;
                LineSegment2D? bestLine = null;

                for (double orientation = 0; orientation < 180; orientation += OrientationResolution)
                {
                    Vector vec = OrientationTable[orientation];
                    double x = vec.X;
                    double y = vec.Y;

                    Point lp1 = new Point((int) (seed.X + x), (int) (seed.Y + y));
                    Point lp2 = new Point((int) (seed.X - x), (int) (seed.Y - y));

                    LineSegment2D lineSeg = new LineSegment2D(lp1, lp2);
                    Gray avg;
                    using (Image<Gray, byte> lineMask = image.CopyBlank())
                    {
                        lineMask.Draw(lineSeg, new Gray(255), lineThickness);
                        avg = image.GetAverage(lineMask);
                    }

                    if (avg.Intensity < minimumAverageIntensity)
                    {
                        //Console.WriteLine(avg.Intensity);
                        continue;
                    }

                    if (avg.Intensity > bestAverage)
                    {
                        bestAverage = avg.Intensity;
                        bestAngle = orientation;
                        bestLine = lineSeg;

                        //if (bestAverage >= maxAvgIntensity)
                        //{
                        //    break;
                        //}
                    }
                }

                if (bestLine.HasValue)
                {
                    //testImg3.Draw(bestLine.Value, new Bgr(Color.Red), 1);
                    output[seed.X, seed.Y] = new Tuple<LineSegment2D, double, double>(bestLine.Value, bestAngle, bestAverage);
                    IWhiskerSegment segment = ModelResolver.Resolve<IWhiskerSegment>();
                    segment.X = seed.X;
                    segment.Y = seed.Y;
                    segment.Line = bestLine.Value;
                    segment.Angle = bestAngle;
                    segment.AvgIntensity = bestAverage;
                    segments.Add(segment);
                }
                else
                {
                    output[seed.X, seed.Y] = null;
                }
            }//);

            
            return segments.ToList();
        }

        private List<IWhiskerSegment> FindBestResponseLines(IEnumerable<Point> seedPoints, int lineThickness, Image<Gray, byte> image, double minimumAverageIntensity, Tuple<LineSegment2D, double, double>[,] output, IWhiskerSegment previousWhisker)
        {
            ConcurrentBag<IWhiskerSegment> segments = new ConcurrentBag<IWhiskerSegment>();
            foreach (Point seed in seedPoints)
            //Parallel.ForEach(seedPoints, new ParallelOptions() {MaxDegreeOfParallelism = 8}, seed =>
            {
                double bestAngle = -1;
                double bestAverage = -1;
                LineSegment2D? bestLine = null;

                for (double orientation = 0; orientation < 180; orientation += OrientationResolution)
                {
                    Vector vec = OrientationTable[orientation];
                    double x = vec.X;
                    double y = vec.Y;

                    Point lp1 = new Point((int)(seed.X + x), (int)(seed.Y + y));
                    Point lp2 = new Point((int)(seed.X - x), (int)(seed.Y - y));

                    LineSegment2D lineSeg = new LineSegment2D(lp1, lp2);
                    Gray avg;
                    using (Image<Gray, byte> lineMask = image.CopyBlank())
                    {
                        lineMask.Draw(lineSeg, new Gray(255), lineThickness);
                        avg = image.GetAverage(lineMask);
                    }

                    if (avg.Intensity < minimumAverageIntensity)
                    {
                        //Console.WriteLine(avg.Intensity);
                        continue;
                    }

                    if (avg.Intensity > bestAverage)
                    {
                        bestAverage = avg.Intensity;
                        bestAngle = orientation;
                        bestLine = lineSeg;

                        //if (bestAverage >= maxAvgIntensity)
                        //{
                        //    break;
                        //}
                    }
                }

                if (bestLine.HasValue)
                {
                    //testImg3.Draw(bestLine.Value, new Bgr(Color.Red), 1);
                    output[seed.X, seed.Y] = new Tuple<LineSegment2D, double, double>(bestLine.Value, bestAngle, bestAverage);
                    IWhiskerSegment segment = ModelResolver.Resolve<IWhiskerSegment>();
                    segment.X = seed.X;
                    segment.Y = seed.Y;
                    segment.Line = bestLine.Value;
                    segment.Angle = bestAngle;
                    segment.AvgIntensity = bestAverage;
                    segments.Add(segment);
                }
                else
                {
                    output[seed.X, seed.Y] = null;
                }
            }//);

           
            return segments.ToList();
        }

        public void RemoveDudWhiskers(PointF midPoint, List<IWhiskerSegment> whiskers, Vector orientation, double minAngleDelta, bool isLeft)
        {
            //foreach (var whisker in whiskers)
            //{
            //    Console.WriteLine(whisker.Angle);
            //}

            PointF headPoint = new PointF((float)(midPoint.X + orientation.X), (float)(midPoint.Y + orientation.Y));

            //Create vector 90 degrees to rotation
            Vector rotatedOrientation = Extensions.MathExtension.RotateByRightAngle(orientation);

            PointF basePoint2 = new PointF((float)(midPoint.X + rotatedOrientation.X), (float)(midPoint.Y + rotatedOrientation.Y));
            PointF headPoint2 = new PointF((float)(headPoint.X + rotatedOrientation.X), (float)(headPoint.Y + rotatedOrientation.Y));

            //Find side of line sign for base point
            PointSideVector baseSide = Extensions.MathExtension.FindSide(midPoint, basePoint2, headPoint);
            PointSideVector headSide = Extensions.MathExtension.FindSide(headPoint, headPoint2, midPoint);

            for (int i = whiskers.Count - 1; i >= 0; i--)
            {
                IWhiskerSegment cWhisker = whiskers[i];
                LineSegment2D line = cWhisker.Line;
                
                bool p1Closer = line.P1.DistanceSquared(headPoint) < line.P2.DistanceSquared(headPoint);

                Vector whiskerVec;// = new Vector();
                Point closestPoint;
                if (p1Closer)
                {
                    whiskerVec = new Vector(line.P2.X - line.P1.X, line.P2.Y - line.P1.Y);
                    closestPoint = line.P1;
                }
                else
                {
                    whiskerVec = new Vector(line.P1.X - line.P2.X, line.P1.Y - line.P2.Y);
                    closestPoint = line.P2;
                }

                PointSideVector whiskerSide = Extensions.MathExtension.FindSide(midPoint, basePoint2, closestPoint);

                if (whiskerSide != baseSide)
                {
                    whiskers.RemoveAt(i);
                    continue;
                }

                whiskerSide = Extensions.MathExtension.FindSide(headPoint, headPoint2, closestPoint);
                if (whiskerSide != headSide)
                {
                    whiskers.RemoveAt(i);
                    continue;
                }
                double angle = Vector.AngleBetween(orientation, whiskerVec);
                using (Image<Bgr, byte> img = new Image<Bgr, byte>(1024, 1024))
                {
                    img.Draw(cWhisker.Line, new Bgr(Color.Red), 1);
                    Point basePoint, farPoint;
                    if (p1Closer)
                    {
                        basePoint = cWhisker.Line.P1;
                        farPoint = cWhisker.Line.P2;
                    }
                    else
                    {
                        basePoint = cWhisker.Line.P2;
                        farPoint = cWhisker.Line.P1;
                    }
                    LineSegment2DF vecSeg = new LineSegment2DF(basePoint, new PointF((float)(basePoint.X + orientation.X), (float)(basePoint.Y + orientation.Y)));
                    LineSegment2DF vecSeg2 = new LineSegment2DF(midPoint, new PointF((float)(midPoint.X + orientation.X), (float)(midPoint.Y + orientation.Y)));
                    img.Draw(vecSeg, new Bgr(Color.Red), 1);
                    img.Draw(vecSeg2, new Bgr(Color.White), 1);
                    img.Draw(new CircleF(farPoint, 2), new Bgr(Color.LightBlue));
                    bool intersect = DoLinesIntersect(line, vecSeg2);

                    if (!intersect)
                    {
                        continue;
                    }
                    //Console.WriteLine(angle);
                    //ImageShower.ShowImage(img);
                }

                
                //double dot = (orientation.X*orientation.Y) + (whiskerVec.X*whiskerVec.Y);
                //double v1Mag = orientation.Length;
                //double v2Mag = whiskerVec.Length;
                //double cosAngle = dot/(v1Mag*v2Mag);
                //double angle2 = Math.Acos(cosAngle)* 57.2958;


                //Check if lines intersect
                //extend whisker line

                if (isLeft)
                {
                    angle *= -1;
                }

                if (angle < 0)
                {
                    whiskers.RemoveAt(i);
                    continue;
                }

                if (angle < 40 || angle > 140)
                {
                    whiskers.RemoveAt(i);
                    continue;
                }

                cWhisker.Angle = angle;

                //Get angle between whisker points
                Vector vec1 = new Vector(cWhisker.Line.P1.X - midPoint.X, cWhisker.Line.P1.Y - midPoint.Y);
                Vector vec2 = new Vector(cWhisker.Line.P2.X - midPoint.X, cWhisker.Line.P2.Y - midPoint.Y);

                double endAngles = Math.Abs(Vector.AngleBetween(vec1, vec2));

                //if (Math.Abs(endAngles) > 10)
                //{
                //    whiskers.RemoveAt(i);
                //    continue;
                //}

                //if (invertVec)
                //{
                //    vec1 *= -1;
                //}

                //Determine which point on line is closest to midpoint


                //double p1ToMidPointAngle = Math.Abs(Vector.AngleBetween(vec1, horiztonalVec));
                //double p2ToMidPointAngle = Vector.AngleBetween(vec2, horiztonalVec);

                //if (Math.Abs(p1ToMidPointAngle - cWhisker.Angle) > 30)
                //double angBetween = Vector.AngleBetween(vec1, whiskerVec);
                //if (Math.Abs(angBetween) > 35)
                //{
                //    whiskers.RemoveAt(i);
                //}
            }
        }

        public static bool DoLinesIntersect(LineSegment2D line1, LineSegment2DF line2)
        {
            return CrossProduct(line1.P1, line1.P2, line2.P1) !=
                   CrossProduct(line1.P1, line1.P2, line2.P2) ||
                   CrossProduct(line2.P1, line2.P2, line1.P1) !=
                   CrossProduct(line2.P1, line2.P2, line1.P2);
        }

        public static bool DoLinesIntersect(LineSegment2DF line1, LineSegment2DF line2)
        {
            return CrossProduct(line1.P1, line1.P2, line2.P1) !=
                   CrossProduct(line1.P1, line1.P2, line2.P2) ||
                   CrossProduct(line2.P1, line2.P2, line1.P1) !=
                   CrossProduct(line2.P1, line2.P2, line1.P2);
        }

        public static bool DoLinesIntersect(LineSegment2D line1, LineSegment2D line2)
        {
            return CrossProduct(line1.P1, line1.P2, line2.P1) !=
                   CrossProduct(line1.P1, line1.P2, line2.P2) ||
                   CrossProduct(line2.P1, line2.P2, line1.P1) !=
                   CrossProduct(line2.P1, line2.P2, line1.P2);
        }
        /// <summary>
        /// Finds the cross product of the 2 vectors created by the 3 vertices.
        /// Vector 1 = v1 -> v2, Vector 2 = v2 -> v3
        /// The vectors make a "right turn" if the sign of the cross product is negative.
        /// The vectors make a "left turn" if the sign of the cross product is positive.
        /// The vectors are colinear (on the same line) if the cross product is zero.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <param name="p3">Third point.</param>
        /// <returns>Cross product of the two vectors.</returns>
        public static double CrossProduct(PointF p1, PointF p2, PointF p3)
        {
            return (p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y);
        }

    }
}
