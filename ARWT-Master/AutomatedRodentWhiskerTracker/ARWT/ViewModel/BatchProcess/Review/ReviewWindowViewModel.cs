/*Automated Rodent Tracker - A program to automatically track rodents
Copyright(C) 2015 Brett Michael Hewitt

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.If not, see<http://www.gnu.org/licenses/>.*/

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
using ArtLibrary.ModelInterface.Behaviours;
using ArtLibrary.ModelInterface.Boundries;
using ArtLibrary.ModelInterface.RBSK;
using ArtLibrary.ModelInterface.Results;
using ArtLibrary.ModelInterface.Video;
using ArtLibrary.ModelInterface.VideoSettings;
using ArtLibrary.Services.Mouse;
using ArtLibrary.Services.RBSK;
using ARWT.Commands;
using ARWT.Extensions;
using ARWT.Model;
using ARWT.Model.Results;
using ARWT.Model.Whiskers;
using ARWT.ModelInterface.Datasets;
using ARWT.ModelInterface.Datasets.Types;
using ARWT.ModelInterface.RBSK2;
using ARWT.ModelInterface.Results;
using ARWT.ModelInterface.Smoothing;
using ARWT.ModelInterface.Whiskers;
using ARWT.Resolver;
using ARWT.Services;
using ARWT.View.Progress;
using ARWT.ViewModel;
using ARWT.ViewModel.Behaviours;
using ARWT.ViewModel.Datasets;
using ARWT.ViewModel.NewSession;
using ARWT.ViewModel.Progress;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Point = System.Drawing.Point;
using ARWT.View;
using ARWT.Foot.review.viewmodel;
using ARWT.Foot.review;
using ARWT.Model.RBSK2;

namespace ARWT.ViewModel.BatchProcess.Review
{
    public class ReviewWindowViewModel : WindowBaseModel, IDisposable
    {
        private ObservableCollection<SingleFileViewModel> m_SingleFiles;
        public ObservableCollection<SingleFileViewModel> SingleFiles
        {
            get
            {
                return m_SingleFiles;
            }
            set
            {
                if (ReferenceEquals(m_SingleFiles, value))
                {
                    return;
                }

                m_SingleFiles = value;

                NotifyPropertyChanged();
            }
        }

        private SingleFileViewModel m_SelectedVideo;
        public SingleFileViewModel SelectedVideo
        {
            get
            {
                return m_SelectedVideo;
            }
            set
            {
                if (Equals(m_SelectedVideo, value))
                {
                    return;
                }

                m_SelectedVideo = value;

                NotifyPropertyChanged();
                SelectedVideoChanged();
            }
        }
        private bool _AreFootResults;

        public bool AreFootResults
        {
            get { return _AreFootResults; }
            set { _AreFootResults = value;
                NotifyPropertyChanged();
            }
        }

        private ISingleMouse m_Model;
        public ISingleMouse Model
        {
            get
            {
                return m_Model;
            }
            private set
            {
                if (Equals(m_Model, value))
                {
                    return;
                }

                m_Model = value;

                NotifyPropertyChanged();
            }
        }

        private BitmapSource m_Image;
        public BitmapSource DisplayImage
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

        private int m_SliderValue;
        public int SliderValue
        {
            get
            {
                return m_SliderValue;
            }
            set
            {
                if (Equals(m_SliderValue, value))
                {
                    return;
                }

                m_SliderValue = value;

                NotifyPropertyChanged();
                SliderValueChanged();
            }
        }

        private int m_AnalyseStart;
        public int AnalyseStart
        {
            get
            {
                return m_AnalyseStart;
            }
            set
            {
                if (Equals(m_AnalyseStart, value))
                {
                    return;
                }

                m_AnalyseStart = value;

                NotifyPropertyChanged();
                CurrentResult.StartFrame = value;
                SaveCommand.RaiseCanExecuteChangedNotification();
            }
        }

        private int m_AnalyseEnd;
        public int AnalyseEnd
        {
            get
            {
                return m_AnalyseEnd;
            }
            set
            {
                if (Equals(m_AnalyseEnd, value))
                {
                    return;
                }

                m_AnalyseEnd = value;

                NotifyPropertyChanged();
                CurrentResult.EndFrame = value;
                SaveCommand.RaiseCanExecuteChangedNotification();
            }
        }

        private int m_Minimum;
        public int Minimum
        {
            get
            {
                return m_Minimum;
            }
            set
            {
                if (Equals(m_Minimum, value))
                {
                    return;
                }

                m_Minimum = value;

                NotifyPropertyChanged();
            }
        }

        private int m_Maximum;
        public int Maximum
        {
            get
            {
                return m_Maximum;
            }
            set
            {
                if (Equals(m_Maximum, value))
                {
                    return;
                }

                m_Maximum = value;

                NotifyPropertyChanged();
            }
        }

        private IVideo m_Video;
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

                if (m_Video != null)
                {
                    m_Video.Dispose();
                }

                m_Video = value;

