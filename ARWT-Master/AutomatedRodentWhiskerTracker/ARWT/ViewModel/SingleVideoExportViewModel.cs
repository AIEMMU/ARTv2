using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.ModelInterface.Video;
using ARWT.Commands;
using ARWT.Model.Analysis;
using ARWT.Model.MWA;
using ARWT.Model.Smoothing;
using ARWT.Model.Whiskers;
using ARWT.ModelInterface.Results;
using ARWT.ModelInterface.Smoothing;
using ARWT.ModelInterface.Whiskers;
using ARWT.Resolver;
using ARWT.Services;
using ARWT.ViewModel.Settings;
using Emgu.CV;
using Emgu.CV.Structure;
using RobynsWhiskerTracker.View.Settings.UnitSettings;
using RobynsWhiskerTracker.ViewModel.Setings;


using System;
using System.Collections.Generic;
using System.Text;

namespace ARWT.ViewModel
{
    public class SingleVideoExportViewModel : WindowBaseModel
    {
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

        private ActionCommand _OkCommand;
        private ActionCommand _ExportCommand;

        public ActionCommand OkCommand
        {
            get
            {
                return _OkCommand ?? (_OkCommand = new ActionCommand()
                {
                    ExecuteAction = () => CloseWindow()
                });
            }
        }

        private ActionCommand _ExportDataCommand;

        public ActionCommand ExportDataCommand
        {
            get
            {
                return _ExportDataCommand ?? (_ExportDataCommand = new ActionCommand()
                {
                    //ExecuteAction = ExportData
                });
            }
        }


        private double _MaxVelocity;
        public double MaxVelocity
        {
            get
            {
                return _MaxVelocity;
            }
            set
            {
                if (Equals(_MaxVelocity, value))
                {
                    return;
                }

                _MaxVelocity = value;

                NotifyPropertyChanged();
            }
        }

        private double _MaxCentroidVelocity;
        public double MaxCentroidVelocity
        {
            get
            {
                return _MaxCentroidVelocity;
            }
            set
            {
                if (Equals(_MaxCentroidVelocity, value))
                {
                    return;
                }

                _MaxCentroidVelocity = value;

                NotifyPropertyChanged();
            }
        }

        private double _MaxAngularVelocity;
        public double MaxAngularVelocity
        {
            get
            {
                return _MaxAngularVelocity;
            }
            set
            {
                if (Equals(_MaxAngularVelocity, value))
                {
                    return;
                }

                _MaxAngularVelocity = value;

                NotifyPropertyChanged();
            }
        }


        private double _AverageVelocity;
        public double AverageVelocity
        {
            get
            {
                return _AverageVelocity;
            }
            set
            {
                if (Equals(_AverageVelocity, value))
                {
                    return;
                }

                _AverageVelocity = value;

                NotifyPropertyChanged();
            }
        }

        private double _AverageCentroidVelocity;
        public double AverageCentroidVelocity
        {
            get
            {
                return _AverageCentroidVelocity;
            }
            set
            {
                if (Equals(_AverageCentroidVelocity, value))
                {
                    return;
                }

                _AverageCentroidVelocity = value;

                NotifyPropertyChanged();
            }
        }

        private double _AverageAngularVelocity;
        public double AverageAngularVelocity
        {
            get
            {
                return _AverageAngularVelocity;
            }
            set
            {
                if (Equals(_AverageAngularVelocity, value))
                {
                    return;
                }

                _AverageAngularVelocity = value;

                NotifyPropertyChanged();
            }
        }

        private double _DistanceTravelled;
        public double DistanceTravelled
        {
            get
            {
                return _DistanceTravelled;
            }
            set
            {
                if (Equals(_DistanceTravelled, value))
                {
                    return;
                }

                _DistanceTravelled = value;

                NotifyPropertyChanged();
            }
        }

        private double _CentroidDistanceTravelled;
        public double CentroidDistanceTravelled
        {
            get
            {
                return _CentroidDistanceTravelled;
            }
            set
            {
                if (Equals(_CentroidDistanceTravelled, value))
                {
                    return;
                }

                _CentroidDistanceTravelled = value;

                NotifyPropertyChanged();
            }
        }

