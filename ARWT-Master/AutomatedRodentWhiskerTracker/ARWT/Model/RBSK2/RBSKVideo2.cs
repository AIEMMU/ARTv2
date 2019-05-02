using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using ArtLibrary.Model.Events;
using ArtLibrary.ModelInterface.BodyDetection;
using ArtLibrary.ModelInterface.Results;
using ArtLibrary.ModelInterface.Video;
using ArtLibrary.Services.Mouse;
using ArtLibrary.Services.RBSK;
using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Tracking;
using Emgu.CV.UI;
using Emgu.CV.Util;
using ARWT.Extensions;
using ARWT.ModelInterface.RBSK2;
using ARWT.ModelInterface.Results;
using ARWT.ModelInterface.Whiskers;
using ARWT.Resolver;
using Point = System.Drawing.Point;
using ARWT.ModelInterface.Feet;
using ARWT.Foot.video_processing;
using ARWT.Foot.centroidTracker;
using ARWT.Model.Feet;
using ARWT.Foot.dataset;

namespace ARWT.Model.RBSK2
{
    internal class RBSKVideo2 : ModelObjectBase, IRBSKVideo2
    {
        private IVideo m_Video;
        private Dictionary<int, ISingleFrameExtendedResults> m_HeadPoints;
        private Dictionary<int, Tuple<PointF[], double>> m_SecondPassHeadPoints;

        private double m_GapDistance;
        private bool m_Cancelled = false;
        private Image<Gray, Byte> m_BackgroundImage;
        private Rectangle m_Roi;

        public event RBSKVideoUpdateEventHandler ProgressUpdates;

        public IVideo Video
        {
            get
            {
                return m_Video;
            }
            set
            {
                if (Equals(m_Video, value))
                {
                    return;
                }

                m_Video = value;

                MarkAsDirty();
            }
        }

        public Dictionary<int, ISingleFrameExtendedResults> HeadPoints
        {
            get
            {
                return m_HeadPoints;
            }
            set
            {
                if (ReferenceEquals(m_HeadPoints, value))
                {
                    return;
                }

                m_HeadPoints = value;

                MarkAsDirty();
            }
        }

        public Dictionary<int, Tuple<PointF[], double>> SecondPassHeadPoints
        {
            get
            {
                return m_SecondPassHeadPoints;
            }
            set
            {
                if (ReferenceEquals(m_SecondPassHeadPoints, value))
                {
                    return;
                }

                m_SecondPassHeadPoints = value;

                MarkAsDirty();
            }
        }

        

        private Dictionary<int, Rectangle> m_AllRects;
        public Dictionary<int, Rectangle> AllRects
        {
            get
            {
                return m_AllRects;
            }
            set
            {
                if (Equals(m_AllRects, value))
                {
                    return;
                }

                m_AllRects = value;

                MarkAsDirty();
            }
        }


        public double GapDistance
        {
            get
            {
                return m_GapDistance;
            }
            set
            {
                if (Equals(m_GapDistance, value))
                {
                    return;
                }

                m_GapDistance = value;

                MarkAsDirty();
            }
        }

        private int m_ThresholdValue;
        public int ThresholdValue
        {
            get
            {
                return m_ThresholdValue;
            }
            set
            {
                if (Equals(m_ThresholdValue, value))
                {
                    return;
                }

                m_ThresholdValue = value;

                MarkAsDirty();
            }
        }

        private int m_ThresholdValue2;
        public int ThresholdValue2
        {
            get
            {
                return m_ThresholdValue2;
            }
            set
            {
                if (Equals(m_ThresholdValue2))
                {
                    return;
                }

                m_ThresholdValue2 = value;

                MarkAsDirty();
            }
        }

        private double m_MovementDelta;
        public double MovementDelta
        {
            get
            {
                return m_MovementDelta;
            }
            set
            {
                if (Equals(m_MovementDelta))
                {
                    return;
                }

                m_MovementDelta = value;

                MarkAsDirty();
            }
        }

        private object CalledLock = new object();
        public bool Cancelled
        {
            get
            {
                lock (CalledLock)
                {
                    return m_Cancelled;
                }
            }
            set
            {
                lock (CalledLock)
                {
                    if (Equals(m_Cancelled, value))
                    {
                        return;
                    }

                    m_Cancelled = value;
                }

                MarkAsDirty();
            }
        }

        private bool m_Paused;
        private object PausedLock = new object();
        public bool Paused
        {
            get
            {
                lock (PausedLock)
                {
                    return m_Paused;
                }
            }
            set
            {
                lock (PausedLock)
                {
                    if (Equals(m_Paused, value))
                    {
                        return;
                    }

                    m_Paused = value;
                }

                MarkAsDirty();
            }
        }

        public Image<Gray, Byte> BackgroundImage
        {
            get
            {
                return m_BackgroundImage;
            }
            set
            {
                if (ReferenceEquals(m_BackgroundImage, value))
                {
                    return;
                }

                m_BackgroundImage = value;

                MarkAsDirty();
            }
        }

        public Rectangle Roi
        {
            get
            {
                return m_Roi;
            }
            set
            {
                if (Equals(m_Roi, value))
                {
                    return;
                }

                m_Roi = value;

                MarkAsDirty();
            }
        }

        public RBSKVideo2()
        {
            BlobDetector = new CvBlobDetector();
        }

        public void Process()
        {
            if (Video == null)
            {
                return;
            }

            Console.WriteLine("Processing head points");
            HeadPoints = GenerateDictionary();

            if (FindWhiskers)
            {
                Console.WriteLine("Processing Whiskers");
                ProcessWhiskers();
            }
            if (FindFoot)
            {
                Console.WriteLine("Processing Feet");
                ProcessFeet();
            }

            Console.WriteLine("Finished");
        }

        private IFootVideoSettings _FootSettings;
        public IFootVideoSettings FootSettings
        {
            get { return _FootSettings; }
            set {
                _FootSettings = value;
                MarkAsDirty(); }
        }

        private IWhiskerVideoSettings _WhiskerSettings;
        public IWhiskerVideoSettings WhiskerSettings
        {
            get
            {
                return _WhiskerSettings;
            }
            set
            {
                if (Equals(_WhiskerSettings, value))
                {
                    return;
                }

                _WhiskerSettings = value;

                MarkAsDirty();
            }
        }

