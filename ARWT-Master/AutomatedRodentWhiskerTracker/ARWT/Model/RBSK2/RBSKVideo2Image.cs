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

namespace ARWT.Model.RBSK2
{
    internal class RBSKVideo2Image : ModelObjectBase
    {
        //private IVideo m_Video;
        private Dictionary<int, ISingleFrameExtendedResults> m_HeadPoints;
        private Dictionary<int, Tuple<PointF[], double>> m_SecondPassHeadPoints;

        private double m_GapDistance;
        private bool m_Cancelled = false;
        private Image<Gray, Byte> m_BackgroundImage;
        private Rectangle m_Roi;

        public event RBSKVideoUpdateEventHandler ProgressUpdates;

        //public IVideo Video
        //{
        //    get
        //    {
        //        return m_Video;
        //    }
        //    set
        //    {
        //        if (Equals(m_Video, value))
        //        {
        //            return;
        //        }

        //        m_Video = value;

        //        MarkAsDirty();
        //    }
        //}

        private Image<Gray, byte> m_Image;
        public Image<Gray, byte> Image
        {
            get
            {
                return m_Image;
            }
            set
            {
                if (Equals(m_Image, value))
                {
                    return;
                }

                m_Image = value;

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

        //private Dictionary<int, IWhiskerCollection> m_WhiskerResults;
        //public Dictionary<int, IWhiskerCollection> WhiskerResults
        //{
        //    get
        //    {
        //        return m_WhiskerResults;
        //    }
        //    set
        //    {
        //        if (ReferenceEquals(m_WhiskerResults, value))
        //        {
        //            return;
        //        }

        //        m_WhiskerResults = value;

        //        MarkAsDirty();
        //    }
        //}

        //private Dictionary<int, IWhiskerCollection> m_WhiskerResultsAll;
        //public Dictionary<int, IWhiskerCollection> WhiskerResultsAll
        //{
        //    get
        //    {
        //        return m_WhiskerResultsAll;
        //    }
        //    set
        //    {
        //        if (ReferenceEquals(m_WhiskerResultsAll, value))
        //        {
        //            return;
        //        }

        //        m_WhiskerResultsAll = value;

        //        MarkAsDirty();
        //    }
        //}

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

        public RBSKVideo2Image()
        {
            BlobDetector = new CvBlobDetector();
        }

        public void Process()
        {
            if (Image == null)
            {
                return;
            }

            Console.WriteLine("Processing head points");
            HeadPoints = GenerateDictionary();
            Console.WriteLine("Processing Whiskers");
            ProcessWhiskers();

            Console.WriteLine("Finished");
        }

        const float ZoomRatio = 4;
        const float ScaleRatio = 2;

        private void ProcessWhiskers()
        {
            bool time = false;
            //bool showOnce = true;
            //Get quarter size of image around nose point
            int newWidth = (int)(VideoWidth / ZoomRatio);
            int newHeight = (int)(VideoHeight / ZoomRatio);

            IWhiskerDetector whiskerDetector = ModelResolver.Resolve<IWhiskerDetector>();
            //float scaleRatio = 4;
            //whiskerDetector.LineLength = 8*(int) scaleRatio;
            whiskerDetector.OrientationResolution = 1;
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

                //Video.SetFrame(i);

                //if (time)
                //{
                //    sw.Stop();
                //    Console.WriteLine("3) " + sw.ElapsedMilliseconds);
                //    sw.Restart();
                //}

                Inter interpolationType = Inter.Area;

                //using (Image<Gray, byte> frame = Video.GetGrayFrameImage())
                {
                    Image.ROI = zoomRect;
                    BackgroundImage.ROI = zoomRect;

                    //if (time)
                    //{
                    //    sw.Stop();
                    //    Console.WriteLine("4) " + sw.ElapsedMilliseconds);
                    //    sw.Restart();
                    //}

                    using (Image<Gray, byte> subImage = Image.Copy())
                    using (Image<Gray, byte> subBinary = BackgroundImage.Copy())
                    using (Image<Gray, byte> zoomedImage = subImage.Resize(ScaleRatio, interpolationType))
                    using (Image<Gray, byte> zoomedBinary = subBinary.Resize(ScaleRatio, interpolationType))
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
                            headPointsCorrected[j] = new PointF((currentFrame.HeadPoints[j].X - zoomRect.X)* ScaleRatio, (currentFrame.HeadPoints[j].Y - zoomRect.Y)* ScaleRatio);
                        }

                        for (int j = 0; j < bodyContourLength; j++)
                        {
                            bodyContourCorrected[j] = new Point((int) ((currentFrame.BodyContour[j].X - zoomRect.X)* ScaleRatio), (int) ((currentFrame.BodyContour[j].Y - zoomRect.Y)* ScaleRatio));
                        }

                        List<IWhiskerSegment>[] cWhisk = whiskerDetector.FindWhiskers(zoomedImage, zoomedBinary, headPointsCorrected, bodyContourCorrected, (int) ScaleRatio, currentFrame, null);
                        List<IWhiskerSegment> cWhiskLeft = cWhisk[0];
                        List<IWhiskerSegment> cWhiskRight = cWhisk[1];

                        //if (time)
                        //{
                        //    sw.Stop();
                        //    Console.WriteLine("6) " + sw.ElapsedMilliseconds);
                        //    sw.Restart();
                        //}

                        //IWhiskerSegment[] correctedWhiskers = new IWhiskerSegment[cWhisk.Length];
                        foreach (var whisker in cWhiskLeft)
                        {
                            whisker.X = (int)((whisker.X / ScaleRatio) + zoomRect.X);
                            whisker.Y = (int)((whisker.Y / ScaleRatio) + zoomRect.Y);
                            Point p1 = new Point((int)((whisker.Line.P1.X / ScaleRatio)) + zoomRect.X, (int)((whisker.Line.P1.Y / ScaleRatio) + zoomRect.Y));
                            Point p2 = new Point((int)((whisker.Line.P2.X / ScaleRatio)) + zoomRect.X, (int)((whisker.Line.P2.Y / ScaleRatio) + zoomRect.Y));
                            whisker.Line = new LineSegment2D(p1, p2);
                        }

                        foreach (var whisker in cWhiskRight)
                        {
                            whisker.X = (int)((whisker.X / ScaleRatio) + zoomRect.X);
                            whisker.Y = (int)((whisker.Y / ScaleRatio) + zoomRect.Y);
                            Point p1 = new Point((int)((whisker.Line.P1.X / ScaleRatio)) + zoomRect.X, (int)((whisker.Line.P1.Y / ScaleRatio) + zoomRect.Y));
                            Point p2 = new Point((int)((whisker.Line.P2.X / ScaleRatio)) + zoomRect.X, (int)((whisker.Line.P2.Y / ScaleRatio) + zoomRect.Y));
                            whisker.Line = new LineSegment2D(p1, p2);
                        }
                        IWhiskerCollection whiskerCollection = ModelResolver.Resolve<IWhiskerCollection>();
                        whiskerCollection.LeftWhiskers = cWhiskLeft.ToArray();
                        whiskerCollection.RightWhiskers = cWhiskRight.ToArray();
                        currentFrame.AllWhiskers = whiskerCollection;
                        //whiskers.Add(i, whiskerCollection);
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

            //WhiskerResultsAll = whiskers;
            //PostProcessWhiskers();
            //WhiskerResults = whiskers;
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

            Dictionary<int, ISingleFrameExtendedResults> results = new Dictionary<int, ISingleFrameExtendedResults>();

            VideoWidth = (int)Image.Width;
            VideoHeight = (int)Image.Height;



            //Video.Reset();
            int counter = 0;

            int frameCount = 1;
            //int frameCount = 70;

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


                //using (Image<Bgr, byte> frame = Video.GetFrameImage())
                {
                    double waist, waistArea, waistArea2, waistArea3, waistArea4;
                    PointF centroid;

                    //if (counter == 500)
                    //{
                    //    frame.ROI = new Rectangle(0,0,1,1);
                    //}
                    Point[] bodyContour;
                    PointF[] headPoints = ProcessFrame(Image, rbsk, out waist, out waistArea, out waistArea2, out waistArea3, out waistArea4, out centroid, out bodyContour, true);

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
                        ProgressUpdates(this, new RBSKVideoUpdateEvent(((double)counter/frameCount)/2));
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
                    //Video.SetFrame(minIndex);
                    PointF[] headPoints2 = null;
                    //using (Image<Bgr, byte> frame = Video.GetFrameImage())
                    {
                        //if (frame == null)
                        //{
                        //    break;
                        //}

                        //if (Roi != Rectangle.Empty)
                        //{
                        //    frame.ROI = Roi;
                        //}

                        headPoints2 = ProcessFrame(Image, rbsk, lastPoint, movementDelta);
                    }

                    rbsk.Settings.BinaryThreshold = previousThreshold;

                    if (headPoints2 != null)
                    {
                        //We've got a good location
                        //double temp = results[minIndex].CentroidSize;
                        results[minIndex].HeadPoints = headPoints2;
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

                    //Video.SetFrame(minIndex);
                    PointF[] headPoints2 = null;
                    //using (Image<Bgr, byte> frame = Video.GetFrameImage())
                    {
                        //if (frame == null)
                        //{
                        //    break;
                        //}

                        //if (Roi != Rectangle.Empty)
                        //{
                        //    frame.ROI = Roi;
                        //}

                        headPoints2 = ProcessFrame(Image, rbsk, lastPoint, movementDelta);
                    }

                    if (headPoints2 != null)
                    {
                        //We've got a good location
                        //double temp = results[maxIndex].Item2;
                        //results[minIndex] = new Tuple<PointF[], double>(headPoints2, temp);
                        results[minIndex].HeadPoints = headPoints2;
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
                            }
                        }

                        break;
                    }
                }

                minIndex--;
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
                    //Video.SetFrame(maxIndex);
                    PointF[] headPoints2 = null;
                    //using (Image<Bgr, byte> frame = Video.GetFrameImage())
                    {
                        //if (frame == null)
                        //{
                        //    break;
                        //}

                        //if (Roi != Rectangle.Empty)
                        //{
                        //    frame.ROI = Roi;
                        //}

                        headPoints2 = ProcessFrame(Image, rbsk, lastPoint, movementDelta);
                    }

                    rbsk.Settings.BinaryThreshold = previousThreshold;

                    if (headPoints2 != null)
                    {
                        //We've got a good location
                        //double temp = results[maxIndex].Item2;
                        //results[maxIndex] = new Tuple<PointF[], double>(headPoints2, temp);
                        results[maxIndex].HeadPoints = headPoints2;
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
                    //Video.SetFrame(maxIndex);
                    PointF[] headPoints2 = null;
                    //using (Image<Bgr, byte> frame = Video.GetFrameImage())
                    {
                        headPoints2 = ProcessFrame(Image, rbsk, lastPoint, movementDelta);
                    }

                    if (headPoints2 != null)
                    {
                        //We've got a good location
                        //double temp = results[maxIndex].Item2;
                        //results[maxIndex] = new Tuple<PointF[], double>(headPoints2, temp);
                        results[maxIndex].HeadPoints = headPoints2;
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
                            }
                        }

                        break;
                    }
                }