        private double _LeftWhiskerFrequency;
        public double LeftWhiskerFrequency
        {
            get
            {
                return _LeftWhiskerFrequency;
            }
            set
            {
                if (Equals(_LeftWhiskerFrequency, value))
                {
                    return;
                }

                _LeftWhiskerFrequency = value;

                NotifyPropertyChanged();
            }
        }


        private double _RightWhiskerFrequency;
        public double RightWhiskerFrequency
        {
            get
            {
                return _RightWhiskerFrequency;
            }
            set
            {
                if (Equals(_RightWhiskerFrequency, value))
                {
                    return;
                }

                _RightWhiskerFrequency = value;

                NotifyPropertyChanged();
            }
        }


        private double _LeftWhiskerAmplitude;
        public double LeftWhiskerAmplitude
        {
            get
            {
                return _LeftWhiskerAmplitude;
            }
            set
            {
                if (Equals(_LeftWhiskerAmplitude, value))
                {
                    return;
                }

                _LeftWhiskerAmplitude = value;

                NotifyPropertyChanged();
            }
        }


        private double _RightWhiskerAmplitude;
        public double RightWhiskerAmplitude
        {
            get
            {
                return _RightWhiskerAmplitude;
            }
            set
            {
                if (Equals(_RightWhiskerAmplitude, value))
                {
                    return;
                }

                _RightWhiskerAmplitude = value;

                NotifyPropertyChanged();
            }
        }


        private double _LeftWhiskerMeanOffset;
        public double LeftWhiskerMeanOffset
        {
            get
            {
                return _LeftWhiskerMeanOffset;
            }
            set
            {
                if (Equals(_LeftWhiskerMeanOffset, value))
                {
                    return;
                }

                _LeftWhiskerMeanOffset = value;

                NotifyPropertyChanged();
            }
        }


        private double _RightWhiskerMeanOffset;
        public double RightWhiskerMeanOffset
        {
            get
            {
                return _RightWhiskerMeanOffset;
            }
            set
            {
                if (Equals(_RightWhiskerMeanOffset, value))
                {
                    return;
                }

                _RightWhiskerMeanOffset = value;

                NotifyPropertyChanged();
            }
        }


        private double _LeftWhiskerAvgAngularVelocity;
        public double LeftWhiskerAvgAngularVelocity
        {
            get
            {
                return _LeftWhiskerAvgAngularVelocity;
            }
            set
            {
                if (Equals(_LeftWhiskerAvgAngularVelocity, value))
                {
                    return;
                }

                _LeftWhiskerAvgAngularVelocity = value;

                NotifyPropertyChanged();
            }
        }

        private double _RightWhiskerAvgAngularVelocity;
        public double RightWhiskerAvgAngularVelocity
        {
            get
            {
                return _RightWhiskerAvgAngularVelocity;
            }
            set
            {
                if (Equals(_RightWhiskerAvgAngularVelocity, value))
                {
                    return;
                }

                _RightWhiskerAvgAngularVelocity = value;

                NotifyPropertyChanged();
            }
        }

        private double _LeftWhiskerAvgProtractionVelocity;
        public double LeftWhiskerAvgProtractionVelocity
        {
            get
            {
                return _LeftWhiskerAvgProtractionVelocity;
            }
            set
            {
                if (Equals(_LeftWhiskerAvgProtractionVelocity, value))
                {
                    return;
                }

                _LeftWhiskerAvgProtractionVelocity = value;

                NotifyPropertyChanged();
            }
        }

        private double _RightWhiskerAvgProtractionVelocity;
        public double RightWhiskerAvgProtractionVelocity
        {
            get
            {
                return _RightWhiskerAvgProtractionVelocity;
            }
            set
            {
                if (Equals(_RightWhiskerAvgProtractionVelocity, value))
                {
                    return;
                }

                _RightWhiskerAvgProtractionVelocity = value;

                NotifyPropertyChanged();
            }
        }

        private double _LeftWhiskerAvgRetractionVelocity;
        public double LeftWhiskerAvgRetractionVelocity
        {
            get
            {
                return _LeftWhiskerAvgRetractionVelocity;
            }
            set
            {
                if (Equals(_LeftWhiskerAvgRetractionVelocity, value))
                {
                    return;
                }

                _LeftWhiskerAvgRetractionVelocity = value;

                NotifyPropertyChanged();
            }
        }

