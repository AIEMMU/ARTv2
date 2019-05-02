using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using ArtLibrary.Model.Behaviours;
using ArtLibrary.Model.Boundries;
using ArtLibrary.Model.Datasets;
using ArtLibrary.Model.Results;
using ArtLibrary.Model.XmlClasses;
using ArtLibrary.ModelInterface.Behaviours;
using ArtLibrary.ModelInterface.BodyDetection;
using ArtLibrary.ModelInterface.Boundries;
using ArtLibrary.ModelInterface.Datasets;
using ArtLibrary.ModelInterface.Datasets.Types;
using ArtLibrary.ModelInterface.RBSK;
using ArtLibrary.ModelInterface.Results;
using ArtLibrary.ModelInterface.Results.Behaviour;
using ArtLibrary.ModelInterface.Results.Behaviour.Movement;
using ArtLibrary.ModelInterface.Results.Behaviour.Rotation;
using ArtLibrary.ModelInterface.Smoothing;
using ArtLibrary.ModelInterface.Video;
using ArtLibrary.ModelInterface.VideoSettings;
using ArtLibrary.Services.Mouse;
using ArtLibrary.Services.RBSK;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using ARWT.Commands;
using ARWT.Extensions;
using ARWT.Model.Analysis;
using ARWT.Model.Bezier;
using ARWT.Model.MWA;
using ARWT.Model.RBSK2;
using ARWT.Model.Results;
using ARWT.Model.Smoothing;
using ARWT.Model.Whiskers;
using ARWT.ModelInterface.Bezier;
using ARWT.ModelInterface.RBSK2;
using ARWT.ModelInterface.Results;
using ARWT.ModelInterface.Whiskers;
using ARWT.Resolver;
using ARWT.Services;
using ARWT.View;
using ARWT.View.BatchProcess;
using ARWT.View.NewSession;
using ARWT.View.Progress;
using ARWT.View.Results;
using ARWT.ViewModel;
using ARWT.ViewModel.BatchProcess;
using ARWT.ViewModel.Behaviours;
using ARWT.ViewModel.NewSession;
using ARWT.ViewModel.Progress;
using BrettFFT = RobynsWhiskerTracker.Services.Maths.BrettFFT;
using IRBSKVideo = ARWT.ModelInterface.RBSK2.IRBSKVideo2;
using PointExtenstion = ArtLibrary.Extensions.PointExtenstion;
using Point = System.Drawing.Point;

namespace ARWT.ViewModel
{
    public class MainWindowViewModel : WindowBaseModel
    {
        private ActionCommand m_NewCommand;
        public ActionCommand NewCommand
        {
            get
            {
                return m_NewCommand ?? (m_NewCommand = new ActionCommand()
                {
                    ExecuteAction = NewSession,
                });
            }
        }

        private ActionCommand m_OpenCommand;

        public ActionCommand OpenCommand
        {
            get
            {
                return m_OpenCommand ?? (m_OpenCommand = new ActionCommand()
                {
                    ExecuteAction = OpenFile
                });
            }
        }

        private ActionCommand m_ReviewData;
        public ActionCommand ReviewData
        {
            get
            {
                return m_ReviewData ?? (m_ReviewData = new ActionCommand()
                {
                    ExecuteAction = () => SaveFile(),
                    CanExecuteAction = CanSaveFile
                });
            }
        }

        private ActionCommand m_SaveCommand;
        public ActionCommand SaveCommand
        {
            get
            {
                return m_SaveCommand ?? (m_SaveCommand = new ActionCommand()
                {
                    ExecuteAction = () => SaveFile(),
                    CanExecuteAction = CanSaveFile
                });
            }
        }

        private ActionCommand m_WhiskerDebugCommand;
        public ActionCommand WhiskerDebugCommand
        {
            get
            {
                return m_WhiskerDebugCommand ?? (m_WhiskerDebugCommand = new ActionCommand()
                {
                    ExecuteAction = WhiskerDebug
                });
            }
        }

        private ActionCommand _BatchProcessCommand;

        public ActionCommand BatchProcessCommand
        {
            get
            {
                return _BatchProcessCommand ?? (_BatchProcessCommand = new ActionCommand()
                {
                    ExecuteAction = OpenBatchProcessWindow
                });
            }
        }

        

        private IVideoSettings m_VideoSettings;
        public IVideoSettings VideoSettings
        {
            get
            {
                return m_VideoSettings;
            }
            set
            {
                if (Equals(m_VideoSettings, value))
                {
                    return;
                }

                m_VideoSettings = value;

                NotifyPropertyChanged();
            }
        }

        public IWhiskerVideoSettings WhiskerSettings
        {
            get;
            set;
        }
        public IFootVideoSettings FootSettings
        {
            get;
            set;
        }

        private IVideo m_Video;
        public IVideo Video
        {
            get
            {
                return m_Video;
            }
            private set
            {
                if (ReferenceEquals(m_Video, value))
                {
                    return;
                }

                if (m_Video != null)
                {
                    m_Video.Dispose();
                }

                m_Video = value;

                NotifyPropertyChanged();

                SaveCommand.RaiseCanExecuteChangedNotification();
            }
        }

        private string _TitleName;
        public string TitleName
        {
            get
            {
                return _TitleName;
            }
            set
            {
                if (Equals(_TitleName, value))
                {
                    return;
                }

                _TitleName = value;

                NotifyPropertyChanged();
            }
        }


        private int m_FrameCount;
        public int FrameCount
        {
            get
            {
                return m_FrameCount;
            }
            private set
            {
                if (Equals(m_FrameCount, value))
                {
                    return;
                }

                m_FrameCount = value;

                NotifyPropertyChanged();
            }
        }

        private int m_CurrentFrame;
        public int CurrentFrame
        {
            get
            {
                return m_CurrentFrame;
            }
            set
            {
                if (Equals(m_CurrentFrame, value))
                {
                    return;
                }

                m_CurrentFrame = value;

                NotifyPropertyChanged();

                UpdateDisplayImage();
                FrameNumberDisplay = "Frame: " + CurrentFrame;
            }
        }

        private string m_FrameNumberDisplay;
        public string FrameNumberDisplay
        {
            get
            {
                return m_FrameNumberDisplay;
            }
            set
            {
                if (Equals(m_FrameNumberDisplay, value))
                {
                    return;
                }

                m_FrameNumberDisplay = value;

                NotifyPropertyChanged();
            }
        }

        private bool m_SliderEnabled;
        public bool SliderEnabled
        {
            get
            {
                return m_SliderEnabled;
            }
            private set
            {
                if (Equals(m_SliderEnabled, value))
                {
                    return;
                }

                m_SliderEnabled = value;

                NotifyPropertyChanged();
            }
        }

        private bool m_VideoLoaded;
        public bool VideoLoaded
        {
            get
            {
                return m_VideoLoaded;
            }
            set
            {
                if (Equals(m_VideoLoaded, value))
                {
                    return;
                }

                m_VideoLoaded = value;

                NotifyPropertyChanged();
                //NextFrameCommand.RaiseCanExecuteChangedNotification();
                //SaveArtFileCommand.RaiseCanExecuteChangedNotification();
                //BodyTestCommand.RaiseCanExecuteChangedNotification();
                //ValidateCommand.RaiseCanExecuteChangedNotification();
            }
        }