        public IWhiskerCollection ProcessWhiskersForSingleFrame(Image<Gray, byte> img, PointF[] headPoints, Point[] bodyContour)
        {
            ISingleFrameExtendedResults currentFrame = ModelResolver.Resolve<ISingleFrameExtendedResults>();
            currentFrame.HeadPoints = headPoints;
            if (headPoints != null)
            {
                currentFrame.HeadPoint = headPoints[2];
            }else
            {
                return null;
            }

            currentFrame.BodyContour = bodyContour;

            int newWidth = (int)(img.Width / WhiskerSettings.CropScaleFactor);
            int newHeight = (int)(img.Height / WhiskerSettings.CropScaleFactor);

            BackgroundImage.ROI = Rectangle.Empty;

            IWhiskerDetector whiskerDetector = ModelResolver.Resolve<IWhiskerDetector>();
            float scaleRatio = WhiskerSettings.ResolutionIncreaseScaleFactor;
            
            whiskerDetector.OrientationResolution = WhiskerSettings.OrientationResolution;

            int x = (int)currentFrame.HeadPoints[2].X - (newWidth / 2);
            int y = (int)currentFrame.HeadPoints[2].Y - (newHeight / 2);

            //if (!Roi.IsEmpty)
            //{
            //    x += Roi.X;
            //    y += Roi.Y;
            //}

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

            if (left > img.Width)
            {
                left = img.Width;
            }

            if (top > img.Height)
            {
                top = img.Height;
            }

            Rectangle zoomRect = new Rectangle(x, y, left - x, top - y);
            
            Inter interpolationType = WhiskerSettings.InterpolationType;

            using (Image<Gray, byte> bgFrame = BackgroundImage.Clone())
            {
                Image<Gray, byte> frame;
                if (!Roi.IsEmpty)
                {
                    frame = img.GetSubRect(Roi);

                }
                else
                {
                    frame = img;
                }

                //using (Image<Gray, byte> frame = frame2.GetSubRect(Roi))
                {
                    frame.ROI = zoomRect;
                    bgFrame.ROI = zoomRect;
                    
                    using (Image<Gray, byte> subImage = frame.Copy())
                    using (Image<Gray, byte> subBinary = bgFrame.Copy())
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
                            bodyContourCorrected[j] = new Point((int)((currentFrame.BodyContour[j].X - zoomRect.X) * scaleRatio), (int)((currentFrame.BodyContour[j].Y - zoomRect.Y) * scaleRatio));
                        }
                        
                        List<IWhiskerSegment>[] cWhisk = whiskerDetector.FindWhiskers(zoomedImage, zoomedBinary, headPointsCorrected, bodyContourCorrected, (int)scaleRatio, currentFrame, WhiskerSettings);

                        List<IWhiskerSegment> cWhiskLeft;
                        List<IWhiskerSegment> cWhiskRight;
                        if (cWhisk != null)
                        {
                            cWhiskLeft = cWhisk[0];
                            cWhiskRight = cWhisk[1];
                        }
                        else
                        {
                            cWhiskLeft = new List<IWhiskerSegment>();
                            cWhiskRight = new List<IWhiskerSegment>();
                        }
                        
                        foreach (var whisker in cWhiskLeft)
                        {
                            whisker.X = (int)((whisker.X / scaleRatio) + zoomRect.X);
                            whisker.Y = (int)((whisker.Y / scaleRatio) + zoomRect.Y);
                            Point p1 = new Point((int)((whisker.Line.P1.X / scaleRatio)) + zoomRect.X, (int)((whisker.Line.P1.Y / scaleRatio) + zoomRect.Y));
                            Point p2 = new Point((int)((whisker.Line.P2.X / scaleRatio)) + zoomRect.X, (int)((whisker.Line.P2.Y / scaleRatio) + zoomRect.Y));
                            whisker.Line = new LineSegment2D(p1, p2);
                        }

                        foreach (var whisker in cWhiskRight)
                        {
                            whisker.X = (int)((whisker.X / scaleRatio) + zoomRect.X);
                            whisker.Y = (int)((whisker.Y / scaleRatio) + zoomRect.Y);
                            Point p1 = new Point((int)((whisker.Line.P1.X / scaleRatio)) + zoomRect.X, (int)((whisker.Line.P1.Y / scaleRatio) + zoomRect.Y));
                            Point p2 = new Point((int)((whisker.Line.P2.X / scaleRatio)) + zoomRect.X, (int)((whisker.Line.P2.Y / scaleRatio) + zoomRect.Y));
                            whisker.Line = new LineSegment2D(p1, p2);
                        }

                        IWhiskerCollection whiskerCollection = ModelResolver.Resolve<IWhiskerCollection>();
                        whiskerCollection.LeftWhiskers = cWhiskLeft.ToArray();
                        whiskerCollection.RightWhiskers = cWhiskRight.ToArray();
                        currentFrame.AllWhiskers = whiskerCollection;

                        return whiskerCollection;
                    }
                }
            }
        }
        private void ProcessFeet()
        {
            Video.Reset();
            //process new 
            int newCount = HeadPoints.Count;
            
            int counter = 0;
            double prevAngle = 0;
            double bodyContourAngle = 0;
            CentroidTracker feetCentroids = new CentroidTracker();
            IEnumerable<float> centroidX = HeadPoints.Select(x => x.Value.Centroid.X);
            IEnumerable<float> centroidY = HeadPoints.Select(x => x.Value.Centroid.Y);
            int frameCounter = 100;
            int angleCounter = 0;
            double width = Video.Width;
            double hight = Video.Height;
            bool notHit = false;
            List<double> angles = new List<double>();
            for(int i = frameCounter; i<centroidX.Count(); i += frameCounter)
            {
               angles.Add(Math.Atan2(centroidY.ElementAt(i) - centroidY.ElementAt(i-frameCounter), centroidX.ElementAt(i) - centroidX.ElementAt(i - frameCounter)) * 180 / Math.PI);
            }
            angles.Add(Math.Atan2(centroidY.ElementAt(HeadPoints.Count-1) - centroidY.ElementAt(HeadPoints.Count - frameCounter), centroidX.ElementAt(HeadPoints.Count-1) - centroidX.ElementAt(HeadPoints.Count - frameCounter)) * 180 / Math.PI);
            for (int i = 0; i < HeadPoints.Count; i++)
            {
                if(i%frameCounter == 0 && !notHit)
                {
                    bodyContourAngle = angles[angleCounter];
                    angleCounter++;
                    notHit = true;
                }
               
                ISingleFrameExtendedResults currentFrame = HeadPoints[i];
                
                counter++;
                if (currentFrame == null || currentFrame.BodyContour == null)
                {
                    ProgressUpdates(this, new RBSKVideoUpdateEvent(0.5 + (((double)counter / newCount) / 2)));
                }
                Video.SetFrame(i);
                //if( i == 108)
                //{
                //    int mooasd = 0;
                //}
                if(currentFrame.HeadPoint!= new Point(0, 0))
                {
                    bodyContourAngle = Math.Atan2(currentFrame.MidPoint.Y - currentFrame.Centroid.Y, currentFrame.MidPoint.X - currentFrame.Centroid.X) * 180 / Math.PI;
                }
                

                Image<Bgr, byte> frame = Video.GetFrameImage();

                var mask = MaskSegmentation.segmentMask(currentFrame.BodyContour, ColorSpaceProcessing.processLogSpace(frame.Clone()).Convert<Bgr, double>(), FootSettings.kernelSize, FootSettings.erosionIterations);
                var moo2oo = ColorSpaceProcessing.processLogSpace(frame.Clone()).Convert<Bgr, double>(); 
                //CvInvoke.Imwrite($"log_{i:D4}.png", moo2oo);
                ContourDetection contourDetection = new ContourDetection();

                List<VectorOfPointF> unified = ContourDetection.detectFootContours(mask.Clone(), FootSettings.scaleFactor, FootSettings.contourDistance);
                //CvInvoke.Imwrite($"mask_{i:D4}.png", mask);

                List<IFootPlacement> footplacement = new List<IFootPlacement>();
                
                foreach (var c in unified)
                {
                    if (CvInvoke.ContourArea(c) > FootSettings.area)
                    {
                        var rect = CvInvoke.BoundingRectangle(c);
                        footplacement.Add(new FootPlacement { minX = rect.X, minY = rect.Y, maxX = rect.X + rect.Width, maxY = rect.Y + rect.Height, width= rect.Width, height = rect.Height });
                        frame.Draw(rect, new Bgr(Color.Aquamarine));

                    }
                }

                //angle stuff

                Point headPoint, backPoint, centrePoint;

                calculateAngles(bodyContourAngle, currentFrame, out headPoint, out backPoint, out centrePoint, width, hight);
                //if (currentFrame.MidPoint != new Point(0,0))
                //{
                //headPoint.X = Convert.ToInt32(currentFrame.MidPoint.X);
                //headPoint.Y = Convert.ToInt32(currentFrame.MidPoint.Y);
                //}
                CvInvoke.Line(frame, backPoint, headPoint, new MCvScalar(255, 255, 0), 2);
                CvInvoke.Circle(frame, headPoint, 8, new MCvScalar(255, 255, 0), 2);
                CvInvoke.Circle(frame, backPoint, 8, new MCvScalar(255, 255, 0), 2);
                CvInvoke.Circle(frame, new Point(Convert.ToInt32(currentFrame.MidPoint.X), Convert.ToInt32(currentFrame.MidPoint.Y)), 8, new MCvScalar(255, 0, 0), 2);
                CvInvoke.Circle(frame, new Point(Convert.ToInt32(currentFrame.Centroid.X), Convert.ToInt32(currentFrame.Centroid.Y)), 8, new MCvScalar(255, 0, 255), 2);
                CvInvoke.Circle(frame, new Point(Convert.ToInt32(currentFrame.Centroid.X * 4), Convert.ToInt32(currentFrame.Centroid.Y * 4)), 8, new MCvScalar(255, 0, 255), 2);
                ////CvInvoke.Imwrite($"{i}_frame.png", frame);

                //Do something for contours etc. 
                //u[date this to something suitable
                if (i == 105)
                {
                   int moasd = 1;
                }
                List<IfeetID> objects = feetCentroids.update(footplacement.ToArray(), centrePoint, headPoint, backPoint, currentFrame.BodyContour);
                IFootCollection footCollection = ModelResolver.Resolve<IFootCollection>();

                foreach (feetID item in objects)
                {
                    CvInvoke.PutText(frame, item.id, new Point(item.value.centroidX, item.value.centroidY), FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255));
                    if (item.id.Contains("LF"))
                    {
                        footCollection.leftfront = item;
                    }
                    else if (item.id.Contains("LH"))
                    {
                        footCollection.leftHind = item;
                    }
                    else if (item.id.Contains("RH"))
                    {
                        footCollection.rightHind = item;
                    }
                    else if (item.id.Contains("RF"))
                    {
                        footCollection.rightfront = item;
                    }
                }

                //CvInvoke.Imwrite($"{i}_frame.png", frame);
                currentFrame.FeetCollection = footCollection;
                ProgressUpdates(this, new RBSKVideoUpdateEvent(0.5 + (((double)counter / newCount) / 2)));
            }
            FootDataResults res = new FootDataResults(HeadPoints);
            HeadPoints = res.PostProcess();

        }

        private static void calculateAngles(double bodyContourAngle, ISingleFrameExtendedResults currentFrame, out Point headPoint, out Point backPoint, out Point centrePoint, double width, double height)
        {

            //xAngle = xAngle * Math.PI / 180;
            //yAngle = yAngle * Math.PI / 180;
            double xAngle = Math.Cos((bodyContourAngle) * Math.PI / 180);
            double yAngle = Math.Sin((bodyContourAngle) * Math.PI / 180);
            int headX = Convert.ToInt32((currentFrame.Centroid.X - 200 * xAngle));
            int headY = Convert.ToInt32((currentFrame.Centroid.Y - 00 * xAngle));
            int backX = Convert.ToInt32((currentFrame.Centroid.X + 70 * xAngle));
            int backY = Convert.ToInt32((currentFrame.Centroid.Y + 70 * xAngle));
            headPoint = new Point(Convert.ToInt32((currentFrame.Centroid.X + 150 * xAngle)), Convert.ToInt32((currentFrame.Centroid.Y + 150 * yAngle)));
            backPoint = new Point(Convert.ToInt32((currentFrame.Centroid.X - 75 * xAngle)), Convert.ToInt32((currentFrame.Centroid.Y - 75 * yAngle)));
            centrePoint = new Point(Convert.ToInt32(currentFrame.Centroid.X), Convert.ToInt32(currentFrame.Centroid.Y));
            if (headPoint.X > width || headPoint.X< 0 || headPoint.Y >height || headPoint.Y < 0)
            {
                centrePoint.X = (headX + backX) / 2;
                centrePoint.Y = (headY + backY) / 2;
            }
            
        }

        private void ProcessWhiskers()
        {
            bool time = false;
            Video.Reset();
            //bool showOnce = true;
            //Get quarter size of image around nose point
            int newWidth = (int)(VideoWidth / WhiskerSettings.CropScaleFactor);
            int newHeight = (int)(VideoHeight / WhiskerSettings.CropScaleFactor);

            BackgroundImage.ROI = Rectangle.Empty;

            IWhiskerDetector whiskerDetector = ModelResolver.Resolve<IWhiskerDetector>();
            float scaleRatio = WhiskerSettings.ResolutionIncreaseScaleFactor;
            //whiskerDetector.LineLength = 8*(int) scaleRatio;
            whiskerDetector.OrientationResolution = WhiskerSettings.OrientationResolution;
            //Dictionary<int, IWhiskerCollection> whiskers = new Dictionary<int, IWhiskerCollection>();
            int newCount = HeadPoints.Count;
            int counter = 0;
            for (int i = 0; i < HeadPoints.Count; i++)
            {
                //Stopwatch sw = new Stopwatch();

                ISingleFrameExtendedResults currentFrame = HeadPoints[i];

                counter++;

                if (currentFrame == null)
                {
                    //whiskers.Add(i, null);

                    if (ProgressUpdates != null)
                    {
                        ProgressUpdates(this, new RBSKVideoUpdateEvent(0.5 + (((double)counter / newCount) / 2)));
                    }
                    continue;
                }

                if (currentFrame.HeadPoints == null)
                {
                    //whiskers.Add(i, null);
                    if (ProgressUpdates != null)
                    {
                        ProgressUpdates(this, new RBSKVideoUpdateEvent(0.5 + (((double)counter / newCount) / 2)));
                    }
                    continue;
                }

                if (currentFrame.BodyContour == null || !currentFrame.BodyContour.Any())
                {
                    //whiskers.Add(i, null);
                    if (ProgressUpdates != null)
                    {
                        ProgressUpdates(this, new RBSKVideoUpdateEvent(0.5 + (((double)counter / newCount) / 2)));
                    }
                    continue;
                }

                //if (time)
                //{
                //    sw.Stop();
                //    Console.WriteLine("1) " + sw.ElapsedMilliseconds);
                //    sw.Restart();
                //}

                int x = (int) currentFrame.HeadPoints[2].X - (newWidth / 2);
                int y = (int) currentFrame.HeadPoints[2].Y - (newHeight / 2);

                //if (!Roi.IsEmpty)
                //{
                //    x += Roi.X;
                //    y += Roi.Y;
                //}

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

                if (left > VideoWidth)
                {
                    left = VideoWidth;
                }

                if (top > VideoHeight)
                {
                    top = VideoHeight;
                }

                Rectangle zoomRect = new Rectangle(x, y, left - x, top - y);

                //if (time)
                //{
                //    sw.Stop();
                //    Console.WriteLine("2) " + sw.ElapsedMilliseconds);
                //    sw.Restart();
                //}

                Video.SetFrame(i);

                //if (time)
                //{
                //    sw.Stop();
                //    Console.WriteLine("3) " + sw.ElapsedMilliseconds);
                //    sw.Restart();
                //}

                Inter interpolationType = WhiskerSettings.InterpolationType;

                using (Image<Gray, byte> frame2 = Video.GetGrayFrameImage())
                using (Image<Gray, byte> bgFrame = BackgroundImage.Clone())
                {
                    Image<Gray, byte> frame;
                    if (!Roi.IsEmpty)
                    {
                        frame = frame2.GetSubRect(Roi);

                    }
                    else
                    {
                        frame = frame2;
                    }

                    //using (Image<Gray, byte> frame = frame2.GetSubRect(Roi))
                    {
                        frame.ROI = zoomRect;
                        bgFrame.ROI = zoomRect;

                        //if (time)
                        //{
                        //    sw.Stop();
                        //    Console.WriteLine("4) " + sw.ElapsedMilliseconds);
                        //    sw.Restart();
                        //}

                        using (Image<Gray, byte> subImage = frame.Copy())
                        using (Image<Gray, byte> subBinary = bgFrame.Copy())
                        using (Image<Gray, byte> zoomedImage = subImage.Resize(scaleRatio, interpolationType))
                        using (Image<Gray, byte> zoomedBinary = subBinary.Resize(scaleRatio, interpolationType))
                        {

                            //if (time)
                            //{
                            //    sw.Stop();
                            //    Console.WriteLine("5) " + sw.ElapsedMilliseconds);
                            //    sw.Restart();
                            //}
                            //if (showOnce)
                            //{
                            //    ImageViewer.Show(zoomedImage);
                            //    ImageViewer.Show(zoomedBinary);
                            //    showOnce = false;
                            //}
                            PointF[] headPointsCorrected = new PointF[5];

                            int bodyContourLength = currentFrame.BodyContour.Length;
                            Point[] bodyContourCorrected = new Point[bodyContourLength];

                            for (int j = 0; j < 5; j++)
                            {
                                headPointsCorrected[j] = new PointF((currentFrame.HeadPoints[j].X - zoomRect.X) * scaleRatio, (currentFrame.HeadPoints[j].Y - zoomRect.Y) * scaleRatio);
                            }

                            for (int j = 0; j < bodyContourLength; j++)
                            {
                                bodyContourCorrected[j] = new Point((int)((currentFrame.BodyContour[j].X - zoomRect.X) * scaleRatio), (int)((currentFrame.BodyContour[j].Y - zoomRect.Y) * scaleRatio));
                            }

                            //if (i == 62)
                            //{
                            //    Console.WriteLine("62");
                            //}
                            IWhiskerSegment leftWhisker = null, rightWhisker = null;

                            if (i > 0)
                            {
                                GetPrevSegments(i, out leftWhisker, out rightWhisker);
                            }

                            List<IWhiskerSegment>[] cWhisk = whiskerDetector.FindWhiskers(zoomedImage, zoomedBinary, headPointsCorrected, bodyContourCorrected, (int)scaleRatio, currentFrame, WhiskerSettings, leftWhisker, rightWhisker);

                            List<IWhiskerSegment> cWhiskLeft;
                            List<IWhiskerSegment> cWhiskRight;
                            if (cWhisk != null)
                            {
                                cWhiskLeft = cWhisk[0];
                                cWhiskRight = cWhisk[1];
                            }
                            else
                            {
                                cWhiskLeft = new List<IWhiskerSegment>();
                                cWhiskRight = new List<IWhiskerSegment>();
                            }


                            //if (time)
                            //{
                            //    sw.Stop();
                            //    Console.WriteLine("6) " + sw.ElapsedMilliseconds);
                            //    sw.Restart();
                            //}

                            //IWhiskerSegment[] correctedWhiskers = new IWhiskerSegment[cWhisk.Length];
                            foreach (var whisker in cWhiskLeft)
                            {
                                whisker.X = (int)((whisker.X / scaleRatio) + zoomRect.X);
                                whisker.Y = (int)((whisker.Y / scaleRatio) + zoomRect.Y);
                                Point p1 = new Point((int)((whisker.Line.P1.X / scaleRatio)) + zoomRect.X, (int)((whisker.Line.P1.Y / scaleRatio) + zoomRect.Y));
                                Point p2 = new Point((int)((whisker.Line.P2.X / scaleRatio)) + zoomRect.X, (int)((whisker.Line.P2.Y / scaleRatio) + zoomRect.Y));
                                whisker.Line = new LineSegment2D(p1, p2);
                            }

                            foreach (var whisker in cWhiskRight)
                            {
                                whisker.X = (int)((whisker.X / scaleRatio) + zoomRect.X);
                                whisker.Y = (int)((whisker.Y / scaleRatio) + zoomRect.Y);
                                Point p1 = new Point((int)((whisker.Line.P1.X / scaleRatio)) + zoomRect.X, (int)((whisker.Line.P1.Y / scaleRatio) + zoomRect.Y));
                                Point p2 = new Point((int)((whisker.Line.P2.X / scaleRatio)) + zoomRect.X, (int)((whisker.Line.P2.Y / scaleRatio) + zoomRect.Y));
                                whisker.Line = new LineSegment2D(p1, p2);
                            }

                            IWhiskerCollection whiskerCollection = ModelResolver.Resolve<IWhiskerCollection>();
                            whiskerCollection.LeftWhiskers = cWhiskLeft.ToArray();
                            whiskerCollection.RightWhiskers = cWhiskRight.ToArray();
                            currentFrame.AllWhiskers = whiskerCollection;
                            //whiskers.Add(i, whiskerCollection);
                            IWhiskerCollection finalCollection = ModelResolver.Resolve<IWhiskerCollection>();
                            IWhiskerCollection bestTrackedWhisker = ModelResolver.Resolve<IWhiskerCollection>();
                            PostProcessWhiskers(currentFrame.MidPoint, currentFrame.Orientation, currentFrame.AllWhiskers, finalCollection);
                            currentFrame.Whiskers = finalCollection;
                            currentFrame.BestTrackedWhisker = bestTrackedWhisker;
                        }
                    }
                }


                if (ProgressUpdates != null)
                {
                    ProgressUpdates(this, new RBSKVideoUpdateEvent(0.5 + (((double)counter / newCount) / 2)));
                }


                //if (time)
                //{
                //    sw.Stop();
                //    Console.WriteLine("7) " + sw.ElapsedMilliseconds);
                //    sw.Restart();
                //}
            }

            TrackBestLeftWhisker();
            TrackBestRightWhisker();


        }

        private bool _FindWhiskers;
        public bool FindWhiskers
        {
            get
            {
                return _FindWhiskers;
            }
            set
            {
                if (Equals(_FindWhiskers, value))
                {
                    return;
                }

                _FindWhiskers = value;

                MarkAsDirty();
            }
        }
        private bool _FindFoot;
        public bool FindFoot
        {
            get
            {
                return _FindFoot;
            }
            set
            {
                if (Equals(_FindFoot, value))
                {
                    return;
                }

                _FindFoot = value;

                MarkAsDirty();
            }
        }

        private void TrackBestRightWhisker()
        {

            KeyValuePair<int, ISingleFrameExtendedResults> firstRightKvp = HeadPoints.FirstOrDefault(x => x.Value.Whiskers != null && x.Value.Whiskers.RightWhiskers.Any());

            if (firstRightKvp.Equals(default(KeyValuePair<int, ISingleFrameExtendedResults>)))
            {
                return;
            }

            int index = firstRightKvp.Key;
            ISingleFrameExtendedResults firstRightFrame = firstRightKvp.Value;

            IWhiskerSegment bestFirstSegment = firstRightFrame.Whiskers.RightWhiskers.Aggregate((curMax, x) => (curMax == null || x.AvgIntensity > curMax.AvgIntensity ? x : curMax));
            firstRightFrame.BestTrackedWhisker.RightWhiskers = new IWhiskerSegment[] { bestFirstSegment };

            IWhiskerSegment previousSegment = bestFirstSegment;
            int distCounter = 1;
            for (int i = index + 1; i < HeadPoints.Count; i++)
            {
                if (HeadPoints[i].HeadPoints == null || HeadPoints[i].Whiskers == null)
                {
                    break;
                }

                IWhiskerSegment[] segs = HeadPoints[i].Whiskers.RightWhiskers;

                if (!segs.Any())
                {
                    distCounter++;
                    continue;
                }

                IEnumerable<IWhiskerSegment> orderedSegs = segs.OrderByDescending(x => x.AvgIntensity);
                HeadPoints[i].BestTrackedWhisker.RightWhiskers = orderedSegs.Take(5).ToArray();
                continue;
                int distance = 10 - distCounter;
                IEnumerable<IWhiskerSegment> closestsSegs = segs.Where(x => DistanceBetweenSegs(x, previousSegment) < distance);

                if (!closestsSegs.Any())
                {
                    distCounter++;
                    continue;
                }

                distCounter = 1;


                IWhiskerSegment bestSeg = closestsSegs.Aggregate((curMax, x) => (curMax == null || x.AvgIntensity > curMax.AvgIntensity ? x : curMax));
                HeadPoints[i].BestTrackedWhisker.RightWhiskers = new IWhiskerSegment[] { bestSeg };
                previousSegment = bestSeg;
            }
        }

        private void TrackBestLeftWhisker()
        {
            KeyValuePair<int, ISingleFrameExtendedResults> firstRightKvp = HeadPoints.FirstOrDefault(x => x.Value.Whiskers != null && x.Value.Whiskers.LeftWhiskers.Any());

            if (firstRightKvp.Equals(default(KeyValuePair<int, ISingleFrameExtendedResults>)))
            {
                return;
            }

            int index = firstRightKvp.Key;
            ISingleFrameExtendedResults firstRightFrame = firstRightKvp.Value;

            IWhiskerSegment bestFirstSegment = firstRightFrame.Whiskers.LeftWhiskers.Aggregate((curMax, x) => (curMax == null || x.AvgIntensity > curMax.AvgIntensity ? x : curMax));
            firstRightFrame.BestTrackedWhisker.LeftWhiskers = new IWhiskerSegment[] { bestFirstSegment };

            IWhiskerSegment previousSegment = bestFirstSegment;
            int distCounter = 1;
            for (int i = index + 1; i < HeadPoints.Count; i++)
            {
                if (HeadPoints[i].HeadPoints == null || HeadPoints[i].Whiskers == null)
                {
                    break;
                }

                IWhiskerSegment[] segs = HeadPoints[i].Whiskers.LeftWhiskers;

                if (!segs.Any())
                {
                    distCounter++;
                    continue;
                }

                IEnumerable<IWhiskerSegment> orderedSegs = segs.OrderByDescending(x => x.AvgIntensity);
                HeadPoints[i].BestTrackedWhisker.LeftWhiskers = orderedSegs.Take(5).ToArray();
                continue;
                int distance = 10 - distCounter;
                IEnumerable<IWhiskerSegment> closestsSegs = segs.Where(x => DistanceBetweenSegs(x, previousSegment) < distance);

                if (!closestsSegs.Any())
                {
                    distCounter++;
                    continue;
                }

                distCounter = 1;


                IWhiskerSegment bestSeg = closestsSegs.Aggregate((curMax, x) => (curMax == null || x.AvgIntensity > curMax.AvgIntensity ? x : curMax));
                HeadPoints[i].BestTrackedWhisker.LeftWhiskers = new IWhiskerSegment[] { bestSeg };
                previousSegment = bestSeg;
            }
        }

        private double DistanceBetweenSegs(IWhiskerSegment seg1, IWhiskerSegment seg2)
        {
            return Math.Sqrt(Math.Pow((seg1.X - seg2.X), 2) + Math.Pow((seg1.Y - seg2.Y), 2));
        }

        private void PostProcessWhiskers(PointF midPoint, Vector orientation, IWhiskerCollection whiskers, IWhiskerCollection finalWhiskers)
        {
            IWhiskerDetector wd = ModelResolver.Resolve<IWhiskerDetector>();
            double minAngleDelta = 15;

            //Whisker angle is measured against horizontal
            //Vector horiztonalVec = new Vector(1, 0);
            //double headAngle = Vector.AngleBetween(orientation, horiztonalVec);

            //return;

            if (whiskers.LeftWhiskers != null)
            {
                List<IWhiskerSegment> leftWhiskers = whiskers.LeftWhiskers.ToList();

                wd.RemoveDudWhiskers(midPoint, leftWhiskers, orientation, minAngleDelta, true);
                finalWhiskers.LeftWhiskers = leftWhiskers.ToArray();
            }

            if (whiskers.RightWhiskers != null)
            {
                List<IWhiskerSegment> rightWhiskers = whiskers.RightWhiskers.ToList();

                wd.RemoveDudWhiskers(midPoint, rightWhiskers, orientation, minAngleDelta, false);
                finalWhiskers.RightWhiskers = rightWhiskers.ToArray();
            }

            //whiskers.LeftWhiskers = leftWhiskers.ToArray();
            //whiskers.RightWhiskers = rightWhiskers.ToArray();
        }

        private void GetPrevSegments(int index, out IWhiskerSegment leftSegment, out IWhiskerSegment rightSegment)
        {
            bool hasLeft = false, hasRight = false;
            leftSegment = null;
            rightSegment = null;
            for (int i = index - 1; i >= 0; i--)
            {
                if (HeadPoints[i].AllWhiskers == null)
                {
                    return;
                }

                if (!hasLeft)
                {
                    IWhiskerSegment[] leftSegments = HeadPoints[i].AllWhiskers.LeftWhiskers;
                    if (leftSegments.Any())
                    {
                        leftSegment = leftSegments[0];
                        hasLeft = true;
                    }
                }

                if (!hasRight)
                {
                    IWhiskerSegment[] rightSegments = HeadPoints[i].AllWhiskers.RightWhiskers;
                    if (rightSegments.Any())
                    {
                        rightSegment = rightSegments[0];
                        hasRight = true;
                    }
                }

                if (hasLeft && hasRight)
                {
                    return;
                }
            }
        }

        private void PostProcessWhiskers()
        {
            Console.WriteLine("Starting post processing");
            KeyValuePair<int, ISingleFrameExtendedResults> initWhiskers = HeadPoints.First(x => x.Value != null && x.Value.AllWhiskers != null && x.Value.AllWhiskers.LeftWhiskers != null && x.Value.AllWhiskers.LeftWhiskers.Any());

            int initKey = initWhiskers.Key;
            
            //int width = (int)Video.Width;
            //int height = (int)Video.Height;

            IWhiskerSegment[] leftWhiskers = initWhiskers.Value.AllWhiskers.LeftWhiskers;
            IWhiskerSegment[] rightWhiskers = initWhiskers.Value.AllWhiskers.RightWhiskers;

            IWhiskerAllocator wa = ModelResolver.Resolve<IWhiskerAllocator>();

            ISingleFrameExtendedResults firstResult = HeadPoints[initKey];
            PointF nosePoint = firstResult.HeadPoint;
            PointF midPoint = firstResult.HeadPoints[1].MidPoint(firstResult.HeadPoints[3]);

            ITrackSingleWhisker[] leftTrackedWhiskers = wa.InitialiseWhiskers(leftWhiskers, nosePoint, midPoint);
            ITrackSingleWhisker[] rightTrackedWhiskers = wa.InitialiseWhiskers(rightWhiskers, nosePoint, midPoint);
            
            initKey++;
            for (int i = initKey; i < HeadPoints.Count; i++)
            {
                //IWhiskerCollection whiskerCollection = preliminaryWhiskers[i];

                IWhiskerSegment[] cLeftWhiskers = HeadPoints[i].AllWhiskers.LeftWhiskers;
                IWhiskerSegment[] cRightWhiskers = HeadPoints[i].AllWhiskers.RightWhiskers;

                //IWhiskerAllocator wa = ModelResolver.Resolve<IWhiskerAllocator>();

                Dictionary<IWhiskerSegment, ITrackSingleWhisker> left = wa.AllocateWhiskers(i, leftTrackedWhiskers, cLeftWhiskers, nosePoint, midPoint);
                Dictionary<IWhiskerSegment, ITrackSingleWhisker> right = wa.AllocateWhiskers(i, rightTrackedWhiskers, cRightWhiskers, nosePoint, midPoint);

                foreach (var kvp in left)
                {
                    kvp.Value.CurrentWhisker = kvp.Key;
                    kvp.Value.WhiskerList.Add(i, kvp.Key);
                }

                foreach (var kvp in right)
                {
                    kvp.Value.CurrentWhisker = kvp.Key;
                    kvp.Value.WhiskerList.Add(i, kvp.Key);
                }

                //foreach (ITrackSingleWhisker iWhisker in leftTrackedWhiskers)
                //{
                //    iWhisker.FindPotentialWhisker(i, cLeftWhiskers);
                //}

                //foreach (ITrackSingleWhisker iWhisker in rightTrackedWhiskers)
                //{
                //    iWhisker.FindPotentialWhisker(i, cRightWhiskers);
                //}
            }

            //Dictionary<int, IWhiskerCollection> finalWhiskers = new Dictionary<int, IWhiskerCollection>();
            

            for (int i = 0; i < HeadPoints.Count; i++)
            {
                IWhiskerCollection whiskerCollection = ModelResolver.Resolve<IWhiskerCollection>();

                List<IWhiskerSegment> leftWhisks = new List<IWhiskerSegment>();
                List<IWhiskerSegment> rightWhisks = new List<IWhiskerSegment>();

                foreach (var trackedWhisker in leftTrackedWhiskers)
                {
                    if (trackedWhisker.WhiskerList.ContainsKey(i))
                    {
                        leftWhisks.Add(trackedWhisker.WhiskerList[i]);
                    }
                }

                foreach (var trackedWhisker in rightTrackedWhiskers)
                {
                    if (trackedWhisker.WhiskerList.ContainsKey(i))
                    {
                        rightWhisks.Add(trackedWhisker.WhiskerList[i]);
                    }
                }

                whiskerCollection.LeftWhiskers = leftWhisks.ToArray();
                whiskerCollection.RightWhiskers = rightWhisks.ToArray();

                HeadPoints[i].AllWhiskers = whiskerCollection;
                //finalWhiskers.Add(i, whiskerCollection);
            }
            
            //AllRects = allRects;
            //WhiskerResults = finalWhiskers;
        }

        private int VideoWidth
        {
            get;
            set;
        }

        private int VideoHeight
        {
            get;
            set;
        }

        public void GetHeadAndBody(Image<Bgr, byte> frame, out PointF[] headPoints, out Point[] body)
        {
            if (frame == null)
            {
                throw new Exception("Frame can't be null");
            }

            if (Roi != Rectangle.Empty)
            {
                frame.ROI = Roi;
            }

            RBSK rbsk = MouseService.GetStandardMouseRules();

            if (GapDistance == 0)
            {
                GapDistance = GetBestGapDistance(rbsk);
            }

            rbsk.Settings.GapDistance = GapDistance;
            rbsk.Settings.BinaryThreshold = ThresholdValue;
            rbsk.Settings.FilterLevel = 13;

            double waist, waistArea, waistArea2, waistArea3, waistArea4;
            PointF centroid;
            
            headPoints = ProcessFrame(frame, rbsk, out waist, out waistArea, out waistArea2, out waistArea3, out waistArea4, out centroid, out body, true);
        }

        private Dictionary<int, ISingleFrameExtendedResults> GenerateDictionary()
        {
            //const int SecondBinaryThresold = 10;
            const double movementDelta = 15;

            RBSK rbsk = MouseService.GetStandardMouseRules();

            if (GapDistance == 0)
            {
                GapDistance = GetBestGapDistance(rbsk);
            }

            rbsk.Settings.GapDistance = GapDistance;
            rbsk.Settings.BinaryThreshold = ThresholdValue;
            rbsk.Settings.FilterLevel = 13;

            Dictionary<int, ISingleFrameExtendedResults> results = new Dictionary<int, ISingleFrameExtendedResults>();

            VideoWidth = (int)Video.Width;
            VideoHeight = (int)Video.Height;

            Video.Reset();
            int counter = 0;

            int frameCount = Video.FrameCount;
            //int frameCount = 100;

            while (true)
            {
                if (Paused)
                {
                    continue;
                }

                if (Cancelled || counter==frameCount)
                {
                    break;
                }

                using (Image<Bgr, byte> frame = Video.GetFrameImage())
                {
                    if (frame == null)
                    {
                        break;
                    }

                    if (Roi != Rectangle.Empty)
                    {
                        frame.ROI = Roi;
                    }

                    double waist, waistArea, waistArea2, waistArea3, waistArea4;
                    PointF centroid;

                    //if (counter == 500)
                    //{
                    //    frame.ROI = new Rectangle(0,0,1,1);
                    //}
                    Point[] bodyContour;
                    PointF[] headPoints = ProcessFrame(frame, rbsk, out waist, out waistArea, out waistArea2, out waistArea3, out waistArea4, out centroid, out bodyContour, true);

                    //Console.WriteLine(waist + " " + waistArea);
                    //if (headPoints != null)
                    //{
                    //    foreach (var point in headPoints)
                    //    {
                    //        frame.Draw(new CircleF(point, 2), new Bgr(Color.Yellow), 2);
                    //    }
                    //}

                    //ImageViewer.Show(frame);
                    ISingleFrameExtendedResults frameResult = ModelResolver.Resolve<ISingleFrameExtendedResults>();
                    frameResult.HeadPoints = headPoints;
                    if (headPoints != null)
                    {
                        frameResult.HeadPoint = headPoints[2];
                    }
                    
                    frameResult.CentroidSize = waist;
                    frameResult.PelvicArea = waistArea;
                    frameResult.PelvicArea2 = waistArea2;
                    frameResult.PelvicArea3 = waistArea3;
                    frameResult.PelvicArea4 = waistArea4;
                    frameResult.Centroid = centroid;
                    frameResult.BodyContour = bodyContour;
                    results.Add(counter, frameResult);
                    counter++;

                    if (ProgressUpdates != null)
                    {
                        double progress = ((double) counter/frameCount);
                        if (FindWhiskers)
                        {
                            progress /= 2;
                        }

                        ProgressUpdates(this, new RBSKVideoUpdateEvent(progress));
                    }
                }
            }

            if (Cancelled)
            {
                return results;
            }

            //Find the most confident run of head detections
            List<List<HeadPointHolder>> confidentPoints = new List<List<HeadPointHolder>>();
            PointF lastPoint = PointF.Empty;
            List<HeadPointHolder> currentList = new List<HeadPointHolder>();
            double currentTotalMovement = 0;
            
            for (int i = 0; i < results.Count; i++)
            {
                PointF[] headPoints = results[i].HeadPoints;

                if (headPoints == null)
                {
                    //No head found, use alternative methods
                    if (currentList.Count > 0)
                    {
                        confidentPoints.Add(currentList);
                    }
                    currentList = new List<HeadPointHolder>();
                    lastPoint = PointF.Empty;
                    currentTotalMovement = 0;
                    //Console.WriteLine("Head points are null " + i);
                    continue;
                }

                //Check against expected position
                PointF currentPoint = headPoints[2];

                if (lastPoint.IsEmpty)
                {
                    lastPoint = currentPoint;
                    currentList.Add(new HeadPointHolder(i, currentPoint, currentTotalMovement));
                    continue;
                }

                double distance = lastPoint.Distance(currentPoint);
                if (distance < movementDelta)
                {
                    //Acceptable
                    currentTotalMovement += distance;
                    currentList.Add(new HeadPointHolder(i, currentPoint, currentTotalMovement));
                    lastPoint = currentPoint;
                }
                else
                {
                    if (currentList.Count > 0)
                    {
                        confidentPoints.Add(currentList);
                    }
                    currentList = new List<HeadPointHolder>();
                    lastPoint = PointF.Empty;
                    currentTotalMovement = 0;
                    //Console.WriteLine("Outside of range " + i);
                }
            }

            if (currentList.Count > 0)
            {
                confidentPoints.Add(currentList);
            }

            if (confidentPoints.Count == 0)
            {
                return results;
            }

            //Find longest list with highest total movement
            List<HeadPointHolder> bestList = confidentPoints[0];
            //double currentMaxTraverse = bestList.Last().TotalDelta;
            foreach (List<HeadPointHolder> list in confidentPoints)
            {
                //double currentTotalDelta = list.Last().TotalDelta;
                if (list.Count > bestList.Count)
                {
                    //currentMaxTraverse = currentTotalDelta;
                    bestList = list;
                }
            }

            //We now have a confident set of headpoints
            int minIndex = bestList.Select(x => x.FrameNumber).Min();
            int maxIndex = bestList.Select(x => x.FrameNumber).Max();

            
            minIndex--;
            while (minIndex >= 0)
            {
                //Traverse backwards
                PointF[] lastPoints = results[minIndex + 1].HeadPoints;
                if (lastPoints != null)
                {
                    lastPoint = lastPoints[2];
                }
                else
                {
                    lastPoint = new PointF(-100, -100);
                }

                PointF[] headPoints = results[minIndex].HeadPoints;

                if (headPoints == null)
                {
                    //No head found, use alternative methods
                    int previousThreshold = rbsk.Settings.BinaryThreshold;
                    rbsk.Settings.BinaryThreshold = ThresholdValue2;
                    Video.SetFrame(minIndex);
                    PointF[] headPoints2 = null;
                    using (Image<Bgr, byte> frame = Video.GetFrameImage())
                    {
                        if (frame == null)
                        {
                            break;
                        }

                        if (Roi != Rectangle.Empty)
                        {
                            frame.ROI = Roi;
                        }

                        headPoints2 = ProcessFrame(frame, rbsk, lastPoint, movementDelta);
                    }

                    rbsk.Settings.BinaryThreshold = previousThreshold;

                    if (headPoints2 != null)
                    {
                        //We've got a good location
                        //double temp = results[minIndex].CentroidSize;
                        results[minIndex].HeadPoints = headPoints2;
                        results[minIndex].HeadPoint = headPoints2[2];
                        //results[minIndex].CentroidSize = temp;
                        //results[minIndex] = new Tuple<PointF[], double>(headPoints2, temp);
                    }
                    else
                    {
                        for (int i = 0; i <= minIndex; i++)
                        {
                            if (results.ContainsKey(i))
                            {
                                //double temp = results[i].Item2;
                                //results[i] = new Tuple<PointF[], double>(null, temp);
                                results[i].HeadPoints = null;
                                results[minIndex].HeadPoint = PointF.Empty;
                            }
                        }

                        break;
                    }

                    minIndex--;
                    continue;
                }

                //Check against expected position
                PointF currentPoint = headPoints[2];

                if (lastPoint.Distance(currentPoint) < movementDelta)
                {
                    //Good point
                }
                else
                {
                    //Wrong point, search for another rbsk that falls within range
                    //if (minIndex == 17 || minIndex == 16)
                    //{
                    //    Console.WriteLine("");
                    //}

                    Video.SetFrame(minIndex);
                    PointF[] headPoints2 = null;
                    using (Image<Bgr, byte> frame = Video.GetFrameImage())
                    {
                        if (frame == null)
                        {
                            break;
                        }

                        if (Roi != Rectangle.Empty)
                        {
                            frame.ROI = Roi;
                        }

                        headPoints2 = ProcessFrame(frame, rbsk, lastPoint, movementDelta);
                    }

                    if (headPoints2 != null)
                    {
                        //We've got a good location
                        //double temp = results[maxIndex].Item2;
                        //results[minIndex] = new Tuple<PointF[], double>(headPoints2, temp);
                        results[minIndex].HeadPoints = headPoints2;
                        results[minIndex].HeadPoint = headPoints2[2];
                    }
                    else
                    {
                        //No other rbsk falls within range, use alternative methods
                        //Console.WriteLine("Need to use alternative methods");
                        for (int i = 0; i <= minIndex; i++)
                        {
                            if (results.ContainsKey(i))
                            {
                                //double temp = results[i].Item2;
                                //results[i] = new Tuple<PointF[], double>(null, temp);
                                results[i].HeadPoints = null;
                                results[minIndex].HeadPoint = PointF.Empty;
                            }
                        }

                        break;
                    }
                }

                minIndex--;
            }

            if (minIndex < 0)
            {
                minIndex = 0;
            }

            maxIndex++;
            while (maxIndex < results.Count)
            {
                //Traverse backwards
                PointF[] lastPoints = results[maxIndex - 1].HeadPoints;
                if (lastPoints != null)
                {
                    lastPoint = lastPoints[2];
                }
                else
                {
                    lastPoint = new PointF(-100,-100);
                }

                PointF[] headPoints = results[maxIndex].HeadPoints;
                

                if (headPoints == null)
                {
                    //No head found, use alternative methods
                    int previousThreshold = rbsk.Settings.BinaryThreshold;
                    rbsk.Settings.BinaryThreshold = ThresholdValue2;
                    Video.SetFrame(maxIndex);
                    PointF[] headPoints2 = null;
                    using (Image<Bgr, byte> frame = Video.GetFrameImage())
                    {
                        if (frame == null)
                        {
                            break;
                        }

                        if (Roi != Rectangle.Empty)
                        {
                            frame.ROI = Roi;
                        }

                        headPoints2 = ProcessFrame(frame, rbsk, lastPoint, movementDelta);
                    }

                    rbsk.Settings.BinaryThreshold = previousThreshold;

                    if (headPoints2 != null)
                    {
                        //We've got a good location
                        //double temp = results[maxIndex].Item2;
                        //results[maxIndex] = new Tuple<PointF[], double>(headPoints2, temp);
                        results[maxIndex].HeadPoints = headPoints2;
                        results[minIndex].HeadPoint = headPoints2[2];
                    }
                    else
                    {
                        int max = results.Keys.Max();
                        for (int i = maxIndex; i <= max; i++)
                        {
                            if (results.ContainsKey(i))
                            {
                                //double temp = results[i].Item2;
                                //results[i] = new Tuple<PointF[], double>(null, temp);
                                results[i].HeadPoints = null;
                                results[minIndex].HeadPoint = PointF.Empty;
                            }
                        }

                        break;
                    }

                    maxIndex++;
                    continue;
                }

                //Check against expected position
                PointF currentPoint = headPoints[2];

                if (lastPoint.Distance(currentPoint) < movementDelta)
                {
                    //Good point
                }
                else
                {
                    //Wrong point, search for another rbsk that falls within range
                    Video.SetFrame(maxIndex);
                    PointF[] headPoints2 = null;
                    using (Image<Bgr, byte> frame = Video.GetFrameImage())
                    {
                        if (frame == null)
                        {
                            break;
                        }

                        if (Roi != Rectangle.Empty)
                        {
                            frame.ROI = Roi;
                        }

                        headPoints2 = ProcessFrame(frame, rbsk, lastPoint, movementDelta);
                    }

                    if (headPoints2 != null)
                    {
                        //We've got a good location
                        //double temp = results[maxIndex].Item2;
                        //results[maxIndex] = new Tuple<PointF[], double>(headPoints2, temp);
                        results[maxIndex].HeadPoints = headPoints2;
                        results[minIndex].HeadPoint = headPoints2[2];
                    }
                    else
                    {
                        //No other rbsk falls within range, use alternative methods
                        //Console.WriteLine("Need to use alternative methods");
                        int max = results.Keys.Max();
                        for (int i = maxIndex; i <= max; i++)
                        {
                            if (results.ContainsKey(i))
                            {
                                //double temp = results[i].Item2;
                                //results[i] = new Tuple<PointF[], double>(null, temp);
                                results[i].HeadPoints = null;
                                results[minIndex].HeadPoint = PointF.Empty;
                            }
                        }

                        break;
                    }
                }

                maxIndex++;
            }

            //Generate mid point and orientation vector
            for (int i = 0; i < counter; i++)
            {
                ISingleFrameExtendedResults cResult = results[i];

                if (cResult.HeadPoints == null || !cResult.HeadPoints.Any())
                {
                    continue;
                }

                cResult.MidPoint = cResult.HeadPoints[1].MidPoint(cResult.HeadPoints[3]);
                cResult.Orientation = new Vector(cResult.HeadPoint.X - cResult.MidPoint.X, cResult.HeadPoint.Y - cResult.MidPoint.Y);
            }

            return results;
        }

        private double GetBestGapDistance(RBSK rbsk, Action<double> progressCallBack = null)
        {
            //Caluclate gap distnace if it hasn't been set
            //Auto Find the gap distance
            //Scan from 20 - 300, the range which gives us consistent results is the right one
            int start = 20;
            int end = 300;
            int interval = 1;
            Video.SetFrame(100);
            Image<Gray, Byte> firstFrame = Video.GetGrayFrameImage();
            Dictionary<int, PointF> nosePoints = new Dictionary<int, PointF>();

            for (int i = start; i <= end; i += interval)
            {
                if (Cancelled)
                {
                    return -1;
                }

                using (Image<Gray, Byte> filteredImage = firstFrame.SmoothMedian(5))
                using (Image<Gray, Byte> binaryImage = filteredImage.ThresholdBinary(new Gray(30), new Gray(255)))
                {
                    rbsk.Settings.GapDistance = i;
                    Point[] temp = null;
                    PointF[] mousePoints = RBSKService.RBSKParallel(binaryImage, rbsk, ref temp);

                    if (mousePoints != null)
                    {
                        //We've found a set of points for this gap distance, store it
                        nosePoints.Add(i, mousePoints[2]);
                    }
                }
                double progressValue = ((i - start) / ((double)end - start)) * 100;
                if (progressCallBack != null)
                {
                    progressCallBack(progressValue);
                }
            }

            const double threshold = 20;
            PointF? previousPoint = null;
            List<int> currentSelection = new List<int>();
            int bestCounter = 0;
            double bestDistanceSoFar = -1;

            foreach (KeyValuePair<int, PointF> kvp in nosePoints)
            {
                if (Cancelled)
                {
                    return -1;
                }

                PointF currentPoint = kvp.Value;

                //Do we have a value?
                if (previousPoint.HasValue)
                {
                    //Is the previous point within the threshold distance of the current point
                    if (currentPoint.Distance(previousPoint.Value) < threshold)
                    {
                        currentSelection.Add(kvp.Key);
                        previousPoint = currentPoint;
                    }
                    else
                    {
                        //We're not within the threshold, compare the current list to see if it's the best
                        if (currentSelection.Count > bestCounter)
                        {
                            bestCounter = currentSelection.Count;
                            bestDistanceSoFar = currentSelection.Average();
                        }

                        currentSelection.Clear();
                        previousPoint = null;
                    }
                }
                else
                {
                    previousPoint = currentPoint;
                }
            }

            if (currentSelection.Count > bestCounter)
            {
                bestDistanceSoFar = currentSelection.Average();
            }

            if (bestDistanceSoFar == -1)
            {
                bestDistanceSoFar = 100;
            }

            return bestDistanceSoFar;
        }

        private CvBlobDetector BlobDetector
        {
            get;
            set;
        }

        private PointF[] ProcessFrame(Image<Bgr, Byte> image, RBSK rbsk, out double waist, out double waistArea, out double waistArea2, out double waistArea3, out double waistArea4, out PointF centroid, out Point[] bodyContour, bool useBackground = false)
        {
            //Rectangle roi = Rectangle.Empty;

            //if (image.IsROISet)
            //{
            //    roi = image.ROI;
            //    image.ROI = Rectangle.Empty;
            //}

            //if (!Roi.IsEmpty)
            //{
            //    BackgroundImage.ROI = Roi;
            //}

            if (BackgroundImage != null && useBackground)
            {
                using (Image<Gray, Byte> grayImage = image.Convert<Gray, Byte>())
                //using (Image<Gray, Byte> filteredImage = grayImage.SmoothMedian(rbsk.Settings.FilterLevel))
                using (Image<Gray, Byte> binaryImage = grayImage.ThresholdBinary(new Gray(rbsk.Settings.BinaryThreshold), new Gray(255)))
                using (Image<Gray, Byte> backgroundNot = BackgroundImage.Not())
                using (Image<Gray, Byte> finalImage = binaryImage.Add(backgroundNot))
                using (Image<Gray, Byte> filteredImage = finalImage.SmoothMedian(rbsk.Settings.FilterLevel))
                {
                    //ImageViewer.Show(filteredImage);
                    //ImageViewer.Show(binaryImage);
                    PointF[] result = RBSKService.RBSK(filteredImage, rbsk);

                    IBodyDetection bodyDetection = ModelResolver.Resolve<IBodyDetection>();
                    bodyDetection.BinaryBackground = BackgroundImage;
                    bodyDetection.ThresholdValue = ThresholdValue;
                    
                    if (result == null)
                    {
                        waist = -1;
                        waistArea = -1;
                        waistArea2 = -1;
                        waistArea3 = -1;
                        waistArea4 = -1;
                        bodyDetection.GetBody(grayImage, out centroid, out bodyContour);
                        return null;
                    }

                    
                    //double vol2, vol3, vol4;
                    //image.

                    //if (!roi.IsEmpty)
                    //{
                    //    image.ROI = roi;
                    //}

                    //PointF dummy;
                    //ImageViewer.Show(finalImage);
                    bodyDetection.FindBody(grayImage, out waist, out waistArea, out waistArea2, out waistArea3, out waistArea4, out centroid, out bodyContour);
                    
                    //if (!centroid.IsEmpty)
                    //{
                    //    ImageViewer.Show(finalImage);
                    //    Console.WriteLine("Showing");
                    //}

                    //CvBlobs blobs = new CvBlobs();
                    //BlobDetector.Detect(finalImage, blobs);

                    //CvBlob mouseBlob = null;
                    //double maxArea = -1;
                    //foreach (var blob in blobs.Values)
                    //{
                    //    if (blob.Area > maxArea)
                    //    {
                    //        mouseBlob = blob;
                    //        maxArea = blob.Area;
                    //    }
                    //}
                    //waist = -1;
                    //waistArea = -1;
                    return result;
                }
            }

            waist = -1;
            waistArea = -1;
            waistArea2 = -1;
            waistArea3 = -1;
            waistArea4 = -1;
            centroid = PointF.Empty;
            bodyContour = null;
            return RBSKService.RBSK(image, rbsk);
        }

        private PointF[] ProcessFrame(Image<Bgr, Byte> image, RBSK rbsk, PointF previousPoint, double movementDelta, bool useBackground = false)
        {
            if (BackgroundImage != null && useBackground)
            {
                using (Image<Gray, Byte> grayImage = image.Convert<Gray, Byte>())
                using (Image<Gray, Byte> binaryImage = grayImage.ThresholdBinary(new Gray(rbsk.Settings.BinaryThreshold), new Gray(255)))
                using (Image<Gray, Byte> backgroundNot = BackgroundImage.Not())
                using (Image<Gray, Byte> binaryFinal = binaryImage.Add(backgroundNot))
                using (Image<Gray, Byte> finalImage = binaryFinal.SmoothMedian(rbsk.Settings.FilterLevel))
                {
                    PointF[] result = RBSKService.RBSK(finalImage, rbsk, previousPoint, movementDelta);
                    return result;
                }
            }


            return RBSKService.RBSK(image, rbsk, previousPoint, movementDelta);
        }

        public void Dispose()
        {
            if (BackgroundImage != null)
            {
                BackgroundImage.Dispose();
            }
        }

        private class HeadPointHolder
        {
            public int FrameNumber
            {
                get;
                set;
            }

            public PointF HeadPoint
            {
                get;
                set;
            }

            public double TotalDelta
            {
                get;
                set;
            }

            public HeadPointHolder(int frameNumber, PointF headPoint, double totalDelta)
            {
                FrameNumber = frameNumber;
                HeadPoint = headPoint;
                TotalDelta = totalDelta;
            }
        }
    }
}