        private double _LeftSTD;
        public double LeftSTD
        {
            get
            {
                return _LeftSTD;
            }
            set
            {
                if (Equals(_LeftSTD, value))
                {
                    return;
                }

                _LeftSTD = value;

                NotifyPropertyChanged();
            }
        }

        private double _RightSTD;
        public double RightSTD
        {
            get
            {
                return _RightSTD;
            }
            set
            {
                if (Equals(_RightSTD, value))
                {
                    return;
                }

                _RightSTD = value;

                NotifyPropertyChanged();
            }
        }
        private double _RightRMS;
        public double RightRMS
        {
            get
            {
                return _RightRMS;
            }
            set
            {
                if (Equals(_RightRMS, value))
                {
                    return;
                }

                _RightRMS = value;

                NotifyPropertyChanged();
            }
        }
        private double _LeftRMS;
        public double LeftRMS
        {
            get
            {
                return _LeftRMS;
            }
            set
            {
                if (Equals(_LeftRMS, value))
                {
                    return;
                }

                _LeftRMS = value;

                NotifyPropertyChanged();
            }
        }

        private double _RightWhiskerAvgRetractionVelocity;
        public double RightWhiskerAvgRetractionVelocity
        {
            get
            {
                return _RightWhiskerAvgRetractionVelocity;
            }
            set
            {
                if (Equals(_RightWhiskerAvgRetractionVelocity, value))
                {
                    return;
                }

                _RightWhiskerAvgRetractionVelocity = value;

                NotifyPropertyChanged();
            }
        }

        private double _OriginalFrameRate;
        public double OriginalFrameRate
        {
            get
            {
                return _OriginalFrameRate;
            }
            set
            {
                if (Equals(_OriginalFrameRate, value))
                {
                    return;
                }

                _OriginalFrameRate = value;

                NotifyPropertyChanged();
            }
        }

        private double _UnitsToMm;
        public double UnitsToMm
        {
            get
            {
                return _UnitsToMm;
            }
            set
            {
                if (Equals(_UnitsToMm, value))
                {
                    return;
                }

                _UnitsToMm = value;

                NotifyPropertyChanged();
            }
        }

        private ActionCommand _ApplyCommand;
        public ActionCommand ApplyCommand
        {
            get
            {
                return _ApplyCommand ?? (_ApplyCommand = new ActionCommand()
                {
                    ExecuteAction = ReloadData
                });
            }
        }