                maxIndex++;
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
            //Video.SetFrame(100);
            //Image<Gray, Byte> firstFrame = Video.GetGrayFrameImage();
            Dictionary<int, PointF> nosePoints = new Dictionary<int, PointF>();

            for (int i = start; i <= end; i += interval)
            {
                if (Cancelled)
                {
                    return -1;
                }

                using (Image<Gray, Byte> filteredImage = Image.SmoothMedian(5))
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

        private PointF[] ProcessFrame(Image<Gray, Byte> grayImage, RBSK rbsk, out double waist, out double waistArea, out double waistArea2, out double waistArea3, out double waistArea4, out PointF centroid, out Point[] bodyContour, bool useBackground = false)
        {
            //Rectangle roi = Rectangle.Empty;

            //if (image.IsROISet)
            //{
            //    roi = image.ROI;
            //    image.ROI = Rectangle.Empty;
            //}

            if (BackgroundImage != null && useBackground)
            {
                //using (Image<Gray, Byte> grayImage = image.Convert<Gray, Byte>())
                using (Image<Gray, Byte> binaryImage = grayImage.ThresholdBinary(new Gray(rbsk.Settings.BinaryThreshold), new Gray(255)))
                using (Image<Gray, Byte> backgroundNot = BackgroundImage.Not())
                using (Image<Gray, Byte> binaryFinal = binaryImage.Add(backgroundNot))
                using (Image<Gray, Byte> finalImage = binaryFinal.SmoothMedian(rbsk.Settings.FilterLevel))
                //using (Image<Gray, Byte> finalImageNot = finalImage.Not())
                {
                    //ImageViewer.Show(finalImage);
                    ImageViewer.Show(finalImage);
                    PointF[] result = RBSKService.RBSK(finalImage, rbsk);

                    IBodyDetection bodyDetection = ModelResolver.Resolve<IBodyDetection>();
                    bodyDetection.BinaryBackground = BackgroundImage;
                    //bodyDetection.ThresholdValue = ThresholdValue;
                    
                    if (result == null)
                    {
                        waist = -1;
                        waistArea = -1;
                        waistArea2 = -1;
                        waistArea3 = -1;
                        waistArea4 = -1;
                        //bodyDetection.GetBody(finalImage, out centroid, out bodyContour);
                        bodyContour = null;
                        centroid = PointF.Empty;
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
                    bodyDetection.FindBody(finalImage, out waist, out waistArea, out waistArea2, out waistArea3, out waistArea4, out centroid, out bodyContour);
                    
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
            return null;
        }

        private PointF[] ProcessFrame(Image<Gray, Byte> grayImage, RBSK rbsk, PointF previousPoint, double movementDelta, bool useBackground = false)
        {
            if (BackgroundImage != null && useBackground)
            {
                //using (Image<Gray, Byte> grayImage = image.Convert<Gray, Byte>())
                using (Image<Gray, Byte> binaryImage = grayImage.ThresholdBinary(new Gray(rbsk.Settings.BinaryThreshold), new Gray(255)))
                using (Image<Gray, Byte> backgroundNot = BackgroundImage.Not())
                using (Image<Gray, Byte> binaryFinal = binaryImage.Add(backgroundNot))
                using (Image<Gray, Byte> finalImage = binaryFinal.SmoothMedian(rbsk.Settings.FilterLevel))
                {
                    PointF[] result = RBSKService.RBSK(finalImage, rbsk, previousPoint, movementDelta);
                    return result;
                }
            }


            return null;//RBSKService.RBSK(image, rbsk, previousPoint, movementDelta);
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