                NotifyPropertyChanged();
            }
        }
        private LineSegment2D addROI(LineSegment2D line)
        {
            LineSegment2D points = line;
            var ROI = Results[SelectedVideo.Model].ROI;
            points.P1 = new Point(ROI.X + points.P1.X, ROI.Y + points.P1.Y);
           points.P2 = new Point(ROI.X + points.P2.X, ROI.Y + points.P2.Y);
            return points;

        }

        private PointF[] addROI(PointF[] Points)
        {
            PointF[] points = new PointF[Points.Length];
            var ROI = Results[SelectedVideo.Model].ROI;
            for (int i = 0; i < Points.Length; i++)
            {
                points[i].X = Points[i].X + ROI.X;
                points[i].Y = Points[i].Y + ROI.Y;
            }

            return points;
        }
        private Point[] addROI(Point[] Points)
        {
            Point[] points = new Point[Points.Count()];
            var ROI = Results[SelectedVideo.Model].ROI;
            for (int i = 0; i < Points.Count(); i++)
            {
                points[i].X = Points[i].X + ROI.X;
                points[i].Y = Points[i].Y + ROI.Y;
            }

            return points;
        }
        private Point addROI(Point Points)
        {
            Point points = new Point();
            var ROI = Results[SelectedVideo.Model].ROI;
            points.X = Points.X + ROI.X;
            points.X = Points.Y + ROI.Y;
            return points;
        }
        private PointF addROI(PointF Points)
        {
            PointF points = new PointF();
            var ROI = Results[SelectedVideo.Model].ROI;
            points.X = Points.X + ROI.X;
            points.Y = Points.Y + ROI.Y;
            return points;
        }
        private Dictionary<ISingleFile, IMouseDataExtendedResult> m_Results;
        public Dictionary<ISingleFile, IMouseDataExtendedResult> Results
        {
            get
            {
                return m_Results;
            }
            set
            {
                if (Equals(m_Results, value))
                {
                    return;
                }

                m_Results = value;

                NotifyPropertyChanged();
            }
        }

        private IMouseDataExtendedResult m_CurrentResult;
        public IMouseDataExtendedResult CurrentResult
        {
            get
            {
                return m_CurrentResult;
            }
            set
            {
                if (Equals(m_CurrentResult, value))
                {
                    return;
                }

                m_CurrentResult = value;

                NotifyPropertyChanged();
                SaveCommand.RaiseCanExecuteChangedNotification();
            }
        }

        private string m_DisplayText;
        public string DisplayText
        {
            get
            {
                return m_DisplayText;
            }
            set
            {
                if (Equals(m_DisplayText, value))
                {
                    return;
                }

                m_DisplayText = value;

                NotifyPropertyChanged();
            }
        }

        private bool m_ShowVideo;
        public bool ShowVideo
        {
            get
            {
                return m_ShowVideo;
            }
            set
            {
                if (Equals(m_ShowVideo, value))
                {
                    return;
                }

                m_ShowVideo = value;

                NotifyPropertyChanged();
            }
        }

        private double m_GapDistance;
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

                NotifyPropertyChanged();
                UpdateGapDistance();
            }
        }

        private double m_GapDistanceMin;
        public double GapDistanceMin
        {
            get
            {
                return m_GapDistanceMin;
            }
            set
            {
                if (Equals(m_GapDistanceMin, value))
                {
                    return;
                }

                m_GapDistanceMin = value;

                NotifyPropertyChanged();
            }
        }

        private double m_GapDistanceMax;
        public double GapDistanceMax
        {
            get
            {
                return m_GapDistanceMax;
            }
            set
            {
                if (Equals(m_GapDistanceMax))
                {
                    return;
                }

                m_GapDistanceMax = value;

                NotifyPropertyChanged();
            }
        }

        private int m_BinaryThreshold;
        public int BinaryThreshold
        {
            get
            {
                return m_BinaryThreshold;
            }
            set
            {
                if (Equals(m_BinaryThreshold, value))
                {
                    return;
                }

                m_BinaryThreshold = value;

                NotifyPropertyChanged();
                UpdateBinaryThreshold();
            }
        }

        private int m_BinaryThresholdMax;
        public int BinaryThresholdMax
        {
            get
            {
                return m_BinaryThresholdMax;
            }
            set
            {
                if (Equals(m_BinaryThresholdMax, value))
                {
                    return;
                }

                m_BinaryThresholdMax = value;

                NotifyPropertyChanged();
            }
        }

        private int m_BinaryThresholdMin;
        public int BinaryThresholdMin
        {
            get
            {
                return m_BinaryThresholdMin;
            }
            set
            {
                if (Equals(m_BinaryThresholdMin, value))
                {
                    return;
                }

                m_BinaryThresholdMin = value;

                NotifyPropertyChanged();
            }
        }

        private int m_BinaryThreshold2;
        public int BinaryThreshold2
        {
            get
            {
                return m_BinaryThreshold2;
            }
            set
            {
                if (Equals(m_BinaryThreshold2, value))
                {
                    return;
                }

                m_BinaryThreshold2 = value;

                NotifyPropertyChanged();
                UpdateBinaryThreshold2();
            }
        }

        private int m_BinaryThreshold2Max;
        public int BinaryThreshold2Max
        {
            get
            {
                return m_BinaryThreshold2Max;
            }
            set
            {
                if (Equals(m_BinaryThreshold2Max, value))
                {
                    return;
                }

                m_BinaryThreshold2Max = value;

                NotifyPropertyChanged();
            }
        }


        private int m_BinaryThreshold2Min;
        public int BinaryThreshold2Min
        {
            get
            {
                return m_BinaryThreshold2Min;
            }
            set
            {
                if (Equals(m_BinaryThreshold2Min, value))
                {
                    return;
                }

                m_BinaryThreshold2Min = value;

                NotifyPropertyChanged();
            }
        }


        private int _areaThresholdMin;
        public int areaThresholdMin
        {
            get
            {
                return _areaThresholdMin;
            }
            set
            {
                if (Equals(_areaThresholdMin, value))
                {
                    return;
                }

                _areaThresholdMin = value;

                NotifyPropertyChanged();
            }
        }
        private int _areaThresholdMax;
        public int areaThresholdMax
        {
            get
            {
                return _areaThresholdMax;
            }
            set
            {
                if (Equals(_areaThresholdMax, value))
                {
                    return;
                }

                _areaThresholdMax = value;

                NotifyPropertyChanged();
            }
        }
        private Image<Bgr, Byte> m_CurrentImage;
        public Image<Bgr, Byte> CurrentImage
        {
            get
            {
                return m_CurrentImage;
            }
            set
            {
                if (Equals(m_CurrentImage, value))
                {
                    return;
                }

                if (m_CurrentImage != null)
                {
                    m_CurrentImage.Dispose();
                }

                m_CurrentImage = value;

                NotifyPropertyChanged();
                
            }
        }
        private string m_WindowTitle;
        public string WindowTitle
        {
            get
            {
                return m_WindowTitle;
            }
            set
            {
                if (Equals(m_WindowTitle, value))
                {
                    return;
                }

                m_WindowTitle = value;

                NotifyPropertyChanged();
            }
        }
        private string m_FrameNumberDisplay;
        public string FrameNumberDisplay
        {
            get
            {
                return  m_FrameNumberDisplay;
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

        private bool m_VideoSelected;
        public bool VideoSelected
        {
            get
            {
                return m_VideoSelected;
            }
            set
            {
                if (Equals(m_VideoSelected, value))
                {
                    return;
                }

                m_VideoSelected = value;

                NotifyPropertyChanged();
                ReRunCommand.RaiseCanExecuteChangedNotification();
                ExportCommand.RaiseCanExecuteChangedNotification();
                ExportCSVCommand.RaiseCanExecuteChangedNotification();
                ExportMeanCSVCommand.RaiseCanExecuteChangedNotification();
                SaveCommand.RaiseCanExecuteChangedNotification();
                SetStartFrameCommand.RaiseCanExecuteChangedNotification();
                SetEndFrameCommand.RaiseCanExecuteChangedNotification();
            }
        }

        private string m_Name;
        

        public string Name
        {
            get
            {
                return m_Name;
            }
            private set
            {
                if (Equals(m_Name, value))
                {
                    return;
                }

                m_Name = value;

                NotifyPropertyChanged();
            }
        }

        private int m_Age;
        public int Age
        {
            get
            {
                return m_Age;
            }
            private set
            {
                if (Equals(m_Age, value))
                {
                    return;
                }

                m_Age = value;

                NotifyPropertyChanged();
            }
        }

        private ITypeBase m_Type;
        public ITypeBase Type
        {
            get
            {
                return m_Type;
            }
            private set
            {
                if (Equals(m_Type, value))
                {
                    return;
                }

                m_Type = value;

                NotifyPropertyChanged();
            }
        }

        private double m_DistanceTravelled;
        public double DistanceTravelled
        {
            get
            {
                return m_DistanceTravelled;
            }
            set
            {
                if (Equals(m_DistanceTravelled, value))
                {
                    return;
                }

                m_DistanceTravelled = value;

                NotifyPropertyChanged();
            }
        }

        private double m_CentroidDistanceTravelled;
        public double CentroidDistanceTravelled
        {
            get
            {
                return m_CentroidDistanceTravelled;
            }
            set
            {
                if (Equals(m_CentroidDistanceTravelled, value))
                {
                    return;
                }

                m_CentroidDistanceTravelled = value;

                NotifyPropertyChanged();
            }
        }


        private bool m_SmoothMotion;
        public bool SmoothMotion
        {
            get
            {
                return m_SmoothMotion;
            }
            set
            {
                if (Equals(m_SmoothMotion, value))
                {
                    return;
                }

                m_SmoothMotion = value;

                NotifyPropertyChanged();

                if (CurrentResult != null)
                {
                    CurrentResult.SmoothMotion = value;
                    SmoothMotionChanged();
                }
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
            }
        }

        private PointF[] m_CentroidMotionTrack;
        public PointF[] CentroidMotionTrack
        {
            get
            {
                return m_CentroidMotionTrack;
            }
            set
            {
                if (Equals(m_CentroidMotionTrack, value))
                {
                    return;
                }

                m_CentroidMotionTrack = value;

                NotifyPropertyChanged();
            }
        }


        private double m_SmoothingValue = 0.68;
        public double SmoothingValue
        {
            get
            {
                return m_SmoothingValue;
            }
            set
            {
                if (Equals(m_SmoothingValue, value))
                {
                    return;
                }

                m_SmoothingValue = value;

                NotifyPropertyChanged();
                //MotionTrack = TestSmooth1();
                //MotionTrack = TestSmooth1(MotionTrack);
                UpdateDisplayImage();
                //Console.WriteLine(value);
            }
        }

        private double m_FrameRate;
        public double FrameRate
        {
            get
            {
                return m_FrameRate;
            }
            set
            {
                if (Equals(m_FrameRate, value))
                {
                    return;
                }

                m_FrameRate = value;

                NotifyPropertyChanged();
            }
        }

        private double m_Duration;
        public double Duration
        {
            get
            {
                return m_Duration;
            }
            set
            {
                if (Equals(m_Duration, value))
                {
                    return;
                }

                m_Duration = value;

                NotifyPropertyChanged();
            }
        }

        private double m_AvgVelocity;
        public double AvgVelocity
        {
            get
            {
                return m_AvgVelocity;
            }
            set
            {
                if (Equals(m_AvgVelocity, value))
                {
                    return;
                }

                m_AvgVelocity = value;

                NotifyPropertyChanged();
            }
        }

        private double m_AvgCentroidVelocity;
        public double AvgCentroidVelocity
        {
            get
            {
                return m_AvgCentroidVelocity;
            }
            set
            {
                if (Equals(m_AvgCentroidVelocity, value))
                {
                    return;
                }

                m_AvgCentroidVelocity = value;

                NotifyPropertyChanged();
            }
        }


        private double m_AvgAngularVelocity;
        public double AvgAngularVelocity
        {
            get
            {
                return m_AvgAngularVelocity;
            }
            set
            {
                if (Equals(m_AvgAngularVelocity, value))
                {
                    return;
                }

                m_AvgAngularVelocity = value;

                NotifyPropertyChanged();
            }
        }

        private double m_MaxVelocity;
        public double MaxVelocity
        {
            get
            {
                return m_MaxVelocity;
            }
            set
            {
                if (Equals(m_MaxVelocity, value))
                {
                    return;
                }

                m_MaxVelocity = value;

                NotifyPropertyChanged();
            }
        }

        private double m_MaxCentroidVelocity;
        public double MaxCentroidVelocity
        {
            get
            {
                return m_MaxCentroidVelocity;
            }
            set
            {
                if (Equals(m_MaxCentroidVelocity, value))
                {
                    return;
                }

                m_MaxCentroidVelocity = value;

                NotifyPropertyChanged();
            }
        }

        private double m_MaxAngularVelocity;
        public double MaxAngularVelocity
        {
            get
            {
                return m_MaxAngularVelocity;
            }
            set
            {
                if (Equals(m_MaxAngularVelocity, value))
                {
                    return;
                }

                m_MaxAngularVelocity = value;

                NotifyPropertyChanged();
            }
        }


        private double m_CentroidSize;
        public double CentroidSize
        {
            get
            {
                return m_CentroidSize;
            }
            set
            {
                if (Equals(m_CentroidSize, value))
                {
                    return;
                }

                m_CentroidSize = value;

                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<BehaviourHolderViewModel> m_SelectedMouseBehaviours;
        public ObservableCollection<BehaviourHolderViewModel> SelectedMouseBehaviours
        {
            get
            {
                return m_SelectedMouseBehaviours;
            }
            set
            {
                if (Equals(m_SelectedMouseBehaviours, value))
                {
                    return;
                }

                m_SelectedMouseBehaviours = value;

                NotifyPropertyChanged();
            }
        }

        private ActionCommand m_OkCommand;
        public ActionCommand OkCommand
        {
            get
            {
                return m_OkCommand ?? (m_OkCommand = new ActionCommand()
                {
                    ExecuteAction = Ok,
                });
            }
        }

        private ActionCommand m_CancelCommand;
        public ActionCommand CancelCommand
        {
            get
            {
                return m_CancelCommand ?? (m_CancelCommand = new ActionCommand()
                {
                    ExecuteAction = Cancel,
                });
            }
        }

        private ActionCommand m_ReRunCommand;
        public ActionCommand ReRunCommand
        {
            get
            {
                return m_ReRunCommand ?? (m_ReRunCommand = new ActionCommand()
                {
                    ExecuteAction = ReRun,
                    CanExecuteAction = CanReRun,
                });
            }
        }

        private ActionCommand m_AutoGapDistanceCommand;
        public ActionCommand AutoGapDistanceCommand
        {
            get
            {
                return m_AutoGapDistanceCommand ?? (m_AutoGapDistanceCommand = new ActionCommand()
                {
                    ExecuteAction = AutoFindGapDistance,
                });
            }
        }

        private ActionCommand m_CancelReRunCommand;
        public ActionCommand CancelReRunCommand
        {
            get
            {
                return m_CancelReRunCommand ?? (m_CancelReRunCommand = new ActionCommand()
                {
                    ExecuteAction = CancelReRun,
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
                    ExecuteAction = SaveFile,
                    CanExecuteAction = CanSaveFile,
                });
            }
        }

        private ActionCommand m_ExportCommand;
        public ActionCommand ExportCommand
        {
            get
            {
                return m_ExportCommand ?? (m_ExportCommand = new ActionCommand()
                {
                    ExecuteAction = Export,
                    CanExecuteAction = CanExport,
                });
            }
        }

        private ActionCommand m_ExportCSVCommand;
        public ActionCommand ExportCSVCommand
        {
            get
            {
                return m_ExportCSVCommand ?? (m_ExportCSVCommand = new ActionCommand()
                {
                    ExecuteAction = ExportCSV,
                    CanExecuteAction = CanExport,
                });
            }
        }
        private ActionCommand m_ExportMeanCSVCommand;
        public ActionCommand ExportMeanCSVCommand
        {
            get
            {
                return m_ExportMeanCSVCommand ?? (m_ExportMeanCSVCommand = new ActionCommand()
                {
                    ExecuteAction = ExportMeanCSV,
                    CanExecuteAction = CanExport,
                });
            }
        }

        private ActionCommand m_SetStartFrameCommand;
        public ActionCommand SetStartFrameCommand
        {
            get
            {
                return m_SetStartFrameCommand ?? (m_SetStartFrameCommand = new ActionCommand()
                {
                    ExecuteAction = SetStartFrame,
                    CanExecuteAction = CanSetStartFrame,
                });
            }
        }

        private ActionCommand m_SetEndFrameCommand;
        public ActionCommand SetEndFrameCommand
        {
            get
            {
                return m_SetEndFrameCommand ?? (m_SetEndFrameCommand = new ActionCommand()
                {
                    ExecuteAction = SetEndFrame,
                    CanExecuteAction = CanSetEndFrame,
                });
            }
        }

        private string m_ArtFile;
        public string ArtFile
        {
            get
            {
                return m_ArtFile;
            }
            set
            {
                if (Equals(m_ArtFile, value))
                {
                    return;
                }

                m_ArtFile = value;

                NotifyPropertyChanged();
            }
        }

        private Dictionary<int, Color> ColorDic
        {
            get;
            set;
        }

        private IWhiskerVideoSettings WhiskerSettings
        {
            get;
            set;
        }
        private IFootVideoSettings FootSettings
        {
            get;
            set;
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
        private int _areaThreshold;

        public int areaTheshold
        {
            get { return _areaThreshold; }
            set { _areaThreshold = value;
                NotifyPropertyChanged();
                CurrentResult.FootSettings.area = value;
            }
        }
        private int _scaleFactor;

        public int scaleFactor
        {
            get { return _scaleFactor; }
            set
            {
                _scaleFactor = value;
                NotifyPropertyChanged();
                CurrentResult.FootSettings.scaleFactor = Math.Abs(value);
            }
        }

        private int _kernelSize;

        public int KernelSize
        {
            get { return _kernelSize; }
            set
            {
                if (value % 2 == 0)
                {
                    _kernelSize = Math.Abs(value) + 1;
                }else
                {
                    _kernelSize = Math.Abs(value);
                }
               
                NotifyPropertyChanged();
                CurrentResult.FootSettings.kernelSize = Math.Abs(value);
            }
        }
        private int _contourDistance;

        public int contourDistance
        {
            get { return _contourDistance; }
            set
            {
                _contourDistance = value;
                NotifyPropertyChanged();
                CurrentResult.FootSettings.contourDistance = Math.Abs(value);
            }
        }
        private int _erosionIterations;

        public int erosionIterations
        {
            get { return _erosionIterations; }
            set
            {
                _erosionIterations = value;
                NotifyPropertyChanged();
                CurrentResult.FootSettings.erosionIterations = Math.Abs(value);
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

        private bool _IncludeFeets = true;
        public bool IncludeFeet
        {
            get
            {
                return _IncludeFeets;
            }
            set
            {
                if (Equals(_IncludeFeets, value))
                {
                    return;
                }

                _IncludeFeets = value;

                NotifyPropertyChanged();
            }
        }
        private bool _IncludeWhiskers = true;
        public bool IncludeWhiskers
        {
            get
            {
                return _IncludeWhiskers;
            }
            set
            {
                if (Equals(_IncludeWhiskers, value))
                {
                    return;
                }

                _IncludeWhiskers = value;

                NotifyPropertyChanged();
            }
        }

        
        private ActionCommand _PreviewCommand;

        public ActionCommand PreviewCommand
        {
            get
            {
                return _PreviewCommand ?? (_PreviewCommand = new ActionCommand()
                {
                    ExecuteAction = Preview
                });
            }
        }

        private ActionCommand _ReviewCommand;

        public ActionCommand ReviewCommand
        {
            get
            {
                return _ReviewCommand ?? (_ReviewCommand = new ActionCommand()
                {
                    ExecuteAction = ShowData,
                    CanExecuteAction = () => Results != null && Results.Any(),
                });
            }
        }
        
            private ActionCommand _ReviewFootCommand;

        public ActionCommand ReviewFootCommand
        {
            get
            {
                return _ReviewFootCommand ?? (_ReviewFootCommand = new ActionCommand()
                {
                    ExecuteAction = ShowFootData,
                    CanExecuteAction = () => Results != null && Results.Any(),
                });
            }
        }
        private void ShowFootData()
        {
            IVideoSettings videoSettings = ModelResolver.Resolve<IVideoSettings>();
            videoSettings.FileName = SelectedVideo.VideoFileName;
            videoSettings.ThresholdValue = BinaryThreshold;

            FoottrackingReview footReviewView = new FoottrackingReview(new FootReviewViewModel(Video, CurrentResult));
            footReviewView.ShowDialog();
        }
        private void ShowData()
        {
            
           
            IVideoSettings videoSettings = ModelResolver.Resolve<IVideoSettings>();
            videoSettings.FileName = SelectedVideo.VideoFileName;
            videoSettings.ThresholdValue = BinaryThreshold;

            SingleVideoExportViewModel vm = new SingleVideoExportViewModel();
            vm.LoadData(CurrentResult);

            SingleVideoExport v = new SingleVideoExport();
            v.DataContext = vm;
            v.ShowDialog();
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

        private KeyValuePair<int, double>[] _LeftGraphWhiskers;
        public KeyValuePair<int, double>[] LeftGraphWhiskers
        {
            get
            {
                return _LeftGraphWhiskers;
            }
            set
            {
                if (Equals(_LeftGraphWhiskers, value))
                {
                    return;
                }

                _LeftGraphWhiskers = value;

                NotifyPropertyChanged();
            }
        }

        private KeyValuePair<int, double>[] _LeftGraphWhiskersSmoothed;
        public KeyValuePair<int, double>[] LeftGraphWhiskersSmoothed
        {
            get
            {
                return _LeftGraphWhiskersSmoothed;
            }
            set
            {
                if (Equals(_LeftGraphWhiskersSmoothed, value))
                {
                    return;
                }

                _LeftGraphWhiskersSmoothed = value;

                NotifyPropertyChanged();
            }
        }

        private KeyValuePair<int, double>[] _RightGraphWhiskers;
        public KeyValuePair<int, double>[] RightGraphWhiskers
        {
            get
            {
                return _RightGraphWhiskers;
            }
            set
            {
                if (Equals(_RightGraphWhiskers, value))
                {
                    return;
                }

                _RightGraphWhiskers = value;

                NotifyPropertyChanged();
            }
        }

        private KeyValuePair<int, double>[] _RightGraphWhiskersSmoothed;
        public KeyValuePair<int, double>[] RightGraphWhiskersSmoothed
        {
            get
            {
                return _RightGraphWhiskersSmoothed;
            }
            set
            {
                if (Equals(_RightGraphWhiskersSmoothed, value))
                {
                    return;
                }

                _RightGraphWhiskersSmoothed = value;

                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<ISmoothingBase> _SmoothingFunctions;
        public ObservableCollection<ISmoothingBase> SmoothingFunctions
        {
            get
            {
                return _SmoothingFunctions;
            }
            set
            {
                if (ReferenceEquals(_SmoothingFunctions, value))
                {
                    return;
                }

                _SmoothingFunctions = value;

                NotifyPropertyChanged();
            }
        }

        private ISmoothingBase _SelectedSmoothingFunction;
        public ISmoothingBase SelectedSmoothingFunction
        {
            get
            {
                return _SelectedSmoothingFunction;
            }
            set
            {
                if (Equals(_SelectedSmoothingFunction, value))
                {
                    return;
                }

                _SelectedSmoothingFunction = value;

                NotifyPropertyChanged();
                UpdateSmoothing();
            }
        }

        private int _RepeatSmooths = 1;
        public int RepeatSmooths
        {
            get
            {
                return _RepeatSmooths;
            }
            set
            {
                if (Equals(_RepeatSmooths, value))
                {
                    return;
                }

                _RepeatSmooths = value;

                NotifyPropertyChanged();
                UpdateSmoothing();
            }
        }

        private void Preview()
        {
            IVideoSettings videoSettings = ModelResolver.Resolve<IVideoSettings>();
            videoSettings.FileName = SelectedVideo.VideoFileName;
            videoSettings.ThresholdValue = BinaryThreshold;
            videoSettings.Roi = Results[SelectedVideo.Model].ROI;
            Image<Gray, Byte> binaryBackground;
            IEnumerable<IBoundaryBase> boundaries;
            videoSettings.GeneratePreview(Video, out binaryBackground, out boundaries);
            

            IRBSKVideo2 rbsk = ModelResolver.Resolve<IRBSKVideo2>();
            rbsk.GapDistance = GapDistance;
            rbsk.ThresholdValue = BinaryThreshold;
            rbsk.Video = Video;
            rbsk.BackgroundImage = binaryBackground;
            rbsk.Roi = Results[SelectedVideo.Model].ROI;
            rbsk.WhiskerSettings = WhiskerSettings;
            rbsk.FootSettings = FootSettings;
            IWhiskerCollection whiskers = null;
            PointF[] headPoints;
            Point[] bodyPoints;
            PointF headPoint;
            rbsk.GetHeadAndBody(CurrentImage.Clone(), out headPoints, out bodyPoints);
            if (headPoints == null)
            {
                return;
            }
            IWhiskerCollection finalWhiskers = ModelResolver.Resolve<IWhiskerCollection>();
            using (Image<Gray, byte> gray = CurrentImage.Convert<Gray, byte>())
            {
                whiskers = rbsk.ProcessWhiskersForSingleFrame(gray, headPoints, bodyPoints);
               
                headPoint = headPoints[2];
                if (WhiskerSettings.RemoveDuds)
                {
                    
                    PointF midPoint = headPoints[1].MidPoint(headPoints[3]);
                    Vector orientation = new Vector(headPoint.X - midPoint.X, headPoint.Y - midPoint.Y);
                    PostProcessWhiskers2(midPoint, orientation, whiskers, finalWhiskers);
                }
                else
                {
                    finalWhiskers = whiskers;
                }
            }
            
            using (Image<Bgr, Byte> img = CurrentImage.Clone())
            {
                headPoints = addROI(headPoints);
                var motionTrack = addROI(MotionTrack.Select(x => x.ToPoint()).ToArray());
                img.DrawPolyline(motionTrack, false, new Bgr(Color.Blue), 2);

                if (headPoints != null)
                {
                    
                    foreach (PointF point in headPoints)
                    {
                        img.Draw(new CircleF(point, 2), new Bgr(Color.Yellow), 2);
                    }

                    if (finalWhiskers != null)
                    {
                        if (finalWhiskers.LeftWhiskers != null && finalWhiskers.LeftWhiskers.Any())
                        {
                            //currentFrame.Draw(cWhiskers.LeftWhiskers[5].Line, new Bgr(Color.White), 1);

                            foreach (IWhiskerSegment whisker in finalWhiskers.LeftWhiskers)
                            {
                                Color color = Color.White;
                                var line = addROI(whisker.Line);
                                img.Draw(line, new Bgr(color), 1);
                            }

                        }

                        if (finalWhiskers.RightWhiskers != null && finalWhiskers.RightWhiskers.Any())
                        {
                            foreach (IWhiskerSegment whisker in finalWhiskers.RightWhiskers)
                            {
                                Color color = Color.White;
                                var line = addROI(whisker.Line);
                                img.Draw(line, new Bgr(color), 1);
                            }
                        }
                    }
                }

                DisplayImage = ImageService.ToBitmapSource(img);
            }
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

        public ReviewWindowViewModel(ISingleMouse model, Dictionary<ISingleFile, IMouseDataExtendedResult> results, string artFile)
        {
            
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

            ObservableCollection<SingleFileViewModel> videos = new ObservableCollection<SingleFileViewModel>();
            foreach (ISingleFile singleFile in model.VideoFiles)
            {
                
                SingleFileViewModel vm = new SingleFileViewModel(singleFile, artFile);
                IMouseDataExtendedResult data = results[singleFile];

                if (data != null)
                {
                    vm.VideoOutcome = data.VideoOutcome;

                    
                    if (data.VideoOutcome != SingleFileResult.Ok)
                    {
                        vm.Comments = data.Message;
                    }
                    else
                    {
                        vm.Comments = data.MainBehaviour;
                        
                    }
                }
                videos.Add(vm);
            }

            SingleFiles = videos;
            Model = model;
            
            Results = results;
            ArtFile = artFile;
            GapDistanceMin = 5;
            GapDistanceMax = 300;
            BinaryThresholdMin = 0;
            BinaryThresholdMax = 255;
            BinaryThreshold2Min = 0;
            BinaryThreshold2Max = 255;
            areaThresholdMax = 200;
            areaThresholdMin = 1;
            Name =  Model.Name;
            Age = Model.Age;
            Type = Model.Type;
            
            SelectedVideo = SingleFiles.First();
            
            ObservableCollection<ISmoothingBase> smoothingFunctions = new ObservableCollection<ISmoothingBase>();
            smoothingFunctions.Add(ModelResolver.Resolve<iMovingAverage2>());
            smoothingFunctions.Add(ModelResolver.Resolve<iMovingAverage>());
            smoothingFunctions.Add(ModelResolver.Resolve<IGaussianSmoothing>());
            smoothingFunctions.Add(ModelResolver.Resolve<IBoxCarSmoothing>());
            SmoothingFunctions = smoothingFunctions;
            SelectedSmoothingFunction = SmoothingFunctions.First();

            IMouseDataExtendedResult fileResult = results.First().Value;
            
            IWhiskerAverageAngles angles = new WhiskerAverageAngles();
            angles.StartFrame = 0;
            angles.EndFrame = 0;
            if(fileResult.Results != null)
            {
                angles.EndFrame = fileResult.Results.Count - 1;
                var allAngles = angles.GetWhiskerAngles(fileResult.Results);
                LoadData(allAngles[0], allAngles[1]);
            }
            
            AreFootResults = true;
        }

        public void LoadData(Dictionary<int, double> leftWhiskers, Dictionary<int, double> rightWhiskers)
        {
            LeftGraphWhiskers = leftWhiskers.Select(x => x).ToArray();
            RightGraphWhiskers = rightWhiskers.Select(x => x).ToArray();

            UpdateSmoothing();
        }

        private void UpdateSmoothing()
        {
            if (SelectedSmoothingFunction == null || LeftGraphWhiskers == null || RightGraphWhiskers == null)
            {
                return;
            }

            double[] leftData = GetDoubleValues(LeftGraphWhiskers);
            double[] rightData = GetDoubleValues(RightGraphWhiskers);

            if (SelectedSmoothingFunction != null)
            {
                for (int i = 0; i < RepeatSmooths; i++)
                {
                    leftData = SelectedSmoothingFunction.Smooth(leftData);
                    rightData = SelectedSmoothingFunction.Smooth(rightData);
                }
            }

            double[] smoothedLeft = leftData;
            double[] smootedRight = rightData;


            List<KeyValuePair<int, double>> smoothedLeftGraph = new List<KeyValuePair<int, double>>();
            List<KeyValuePair<int, double>> smoothedRightGraph = new List<KeyValuePair<int, double>>();

            int leftCounter = 0;
            foreach (var kvp in LeftGraphWhiskers)
            {
                smoothedLeftGraph.Add(new KeyValuePair<int, double>(kvp.Key, smoothedLeft[leftCounter]));
                leftCounter++;
            }

            int rightCounter = 0;
            foreach (var kvp in RightGraphWhiskers)
            {
                smoothedRightGraph.Add(new KeyValuePair<int, double>(kvp.Key, smootedRight[rightCounter]));
                rightCounter++;
            }

            LeftGraphWhiskersSmoothed = smoothedLeftGraph.ToArray();
            RightGraphWhiskersSmoothed = smoothedRightGraph.ToArray();
        }

        private double[] GetDoubleValues(KeyValuePair<int, double>[] array)
        {
            return array.Select(x => x.Value).ToArray();
        }

        private void SelectedVideoChanged()
        {
            VideoSelected = SelectedVideo != null;

            if (SelectedVideo == null)
            {
                DisplayText = "";
                ShowVideo = false;
                return;
            }

            CurrentResult = Results[SelectedVideo.Model];
            
            //if (CurrentResult.VideoOutcome == SingleFileResult.Ok)
            //{
                Video = ModelResolver.Resolve<IVideo>();
                Video.SetVideo(SelectedVideo.VideoFileName);
                Maximum = Video.FrameCount - 1;
                Minimum = 0;
                m_SliderValue = -1;
                m_WindowTitle = SelectedVideo.VideoFileName;
                ShowVideo = true;
                m_GapDistance = CurrentResult.GapDistance;
                WhiskerSettings = CurrentResult.WhiskerSettings;
                FootSettings = CurrentResult.FootSettings;
                if(FootSettings == null ||  FootSettings.kernelSize == 0 && FootSettings.contourDistance == 0 && FootSettings.scaleFactor == 0 && FootSettings.erosionIterations == 0)
                {
                    FootSettings = new FootVideoSettings();
                    FootSettings.AssignDefaultValues();
                    CurrentResult.FootSettings = FootSettings;
                }
                m_BinaryThreshold = CurrentResult.ThresholdValue;
                m_BinaryThreshold2 = CurrentResult.ThresholdValue2;
                areaTheshold = FootSettings.area;
                scaleFactor = FootSettings.scaleFactor;
                KernelSize = FootSettings.kernelSize;
                erosionIterations = FootSettings.erosionIterations;
                contourDistance = FootSettings.contourDistance;
                AnalyseEnd = CurrentResult.EndFrame;
                DistanceTravelled = CurrentResult.DistanceTravelled;
                CentroidDistanceTravelled = CurrentResult.CentroidDistanceTravelled;
                MotionTrack = CurrentResult.MotionTrack;
                CentroidMotionTrack = CurrentResult.CentroidMotionTrack;
                FrameRate = CurrentResult.FrameRate;
                Duration = CurrentResult.Duration;
                AvgVelocity = CurrentResult.AverageVelocity;
                AvgCentroidVelocity = CurrentResult.AverageCentroidVelocity;
                AvgAngularVelocity = CurrentResult.AverageAngularVelocity;
                MaxVelocity = CurrentResult.MaxSpeed;
                MaxCentroidVelocity = CurrentResult.MaxCentroidSpeed;
                MaxAngularVelocity = CurrentResult.MaxAngularVelocty;
                CentroidSize = CurrentResult.CentroidSize;

                List<BehaviourHolderViewModel> behaviours = new List<BehaviourHolderViewModel>();
                Dictionary<IBoundaryBase, IBehaviourHolder[]> interactingBoundries = CurrentResult.InteractingBoundries;
                if(interactingBoundries != null)
                {
                    foreach (KeyValuePair<IBoundaryBase, IBehaviourHolder[]> kvp in interactingBoundries)
                    {
                        BoundaryBaseViewModel boundaryVm = BoundaryBaseViewModel.GetBoundaryFromModel(kvp.Key);
                        IBehaviourHolder[] currentBehaviours = kvp.Value;
                        behaviours.AddRange(currentBehaviours.Select(currentBehaviour => new BehaviourHolderViewModel(boundaryVm, currentBehaviour.Interaction, currentBehaviour.FrameNumber)));
                    }
                }


                var tempBehaviours = behaviours.OrderBy(x => x.FrameNumber);
                SelectedMouseBehaviours = new ObservableCollection<BehaviourHolderViewModel>(tempBehaviours);


                NotifyPropertyChanged("GapDistance");
                NotifyPropertyChanged("BinaryThreshold");
                NotifyPropertyChanged("BinaryThreshold2");

                //if (SmoothMotion)
                //{
                //    SmoothMotionChanged(true);
                //}
                
                SliderValue = 0;
                SmoothMotion = CurrentResult.SmoothMotion;
            //}
            //else
            //{
            //    DisplayText = CurrentResult.Message;
            //    WhiskerSettings = CurrentResult.WhiskerSettings;
            //    ShowVideo = false;
            //}
        }

        private void SliderValueChanged()
        {
            Video.SetFrame(SliderValue);
            CurrentImage = Video.GetFrameImage();
            UpdateDisplayImage();

            FrameNumberDisplay = "Frame: " + SliderValue;
        }
        private void UpdateDisplayImage()
        {
            using (Image<Bgr, Byte> img = CurrentImage.Clone())
            {   
                if(MotionTrack != null)
                {
                    Point[] motionPoints = addROI(MotionTrack.Select(x => x.ToPoint()).ToArray());
                    Point[] centroidPoints = addROI(CentroidMotionTrack.Select(x => x.ToPoint()).ToArray());
                    img.DrawPolyline(motionPoints, false, new Bgr(Color.Blue), 2);
                    img.DrawPolyline(centroidPoints, false, new Bgr(Color.Yellow), 2);

                    if (SliderValue >= AnalyseStart && SliderValue <= AnalyseEnd)
                    {
                        ISingleFrameExtendedResults frame = CurrentResult.Results[SliderValue];

                        if (!frame.HeadPoint.IsEmpty)
                        {
                            PointF headPoint = addROI(frame.HeadPoint);
                            PointF centr = addROI(frame.Centroid);
                            img.Draw(new CircleF(headPoint, 2), new Bgr(Color.Red), 2);
                            img.Draw(new CircleF(centr, 2), new Bgr(Color.Red), 2);

                            if (Results != null && Results.Any() && frame.HeadPoints != null)
                            {
                                PointF midPoint = addROI(frame.MidPoint);
                                // IWhiskerCollection cWhiskers = currentResult.Whiskers;
                                IWhiskerCollection cWhiskers = frame.BestTrackedWhisker;
                                Vector orientation = frame.Orientation;
                                orientation = new Vector(frame.HeadPoints[2].X - midPoint.X, frame.HeadPoints[2].Y - midPoint.Y);
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
                                            LineSegment2D line = addROI(whisker.Line);
                                            img.Draw(line, new Bgr(color), 1);
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
                                        lineSeg = addROI(lineSeg);
                                        img.Draw(lineSeg, new Bgr(Color.Cyan), 1);
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
                                            LineSegment2D line = addROI(whisker.Line);
                                            img.Draw(line, new Bgr(color), 1);
                                            counter++;
                                        }

                                        var xs = cWhiskers.RightWhiskers.Select(x => x.X).Average();
                                        var ys = cWhiskers.RightWhiskers.Select(x => x.Y).Average();

                                        var angAvg = cWhiskers.RightWhiskers.Select(x => x.Angle).Average();


                                        double horizontalAngle = Vector.AngleBetween(orientation, new Vector(1, 0));
                                        double hA = angAvg - horizontalAngle;
                                        double dX = 10 * Math.Cos(hA * 0.0174533);
                                        double dY = 10 * Math.Sin(hA * 0.0174533);
                                        Point lp1 = new Point((int)(xs + dX), (int)(ys + dY));
                                        Point lp2 = new Point((int)(xs - dX), (int)(ys - dY));

                                        LineSegment2D lineSeg = new LineSegment2D(lp1, lp2);
                                        lineSeg = addROI(lineSeg);
                                        img.Draw(lineSeg, new Bgr(Color.Cyan), 1);
                                    }
                                }
                            }
                        }
                    }
                }

                DisplayImage = ImageService.ToBitmapSource(img);
            }
        }

        public void Dispose()
        {
            if (Video != null)
            {
                Video.Dispose();
            }
        }

        private void Ok()
        {
            
            Close = true;
        }

        private void Cancel()
        {
            Close = true;
        }

        private void ReRun()
        {
            if (SelectedVideo == null)
            {
                return;
            }

            ReRunVideo(SelectedVideo.VideoFileName);
        }

        private void UpdateGapDistance()
        {
            if (CurrentResult != null)
            {
                CurrentResult.GapDistance = GapDistance;
            }
           
            RBSK rbsk = MouseService.GetStandardMouseRules();
            rbsk.Settings.GapDistance = GapDistance;
            rbsk.Settings.BinaryThreshold = BinaryThreshold;
            PointF[] result = RBSKService.RBSK(CurrentImage, rbsk);
            using (Image<Bgr, Byte> img = CurrentImage.Clone())
            {
                img.DrawPolyline(MotionTrack.Select(x => x.ToPoint()).ToArray(), false, new Bgr(Color.Blue), 2);

                if (result != null)
                {
                    foreach (PointF point in result)
                    {
                        img.Draw(new CircleF(point, 2), new Bgr(Color.Yellow), 2);
                    }
                }

                DisplayImage = ImageService.ToBitmapSource(img);
            }
        }

        private void UpdateBinaryThreshold()
        {
            if (CurrentResult != null)
            {
                CurrentResult.ThresholdValue = BinaryThreshold;
            }

            using (Image<Gray, Byte> grayImage = CurrentImage.Convert<Gray, Byte>())
            using (Image<Gray, Byte> binaryImage = grayImage.ThresholdBinary(new Gray(BinaryThreshold), new Gray(255)))
            {
                DisplayImage = ImageService.ToBitmapSource(binaryImage);
            }
        }

        private void UpdateBinaryThreshold2()
        {
            if (CurrentResult != null)
            {
                CurrentResult.ThresholdValue2 = BinaryThreshold2;
            }

            using (Image<Gray, Byte> grayImage = CurrentImage.Convert<Gray, Byte>())
            using (Image<Gray, Byte> binaryImage = grayImage.ThresholdBinary(new Gray(BinaryThreshold2), new Gray(255)))
            {
                DisplayImage = ImageService.ToBitmapSource(binaryImage);
            }
        }

        private void AutoFindGapDistance()
        {
            
        }

        private bool m_CancelTask;
        private object m_CancelTaskLock = new object();
        private bool CancelTask
        {
            get
            {
                lock (m_CancelTaskLock)
                {
                    return m_CancelTask;
                }
            }
            set
            {
                lock (m_CancelTaskLock)
                {
                    m_CancelTask = value;
                }
            }
        }

        private IRBSKVideo2 RbskVideo
        {
            get;
            set;
        }

        private void ReRunVideo(string fileName)
        {
            ProgressView view = new ProgressView();
            ProgressViewModel viewModel = new ProgressViewModel();
            view.DataContext = viewModel;
            viewModel.CancelPressed += (sender, args) =>
            {
                CancelReRun();
            };

            viewModel.WindowAboutToClose += (sender, args) =>
            {
                if (RbskVideo != null)
                {
                    RbskVideo.Paused = true;
                }
            };

            viewModel.WindowClosingCancelled += (sender, args) =>
            {
                if (RbskVideo != null)
                {
                    RbskVideo.Paused = false;
                }
            };

            CancelTask = false;

            Task.Factory.StartNew(() =>
            {
                IMouseDataExtendedResult result = Model.Results[SelectedVideo.Model];
                try
                {
                    IVideoSettings videoSettings = ModelResolver.Resolve<IVideoSettings>();
                    using (IRBSKVideo2 rbskVideo = ModelResolver.Resolve<IRBSKVideo2>())
                    using (IVideo video = ModelResolver.Resolve<IVideo>())
                    {
                        RbskVideo = rbskVideo;
                        video.SetVideo(fileName);
                        if (video.FrameCount < 100)
                        {
                            result.VideoOutcome = SingleFileResult.FrameCountTooLow;
                            result.Message = "Exception: " + SelectedVideo.VideoFileName + " - Frame count too low";
                            //allResults.TryAdd(file, result);
                            viewModel.ProgressValue = 1;
                            //continue;
                            return;
                        }

                        videoSettings.FileName = SelectedVideo.VideoFileName;
                        videoSettings.ThresholdValue = BinaryThreshold;


                        video.SetFrame(0);
                        Image<Gray, Byte> binaryBackground;
                        IEnumerable<IBoundaryBase> boundaries;
                        videoSettings.GeneratePreview(video, out binaryBackground, out boundaries);
                        result.Boundaries = boundaries.ToArray();

                        rbskVideo.GapDistance = GapDistance;
                        rbskVideo.ThresholdValue = BinaryThreshold;

                        rbskVideo.Video = video;
                        rbskVideo.BackgroundImage = binaryBackground;

                        rbskVideo.ProgressUpdates += (s, e) =>
                        {
                            double progress = e.Progress;
                            if (progress >= 1)
                            {
                                progress = 0.999;
                            }
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                viewModel.ProgressValue = progress;
                            });
                        };

                        rbskVideo.FindWhiskers = IncludeWhiskers;
                        rbskVideo.FindFoot = IncludeFeet;
                        rbskVideo.WhiskerSettings = WhiskerSettings;
                        rbskVideo.FootSettings = FootSettings;
                        rbskVideo.Process();
                        RbskVideo = null;
                        //if (Stop)
                        //{
                        //    state.Stop();
                        //    return;
                        //}
                        if (CancelTask)
                        {
                            return;
                        }
                        result.Results = rbskVideo.HeadPoints;
                        result.GenerateResults();
                        result.VideoOutcome = SingleFileResult.Ok;
                        //allResults.TryAdd(SelectedVideo, result);
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        result.ResetFrames();
                        Results[SelectedVideo.Model] = result;
                        CurrentResult = result;
                        SliderValueChanged();
                        SaveCommand.RaiseCanExecuteChangedNotification();
                        viewModel.ProgressValue = 1;
                        SelectedVideoChanged();
                    });
                }
                catch (Exception e)
                {
                    result.VideoOutcome = SingleFileResult.Error;
                    result.Message = "Exception: " + SelectedVideo.VideoFileName + " - " + e.Message;
                    //allResults.TryAdd(SelectedVideo, result);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        viewModel.ProgressValue = 1;
                    });
                    RbskVideo = null;
                }
            });

            view.ShowDialog();
        }

        private bool CanReRun()
        {
            return VideoSelected;
        }

        private void CancelReRun()
        {
            if (RbskVideo != null)
            {
                RbskVideo.Cancelled = true;
            }
            
            CancelTask = true;
        }

        protected override void WindowClosing(CancelEventArgs closingEventArgs)
        {
            if (CurrentImage != null)
            {
                CurrentImage.Dispose();
            }
        }

        private void SaveFile()
        {
            if (File.Exists(ArtFile))
            {
                var result = MessageBox.Show("Would you like to overwrite the original file?", "Overwrite?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
                
                ISaveArtFile save = ModelResolver.Resolve<ISaveArtFile>();
                string videoFile = SelectedVideo.VideoFileName;

                if (result == MessageBoxResult.Yes)
                {
                    save.SaveFile(ArtFile, videoFile, CurrentResult);
                }
                else
                {
                    string saveFile = FileBrowser.SaveFile("ARWT|*.arwt");

                    if (string.IsNullOrWhiteSpace(saveFile))
                    {
                        return;
                    }
                    
                    save.SaveFile(saveFile, videoFile, CurrentResult);
                }
            }
        }

        private bool CanSaveFile()
        {
            if (CurrentResult == null)
            {
                return false;
            }
            return CurrentResult.ModelObjectState == ModelObjectState.Dirty;
        }

        private void Export()
        {
            string artFile = SelectedVideo.ArtFileLocation;
            string folderLocation = Path.GetDirectoryName(artFile);

            string saveLocation = FileBrowser.SaveFile("Excel|*.xlsx", folderLocation);

            if (string.IsNullOrWhiteSpace(saveLocation))
            {
                return;
            }

            ExportRawData(saveLocation);
        }

        private void ExportCSV()
        {

            string artFile = SelectedVideo.ArtFileLocation;
            string folderLocation = Path.GetDirectoryName(artFile);

            string saveLocation = FileBrowser.SaveFile("CSV|*.csv", folderLocation);

            if (string.IsNullOrWhiteSpace(saveLocation))
            {
                return;
            }
            string videoFile = SelectedVideo.VideoFileName;
            object[,] data = CurrentResult.GetResults();
            iSaveCSVFile save = ModelResolver.Resolve<iSaveCSVFile>();
            save.saveCSV(saveLocation, CurrentResult);
        }

        private void ExportMeanCSV()
        {

            string artFile = SelectedVideo.ArtFileLocation;
            string folderLocation = Path.GetDirectoryName(artFile);

            string saveLocation = FileBrowser.SaveFile("CSV|*.csv", folderLocation);

            if (string.IsNullOrWhiteSpace(saveLocation))
            {
                return;
            }
            string videoFile = SelectedVideo.VideoFileName;
            object[,] data = CurrentResult.GetResults();
            iSaveCSVFile save = ModelResolver.Resolve<iSaveCSVFile>();
            
            save.saveCSV(saveLocation, CurrentResult, true);
        }
        private void ExportRawData(string saveLocation)
        {
            object[,] data = CurrentResult.GetResults();
            ExcelService.WriteData(data, saveLocation, true);
        }

        private bool CanExport()
        {
            return VideoSelected;
        }

        private void SetStartFrame()
        {
            AnalyseStart = SliderValue;
            CurrentResult.StartFrame = SliderValue;
            CurrentResult.GenerateResults();

            UpdateResults();
        }

        private bool CanSetStartFrame()
        {
            return VideoSelected;
        }

        private void SetEndFrame()
        {
            AnalyseEnd = SliderValue;
            CurrentResult.EndFrame = SliderValue;
            CurrentResult.GenerateResults();

            UpdateResults();
        }

        private void UpdateResults()
        {
            DistanceTravelled = CurrentResult.DistanceTravelled;
            CentroidDistanceTravelled = CurrentResult.CentroidDistanceTravelled;
            Duration = CurrentResult.Duration;
            AvgVelocity = CurrentResult.AverageVelocity;
            AvgCentroidVelocity = CurrentResult.AverageCentroidVelocity;
            AvgAngularVelocity = CurrentResult.AverageAngularVelocity;
            MaxVelocity = CurrentResult.MaxSpeed;
            MaxCentroidVelocity = CurrentResult.MaxCentroidSpeed;
            MaxAngularVelocity = CurrentResult.MaxAngularVelocty;
            CentroidSize = CurrentResult.CentroidSize;
            
        }

        private bool CanSetEndFrame()
        {
            return VideoSelected;
        }

        private void SmoothMotionChanged()
        {
            CurrentResult.GenerateResults();
            MotionTrack = CurrentResult.GetMotionTrack();
            UpdateResults();
            UpdateDisplayImage();
        }
    }
}