        private ActionCommand _SelectUnitsToMmCommand;
        public ActionCommand SelectUnitsToMmCommand
        {
            get
            {
                return _SelectUnitsToMmCommand ?? (_SelectUnitsToMmCommand = new ActionCommand()
                {
                    ExecuteAction = SelectUnits
                });
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

        private void SelectUnits()
        {
            string location = FileBrowser.BrowseForFile("Video|*.avi;*.mpg;*.mpeg;*.mp4;*.mov|Image|*.jpg;*.jpeg*.bmp;*.png");

            if (string.IsNullOrWhiteSpace(location))
            {
                return;
            }

            string ext = Path.GetExtension(location);
            Bitmap bitmap;
            if (ext.Contains("avi") || ext.Contains("mpg") || ext.Contains("mpeg") || ext.Contains("mov"))
            {
                //Is video file
                using (IVideo video = ModelResolver.Resolve<IVideo>())
                {
                    video.SetVideo(location);
                    using (Image<Bgr, Byte> image = video.GetFrameImage())
                    {
                        bitmap = new Bitmap(image.Bitmap);
                    }
                }
            }
            else
            {
                //Is image file
                bitmap = new Bitmap(location);
            }

            PickUnitsPointsView view = new PickUnitsPointsView();
            PickUnitsPointsViewModel viewModel = new PickUnitsPointsViewModel(bitmap);
            view.DataContext = viewModel;
            view.ShowDialog();

            if (viewModel.ExitResult != WindowExitResult.Ok)
            {
                return;
            }

            UnitsToMm = viewModel.UnitsToMm;
        }

        public SingleVideoExportViewModel()
        {
            ObservableCollection<ISmoothingBase> smoothingFunctions = new ObservableCollection<ISmoothingBase>();
            smoothingFunctions.Add(ModelResolver.Resolve<iMovingAverage2>());
            smoothingFunctions.Add(ModelResolver.Resolve<iMovingAverage>());
            smoothingFunctions.Add(ModelResolver.Resolve<IGaussianSmoothing>());
            smoothingFunctions.Add(ModelResolver.Resolve<IBoxCarSmoothing>());
            SmoothingFunctions = smoothingFunctions;
            SelectedSmoothingFunction = SmoothingFunctions.First();
        }

        private IMouseDataExtendedResult Data
        {
            get;
            set;
        }


        private bool _UseDft;
        public bool UseDft
        {
            get
            {
                //_UseDft = true;
                return _UseDft;
            }
            set
            {
                if (Equals(_UseDft, value))
                {
                    return;
                }

                _UseDft = value;

                NotifyPropertyChanged();
            }
        }


        private int _StartFrame;
        public int StartFrame
        {
            get
            {
                return _StartFrame;
            }
            set
            {
                if (Equals(_StartFrame, value))
                {
                    return;
                }

                _StartFrame = value;

                NotifyPropertyChanged();
            }
        }


        private int _EndFrame;
        public int EndFrame
        {
            get
            {
                return _EndFrame;
            }
            set
            {
                if (Equals(_EndFrame, value))
                {
                    return;
                }

                _EndFrame = value;

                NotifyPropertyChanged();
            }
        }


        public void ReloadData()
        {
            if(Data != null)
            {
                Data.FrameRate = OriginalFrameRate;
                Data.UnitsToMilimeters = UnitsToMm;
                Data.StartFrame = StartFrame;
                Data.EndFrame = EndFrame;
                Data.GenerateResults();
                MaxVelocity = Data.MaxSpeed;
                MaxCentroidVelocity = Data.MaxCentroidSpeed;
                MaxAngularVelocity = Data.MaxAngularVelocty;
                AverageVelocity = Data.AverageVelocity;
                AverageCentroidVelocity = Data.AverageCentroidVelocity;
                AverageAngularVelocity = Data.AverageAngularVelocity;
                DistanceTravelled = Data.DistanceTravelled;
                CentroidDistanceTravelled = Data.CentroidDistanceTravelled;

                StatGenerator stat = new StatGenerator();
                stat.StartFrame = StartFrame;
                stat.EndFrame = EndFrame;
                stat.SmoothingFunction = SelectedSmoothingFunction;
                stat.SmoothRepeats = RepeatSmooths;
                stat.UseDft = UseDft;
                stat.Generate(Data.Results, OriginalFrameRate);

                LeftWhiskerAmplitude = stat.LeftWhiskerAmplitude;
                RightWhiskerAmplitude = stat.RightWhiskerAmplitude;
                LeftWhiskerAvgAngularVelocity = stat.LeftAverageAngularVelocity;
                RightWhiskerAvgAngularVelocity = stat.RightAverageAngularVelocity;
                LeftWhiskerAvgProtractionVelocity = stat.LeftAverageProtractionVelocity;
                RightWhiskerAvgProtractionVelocity = stat.RightAverageProtractionVelocity;
                LeftWhiskerAvgRetractionVelocity = stat.LeftAverageRetractionVelocity;
                RightWhiskerAvgRetractionVelocity = stat.RightAverageRetractionVelocity;
                LeftWhiskerFrequency = stat.LeftWhiskerFrequency;
                RightWhiskerFrequency = stat.RightWhiskerFrequnecy;
                LeftWhiskerMeanOffset = stat.LeftMeanOffset;
                RightWhiskerMeanOffset = stat.RightMeanOffset;
                LeftRMS = Math.Sqrt(2) * (2 * stat.LeftSTD);
                RightRMS = Math.Sqrt(2) * (2 * stat.RightSTD);
                RightSTD = stat.RightSTD;
                LeftSTD = stat.LeftSTD;

                IWhiskerAverageAngles angles = new WhiskerAverageAngles();
                angles.StartFrame = StartFrame;
                angles.EndFrame = EndFrame - 1;
                //angles.StartFrame = 0;
                //angles.EndFrame = data.Results.Count - 1;
                var allAngles = angles.GetWhiskerAngles(Data.Results);
                LoadData(allAngles[0], allAngles[1]);
            }
           
            UpdateSmoothing();
        }

        public void LoadData(IMouseDataExtendedResult data)
        {
            StartFrame = 0;
            if(data.Results != null)
            {
                KeyValuePair<int, ISingleFrameExtendedResults> kvp = data.Results.FirstOrDefault(x => x.Value.HeadPoints == null);

                if (kvp.Equals(default(KeyValuePair<int, ISingleFrameExtendedResults>)))
                {
                    EndFrame = data.Results.Count;
                }
                else
                {
                    EndFrame = kvp.Key;
                }
                StartFrame = data.StartFrame;
                EndFrame = data.EndFrame;
                RepeatSmooths = 1;
                Data = data;
                MaxVelocity = data.MaxSpeed;
                MaxCentroidVelocity = data.MaxCentroidSpeed;
                MaxAngularVelocity = data.MaxAngularVelocty;
                AverageVelocity = data.AverageVelocity;
                AverageCentroidVelocity = data.AverageCentroidVelocity;
                AverageAngularVelocity = data.AverageAngularVelocity;
                DistanceTravelled = data.DistanceTravelled;
                CentroidDistanceTravelled = data.CentroidDistanceTravelled;
                OriginalFrameRate = data.FrameRate;
                UnitsToMm = data.UnitsToMilimeters;

                StatGenerator stat = new StatGenerator();
                stat.StartFrame = StartFrame;
                stat.EndFrame = EndFrame;
                stat.SmoothingFunction = SelectedSmoothingFunction;
                stat.UseDft = UseDft;
                stat.SmoothRepeats = RepeatSmooths;

                stat.Generate(data.Results, OriginalFrameRate);

                IWhiskerAverageAngles angles = new WhiskerAverageAngles();
                angles.StartFrame = StartFrame;
                angles.EndFrame = EndFrame - 1;
                //angles.StartFrame = 0;
                //angles.EndFrame = data.Results.Count - 1;
                var allAngles = angles.GetWhiskerAngles(data.Results);
                LoadData(allAngles[0], allAngles[1]);


                LeftWhiskerAmplitude = stat.LeftWhiskerAmplitude;
                RightWhiskerAmplitude = stat.RightWhiskerAmplitude;
                LeftWhiskerAvgAngularVelocity = stat.LeftAverageAngularVelocity;
                RightWhiskerAvgAngularVelocity = stat.RightAverageAngularVelocity;
                LeftWhiskerAvgProtractionVelocity = stat.LeftAverageProtractionVelocity;
                RightWhiskerAvgProtractionVelocity = stat.RightAverageProtractionVelocity;
                LeftWhiskerAvgRetractionVelocity = stat.LeftAverageRetractionVelocity;
                RightWhiskerAvgRetractionVelocity = stat.RightAverageRetractionVelocity;
                LeftWhiskerFrequency = stat.LeftWhiskerFrequency;
                RightWhiskerFrequency = stat.RightWhiskerFrequnecy;
                LeftWhiskerMeanOffset = stat.LeftMeanOffset;
                RightWhiskerMeanOffset = stat.RightMeanOffset;
                LeftRMS = Math.Sqrt(2) * (2 * stat.LeftSTD);
                RightRMS = Math.Sqrt(2) * (2 * stat.RightSTD);
                RightSTD = stat.RightSTD;
                LeftSTD = stat.LeftSTD;
            }
            
            UpdateSmoothing();
        }

        public void LoadData(Dictionary<int, double> leftWhiskers, Dictionary<int, double> rightWhiskers)
        {
            LeftGraphWhiskers = leftWhiskers.Select(x => x).ToArray();
            RightGraphWhiskers = rightWhiskers.Select(x => x).ToArray();

            UpdateSmoothing();
        }

        public void LoadData(double[] leftWhiskers, double[] rightWhiskers)
        {
            LeftGraphWhiskers = ConvertToDictionary(leftWhiskers).ToArray();
            RightGraphWhiskers = ConvertToDictionary(rightWhiskers).ToArray();

            UpdateSmoothing();
        }

        private Dictionary<int, double> ConvertToDictionary(double[] signal)
        {
            Dictionary<int, double> dicSignal = new Dictionary<int, double>();
            for (int i = 0; i < signal.Length; i++)
            {
                dicSignal.Add(i, signal[i]);
            }
            return dicSignal;
        }
        private double[] GetDoubleValues(KeyValuePair<int, double>[] array)
        {
            return array.Select(x => x.Value).ToArray();
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
    }
}