        private PointF[] m_MotionTrack;
        public PointF[] MotionTrack
        {
            get
            {
                return m_MotionTrack;
            }
            set
            {
                if (ReferenceEquals(m_MotionTrack, value))
                {
                    return;
                }

                m_MotionTrack = value;

                NotifyPropertyChanged();
                ExportRawDataCommand.RaiseCanExecuteChangedNotification();
                //ExportInteractionsCommand.RaiseCanExecuteChangedNotification();
            }
        }

        private ActionCommand _ExportRawDataCommand;
        public ActionCommand ExportRawDataCommand
        {
            get
            {
                return _ExportRawDataCommand ?? (_ExportRawDataCommand = new ActionCommand()
                {
                    ExecuteAction = ExportRawData,
                    CanExecuteAction = () => MotionTrack != null && MotionTrack.Any()
                });
            }
        }

        private Vector[] m_OrientationTrack;
        public Vector[] OrientationTrack
        {
            get
            {
                return m_OrientationTrack;
            }
            set
            {
                if (ReferenceEquals(m_OrientationTrack, value))
                {
                    return;
                }

                m_OrientationTrack = value;

                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<BoundaryBaseViewModel> m_Boundries; 
        public ObservableCollection<BoundaryBaseViewModel> Boundries
        {
            get
            {
                return m_Boundries;
            }
            set
            {
                if (ReferenceEquals(m_Boundries, value))
                {
                    return;
                }

                m_Boundries = value;

                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<BehaviourHolderViewModel> m_Events; 
        public ObservableCollection<BehaviourHolderViewModel> Events
        {
            get
            {
                return m_Events;
            }
            set
            {
                if (ReferenceEquals(m_Events, value))
                {
                    return;
                }

                m_Events = value;

                NotifyPropertyChanged();
            }
        }

        private Dictionary<BoundaryBaseViewModel, List<BehaviourHolderViewModel>> m_InteractingBoundries; 
        public Dictionary<BoundaryBaseViewModel, List<BehaviourHolderViewModel>> InteractingBoundries
        {
            get
            {
                return m_InteractingBoundries;
            }
            set
            {
                if (ReferenceEquals(m_InteractingBoundries, value))
                {
                    return;
                }

                m_InteractingBoundries = value;

                NotifyPropertyChanged();
            }
        }

        private Dictionary<int, ISingleFrameExtendedResults> m_HeadPoints; 
        public Dictionary<int, ISingleFrameExtendedResults> Results
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

                NotifyPropertyChanged();

                ShowDataCommand.RaiseCanExecuteChangedNotification();

                if (Video != null)
                {
                    TitleName = $"- {Video.FilePath}";
                }
                else
                {
                    TitleName = String.Empty;
                }
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

        //        NotifyPropertyChanged();
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

        //        NotifyPropertyChanged();
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
                if (ReferenceEquals(m_AllRects, value))
                {
                    return;
                }

                m_AllRects = value;

                NotifyPropertyChanged();
            }
        }

        private string m_WorkingFile;
        public string WorkingFile
        {
            get
            {
                return m_WorkingFile;
            }
            set
            {
                if (Equals(m_WorkingFile, value))
                {
                    return;
                }

                m_WorkingFile = value;

                NotifyPropertyChanged();
            }
        }

        private string m_WorkingArwtFile;
        public string WorkingArwtFile
        {
            get
            {
                return m_WorkingArwtFile;
            }
            set
            {
                if (Equals(m_WorkingArwtFile, value))
                {
                    return;
                }

                m_WorkingArwtFile = value;

                NotifyPropertyChanged();
            }
        }


        private BitmapSource m_Image;
        public BitmapSource Image
        {
            get
            {
                return m_Image;
            }
            set
            {
                if (ReferenceEquals(m_Image, value))
                {
                    return;
                }

                m_Image = value;

                NotifyPropertyChanged();
            }
        }

        private Rectangle m_ROI;
        public Rectangle ROI
        {
            get
            {
                return m_ROI;
            }
            set
            {
                if (Equals(m_ROI, value))
                {
                    return;
                }

                m_ROI = value;

                NotifyPropertyChanged();
            }
        }

        private ActionCommand _ShowDataCommand;
        public ActionCommand ShowDataCommand
        {
            get
            {
                return _ShowDataCommand ?? (_ShowDataCommand = new ActionCommand()
                {
                    ExecuteAction = ShowData,
                    CanExecuteAction = () => Results != null && Results.Any(),
                });
            }
        }

        private void ShowData()
        {
            
            IMovementBehaviour movementBehaviour;
            List<Tuple<int, int>> rotationFrames;
            IMouseDataExtendedResult result = GetMouseData(Boundries.Select(x => x.Model).ToArray(), VideoSettings.GapDistance, VideoSettings.ThresholdValue, VideoSettings.ThresholdValue2, 0, Results.Count - 1, false, Video.FrameRate, 1, out movementBehaviour, out rotationFrames);

            SingleVideoExportViewModel vm = new SingleVideoExportViewModel();
            vm.LoadData(result);

            SingleVideoExport v = new SingleVideoExport();
            v.DataContext = vm;
            v.ShowDialog();
        }

        private void LogicGateImages()
        {
            using (Image<Bgr, byte> img1 = new Image<Bgr, byte>(@"D:\OneDrive\Thesis\images\ExampleImage.png"))
            using (Image<Bgr, byte> median1 = img1.SmoothMedian(3))
            using (Image<Bgr, byte> median2 = img1.SmoothMedian(5))
            {
                median1.Save(@"D:\OneDrive\Thesis\images\ImgProc\Median1.png");
                median2.Save(@"D:\OneDrive\Thesis\images\ImgProc\Median2.png");
            }


            //using (Image<Gray, byte> grayImg = img1.Convert<Gray, byte>())
            //using (Image<Gray, byte> threshold1 = grayImg.ThresholdBinary(new Gray(150), new Gray(255)))
            //using (Image<Gray, byte> threshold2 = grayImg.ThresholdBinary(new Gray(100), new Gray(255)))
            //{
            //    grayImg.Save(@"D:\OneDrive\Thesis\images\ImgProc\GrayImg.png");
            //    threshold1.Save(@"D:\OneDrive\Thesis\images\ImgProc\Threshold1.png");
            //    threshold2.Save(@"D:\OneDrive\Thesis\images\ImgProc\Threshold2.png");
            //}

            //using (Image<Gray, byte> img1 = new Image<Gray, byte>(1024, 1024))
            //using (Image<Gray, byte> img2 = new Image<Gray, byte>(1024, 1024))
            //{
            //    img1.SetValue(new Gray(0));
            //    img1.Draw(new CircleF(new PointF(341, 512), 256), new Gray(255), -1);
            //    img2.SetValue(new Gray(0));
            //    img2.Draw(new CircleF(new PointF(682, 512), 256), new Gray(255), -1);

            //    using (Image<Gray, byte> andImg = img1.And(img2))
            //    using (Image<Gray, byte> orImg = img1.Or(img2))
            //    using (Image<Gray, byte> notImg = img1.Not())
            //    {
            //        img1.Save(@"D:\OneDrive\Thesis\images\ImgProc\img1.png");
            //        img2.Save(@"D:\OneDrive\Thesis\images\ImgProc\img2.png");
            //        andImg.Save(@"D:\OneDrive\Thesis\images\ImgProc\And.png");
            //        orImg.Save(@"D:\OneDrive\Thesis\images\ImgProc\Or.png");
            //        notImg.Save(@"D:\OneDrive\Thesis\images\ImgProc\Not.png");
            //    }
            //}
        }


        public MainWindowViewModel()
        {
            IWhiskerDetector whiskerDetector = ModelResolver.Resolve<IWhiskerDetector>();
            whiskerDetector.OrientationResolution = 1;

            //string[] files = Directory.GetFiles(@"C:\Users\10488835\Dropbox", "*.tex", SearchOption.AllDirectories);

            //foreach (string file in files)
            //{
            //    string[] lines = File.ReadAllLines(file);

            //    if (lines.Any(x => x.Contains("Rotarod")))
            //    {
            //        Console.WriteLine(file);
            //    }
            //}


            //LogicGateImages();
            //GenerateMeanAngleData();

            try
            {
                //BatchProcess();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                List<string> errormsgs = new List<string>();
                errormsgs.Add(e.Message);
                errormsgs.Add(e.StackTrace);
                //errormsgs.Add(e.InnerException.Message);
                File.WriteAllLines(@"F:\Error.txt", errormsgs);
            }

            //MessageBox.Show("Done!");
            //GenerateTps();

            ColorDic = new Dictionary<int, Color>();
            ColorDic.Add(0, Color.White);
            ColorDic.Add(1, Color.Red);
            ColorDic.Add(2, Color.Blue);
            ColorDic.Add(3, Color.Green);
            ColorDic.Add(4, Color.Yellow);
            ColorDic.Add(5, Color.Magenta);
            ColorDic.Add(6, Color.Turquoise);
            ColorDic.Add(7, Color.Orange);
            ColorDic.Add(8, Color.LightGreen);
            ColorDic.Add(9, Color.Purple);
            ColorDic.Add(10, Color.White);
            ColorDic.Add(11, Color.White);
            ColorDic.Add(12, Color.White);
            ColorDic.Add(13, Color.White);
            ColorDic.Add(14, Color.White);
        }

        private Image<Gray, byte> BgImage
        {
            get;
            set;
        }

        private void NewSession()
        {
            if (VideoLoaded)
            {
                var result = MessageBox.Show("Do you want to save before opening another video?", "Save?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }

                if (result == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
            }

            string workingFile = FileBrowser.BrowseForFile("Video Files|*.avi;*.mpg;*.mpeg;*.mp4;*.mov|Image Files|*.bmp;*.gif;*.jpg;*.jpeg;*.png;*.tif;*.tiff");

            if (string.IsNullOrWhiteSpace(workingFile))
            {
                return;
            }

            NewSession(workingFile);
        }

        private void NewSession(string workingFile)
        {
            NewSessionViewModel viewModel = new NewSessionViewModel(workingFile);
            NewSessionView view = new NewSessionView();
            view.DataContext = viewModel;
            view.ShowDialog();
            //viewModel.Preview();
            //viewModel.Ok();

            if (viewModel.ExitResult != WindowExitResult.Ok)
            {
                return;
            }

            VideoSettings = viewModel.VideoSettings;
            Video = viewModel.Video;
            
            Boundries = viewModel.Boundries;
            VideoLoaded = true;
            SliderEnabled = true;
            WorkingFile = workingFile;
            CurrentFrame = 0;
            IRBSKVideo2 rbskVideo = ModelResolver.Resolve<IRBSKVideo2>();
            rbskVideo.Video = Video;
            rbskVideo.BackgroundImage = viewModel.BinaryBackground;
            rbskVideo.ThresholdValue = viewModel.ThresholdValue;


            BgImage = viewModel.BinaryBackground;

            rbskVideo.Roi = VideoSettings.Roi;
            ROI = VideoSettings.Roi;

            //Rectangle roi = new Rectangle(184, 141, 778, 803);
            //rbskVideo.Roi = roi;
            //ROI = roi;

            rbskVideo.GapDistance = VideoSettings.GapDistance;
            rbskVideo.FindWhiskers = viewModel.FindWhiskers;
            //rbskVideo.FindFoot = viewModel.FindFeet;
            if (rbskVideo.FindWhiskers)
            {
                NewWhiskerSessionViewModel whiskerVm = new NewWhiskerSessionViewModel(rbskVideo, WorkingFile);
                NewWhiskerSessionView whiskerView = new NewWhiskerSessionView();
                whiskerView.DataContext = whiskerVm;
                whiskerView.ShowDialog();

                if (whiskerVm.ExitResult != WindowExitResult.Ok)
                {
                    return;
                }

                WhiskerSettings = rbskVideo.WhiskerSettings;
                FootSettings = rbskVideo.FootSettings;
            }

            ProgressView progressView = new ProgressView();
            ProgressViewModel progressViewModel = new ProgressViewModel();
            progressView.DataContext = progressViewModel;
            rbskVideo.ProgressUpdates += (sender, args) => progressViewModel.ProgressValue = args.Progress;

            Task.Factory.StartNew(rbskVideo.Process).ContinueWith(x => Application.Current.Dispatcher.Invoke(() =>
            {
                progressView.Close();
                PostProcess(rbskVideo);

            }));

            progressView.ShowDialog();
        }

        private void PostProcess(Dictionary<int, ISingleFrameExtendedResults> results)
        {
            Results = results;

            FrameCount = Results.Count - 1;

            GenerateMotionTrack();
            

        }

        private void PostProcess(IRBSKVideo2 video)
        {
            //Results = video.HeadPoints;
            //WhiskerResults = video.WhiskerResults;
            //WhiskerResultsAll = video.WhiskerResultsAll;
            //AllRects = video.AllRects;

            if (Video.FrameCount != video.HeadPoints.Count)
            {
                MessageBox.Show("The expected frame count does not match the generated frame count, this is often an indication the video is corrupt and can lead to inaccurate results, proceed with caution", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            PostProcess(video.HeadPoints);

            if (!ROI.IsEmpty)
            {
                foreach (BoundaryBaseViewModel boundary in Boundries)
                {
                    Point[] points = boundary.Model.Points;
                    for (int i = 0; i < points.Length; i++)
                    {
                        points[i].X += ROI.X;
                        points[i].Y += ROI.Y;
                    }
                }
            }



        }


        private double[,] GetAverageAngles(int windowSize, List<Tuple<int, int>> bestFrames)
        {
            List<double> leftAngles = new List<double>();
            List<double> rightAngles = new List<double>();

            int starting = 0;
            int ending = Results.Count - 1;
            
            //Vector horizontal = new Vector(1, 0);
            foreach (var result in Results)
            //for (int i = starting; i <= ending; i++)
            {
                bool go = bestFrames.Any(tup => result.Key >= tup.Item1 && result.Key <= tup.Item2);
                //var result = Results[i];

                if (!go)
                {
                    continue;
                }

                Vector orientation = result.Value.Orientation;
                IWhiskerCollection cWhiskers = result.Value.Whiskers;
                PointF midPoint = result.Value.MidPoint;
                if (cWhiskers != null)
                {
                    if (cWhiskers.LeftWhiskers != null && cWhiskers.LeftWhiskers.Any())
                    {
                        List<double> leftWhiskerAngles = new List<double>();

                        foreach (var whisk in cWhiskers.LeftWhiskers)
                        {
                            double d1 = whisk.Line.P1.DistanceSquared(midPoint);
                            double d2 = whisk.Line.P2.DistanceSquared(midPoint);
                            Vector wVec;
                            if (d1 > d2)
                            {
                                wVec = new Vector(whisk.Line.P1.X - whisk.Line.P2.X, whisk.Line.P1.Y - whisk.Line.P2.Y);
                                //angle -= 180;
                            }
                            else
                            {
                                wVec = new Vector(whisk.Line.P2.X - whisk.Line.P1.X, whisk.Line.P2.Y - whisk.Line.P1.Y);
                            }

                            leftWhiskerAngles.Add(Vector.AngleBetween(wVec, orientation));
                        }

                        leftAngles.Add(leftWhiskerAngles.Average());
                    }
                    else
                    {
                        leftAngles.Add(-2000);
                    }

                    if (cWhiskers.RightWhiskers != null && cWhiskers.RightWhiskers.Any())
                    {
                        List<double> rightWhiskerAngles = new List<double>();

                        foreach (var whisk in cWhiskers.RightWhiskers)
                        {
                            double d1 = whisk.Line.P1.DistanceSquared(midPoint);
                            double d2 = whisk.Line.P2.DistanceSquared(midPoint);

                            Vector wVec;
                            if (d1 > d2)
                            {
                                wVec = new Vector(whisk.Line.P1.X - whisk.Line.P2.X, whisk.Line.P1.Y - whisk.Line.P2.Y);
                            }
                            else
                            {
                                wVec = new Vector(whisk.Line.P2.X - whisk.Line.P1.X, whisk.Line.P2.Y - whisk.Line.P1.Y);
                            }

                            rightWhiskerAngles.Add(Vector.AngleBetween(orientation, wVec));
                        }

                        rightAngles.Add(rightWhiskerAngles.Average());
                    }
                    else
                    {
                        rightAngles.Add(-2000);
                    }
                }
            }

            RemoveZeroNumbers(leftAngles);
            RemoveZeroNumbers(rightAngles);

            int colMax = rightAngles.Count > leftAngles.Count ? rightAngles.Count : leftAngles.Count;
            double[,] data = new double[4, colMax];
            for (int i = 0; i < colMax; i++)
            {
                int leftDelta = i - windowSize;
                if (leftDelta < 0)
                {
                    leftDelta = 0;
                }

                if (i < leftAngles.Count)
                {
                    data[0, i] = leftAngles[i];
                    int leftRightDelta = i + windowSize;
                    if (leftRightDelta >= leftAngles.Count)
                    {
                        leftRightDelta = leftAngles.Count - 1;
                    }

                    List<double> leftValues = new List<double>();
                    for (int window = leftDelta; window <= leftRightDelta; window++)
                    {
                        leftValues.Add(leftAngles[window]);
                    }
                    data[1, i] = leftValues.Average();
                }

                if (i < rightAngles.Count)
                {
                    data[2, i] = rightAngles[i];

                    int rightRightDelta = i + windowSize;

                    if (rightRightDelta >= rightAngles.Count)
                    {
                        rightRightDelta = rightAngles.Count - 1;
                    }

                    List<double> rightValues = new List<double>();
                    for (int window = leftDelta; window <= rightRightDelta; window++)
                    {
                        rightValues.Add(rightAngles[window]);
                    }

                    data[3, i] = rightValues.Average();
                }
            }
            ExcelService.WriteData(data, @"C:\Users\Brett\OneDrive\Results\TestResultsArea2.xlsx");

            return data;
        }

        private void RemoveZeroNumbers(List<double> angles)
        {
            int rightIndexRemove;
            int leftIndexRemove;
            for (int i = 0; i < angles.Count; i++)
            {
                if (angles[i] != -2000)
                {
                    continue;
                }

                //Need to adjust
                bool avgLeft = false;
                int leftDelta = i - 1;
                int counter = 1;

                if (leftDelta < 0)
                {
                    leftIndexRemove = i;
                    avgLeft = false;
                }
                else
                {
                    while (leftDelta > 0)
                    {
                        if (angles[leftDelta] != -2000)
                        {
                            //We're good
                            avgLeft = true;
                            break;
                        }

                        leftDelta--;
                        counter++;
                    }

                    leftIndexRemove = i;
                }

                int maxCount = angles.Count;
                bool avgRight = false;
                int rightDelta = i + 1;

                if (rightDelta >= maxCount)
                {
                    rightIndexRemove = i;
                    avgRight = false;
                }
                else
                {
                    while (rightDelta < maxCount - 1)
                    {
                        if (angles[rightDelta] != -2000)
                        {
                            //We're good
                            avgRight = true;
                            break;
                        }

                        rightDelta++;
                    }
                }

                if (avgLeft && avgRight)
                {
                    int delta = rightDelta - leftDelta;
                    double leftValue = angles[leftDelta];
                    double valueDelta = angles[rightDelta] - angles[leftDelta];
                    for (int j = leftDelta + 1; j < rightDelta; j++)
                    {
                        angles[j] = leftValue + (valueDelta*(1d/delta)*(j - leftDelta)); //((angles[rightDelta] + angles[leftDelta])/delta)*(j - leftDelta);
                    }
                }
            }

            angles.RemoveAll(x => x == -2000);
        }
        public double CalculateFrequencyDFT(double[] signal, double frameRate, double frameInterval)
        {
            double bestFrequency;

            double sampleLength = (signal.Length * frameInterval) / frameRate;
            BrettFFT.BrettDFT(signal, sampleLength, out bestFrequency, 1, 40);

            return bestFrequency;
        }

        public double CalculateFrequencyAuto(double[] signal, double frameRate, double frameInterval)
        {
            return AutoCorrelogram.CalculateFrequency(signal, frameRate, frameInterval);
        }

        //private void GenerateWhiskerLocations()
        //{
        //    if (WhiskerResults == null || !WhiskerResults.Any())
        //    {
        //        return;
        //    }


        //}

        private Dictionary<int, Color> ColorDic
        {
            get;
            set;
        } 

        private void UpdateDisplayImage()
        {
            if (Results == null)
            {
                return;
            }

            ISingleFrameExtendedResults currentResult = null;// new ISingleFrameExtendedResults();
            PointF[] headPoint = null;// currentResult.HeadPoints;
            PointF centroid = new PointF(0,0);// currentResult.Centroid;
            if (Results.Count() != 0)
            {
                currentResult = Results[CurrentFrame];
                headPoint = currentResult.HeadPoints;
                centroid = currentResult.Centroid;
            }

            Video.SetFrame(CurrentFrame);
            using (Image<Bgr, Byte> currentFrame = Video.GetFrameImage())
            {
                if (currentFrame == null)
                {
                    return;
                }

                if (!ROI.IsEmpty)
                {
                    currentFrame.ROI = ROI;
                }

                if (InteractingBoundries != null)
                {
                    foreach (var interaction in InteractingBoundries)
                    {
                        BoundaryBaseViewModel boundary = interaction.Key;
                        if (boundary.Enabled)
                        {
                            currentFrame.Draw(boundary.Points, boundary.Color, 2);

                            System.Drawing.Point avgPoint = new System.Drawing.Point
                            {
                                X = (int)Math.Round((double)boundary.Points.Select(p => p.X).Min() + 5),
                                Y = (int)Math.Round((double)boundary.Points.Select(p => p.Y).Max() - 5),
                            };

                            CvInvoke.PutText(currentFrame, boundary.Name, avgPoint, FontFace.HersheyComplex, 0.6, boundary.Color.MCvScalar);
                        }
                    }
                }

                //currentFrame.ROI = VideoSettings.Roi;
                if (MotionTrack != null)
                {
                    currentFrame.DrawPolyline(MotionTrack.Select(x => new System.Drawing.Point((int)x.X, (int)x.Y)).ToArray(), false, new Bgr(Color.Blue), 2);
                }

                if (!centroid.IsEmpty)
                {
                    currentFrame.Draw(new CircleF(centroid, 4), new Bgr(Color.Red), 2);
                }

                if(currentResult != null)
                {
                    if ( currentResult.BodyContour.Any())
                    {
                        currentFrame.DrawPolyline(currentResult.BodyContour, true, new Bgr(Color.Yellow), 1);
                    }
                }

                if (Results != null && Results.Any() && currentResult.HeadPoints != null)
                {
                    PointF midPoint = currentResult.MidPoint;
                    // IWhiskerCollection cWhiskers = currentResult.Whiskers;
                    IWhiskerCollection cWhiskers = currentResult.BestTrackedWhisker;
                    Vector orientation = currentResult.Orientation;
                    orientation = new Vector(currentResult.HeadPoints[2].X - midPoint.X, currentResult.HeadPoints[2].Y - midPoint.Y);
                    double orientationHorizontalAngle = Vector.AngleBetween(new Vector(1, 0), orientation);
                    if (cWhiskers != null)
                    {
                        if (cWhiskers.LeftWhiskers != null && cWhiskers.LeftWhiskers.Any())
                        {
                            //currentFrame.Draw(cWhiskers.LeftWhiskers[5].Line, new Bgr(Color.White), 1);
                            int counter = 0;
                            foreach (IWhiskerSegment whisker in cWhiskers.LeftWhiskers)
                            {
                                Color color = Color.White;
                                if (ColorDic.ContainsKey(counter))
                                {
                                    color = ColorDic[counter];
                                }
                                currentFrame.Draw(whisker.Line, new Bgr(color), 1);
                                counter++;
                            }

                            var xs = cWhiskers.LeftWhiskers.Select(x => x.X).Average();
                            var ys = cWhiskers.LeftWhiskers.Select(x => x.Y).Average();
                            
                            var angAvg = cWhiskers.LeftWhiskers.Select(x => x.Angle).Average();


                            double horizontalAngle = Vector.AngleBetween(orientation, new Vector(1, 0));
                            double hA = angAvg + horizontalAngle;
                            double dX = 10 * Math.Cos(hA * 0.0174533);
                            double dY = 10 * Math.Sin(hA * 0.0174533);
                            Point lp1 = new Point((int)(xs - dX), (int)(ys + dY));
                            Point lp2 = new Point((int)(xs + dX), (int)(ys - dY));

                            LineSegment2D lineSeg = new LineSegment2D(lp1, lp2);

                            currentFrame.Draw(lineSeg, new Bgr(Color.Cyan), 1);
                        }

                        
                        if (cWhiskers.RightWhiskers != null && cWhiskers.RightWhiskers.Any())
                        {
                            //    //currentFrame.Draw(cWhiskers.RightWhiskers[6].Line, new Bgr(Color.White), 1);
                            int counter = 0;
                            foreach (IWhiskerSegment whisker in cWhiskers.RightWhiskers)
                            {
                                Color color = Color.White;
                                if (ColorDic.ContainsKey(counter))
                                {
                                    color = ColorDic[counter];
                                }
                                currentFrame.Draw(whisker.Line, new Bgr(color), 1);
                                counter++;
                            }

                            var xs = cWhiskers.RightWhiskers.Select(x => x.X).Average();
                            var ys = cWhiskers.RightWhiskers.Select(x => x.Y).Average();
                            
                            var angAvg = cWhiskers.RightWhiskers.Select(x => x.Angle).Average();

                            
                            double horizontalAngle = Vector.AngleBetween(orientation, new Vector(1, 0));
                            double hA = angAvg - horizontalAngle;
                            double dX = 10*Math.Cos(hA* 0.0174533);
                            double dY = 10*Math.Sin(hA* 0.0174533);
                            Point lp1 = new Point((int)(xs + dX), (int)(ys + dY));
                            Point lp2 = new Point((int)(xs - dX), (int)(ys - dY));

                            LineSegment2D lineSeg = new LineSegment2D(lp1, lp2);

                            currentFrame.Draw(lineSeg, new Bgr(Color.Cyan), 1);
                        }
                    }
                }

                //if (Results != null && Results.Any())
                //{
                //    IWhiskerCollection cWhiskers = Results[CurrentFrame].Whiskers;
                //    if (cWhiskers != null)
                //    {
                //        if (cWhiskers.LeftWhiskers != null)
                //        {
                //            var lines = cWhiskers.LeftWhiskers.Select(x => x.Line);

                //            var p1s = lines.Select(x => x.P1);
                //            var p2s = lines.Select(x => x.P2);

                //            var p1xAvg = p1s.Select(x => x.X).Average();
                //            var p1yAvg = p1s.Select(x => x.Y).Average();

                //            var p2xAvg = p2s.Select(x => x.X).Average();
                //            var p2yAvg = p2s.Select(x => x.Y).Average();

                //            currentFrame.Draw(new LineSegment2DF(new PointF((float)p1xAvg, (float)p1yAvg), new PointF((float)p2xAvg, (float)p2yAvg)), new Bgr(Color.Red), 1);

                //            //foreach (IWhiskerSegment whisker in cWhiskers.LeftWhiskers)
                //            //{
                //                //if (whisker.Color == Color.Red)
                //                //{
                //                //    currentFrame.Draw(whisker.Line, new Bgr(whisker.Color), 1);
                //                //}
                //            //}
                //        }

                //        if (cWhiskers.RightWhiskers != null)
                //        {
                //            foreach (IWhiskerSegment whisker in cWhiskers.RightWhiskers)
                //            {
                //                //if (whisker.Color == Color.Red)
                //                {
                //                    currentFrame.Draw(whisker.Line, new Bgr(whisker.Color), 1);
                //                }
                //            }
                //        }
                //    }
                //}

                //if (CurrentFrame > 0)
                //{
                //    ISingleFrameResult prev = Results[CurrentFrame-1];
                //    Vector prevOrientation = prev.Orientation;
                //    Vector currentOrientation = Results[CurrentFrame].Orientation;
                //    Console.WriteLine(Vector.AngleBetween(prevOrientation, currentOrientation));
                //}

                //PointF smoothedHeadPoint = Results[CurrentFrame].HeadPoint;
                //LineSegment2DF line = new LineSegment2DF(smoothedHeadPoint, new PointF((float) (smoothedHeadPoint.X - Results[CurrentFrame].Orientation.X), (float) (smoothedHeadPoint.Y - Results[CurrentFrame].Orientation.Y)));
                //o1.Add(Results[CurrentFrame].AngularVelocity/500);
                //Console.WriteLine(Results[CurrentFrame].AngularVelocity);
                //currentFrame.Draw(line, new Bgr(Color.Red), 2);

                //if ( != null)
                //{
                //    currentFrame.DrawPolyline(MotionTrack.Select(x => new Point((int)x.X, (int)x.Y)).ToArray(), false, new Bgr(Color.Blue), 2);
                //}
                if (headPoint != null)
                {
                    currentFrame.Draw(new CircleF(headPoint[0], 2), new Bgr(Color.Yellow));
                    currentFrame.Draw(new CircleF(headPoint[1], 2), new Bgr(Color.Yellow));
                    currentFrame.Draw(new CircleF(headPoint[2], 2), new Bgr(Color.Yellow));
                    currentFrame.Draw(new CircleF(headPoint[3], 2), new Bgr(Color.Yellow));
                    currentFrame.Draw(new CircleF(headPoint[4], 2), new Bgr(Color.Yellow));
                    currentFrame.Draw(new CircleF(PointExtenstion.MidPoint(headPoint[3], headPoint[1]), 2), new Bgr(Color.Red));
                }

                Image = ImageService.ToBitmapSource(currentFrame);
            }
        }

        private void GenerateMotionTrack()
        {
            List<PointF> motionTrack = new List<PointF>();
            List<Vector> orientationTrack = new List<Vector>();
            List<PointF> noseTrack = new List<PointF>();
            int startOffset = 0;
            bool inStart = true;
            for (int i = 0; i < FrameCount; i++)
            {
                PointF[] headPoints = Results[i].HeadPoints;

                if (!Results[i].Centroid.IsEmpty)
                {
                    motionTrack.Add(Results[i].Centroid);
                }

                if (headPoints != null)
                {
                    PointF midPoint = PointExtenstion.MidPoint(headPoints[1], headPoints[3]);
                    //Vector up = new Vector(0,1);
                    Vector dir = new Vector(headPoints[2].X - midPoint.X, headPoints[2].Y - midPoint.Y);
                    orientationTrack.Add(dir);
                    noseTrack.Add(headPoints[2]);
                    //motionTrack.Add(headPoints[2]);
                    //motionTrack.Add(midPoint);
                    inStart = false;
                }
                else if (inStart)
                {
                    startOffset++;
                }
            }

            MotionTrack = motionTrack.ToArray();
            OrientationTrack = orientationTrack.ToArray();
            GenerateBheaviouralAnalysis(noseTrack.ToArray(), Boundries, startOffset);
            m_CurrentFrame = -1;
            CurrentFrame = 0;
        }

        private void GenerateBheaviouralAnalysis(PointF[] motionTrack, IEnumerable<BoundaryBaseViewModel> objects, int startOffset)
        {
            if(objects == null)
            {
                return;
            }
            double minInteractionDistance = VideoSettings.MinimumInteractionDistance;
            Dictionary<BoundaryBaseViewModel, List<BehaviourHolderViewModel>> interactingBoundries = new Dictionary<BoundaryBaseViewModel, List<BehaviourHolderViewModel>>();
            int trackCount = motionTrack.Length;
            foreach (BoundaryBaseViewModel boundary in objects)
            {
                if (!boundary.Enabled)
                {
                    continue;
                }

                interactingBoundries.Add(boundary, new List<BehaviourHolderViewModel>());

                //Manually handle first point
                PointF currentPoint = motionTrack[0];
                double distance = boundary.GetMinimumDistance(currentPoint);
                BehaviourHolderViewModel previousHolder;
                if (distance < minInteractionDistance)
                {
                    //Interaction
                    previousHolder = new BehaviourHolderViewModel(boundary, InteractionBehaviour.Started, 0);
                    //interactingBoundries[boundary].Add(previousHolder);
                }
                else
                {
                    //No Interaction
                    previousHolder = new BehaviourHolderViewModel(boundary, InteractionBehaviour.Ended, 0);
                    //interactingBoundries[boundary].Add(previousHolder);
                }

                for (int i = 1; i < trackCount; i++)
                {
                    currentPoint = motionTrack[i];
                    distance = boundary.GetMinimumDistance(currentPoint);

                    if (distance < minInteractionDistance)
                    {
                        //Interaction
                        if (previousHolder.Interaction != InteractionBehaviour.Started)
                        {
                            previousHolder = new BehaviourHolderViewModel(boundary, InteractionBehaviour.Started, i + startOffset);
                            interactingBoundries[boundary].Add(previousHolder);
                        }
                    }
                    else
                    {
                        //No Interaction
                        if (previousHolder.Interaction != InteractionBehaviour.Ended)
                        {
                            previousHolder = new BehaviourHolderViewModel(boundary, InteractionBehaviour.Ended, i + startOffset);
                            interactingBoundries[boundary].Add(previousHolder);
                        }
                    }
                }
            }

            foreach (var interaction in interactingBoundries)
            {
                List<BehaviourHolderViewModel> behaviours = interaction.Value;
                int bCount = behaviours.Count;
                bCount--;
                int prevFrameNumber = -10;

                for (int i = bCount; i >= 0; i--)
                {
                    BehaviourHolderViewModel vm = behaviours[i];
                    int currentFrameNumber = vm.FrameNumber;
                    if (prevFrameNumber > 0 && Math.Abs(vm.FrameNumber - prevFrameNumber) <= 2)
                    {
                        behaviours.RemoveAt(i + 1);
                        behaviours.RemoveAt(i);
                        prevFrameNumber = -10;
                        continue;
                    }

                    prevFrameNumber = currentFrameNumber;
                }
            }

            InteractingBoundries = interactingBoundries;
            ObservableCollection<BehaviourHolderViewModel> events = new ObservableCollection<BehaviourHolderViewModel>();
            foreach (var boundary in interactingBoundries)
            {
                foreach (BehaviourHolderViewModel behaviour in boundary.Value)
                {
                    events.Add(behaviour);
                }
            }

            Events = new ObservableCollection<BehaviourHolderViewModel>(events.OrderBy(x => x.FrameNumber));
        }

        private void WhiskerDebug()
        {
            //string folderLoc = FileBrowser.BrowseForFolder();

            //if (string.IsNullOrWhiteSpace(folderLoc))
            //{
            //    return;
            //}

            IWhiskerDetector wd = ModelResolver.Resolve<IWhiskerDetector>();
            Video.SetFrame(CurrentFrame);
            using (Image<Gray, Byte> currentFrame = Video.GetGrayFrameImage())
            {
                wd.Debug(currentFrame, BgImage, currentFrame.Width, currentFrame.Height, Results[CurrentFrame], @"C:\Users\Brett\OneDrive\Final11\Final\Results\Brett7", null);
            }
        }

        private void OpenFile()
        {
            if (VideoLoaded)
            {
                var result = MessageBox.Show("Do you want to save before opening another video?", "Save?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }

                if (result == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
            }

            string filePath = FileBrowser.BrowseForFile("ARWT|*.arwt");

            if (string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }

            OpenFile(filePath);
        }

        private void OpenFile(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TrackedVideoWithSettingsXml));
            TrackedVideoWithSettingsXml trackedVideoXml;
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    trackedVideoXml = (TrackedVideoWithSettingsXml)serializer.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    XmlSerializer serializer2 = new XmlSerializer(typeof(TrackedVideoXml));
                    trackedVideoXml = new TrackedVideoWithSettingsXml((TrackedVideoXml)serializer2.Deserialize(reader));
                }
            }
            

            ModelInterface.Results.ITrackedVideo trackedVideo = trackedVideoXml.GetData();

            if (!File.Exists(trackedVideo.FileName))
            {
                MessageBoxResult result = MessageBox.Show("Can't find video, would you like to browse for it?", "Video doesn't exist", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    return;
                }

                string newFilePath = FileBrowser.BroseForVideoFiles();

                if (string.IsNullOrWhiteSpace(newFilePath))
                {
                    return;
                }

                trackedVideo.FileName = newFilePath;
            }

            IVideo video = ModelResolver.Resolve<IVideo>();
            video.SetVideo(trackedVideo.FileName);

            Dictionary<int, ISingleFrameExtendedResults> results = new Dictionary<int, ISingleFrameExtendedResults>();
            if (trackedVideo.Results != null) {
                foreach (var kvp in trackedVideo.Results)
                {
                    int key = kvp.Key;
                    ISingleFrameExtendedResults frame = ModelResolver.Resolve<ISingleFrameExtendedResults>();

                    if (kvp.Value != null)
                    {
                        results.Add(key, kvp.Value);
                    }
                    else
                    {
                        results.Add(key, frame);
                    }
                }
            }
          
            //Dictionary<int, ISingleFrameResult> results = trackedVideo.Results;

            if (trackedVideo.Boundries != null)
            {
                List<BoundaryBaseViewModel> boundries = new List<BoundaryBaseViewModel>();
                foreach (IBoundaryBase boundary in trackedVideo.Boundries)
                {
                    int id = boundary.Id;
                    System.Drawing.Point[] points = boundary.Points;

                    IArtefactsBoundary artModel = boundary as IArtefactsBoundary;
                    IBoxBoundary boxModel = boundary as IBoxBoundary;
                    ICircleBoundary circleModel = boundary as ICircleBoundary;
                    IOuterBoundary outerModel = boundary as IOuterBoundary;

                    if (artModel != null)
                    {
                        boundries.Add(new ArtefactsBoundaryViewModel(artModel, points));
                    }
                    else if (boxModel != null)
                    {
                        boundries.Add(new BoxesBoundary(boxModel, points));
                    }
                    else if (circleModel != null)
                    {
                        boundries.Add(new CircularBoundary(circleModel, points));
                    }
                    else if (outerModel != null)
                    {
                        boundries.Add(new OuterBoundaryViewModel(outerModel, points));
                    }
                }

                IBoundaryBase[] boundries2 = trackedVideo.InteractingBoundries.Keys.ToArray();
                IBehaviourHolder[][] behaviours = trackedVideo.InteractingBoundries.Values.ToArray();

                BoundaryBaseViewModel[] newKeys = boundries2.Select(BoundaryBaseViewModel.GetBoundaryFromModel).ToArray();
                List<BehaviourHolderViewModel>[] newValues = behaviours.Select(x => new List<BehaviourHolderViewModel>(x.Where(z => z.FrameNumber > 0).Select(y => new BehaviourHolderViewModel(BoundaryBaseViewModel.GetBoundaryFromModel(y.Boundary), y.Interaction, y.FrameNumber)))).ToArray();

                Dictionary<BoundaryBaseViewModel, List<BehaviourHolderViewModel>> interactingBoundries = new Dictionary<BoundaryBaseViewModel, List<BehaviourHolderViewModel>>();

                int length = newKeys.Length;
                for (int i = 0; i < length; i++)
                {
                    interactingBoundries.Add(newKeys[i], newValues[i]);
                }
                Boundries = new ObservableCollection<BoundaryBaseViewModel>(boundries);
                InteractingBoundries = interactingBoundries;
            }

            IMovementBehaviour movementBehaviour;
            List<Tuple<int, int>> bestFrames;
            

            Video = video;
            
            VideoLoaded = true;
            SliderEnabled = true;
            WorkingFile = trackedVideo.FileName;//filePath;
            WorkingArwtFile = filePath;
            CurrentFrame = 0;
            Results = results;
            FrameCount = Results.Count - 1;
            ROI = trackedVideo.ROI;

            OrientationTrack = trackedVideo.OrientationTrack;
            
            ObservableCollection<BehaviourHolderViewModel> events = new ObservableCollection<BehaviourHolderViewModel>();
            if(InteractingBoundries != null)
            {
                foreach (var boundary in InteractingBoundries)
                {
                    foreach (BehaviourHolderViewModel behaviour in boundary.Value)
                    {
                        events.Add(behaviour);
                    }
                }
            }
            

            Events = new ObservableCollection<BehaviourHolderViewModel>(events.OrderBy(x => x.FrameNumber));

            VideoSettings = ModelResolver.Resolve<IVideoSettings>();
            VideoSettings.MinimumInteractionDistance = trackedVideo.MinInteractionDistance;

            if (Video.FrameCount != Results.Count)
            {
                MessageBox.Show("The expected frame count does not match the generated frame count, this is often an indication the video is corrupt and can lead to inaccurate results, proceed with caution", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            GenerateMotionTrack();
            UpdateDisplayImage();
            TrackedVideo = trackedVideo;

            //GenerateStats(Fra);
        }

        private ModelInterface.Results.ITrackedVideo TrackedVideo
        {
            get;
            set;
        }
        private void SaveFile(CancelEventArgs args = null)
        {
            string fileName = Path.GetFileNameWithoutExtension(Video.FilePath);
            string filePath = FileBrowser.SaveFile("ARWT|*.arwt", fileName, string.Empty);

            if (string.IsNullOrWhiteSpace(filePath))
            {
                if (args != null)
                {
                    args.Cancel = true;
                }

                return;
            }

            SaveFile(filePath);
        }

        private IMouseDataExtendedResult GetMouseData(IBoundaryBase[] boundries, double gapDistance, int thresholdValue, int thresholdValue2, int startFrame, int endFrame, bool smoothMotion, double frameRate, double unitsToMm, out IMovementBehaviour movementBehaviour, out List<Tuple<int, int>> rotFrames)
        {
            IMouseDataExtendedResult mouseDataResult = ModelResolver.Resolve<IMouseDataExtendedResult>();
            mouseDataResult.Results = Results;
            mouseDataResult.Age = 10;
            mouseDataResult.Boundaries = boundries;
            mouseDataResult.VideoOutcome = Model.Results.SingleFileResult.Ok;
            mouseDataResult.GapDistance = gapDistance;
            mouseDataResult.WhiskerSettings = WhiskerSettings;
            mouseDataResult.FootSettings = FootSettings;
            //mouseDataResult.FootSettings = 
            mouseDataResult.ThresholdValue = thresholdValue;
            mouseDataResult.ThresholdValue2 = thresholdValue2;
            mouseDataResult.StartFrame = startFrame;
            mouseDataResult.EndFrame = endFrame;
            mouseDataResult.SmoothMotion = smoothMotion;
            mouseDataResult.FrameRate = frameRate;
            mouseDataResult.UnitsToMilimeters = unitsToMm;
            mouseDataResult.SmoothFactor = 0.68;
            mouseDataResult.GenerateResults();
            //double rs = mouseDataResult.GetAverageSpeedForMoving();
            //var rs2 = mouseDataResult.GetFrameNumbersForRunning();
            //var rs3 = mouseDataResult.GetFrameNumbersForMoving();
            IBehaviourSpeedDefinitions speedDef = ModelResolver.Resolve<IBehaviourSpeedDefinitions>();
            movementBehaviour = speedDef.GetMovementBehaviour(mouseDataResult.AverageCentroidVelocity);

            //int noFrames = mouseDataResult.RotationBehaviours.Where(x => x is INoRotation).Sum(nF => nF.EndFrame - nF.StartFrame);

            //int bestStart = -1, bestEnd = -1;
            //int currentMax = -1;
            List<Tuple<int, int>> bFrames = new List<Tuple<int, int>>();
            foreach (var rot in mouseDataResult.RotationBehaviours)
            {
                if (!(rot is INoRotation))
                {
                    bFrames.Add(new Tuple<int, int>(rot.StartFrame, rot.EndFrame));
                    //int cMax = rot.EndFrame - rot.StartFrame;
                    //if (cMax > currentMax)
                    //{
                    //    currentMax = cMax;
                    //    bestStart = rot.StartFrame;
                    //    bestEnd = rot.EndFrame;
                    //}
                }
            }

            rotFrames = bFrames;
            //bestStart2 = bestStart;
            //bestEnd2 = bestEnd;
            //var roFrames = mouseDataResult.RotationBehaviours.Where(x => !(x is INoRotation)).Max(nF => nF.EndFrame - nF.StartFrame);

            

            //if (noFrames > roFrames)
            //{
            //    rotationBehaviour = ModelResolver.Resolve<INoRotation>();
            //}
            //else
            //{
            //    rotationBehaviour = ModelResolver.Resolve<ISlowTurning>();
            //}

            return mouseDataResult;
        }
        private void SaveFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }

            int headCount = Results.Count;
            SingleFrameExtendedResultXml[] allPoints = new SingleFrameExtendedResultXml[headCount];
            for (int i = 0; i < headCount; i++)
            {
                if (Results[i].HeadPoints == null)
                {
                    allPoints[i] = null;
                }
                else
                {
                    allPoints[i] = new SingleFrameExtendedResultXml(Results[i]);
                }
            }

            DictionaryXml<int, SingleFrameExtendedResultXml> results = new DictionaryXml<int, SingleFrameExtendedResultXml>(Results.Keys.ToArray(), allPoints);
            PointFXml[] motionTrack;
            
            if (MotionTrack != null && MotionTrack.Any())
            {
                motionTrack = MotionTrack.Select(point => new PointFXml(point.X, point.Y)).ToArray();
            }
            else
            {
                motionTrack = null;
            }

            PointF[] smoothedMotionTrack = new PointF[0];
            if (MotionTrack != null && MotionTrack.Any())
            {
                ITrackSmoothing smoothing = ModelResolver.Resolve<ITrackSmoothing>();
                smoothedMotionTrack = smoothing.SmoothTrack(MotionTrack);
            }
            
            PointFXml[] smoothedMotionTrackXml = smoothedMotionTrack.Select(point => new PointFXml(point)).ToArray();
            VectorXml[] orientationTrack = OrientationTrack.Select(vector => new VectorXml(vector)).ToArray();

            RectangleXml rect = null;

            if (!ROI.IsEmpty)
            {
                rect = new RectangleXml(ROI);
            }

            List<BoundaryBaseXml> boundries = new List<BoundaryBaseXml>();
            foreach (BoundaryBaseViewModel boundary in Boundries)
            {
                boundries.Add(boundary.Model.GetData());
            }

            List<BehaviourHolderXml> events = new List<BehaviourHolderXml>();
            foreach (BehaviourHolderViewModel behaviour in Events)
            {
                BoundaryBaseXml boundary = behaviour.Boundary.Model.GetData();
                InteractionBehaviour interaction = behaviour.Interaction;
                int frameNumber = behaviour.FrameNumber;
                events.Add(new BehaviourHolderXml(boundary, interaction, frameNumber));
            }

            BoundaryBaseXml[] keys = InteractingBoundries.Keys.Select(key => key.Model.GetData()).ToArray();
            BehaviourHolderXml[][] values = InteractingBoundries.Values.Select(value => value.Select(behavHolder => new BehaviourHolderXml(behavHolder.Boundary.Model.GetData(), behavHolder.Interaction, behavHolder.FrameNumber)).ToArray()).ToArray();
            DictionaryXml<BoundaryBaseXml, BehaviourHolderXml[]> interactionBoundries = new DictionaryXml<BoundaryBaseXml, BehaviourHolderXml[]>(keys, values);
            RectangleXml roiXml = new RectangleXml(ROI);
            WhiskerVideoSettingsXml whiskerSettingsXml = WhiskerSettings != null ? new WhiskerVideoSettingsXml(WhiskerSettings) : null;
            FootVideoSettingsXML FootSettingsXML = FootSettings != null ? new FootVideoSettingsXML() : null;
            TrackedVideoWithSettingsXml filXml = new TrackedVideoWithSettingsXml(WorkingFile, Model.Results.SingleFileResult.Ok, results, motionTrack, smoothedMotionTrackXml, orientationTrack, boundries.ToArray(), events.ToArray(), interactionBoundries, VideoSettings.MinimumInteractionDistance, VideoSettings.GapDistance, VideoSettings.ThresholdValue, VideoSettings.ThresholdValue2, 0, Video.FrameCount - 1, 25, false, 0.68, 0, 0, 0, 0, 0, 1, rect, whiskerSettingsXml, FootSettingsXML);
            XmlSerializer serializer = new XmlSerializer(typeof(TrackedVideoWithSettingsXml));

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, filXml);
            }
        }

        private bool CanSaveFile()
        {
            if (Video == null)
            {
                return false;
            }

            return true;
        }
        
        private void OpenBatchProcessWindow()
        {
            BatchProcessView view = new BatchProcessView();
            BatchProcessViewModel viewModel = new BatchProcessViewModel();
            view.DataContext = viewModel;
            view.Show();
        }

        protected override void WindowClosing(CancelEventArgs closingEventArgs)
        {
            if (VideoLoaded)
            {
                var result = MessageBox.Show("Do you want to save before exiting?", "Save?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                {
                    closingEventArgs.Cancel = true;
                    return;
                }

                if (result == MessageBoxResult.Yes)
                {
                    SaveFile(closingEventArgs);
                }
            }
        }

        private void ExportRawData()
        {
            string fileName = FileBrowser.SaveFile("xlsx|*.xlsx");

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }


            object[,] data = new object[MotionTrack.Length+1,2];
            data[0, 0] = "X";
            data[0, 1] = "Y";

            for (int i = 0; i < MotionTrack.Length; i++)
            {
                data[i + 1, 0] = MotionTrack[i].X;
                data[i + 1, 1] = MotionTrack[i].Y;
            }

            ExcelService.WriteData(data, fileName, false);
        }
    }
}
