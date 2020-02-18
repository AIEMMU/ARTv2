using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ArtLibrary.ModelInterface.Video;
using ArtLibrary.Services.RBSK;
using ARWT.Commands;
using ARWT.Extensions;
using ARWT.Model.Whiskers;
using ARWT.ModelInterface.RBSK2;
using ARWT.ModelInterface.Whiskers;
using ARWT.Resolver;
using ARWT.Services;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace ARWT.ViewModel.NewSession
{
    public class NewWhiskerSessionViewModel : WindowBaseModel
    {
        private ActionCommand _PreviewCommand;
        private ActionCommand _ShowStepsCommand;
        private ActionCommand _OkCommand;
        private ActionCommand _CancelCommand;

        public ActionCommand PreviewCommand
        {
            get
            {
                return _PreviewCommand ?? (_PreviewCommand = new ActionCommand()
                {
                    ExecuteAction = Preview,
                    CanExecuteAction = () => !HeadPoint.IsEmpty,
                });
            }
        }

        public ActionCommand ShowStepsCommand
        {
            get
            {
                return _ShowStepsCommand ?? (_ShowStepsCommand = new ActionCommand()
                {
                    ExecuteAction = ShowSteps
                });
            }
        }

        public ActionCommand OkCommand
        {
            get
            {
                return _OkCommand ?? (_OkCommand = new ActionCommand()
                {
                    ExecuteAction = Ok,
                    CanExecuteAction = CanOk,
                });
            }
        }

        public ActionCommand CancelCommand
        {
            get
            {
                return _CancelCommand ?? (_CancelCommand = new ActionCommand()
                {
                    ExecuteAction = Cancel
                });
            }
        }

        private ImageSource _DisplayImage;
        public ImageSource DisplayImage
        {
            get
            {
                return _DisplayImage;
            }
            set
            {
                if (ReferenceEquals(_DisplayImage, value))
                {
                    return;
                }

                _DisplayImage = value;

                NotifyPropertyChanged();
            }
        }

        private Image<Gray, byte> _WorkingImage;
        public Image<Gray, byte> WorkingImage
        {
            get
            {
                return _WorkingImage;
            }
            set
            {
                if (ReferenceEquals(_WorkingImage, value))
                {
                    return;
                }

                if (WorkingImage != null)
                {
                    WorkingImage.Dispose();
                }

                _WorkingImage = value;

                NotifyPropertyChanged();
            }
        }

        private PointF[] _HeadPoints;
        public PointF[] HeadPoints
        {
            get
            {
                return _HeadPoints;
            }
            set
            {
                if (Equals(_HeadPoints, value))
                {
                    return;
                }

                _HeadPoints = value;

                NotifyPropertyChanged();
            }
        }


        private PointF _HeadPoint;
        public PointF HeadPoint
        {
            get
            {
                return _HeadPoint;
            }
            set
            {
                if (Equals(_HeadPoint, value))
                {
                    return;
                }

                _HeadPoint = value;

                NotifyPropertyChanged();

                PreviewCommand.RaiseCanExecuteChangedNotification();
            }
        }

        private Point[] _BodyContour;
        public Point[] BodyContour
        {
            get
            {
                return _BodyContour;
            }
            set
            {
                if (Equals(_BodyContour, value))
                {
                    return;
                }

                _BodyContour = value;

                NotifyPropertyChanged();
            }
        }

        private IVideo _Video;
        public IVideo Video
        {
            get
            {
                return _Video;
            }
            set
            {
                if (Equals(_Video, value))
                {
                    return;
                }

                _Video = value;

                NotifyPropertyChanged();
            }
        }

        private IRBSKVideo2 _Rbsk;
        public IRBSKVideo2 Rbsk
        {
            get
            {
                return _Rbsk;
            }
            set
            {
                if (Equals(_Rbsk, value))
                {
                    return;
                }

                _Rbsk = value;

                NotifyPropertyChanged();
            }
        }


        private bool _PreviewGenerated = false;
        public bool PreviewGenerated
        {
            get
            {
                return _PreviewGenerated;
            }
            set
            {
                if (Equals(_PreviewGenerated, value))
                {
                    return;
                }

                _PreviewGenerated = value;

                NotifyPropertyChanged();
                OkCommand.RaiseCanExecuteChangedNotification();
            }
        }

        private string _FileName;
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                if (Equals(_FileName, value))
                {
                    return;
                }

                _FileName = value;

                NotifyPropertyChanged();
            }
        }

        private int _MaxFrame;
        public int MaxFrame
        {
            get
            {
                return _MaxFrame;
            }
            set
            {
                if (Equals(_MaxFrame, value))
                {
                    return;
                }

                _MaxFrame = value;

                NotifyPropertyChanged();
            }
        }

        private int _FrameNumber = -1;
        public int FrameNumber
        {
            get
            {
                return _FrameNumber;
            }
            set
            {
                if (Equals(_FrameNumber, value))
                {
                    return;
                }

                _FrameNumber = value;

                NotifyPropertyChanged();

                UpdateFrameNumber(value);
            }
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

                NotifyPropertyChanged();
            }
        }

        private IFootVideoSettings _FootSettings;
        public IFootVideoSettings FootSettings
        {
            get
            {
                return _FootSettings;
            }
            set
            {
                if (Equals(_FootSettings, value))
                {
                    return;
                }

                _FootSettings = value;

                NotifyPropertyChanged();
            }
        }

        public double CropScaleFactor
        {
            get
            {
                return WhiskerSettings.CropScaleFactor;
            }
            set
            {
                if (Equals(WhiskerSettings.CropScaleFactor, value))
                {
                    return;
                }

                WhiskerSettings.CropScaleFactor = value;

                NotifyPropertyChanged();
            }
        }

        public float ResolutionIncreaseScaleFactor
        {
            get
            {
                return WhiskerSettings.ResolutionIncreaseScaleFactor;
            }
            set
            {
                if (Equals(WhiskerSettings.ResolutionIncreaseScaleFactor, value))
                {
                    return;
                }

                WhiskerSettings.ResolutionIncreaseScaleFactor = value;

                NotifyPropertyChanged();
            }
        }

        public Inter InterpolationType
        {
            get
            {
                return WhiskerSettings.InterpolationType;
            }
            set
            {
                if (Equals(WhiskerSettings.InterpolationType, value))
                {
                    return;
                }

                WhiskerSettings.InterpolationType = value;

                NotifyPropertyChanged();
            }
        }

        private IWhiskerCollection _Whiskers;
        public IWhiskerCollection Whiskers
        {
            get
            {
                return _Whiskers;
            }
            set
            {
                if (Equals(_Whiskers, value))
                {
                    return;
                }

                _Whiskers = value;

                NotifyPropertyChanged();
            }
        }

        private IWhiskerCollection _FinalWhiskers;
        public IWhiskerCollection FinalWhiskers
        {
            get
            {
                return _FinalWhiskers;
            }
            set
            {
                if (Equals(_FinalWhiskers, value))
                {
                    return;
                }

                _FinalWhiskers = value;

                NotifyPropertyChanged();
            }
        }
        
        public bool RemoveDuds
        {
            get
            {
                return WhiskerSettings.RemoveDuds;
            }
            set
            {
                if (Equals(WhiskerSettings.RemoveDuds, value))
                {
                    return;
                }

                WhiskerSettings.RemoveDuds = value;

                NotifyPropertyChanged();
            }
        }

        public byte LineThreshold
        {
            get
            {
                return WhiskerSettings.LineMinIntensity;
            }
            set
            {
                if (Equals(WhiskerSettings.LineMinIntensity))
                {
                    return;
                }

                WhiskerSettings.LineMinIntensity = value;

                NotifyPropertyChanged();
            }
        }

        public int LowerBound
        {
            get
            {
                return WhiskerSettings.LowerBound;
            }
            set
            {
                if (Equals(WhiskerSettings.LowerBound, value))
                {
                    return;
                }

                WhiskerSettings.LowerBound = value;

                NotifyPropertyChanged();
            }
        }

        public int UpperBound
        {
            get
            {
                return WhiskerSettings.UpperBound;
            }
            set
            {
                if (Equals(WhiskerSettings.UpperBound, value))
                {
                    return;
                }

                WhiskerSettings.UpperBound = value;

                NotifyPropertyChanged();
            }
        }

        private string _WorkingFile;
        public string WorkingFile
        {
            get
            {
                return _WorkingFile;
            }
            set
            {
                if (Equals(_WorkingFile, value))
                {
                    return;
                }

                _WorkingFile = value;

                NotifyPropertyChanged();
            }
        }


        public ObservableCollection<Inter> InterpolationTypes
        {
            get;
        } = new ObservableCollection<Inter>()
        {
            Inter.Area,
            Inter.Cubic,
            Inter.Lanczos4,
            Inter.Linear,
            Inter.Nearest,
        };

        public NewWhiskerSessionViewModel(IRBSKVideo2 video, string videoFile)
        {
            Rbsk = video;
            Video = ModelResolver.Resolve<IVideo>();
            Video.SetVideo(videoFile);
            
            MaxFrame = Video.FrameCount - 1;
            FrameNumber = 0;
            WhiskerSettings = ModelResolver.Resolve<IWhiskerVideoSettings>();
            WhiskerSettings = Rbsk.WhiskerSettings;
           
        }

        private void UpdateFrameNumber(int frameNumber)
        {
            PreviewGenerated = false;
            FinalWhiskers = null;
            Video.SetFrame(frameNumber);
            UpdateFrameImage();
        }

        private void UpdateFrameImage()
        {
            WorkingImage = Video.GetGrayFrameImage();
            PointF[] headPoints;
            Point[] bodyPoints;

            //WorkingImage.ROI = Rbsk.Roi;
            //Rbsk.Roi = _Rbsk.Roi;
            Rbsk.GetHeadAndBody(WorkingImage.Convert<Bgr, byte>(), out headPoints, out bodyPoints);
            Rbsk.Roi = _Rbsk.Roi; //MAY NEED TO CHANGE THIS 
            HeadPoints = headPoints;
            if (headPoints == null)
            {
                HeadPoint = PointF.Empty;
            }
            else
            {

                HeadPoint = headPoints[2];
            }
            
            BodyContour = bodyPoints;
            DrawAndDisplayImage();
        }

        private void DrawAndDisplayImage()
        {
            if (HeadPoint.IsEmpty)
            {
                using (Image<Bgr, byte> img = WorkingImage.Convert<Bgr, byte>())
                {
                    DisplayImage = ImageService.ToBitmapSource(img);
                }

                return;
            }

            using (Image<Bgr, byte> img = WorkingImage.Convert<Bgr, byte>())
            {
                Point[] bodyPoints = addROI(BodyContour); 
                PointF[] headPoints = addROI(HeadPoints);
                img.DrawPolyline(bodyPoints, true, new Bgr(Color.Yellow));
                img.Draw(new CircleF(headPoints[2], 2), new Bgr(Color.Red));
                PointF midPoint = headPoints[1].MidPoint(headPoints[3]);
                img.Draw(new LineSegment2DF(midPoint, headPoints[2]), new Bgr(Color.Red), 1);

                if (FinalWhiskers != null)
                {
                    if (FinalWhiskers.LeftWhiskers != null && FinalWhiskers.LeftWhiskers.Any())
                    {
                        //currentFrame.Draw(cWhiskers.LeftWhiskers[5].Line, new Bgr(Color.White), 1);

                        foreach (IWhiskerSegment whisker in FinalWhiskers.LeftWhiskers)
                        {
                            Color color = Color.White;
                            LineSegment2D line = addROI(whisker.Line);
                            img.Draw(line, new Bgr(color), 1);
                        }

                    }

                    if (FinalWhiskers.RightWhiskers != null && FinalWhiskers.RightWhiskers.Any())
                    {
                        foreach (IWhiskerSegment whisker in FinalWhiskers.RightWhiskers)
                        {
                            Color color = Color.White;
                            LineSegment2D line = addROI(whisker.Line);
                            img.Draw(line, new Bgr(color), 1);
                        }
                    }
                }

                DisplayImage = ImageService.ToBitmapSource(img);
            }
        }

        private LineSegment2D addROI(LineSegment2D line)
        {
            LineSegment2D points = line;
            points.P1  = new Point(_Rbsk.Roi.X + points.P1.X, _Rbsk.Roi.Y + points.P1.Y);
            points.P2 = new Point(_Rbsk.Roi.X + points.P2.X, _Rbsk.Roi.Y + points.P2.Y);
            return points;

        }

        private PointF[] addROI(PointF[] Points)
        {
            PointF[] points = new PointF[Points.Length];
            ;
            for (int i = 0; i < Points.Length; i++)
            {
                points[i].X = Points[i].X + +_Rbsk.Roi.X;
                points[i].Y = Points[i].Y + +_Rbsk.Roi.Y;
            }

            return points;
        }
        private Point[] addROI(Point[] Points)
        {
            Point[] points = new Point[Points.Count()];
            ;
            for (int i = 0; i < Points.Count(); i++)
            {
                points[i].X = Points[i].X + +_Rbsk.Roi.X;
                points[i].Y = Points[i].Y + +_Rbsk.Roi.Y;
            }

            return points;
        }

        private void Preview()
        {
            using (Image<Gray, byte> gray = WorkingImage.Convert<Gray, byte>())
            {
                Whiskers = Rbsk.ProcessWhiskersForSingleFrame(gray, HeadPoints, BodyContour);

                if (WhiskerSettings.RemoveDuds)
                {
                    FinalWhiskers = ModelResolver.Resolve<IWhiskerCollection>();
                    PointF midPoint = HeadPoints[1].MidPoint(HeadPoints[3]);
                    Vector orientation = new Vector(HeadPoint.X - midPoint.X, HeadPoint.Y - midPoint.Y);
                    PostProcessWhiskers2(midPoint, orientation, Whiskers, FinalWhiskers);
                }
                else
                {
                    FinalWhiskers = Whiskers;
                }
            }
                

            PreviewGenerated = true;
            DrawAndDisplayImage();
        }

        private void PostProcessWhiskers2(PointF midPoint, Vector orientation, IWhiskerCollection whiskers, IWhiskerCollection finalWhiskers)
        {
            IWhiskerDetector wd = ModelResolver.Resolve<IWhiskerDetector>();
            double minAngleDelta = 15;

            //Whisker angle is measured against horizontal
            //Vector horiztonalVec = new Vector(1, 0);
            //double headAngle = Vector.AngleBetween(orientation, horiztonalVec);

            //return;

            if (whiskers.LeftWhiskers != null && whiskers.LeftWhiskers.Any())
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
        }

        private void ShowSteps()
        {
            
        }

        private void Ok()
        {
            ExitResult = WindowExitResult.Ok;
            CloseWindow();
        }

        private bool CanOk()
        {
            return PreviewGenerated;
        }

        private void Cancel()
        {
            ExitResult = WindowExitResult.Cancel;
            CloseWindow();
        }
    }
}
