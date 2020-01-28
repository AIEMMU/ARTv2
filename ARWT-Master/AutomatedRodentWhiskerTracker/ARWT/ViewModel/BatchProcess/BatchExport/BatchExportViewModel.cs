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
using System.Linq;
using System.Text;
using ArtLibrary.ModelInterface.Results;
using ArtLibrary.ModelInterface.Results.Behaviour;
using ArtLibrary.ModelInterface.Results.Behaviour.Movement;
using ARWT.Commands;
using ARWT.Foot.dataset;
using ARWT.Model.Analysis;
using ARWT.ModelInterface.Results;
using ARWT.ModelInterface.Smoothing;
using ARWT.Resolver;
using ARWT.Services;
using ARWT.ViewModel;
using ARWT.ViewModel.Datasets;

namespace ARWT.ViewModel.BatchProcess.BatchExport
{
    public class BatchExportViewModel : WindowBaseModel
    {
        private ActionCommand m_ExportCommand;
        public ActionCommand ExportCommand
        {
            get
            {
                return m_ExportCommand ?? (m_ExportCommand = new ActionCommand()
                {
                    ExecuteAction = Process,
                    CanExecuteAction = () => Videos != null && Videos.Any()
                });
            }
        }
        private ActionCommand m_ExportFeetCommand;
        public ActionCommand ExportFeetCommand
        {
            get
            {
                return m_ExportFeetCommand ?? (m_ExportFeetCommand = new ActionCommand()
                {
                    ExecuteAction = ProcessFeet,
                    CanExecuteAction = () => Videos != null && Videos.Any()
                });
            }
        }
        private ActionCommand _ApplyCommand;
        public ActionCommand ApplyCommand
        {
            get
            {
                return _ApplyCommand ?? (_ApplyCommand = new ActionCommand()
                {
                    ExecuteAction = Apply,
                    CanExecuteAction = () => Videos != null && Videos.Any()
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

        private ObservableCollection<SingleMouseViewModel> m_Videos;
        public ObservableCollection<SingleMouseViewModel> Videos
        {
            get
            {
                return m_Videos;
            }
            set
            {
                if (ReferenceEquals(m_Videos, value))
                {
                    return;
                }

                m_Videos = value;

                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<string> m_VelocityOptions;
        public ObservableCollection<string> VelocityOptions
        {
            get
            {
                return m_VelocityOptions;
            }
            set
            {
                if (ReferenceEquals(m_VelocityOptions, value))
                {
                    return;
                }

                m_VelocityOptions = value;

                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<string> m_RotationOptions;
        public ObservableCollection<string> RotationOptions
        {
            get
            {
                return m_RotationOptions;
            }
            set
            {
                if (ReferenceEquals(m_RotationOptions, value))
                {
                    return;
                }

                m_RotationOptions = value;

                NotifyPropertyChanged();
            }
        }

        private string m_SelectedVelocityOption;
        public string SelectedVelocityOption
        {
            get
            {
                return m_SelectedVelocityOption;
            }
            set
            {
                if (Equals(m_SelectedVelocityOption, value))
                {
                    return;
                }

                m_SelectedVelocityOption = value;

                NotifyPropertyChanged();
            }
        }

        private string m_SelectedRotationOption;
        public string SelectedRotationOption
        {
            get
            {
                return m_SelectedRotationOption;
            }
            set
            {
                if (Equals(m_SelectedRotationOption, value))
                {
                    return;
                }

                m_SelectedRotationOption = value;

                NotifyPropertyChanged();
            }
        }

        private double m_UnitsToMm = 1;
        public double UnitsToMm
        {
            get
            {
                return m_UnitsToMm;
            }
            set
            {
                if (Equals(m_UnitsToMm, value))
                {
                    return;
                }

                m_UnitsToMm = value;

                NotifyPropertyChanged();
            }
        }

        private double m_OriginalFrameRate = 1;
        public double OriginalFrameRate
        {
            get
            {
                return m_OriginalFrameRate;
            }
            set
            {
                if (Equals(m_OriginalFrameRate, value))
                {
                    return;
                }

                m_OriginalFrameRate = value;

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
            }
        }

        private bool _UseDft;
        public bool UseDft
        {
            get
            {
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
        
        private double _StillCutOff;
        public double StillCutOff
        {
            get
            {
                return _StillCutOff;
            }
            set
            {
                if (Equals(_StillCutOff, value))
                {
                    return;
                }

                _StillCutOff = value;

                NotifyPropertyChanged();
                SpeedDefs.UpdateCutOffs(StillCutOff, WalkingCutOff);
            }
        }

        private double _WalkingCutOff;
        public double WalkingCutOff
        {
            get
            {
                return _WalkingCutOff;
            }
            set
            {
                if (Equals(_WalkingCutOff, value))
                {
                    return;
                }

                _WalkingCutOff = value;

                NotifyPropertyChanged();
                SpeedDefs.UpdateCutOffs(StillCutOff, WalkingCutOff);
            }
        }


        private IBehaviourSpeedDefinitions  _SpeedDefs;
        public IBehaviourSpeedDefinitions SpeedDefs
        {
            get
            {
                return _SpeedDefs;
            }
            set
            {
                if (Equals(_SpeedDefs, value))
                {
                    return;
                }

                _SpeedDefs = value;

                NotifyPropertyChanged();
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

        private bool _InteractionsOnly;
        public bool InteractionsOnly
        {
            get
            {
                return _InteractionsOnly;
            }
            set
            {
                if (Equals(_InteractionsOnly, value))
                {
                    return;
                }

                _InteractionsOnly = value;

                NotifyPropertyChanged();
            }
        }


        private int _MinimumNumberDetectedFrames = 100;
        public int MinimumNumberDetectedFrames
        {
            get
            {
                return _MinimumNumberDetectedFrames;
            }
            set
            {
                if (Equals(_MinimumNumberDetectedFrames, value))
                {
                    return;
                }

                _MinimumNumberDetectedFrames = value;

                NotifyPropertyChanged();
            }
        }

        public BatchExportViewModel(IEnumerable<SingleMouseViewModel> mice)
        {
            SpeedDefs = ModelResolver.Resolve<IBehaviourSpeedDefinitions>();
            _StillCutOff = SpeedDefs.StillCutOff;
            _WalkingCutOff = SpeedDefs.WalkingCutOff;

            ObservableCollection<ISmoothingBase> smoothingFunctions = new ObservableCollection<ISmoothingBase>();
            smoothingFunctions.Add(ModelResolver.Resolve<iMovingAverage2>());
            smoothingFunctions.Add(ModelResolver.Resolve<iMovingAverage>());
            smoothingFunctions.Add(ModelResolver.Resolve<IGaussianSmoothing>());
            smoothingFunctions.Add(ModelResolver.Resolve<IBoxCarSmoothing>());
            SmoothingFunctions = smoothingFunctions;
            SelectedSmoothingFunction = SmoothingFunctions.First();

            Videos = new ObservableCollection<SingleMouseViewModel>(mice);

            ObservableCollection<string> movements = new ObservableCollection<string>();
            movements.Add("Still");
            movements.Add("Moving");
            movements.Add("Running");
            movements.Add("All");
            VelocityOptions = movements;

            ObservableCollection<string> rotations = new ObservableCollection<string>();
            rotations.Add("Turning only");
            rotations.Add("All");
            RotationOptions = rotations;

            SelectedVelocityOption = "All";
            SelectedRotationOption = "All";

            InteractionsOnly = false;
        }

        private void Cancel()
        {
            CloseWindow();
        }

        private void Process()
        {
            GenerateAllResults();
            //GenerateFootResults();
            //GenerateIndivResultsFinal();
        }
        private void ProcessFeet()
        {
            GenerateFootResults();
        }
        private void GenerateFootResults()
        {
            
            int rows = Videos.Count+1;
            int columns = 13;
            object[,] data = new object[rows, columns];

            int rowCounter = 1;

            data[0, 0] = "Video";
            data[0, 1] = "Left Front Stride Distance";
            data[0, 2] = "Left Front Stance Time";
            data[0, 3] = "Left Front Swing Time";
            data[0, 4] = "Left Back Stride Distance";
            data[0, 5] = "Left Back Stance Time";
            data[0, 6] = "Left Back Swing Time Time";
            data[0, 7] = "Right Front Stride Distance";
            data[0, 8] = "Right Front Stance Time";
            data[0, 9] = "Right Front Swing Time";
            data[0, 10] = "Right Hind Stride Distance";
            data[0, 11] = "Right Hind Stance Time";
            data[0, 12] = "Right Hind Swing Time";

            

            List<MouseHolder> mice = genMice();

            if(mice == null)
            {
                return;
            }

           
            try
            {
                foreach (MouseHolder mouse in mice)
                {
                    if (mouse.Result.Results[0].FeetCollection == null)
                    {
                        int i = 0;
                        continue;
                    }
                    IMouseDataExtendedResult result = mouse.Result;
                    result.UnitsToMilimeters = UnitsToMm;
                    result.FrameRate = OriginalFrameRate;
                    
                    FootStrideData footData = new FootStrideData(result.Results, result.UnitsToMilimeters);
                    double deltaTime = (1000 / OriginalFrameRate);
                    data[rowCounter, 0] = mouse.File;
                    data[rowCounter, 1] = footData.leftFront.strideLength;
                    data[rowCounter, 2] = footData.leftFront.footPlacementTime * deltaTime;
                    data[rowCounter, 3] =footData.leftFront.airTime * deltaTime;

                    data[rowCounter, 4] = footData.leftHind.strideLength;
                    data[rowCounter, 5] =footData.leftHind.footPlacementTime * deltaTime;
                    data[rowCounter, 6] =footData.leftHind.airTime * deltaTime;

                    data[rowCounter, 7] = footData.rightFront.strideLength;
                    data[rowCounter, 8] =footData.rightFront.footPlacementTime * deltaTime;
                    data[rowCounter, 9] =footData.rightFront.airTime * deltaTime;

                    data[rowCounter, 10] = footData.righHind.strideLength;
                    data[rowCounter, 11] =footData.righHind.footPlacementTime * deltaTime;
                    data[rowCounter, 12] =footData.righHind.airTime * deltaTime; 
                    rowCounter++;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            string fileLocation = FileBrowser.SaveFile("Excel|*.xlsx");

            if (string.IsNullOrWhiteSpace(fileLocation))
            {
                return;
            }

            ExcelService.WriteData(data, fileLocation, true);

        }

        private List<MouseHolder> genMice()
        {
            List<MouseHolder> mice = new List<MouseHolder>();
            bool isNull = false;
            foreach (SingleMouseViewModel mouse in Videos)
            {
                if (mouse.Results != null)
                {
                    mice.AddRange(from video in mouse.VideoFiles let result = mouse.Results[video] select new MouseHolder() { Age = mouse.Age.ToString(), Class = mouse.Class, File = video.VideoFileName, Mouse = mouse, Result = result, Type = mouse.Type.Name });
                }
                else
                {
                    isNull = true;

                }
            }
            if (isNull == true && mice.Count() == 0)
            {
                System.Windows.MessageBox.Show("Your file has not been processed, and will not be exported.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return mice;
            }
            else if (isNull == true)
            {
                System.Windows.MessageBox.Show("One or more files has not been processed, and will be not be exported", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            return mice;
        }

        private void GenerateAllResults()
        {
            int rows = Videos.Count + 10;
            int columns = 27+4;
            object[,] data = new object[rows, columns];

            int rowCounter = 1;
            data[0, 0] = "Mouse";
            data[0, 1] = "Distance";
            data[0, 2] = "Centroid Distance";
            data[0, 3] = "Max Velocity";
            data[0, 4] = "Max Angular Velocity";
            data[0, 5] = "Average Velocity";
            data[0, 6] = "Average Angular Velocity";
            data[0, 7] = "Average Centroid Velocity";
            data[0, 8] = "Max Centroid Velocity";
            data[0, 9] = "Left Whisker Frequency";
            data[0, 10] = "Right Whisker Frequency";
            data[0, 11] = "Left Whisker Amplitude";
            data[0, 12] = "Right Whisker Amplitude";
            data[0, 13] = "Left Whisker Mean Offset";
            data[0, 14] = "Right Whisker Mean Offset";
            data[0, 15] = "Left Whisker Average Angular Velocity";
            data[0, 16] = "Right Whisker Average Angular Velocity";
            data[0, 17] = "Left Whisker Average Retraction Velocity";
            data[0, 18] = "Right Whisker Average Retraction Velocity";
            data[0, 19] = "Left Whisker Average Protraction Velocity";
            data[0, 20] = "Right Whisker Average Protraction Velocity";
            data[0, 21] = "Clip Duration";
            data[0, 22] = "Percentage Running";
            data[0, 23] = "Percentage moving";
            data[0, 24] = "Percentage turning";
            data[0, 25] = "Percentage Still";
            data[0, 26] = "Left Whisker Spread";
            data[0, 27] = "Right Whisker spread";
            data[0, 28] = "Left Whisker peak-to-peak amplitude";
            data[0, 29] = "Right Whisker peak-to-peak amplitude";
            //data[0, 26] = "Percentage Interacting";

            double maxVelocity = 0;
            double totalMaxCentroidVelocity = 0;
            double averageDistance = 0;
            double averageCentroidDistance = 0;
            double maxAngularVelocity = 0;
            double averageVelocity = 0;
            double averageAngularVelocity = 0;
            double averageCentroidVelocity = 0;
            double averageRunning = 0;
            double averageMoving = 0;
            double averageTurning = 0;
            double averageStill = 0;
            double averageInteracting = 0;
            int counter = 0;

            double avgleftWhiskerAmplitude = 0;
            double avgrightWhiskerAmplitude = 0;
            double avgleftWhiskerAvgAngularVelocity = 0;
            double avgrightWhiskerAvgAngularVelocity = 0;
            double avgleftWhiskerAvgProtractionVelocity = 0;
            double avgrightWhiskerAvgProtractionVelocity = 0;
            double avgleftWhiskerAvgRetractionVelocity = 0;
            double avgrightWhiskerAvgRetractionVelocity = 0;
            double avgleftWhiskerFrequency = 0;
            double avgrightWhiskerFrequency = 0;
            double avgleftWhiskerMeanOffset = 0;
            double avgrightWhiskerMeanOffset = 0;
            double leftRMS = 0;
            double rightRMS = 0;
            double leftSTD = 0;
            double rightSTD = 0;
            int freqCounter = 0;


            List<MouseHolder> mice = new List<MouseHolder>();
            bool isNull = false;
            foreach (SingleMouseViewModel mouse in Videos)
            {
                if (mouse.Results.Count!=0)
                {
                    mice.AddRange(from video in mouse.VideoFiles let result = mouse.Results[video] select new MouseHolder() { Age = mouse.Age.ToString(), Class = mouse.Class, File = video.VideoFileName, Mouse = mouse, Result = result, Type = mouse.Type.Name });
                }
                else
                {
                    isNull = true; 

                }
            }
            if (isNull == true && mice.Count() ==0)
            {
                System.Windows.MessageBox.Show("Your file has not been processed, and will not be exported.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return;
            } else if (isNull== true)
            {
                System.Windows.MessageBox.Show("One or more files has not been processed, and will be not be exported", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
      
            try
            {
                foreach (MouseHolder mouse in mice)
                {
                    IMouseDataExtendedResult result = mouse.Result;
                    result.UnitsToMilimeters = UnitsToMm;
                    result.FrameRate = OriginalFrameRate;
                    result.GenerateResults();
                    
                    if (result.EndFrame - result.StartFrame < MinimumNumberDetectedFrames)
                    {
                        data[rowCounter, 0] = mouse.File;
                        data[rowCounter, 1] = $"Skipped as total frames less than {MinimumNumberDetectedFrames}";
                        rowCounter++;
                        continue;
                    }

                    StatGenerator stat = new StatGenerator();
                    stat.StartFrame = result.StartFrame;
                    stat.EndFrame = result.EndFrame;
                    stat.SmoothingFunction = SelectedSmoothingFunction;
                    stat.SmoothRepeats = RepeatSmooths;
                    stat.UseDft = UseDft;
                    var whiskerResult = stat.Generate(result.Results, OriginalFrameRate, InteractionsOnly);
                    if (whiskerResult != null)
                    {
                        double leftWhiskerAmplitude = stat.LeftWhiskerAmplitude;
                        double rightWhiskerAmplitude = stat.RightWhiskerAmplitude;
                        double leftWhiskerAvgAngularVelocity = stat.LeftAverageAngularVelocity;
                        double rightWhiskerAvgAngularVelocity = stat.RightAverageAngularVelocity;
                        double leftWhiskerAvgProtractionVelocity = stat.LeftAverageProtractionVelocity;
                        double rightWhiskerAvgProtractionVelocity = stat.RightAverageProtractionVelocity;
                        double leftWhiskerAvgRetractionVelocity = stat.LeftAverageRetractionVelocity;
                        double rightWhiskerAvgRetractionVelocity = stat.RightAverageRetractionVelocity;
                        double leftWhiskerFrequency = stat.LeftWhiskerFrequency;
                        double rightWhiskerFrequency = stat.RightWhiskerFrequnecy;
                        double leftWhiskerMeanOffset = stat.LeftMeanOffset;
                        double rightWhiskerMeanOffset = stat.RightMeanOffset;

                        leftRMS = stat.LeftRMS;
                        rightRMS = stat.RightRMS;
                        leftSTD = stat.LeftSTD;
                        rightSTD = stat.RightSTD;

                        IMovementBehaviour movingBehaviour = SpeedDefs.GetMovementBehaviour(result.GetAverageCentroidSpeed());

                        if (SelectedVelocityOption == "Running" && movingBehaviour is IRunning)
                        {
                            avgleftWhiskerFrequency += leftWhiskerFrequency;
                            data[rowCounter, 9] = leftWhiskerFrequency;

                            avgrightWhiskerFrequency += rightWhiskerFrequency;
                            data[rowCounter, 10] = rightWhiskerFrequency;

                            avgleftWhiskerAmplitude += leftWhiskerAmplitude;
                            data[rowCounter, 11] = leftWhiskerAmplitude;

                            avgrightWhiskerAmplitude += rightWhiskerAmplitude;
                            data[rowCounter, 12] = rightWhiskerAmplitude;

                            avgleftWhiskerMeanOffset += leftWhiskerMeanOffset;
                            data[rowCounter, 13] = leftWhiskerMeanOffset;

                            avgrightWhiskerMeanOffset += rightWhiskerMeanOffset;
                            data[rowCounter, 14] = rightWhiskerMeanOffset;

                            avgleftWhiskerAvgAngularVelocity += leftWhiskerAvgAngularVelocity;
                            data[rowCounter, 15] = leftWhiskerAvgAngularVelocity;

                            avgrightWhiskerAvgAngularVelocity += rightWhiskerAvgAngularVelocity;
                            data[rowCounter, 16] = rightWhiskerAvgAngularVelocity;

                            avgleftWhiskerAvgRetractionVelocity += leftWhiskerAvgRetractionVelocity;
                            data[rowCounter, 17] = leftWhiskerAvgRetractionVelocity;

                            avgrightWhiskerAvgRetractionVelocity += rightWhiskerAvgRetractionVelocity;
                            data[rowCounter, 18] = rightWhiskerAvgRetractionVelocity;

                            avgleftWhiskerAvgProtractionVelocity += leftWhiskerAvgProtractionVelocity;
                            data[rowCounter, 19] = leftWhiskerAvgProtractionVelocity;

                            avgrightWhiskerAvgProtractionVelocity += rightWhiskerAvgProtractionVelocity;
                            data[rowCounter, 20] = rightWhiskerAvgProtractionVelocity;

                            freqCounter++;
                        }
                        else if (SelectedVelocityOption == "Moving" && (movingBehaviour is IWalking || movingBehaviour is IRunning))
                        {
                            avgleftWhiskerFrequency += leftWhiskerFrequency;
                            data[rowCounter, 9] = leftWhiskerFrequency;

                            avgrightWhiskerFrequency += rightWhiskerFrequency;
                            data[rowCounter, 10] = rightWhiskerFrequency;

                            avgleftWhiskerAmplitude += leftWhiskerAmplitude;
                            data[rowCounter, 11] = leftWhiskerAmplitude;

                            avgrightWhiskerAmplitude += rightWhiskerAmplitude;
                            data[rowCounter, 12] = rightWhiskerAmplitude;

                            avgleftWhiskerMeanOffset += leftWhiskerMeanOffset;
                            data[rowCounter, 13] = leftWhiskerMeanOffset;

                            avgrightWhiskerMeanOffset += rightWhiskerMeanOffset;
                            data[rowCounter, 14] = rightWhiskerMeanOffset;

                            avgleftWhiskerAvgAngularVelocity += leftWhiskerAvgAngularVelocity;
                            data[rowCounter, 15] = leftWhiskerAvgAngularVelocity;

                            avgrightWhiskerAvgAngularVelocity += rightWhiskerAvgAngularVelocity;
                            data[rowCounter, 16] = rightWhiskerAvgAngularVelocity;

                            avgleftWhiskerAvgRetractionVelocity += leftWhiskerAvgRetractionVelocity;
                            data[rowCounter, 17] = leftWhiskerAvgRetractionVelocity;

                            avgrightWhiskerAvgRetractionVelocity += rightWhiskerAvgRetractionVelocity;
                            data[rowCounter, 18] = rightWhiskerAvgRetractionVelocity;

                            avgleftWhiskerAvgProtractionVelocity += leftWhiskerAvgProtractionVelocity;
                            data[rowCounter, 19] = leftWhiskerAvgProtractionVelocity;

                            avgrightWhiskerAvgProtractionVelocity += rightWhiskerAvgProtractionVelocity;
                            data[rowCounter, 20] = rightWhiskerAvgProtractionVelocity;
                            freqCounter++;
                        }
                        else if (SelectedVelocityOption == "Still" && movingBehaviour is IStill)
                        {
                            avgleftWhiskerFrequency += leftWhiskerFrequency;
                            data[rowCounter, 9] = leftWhiskerFrequency;

                            avgrightWhiskerFrequency += rightWhiskerFrequency;
                            data[rowCounter, 10] = rightWhiskerFrequency;

                            avgleftWhiskerAmplitude += leftWhiskerAmplitude;
                            data[rowCounter, 11] = leftWhiskerAmplitude;

                            avgrightWhiskerAmplitude += rightWhiskerAmplitude;
                            data[rowCounter, 12] = rightWhiskerAmplitude;

                            avgleftWhiskerMeanOffset += leftWhiskerMeanOffset;
                            data[rowCounter, 13] = leftWhiskerMeanOffset;

                            avgrightWhiskerMeanOffset += rightWhiskerMeanOffset;
                            data[rowCounter, 14] = rightWhiskerMeanOffset;

                            avgleftWhiskerAvgAngularVelocity += leftWhiskerAvgAngularVelocity;
                            data[rowCounter, 15] = leftWhiskerAvgAngularVelocity;

                            avgrightWhiskerAvgAngularVelocity += rightWhiskerAvgAngularVelocity;
                            data[rowCounter, 16] = rightWhiskerAvgAngularVelocity;

                            avgleftWhiskerAvgRetractionVelocity += leftWhiskerAvgRetractionVelocity;
                            data[rowCounter, 17] = leftWhiskerAvgRetractionVelocity;

                            avgrightWhiskerAvgRetractionVelocity += rightWhiskerAvgRetractionVelocity;
                            data[rowCounter, 18] = rightWhiskerAvgRetractionVelocity;

                            avgleftWhiskerAvgProtractionVelocity += leftWhiskerAvgProtractionVelocity;
                            data[rowCounter, 19] = leftWhiskerAvgProtractionVelocity;

                            avgrightWhiskerAvgProtractionVelocity += rightWhiskerAvgProtractionVelocity;
                            data[rowCounter, 20] = rightWhiskerAvgProtractionVelocity;
                            freqCounter++;
                        }
                        else
                        {
                            avgleftWhiskerFrequency += leftWhiskerFrequency;
                            data[rowCounter, 9] = leftWhiskerFrequency;

                            avgrightWhiskerFrequency += rightWhiskerFrequency;
                            data[rowCounter, 10] = rightWhiskerFrequency;

                            avgleftWhiskerAmplitude += leftWhiskerAmplitude;
                            data[rowCounter, 11] = leftWhiskerAmplitude;

                            avgrightWhiskerAmplitude += rightWhiskerAmplitude;
                            data[rowCounter, 12] = rightWhiskerAmplitude;

                            avgleftWhiskerMeanOffset += leftWhiskerMeanOffset;
                            data[rowCounter, 13] = leftWhiskerMeanOffset;

                            avgrightWhiskerMeanOffset += rightWhiskerMeanOffset;
                            data[rowCounter, 14] = rightWhiskerMeanOffset;

                            avgleftWhiskerAvgAngularVelocity += leftWhiskerAvgAngularVelocity;
                            data[rowCounter, 15] = leftWhiskerAvgAngularVelocity;

                            avgrightWhiskerAvgAngularVelocity += rightWhiskerAvgAngularVelocity;
                            data[rowCounter, 16] = rightWhiskerAvgAngularVelocity;

                            avgleftWhiskerAvgRetractionVelocity += leftWhiskerAvgRetractionVelocity;
                            data[rowCounter, 17] = leftWhiskerAvgRetractionVelocity;

                            avgrightWhiskerAvgRetractionVelocity += rightWhiskerAvgRetractionVelocity;
                            data[rowCounter, 18] = rightWhiskerAvgRetractionVelocity;

                            avgleftWhiskerAvgProtractionVelocity += leftWhiskerAvgProtractionVelocity;
                            data[rowCounter, 19] = leftWhiskerAvgProtractionVelocity;

                            avgrightWhiskerAvgProtractionVelocity += rightWhiskerAvgProtractionVelocity;
                            data[rowCounter, 20] = rightWhiskerAvgProtractionVelocity;
                            freqCounter++;
                        }
                    }
                    
                    data[rowCounter, 0] = mouse.File;
                
                    //double centroidWidth = result.GetCentroidWidthForRunning();
                    double distanceTravelled = result.DistanceTravelled;
                    double centroidDistanceTravlled = result.CentroidDistanceTravelled;
                    double maxSpeed = result.MaxSpeed;
                    double maxAngVelocity = result.MaxAngularVelocty;
                    double avgVelocity;// = result.GetAverageSpeedForMoving();
                    double avgCentroidVelocity;// = result.GetAverageCentroidSpeedForMoving();
                    if (SelectedVelocityOption == "Running")
                    {
                        avgVelocity = result.GetAverageSpeedForRunning();
                        avgCentroidVelocity = result.GetAverageCentroidSpeedForRunning();
                    }
                    else if (SelectedVelocityOption == "Moving")
                    {
                        avgVelocity = result.GetAverageSpeedForMoving();
                        avgCentroidVelocity = result.GetAverageCentroidSpeedForMoving();
                    }
                    else if (SelectedVelocityOption == "Still")
                    {
                        avgVelocity = result.GetAverageSpeedForStill();
                        avgCentroidVelocity = result.GetAverageCentroidSpeedForStill();
                    }
                    else
                    {
                        avgVelocity = result.GetAverageSpeed();
                        avgCentroidVelocity = result.GetAverageCentroidSpeed();
                    }
                    
                    double averageAngVelocity;
                    if (SelectedRotationOption == "Turning only")
                    {
                        averageAngVelocity = result.GetAverageAngularSpeedForTurning();
                    }
                    else
                    {
                        averageAngVelocity = result.GetAverageAngularSpeed();
                    }
                    
                    //double avgPelvic1 = result.GetCentroidWidthForPelvic1();
                    //double avgPelvic2 = result.GetCentroidWidthForPelvic2();
                    //double avgPelvic3 = result.GetCentroidWidthForPelvic3();
                    //double avgPelvic4 = result.GetCentroidWidthForPelvic4();
                    
                    double maxCentroidVelocity = result.MaxCentroidSpeed;
                    double duration = result.EndFrame - result.StartFrame;

                    //distanceTravelled /= 1000;
                    //avgVelocity /= 1000;
                    //avgCentroidVelocity /= 1000;
                    //maxSpeed /= 1000;
                    //maxCentroidVelocity /= 1000;
                    //averageAngVelocity /= 1000;
                    //maxAngVelocity /= 1000;

                    int frameDelta = result.EndFrame - result.StartFrame;
                    List<Tuple<int, int>> movingFrameNumbers = result.GetFrameNumbersForMoving();
                    List<Tuple<int, int>> runningFrameNumbers = result.GetFrameNumbersForRunning();
                    List<Tuple<int, int>> turningFrameNumbers = result.GetFrameNumbersForTurning();
                    List<Tuple<int, int>> stillFrameNumbers = result.GetFrameNumbersForStill();
                    List<Tuple<int, int>> interactingFrameNumbers = result.GetFrameNumbesrForInteracting();
                    
                    int movingFrameCount = 0, runningFrameCount = 0, turningFrameCount = 0, stillFrameCount = 0;

                    if (movingFrameNumbers != null && movingFrameNumbers.Any())
                    {
                        foreach (Tuple<int, int> t in movingFrameNumbers)
                        {
                            int start = t.Item1;
                            int end = t.Item2;

                            if (start > result.EndFrame)
                            {
                                continue;
                            }

                            if (start < result.StartFrame)
                            {
                                start = result.StartFrame;
                            }

                            if (end > result.EndFrame)
                            {
                                end = result.EndFrame;
                            }
                            int delta = end - start + 1;
                            movingFrameCount += delta;
                        }
                    }

                    if (runningFrameNumbers != null && runningFrameNumbers.Any())
                    {
                        foreach (Tuple<int, int> t in runningFrameNumbers)
                        {
                            int start = t.Item1;
                            int end = t.Item2;

                            if (start > result.EndFrame)
                            {
                                continue;
                            }

                            if (start < result.StartFrame)
                            {
                                start = result.StartFrame;
                            }

                            if (end > result.EndFrame)
                            {
                                end = result.EndFrame;
                            }
                            int delta = end - start + 1;
                            runningFrameCount += delta;
                        }
                    }

                    if (stillFrameNumbers != null && stillFrameNumbers.Any())
                    {
                        foreach (Tuple<int, int> t in stillFrameNumbers)
                        {
                            int start = t.Item1;
                            int end = t.Item2;

                            if (start > result.EndFrame)
                            {
                                continue;
                            }

                            if (start < result.StartFrame)
                            {
                                start = result.StartFrame;
                            }

                            if (end > result.EndFrame)
                            {
                                end = result.EndFrame;
                            }
                            int delta = end - start + 1;
                            stillFrameCount += delta;
                        }
                    }

                    if (turningFrameNumbers != null && turningFrameNumbers.Any())
                    {
                        foreach (Tuple<int, int> t in turningFrameNumbers)
                        {
                            int start = t.Item1;
                            int end = t.Item2;

                            if (start > result.EndFrame)
                            {
                                continue;
                            }

                            if (start < result.StartFrame)
                            {
                                start = result.StartFrame;
                            }

                            if (end > result.EndFrame)
                            {
                                end = result.EndFrame;
                            }
                            int delta = end - start + 1;
                            turningFrameCount += delta;
                        }
                    }

                    List<int> allInteractingFrameNumbers = new List<int>();
                    if (interactingFrameNumbers != null && interactingFrameNumbers.Any())
                    {
                        foreach (Tuple<int, int> t in interactingFrameNumbers)
                        {
                            int start = t.Item1;
                            int end = t.Item2;

                            if (start > result.EndFrame)
                            {
                                continue;
                            }

                            if (start < result.StartFrame)
                            {
                                start = result.StartFrame;
                            }

                            if (end > result.EndFrame)
                            {
                                end = result.EndFrame;
                            }

                            for (int i = start; i <= end; i++)
                            {
                                if (!allInteractingFrameNumbers.Contains(i))
                                {
                                    allInteractingFrameNumbers.Add(i);
                                }
                            }
                        }
                    }

                    double movingPercentage = (double) movingFrameCount/(frameDelta - 40 + 1);
                    double runningPercentage = (double) runningFrameCount/ (frameDelta - 40 + 1);
                    double turningPercentage = (double) turningFrameCount/ (frameDelta - 40 + 1);
                    double stillPercentage = (double) stillFrameCount/ (frameDelta - 40 + 1);

                    int fCount = allInteractingFrameNumbers.Count - 1;
                    if (fCount < 0)
                    {
                        fCount = 0;
                    }
                    double interactionPercentage = (double)fCount / frameDelta;

                    //if (centroidWidth > 0)
                    //{
                    //    data[rowCounter, 4] = centroidWidth;
                    //}
                    counter++;
                    if (distanceTravelled > 0)
                    {
                        averageDistance += distanceTravelled;
                        data[rowCounter, 1] = distanceTravelled;
                    }

                    if (centroidDistanceTravlled > 0)
                    {
                        averageCentroidDistance += centroidDistanceTravlled;
                        data[rowCounter, 2] = centroidDistanceTravlled;
                    }

                    if (maxSpeed > 0)
                    {
                        if (maxSpeed > maxVelocity)
                        {
                            maxVelocity = maxSpeed;
                        }
                        data[rowCounter, 3] = maxSpeed;
                    }

                    if (maxAngVelocity > 0)
                    {
                        if (maxAngVelocity > maxAngularVelocity)
                        {
                            maxAngularVelocity = maxAngVelocity;
                        }
                        data[rowCounter, 4] = maxAngVelocity;
                    }

                    if (avgVelocity > 0)
                    {
                        averageVelocity += avgVelocity;
                        data[rowCounter, 5] = avgVelocity;
                    }

                    if (averageAngVelocity > 0)
                    {
                        averageAngularVelocity += averageAngVelocity;
                        data[rowCounter, 6] = averageAngVelocity;
                    }
                    
                    if (avgCentroidVelocity > 0)
                    {
                        averageCentroidVelocity += avgCentroidVelocity;
                        data[rowCounter, 7] = avgCentroidVelocity;
                    }

                    if (maxCentroidVelocity > 0)
                    {
                        if (maxCentroidVelocity > totalMaxCentroidVelocity)
                        {
                            totalMaxCentroidVelocity = maxCentroidVelocity;
                        }
                        data[rowCounter, 8] = maxCentroidVelocity;
                    }

                    if (duration > 0)
                    {
                        data[rowCounter, 21] = duration;
                    }

                    //if (runningPercentage > 0)velocities
                    //{
                    averageRunning += runningPercentage;
                        data[rowCounter, 22] = runningPercentage;
                    //}

                    //if (movingPercentage > 0)
                    //{
                    averageMoving += movingPercentage;
                        data[rowCounter, 23] = movingPercentage;
                    //}

                    //if (turningPercentage > 0)
                    //{
                    averageTurning += turningPercentage;
                        data[rowCounter, 24] = turningPercentage;
                    //}

                    //if (stillPercentage > 0)
                    //{
                    averageStill += stillPercentage;
                        data[rowCounter, 25] = stillPercentage;
                    data[rowCounter, 26] = leftRMS;
                    data[rowCounter, 27] = rightRMS;
                    data[rowCounter, 28] = Math.Sqrt(2) * (2 * leftSTD);
                    data[rowCounter, 29] = Math.Sqrt(2) * (2 * rightSTD);
                    rowCounter++;
                    
                }
                rowCounter++;
             
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            string fileLocation = FileBrowser.SaveFile("Excel|*.xlsx");

            if (string.IsNullOrWhiteSpace(fileLocation))
            {
                return;
            }

            ExcelService.WriteData(data, fileLocation, true);
        }

        private class CounterClass
        {
            public int CentroidWidth
            {
                get;
                set;
            }

            public int Distance
            {
                get;
                set;
            }

            public int AvgVelocity
            {
                get;
                set;
            }

            public int AvgAngVelocity
            {
                get;
                set;
            }

            public int AvgPelvic1
            {
                get;
                set;
            }

            public int AvgPelvic2
            {
                get;
                set;
            }

            public int AvgPelvic3
            {
                get;
                set;
            }

            public int AvgPelvic4
            {
                get;
                set;
            }

            public int AvgCentroidVelocity
            {
                get;
                set;
            }

            public int ClipDuration
            {
                get;
                set;
            }

            public int PercentageRunning
            {
                get;
                set;
            }

            public int PercentageMoving
            {
                get;
                set;
            }

            public int PercentageTurning
            {
                get;
                set;
            }

            public int PercentageStill
            {
                get;
                set;
            }

            public int PercentageInteracting
            {
                get;
                set;
            }
        }

        private void GenerateIndivResultsFinal()
        {
            int rows = 1000;
            int columns = 22;
            object[,] data = new object[rows, columns];

            int rowCounter = 1;
            data[0, 0] = "Mouse";
            data[0, 1] = "Type";
            data[0, 2] = "Age";
            data[0, 3] = "Clip";
            data[0, 4] = "Centroid Width";
            data[0, 5] = "Distance";
            data[0, 6] = "Max Velocity";
            data[0, 7] = "Max Angular Velocity";
            data[0, 8] = "Average Velocity";
            data[0, 9] = "Average Angular Velocity";
            data[0, 10] = "Average Pelvic Area 1";
            data[0, 11] = "Average Pelvic Area 2";
            data[0, 12] = "Average Pelvic Area 3";
            data[0, 13] = "Average Pelvic Area 4";
            data[0, 14] = "Average Centroid Velocity";
            data[0, 15] = "Max Centroid Velocity";
            data[0, 16] = "Clip Duration";
            data[0, 17] = "Percentage Running";
            data[0, 18] = "Percentage moving";
            data[0, 19] = "Percentage turning";
            data[0, 20] = "Percentage Still";
            data[0, 21] = "Percentage Interacting";
            data[0, 26] = "Left Whisker Spread";
            data[0, 27] = "Right Whisker spread";
            data[0, 28] = "Left Whisker peak-to-peak amplitude";
            data[0, 29] = "Right Whisker peak-to-peak amplitude";
            Dictionary<BatchProcessViewModel.BrettTuple<string, int>, IMouseDataExtendedResult> sortedResults = new Dictionary<BatchProcessViewModel.BrettTuple<string, int>, IMouseDataExtendedResult>();
            Dictionary<BatchProcessViewModel.BrettTuple<string, int>, CounterClass> sortedCounters = new Dictionary<BatchProcessViewModel.BrettTuple<string, int>, CounterClass>();
            List<MouseHolder> mice = new List<MouseHolder>();

            foreach (SingleMouseViewModel mouse in Videos)
            {
                mice.AddRange(from video in mouse.VideoFiles let result = mouse.Results[video] select new MouseHolder() { Age = mouse.Age.ToString(), Class = mouse.Class, File = video.VideoFileName, Mouse = mouse, Result = result, Type = mouse.Type.Name });
            }
            try
            {


                foreach (MouseHolder mouse in mice)
                {
                    BatchProcessViewModel.BrettTuple<string, int> key = new BatchProcessViewModel.BrettTuple<string, int>(mouse.Mouse.Id, mouse.Mouse.Age);

                    IMouseDataExtendedResult finalResult;
                    CounterClass counter;
                    if (sortedResults.ContainsKey(key))
                    {
                        finalResult = sortedResults[key];
                        counter = sortedCounters[key];
                    }
                    else
                    {
                        finalResult = ModelResolver.Resolve<IMouseDataExtendedResult>();
                        finalResult.Name = mouse.Mouse.Id;
                        finalResult.Age = mouse.Mouse.Age;
                        finalResult.Type = mouse.Mouse.Type;

                        counter = new CounterClass();
                        sortedResults.Add(key, finalResult);
                        sortedCounters.Add(key, counter);
                    }

                    IMouseDataExtendedResult result = mouse.Result;

                    if (result.EndFrame - result.StartFrame < 100)
                    {
                        continue;
                    }
                    
                    double centroidWidth = result.GetCentroidWidthForRunning();
                    double distanceTravelled = result.DistanceTravelled;
                    double maxSpeed = result.MaxSpeed;
                    double maxAngVelocity = result.MaxAngularVelocty;
                    double avgVelocity = result.GetAverageSpeedForMoving();
                    double averageAngVelocity = result.AverageAngularVelocity;
                    double avgPelvic1 = result.GetCentroidWidthForPelvic1();
                    double avgPelvic2 = result.GetCentroidWidthForPelvic2();
                    double avgPelvic3 = result.GetCentroidWidthForPelvic3();
                    double avgPelvic4 = result.GetCentroidWidthForPelvic4();
                    double avgCentroidVelocity = result.GetAverageCentroidSpeedForMoving();
                    double maxCentroidVelocity = result.MaxCentroidSpeed;
                    double duration = result.EndFrame - result.StartFrame;

                    avgVelocity /= 1000;
                    avgCentroidVelocity /= 1000;
                    maxSpeed /= 1000;
                    maxCentroidVelocity /= 1000;
                    averageAngVelocity /= 1000;
                    maxAngVelocity /= 1000;

                    int frameDelta = result.EndFrame - result.StartFrame;
                    List<Tuple<int, int>> movingFrameNumbers = result.GetFrameNumbersForMoving();
                    List<Tuple<int, int>> runningFrameNumbers = result.GetFrameNumbersForRunning();
                    List<Tuple<int, int>> turningFrameNumbers = result.GetFrameNumbersForTurning();
                    List<Tuple<int, int>> stillFrameNumbers = result.GetFrameNumbersForStill();
                    List<Tuple<int, int>> interactingFrameNumbers = result.GetFrameNumbesrForInteracting();

                    int movingFrameCount = 0, runningFrameCount = 0, turningFrameCount = 0, stillFrameCount = 0;

                    if (movingFrameNumbers != null && movingFrameNumbers.Any())
                    {
                        foreach (Tuple<int, int> t in movingFrameNumbers)
                        {
                            int delta = t.Item2 - t.Item1;
                            movingFrameCount += delta;
                        }
                    }

                    if (runningFrameNumbers != null && runningFrameNumbers.Any())
                    {
                        foreach (Tuple<int, int> t in runningFrameNumbers)
                        {
                            int delta = t.Item2 - t.Item1;
                            runningFrameCount += delta;
                        }
                    }

                    if (stillFrameNumbers != null && stillFrameNumbers.Any())
                    {
                        foreach (Tuple<int, int> t in stillFrameNumbers)
                        {
                            int delta = t.Item2 - t.Item1;
                            stillFrameCount += delta;
                        }
                    }

                    if (turningFrameNumbers != null && turningFrameNumbers.Any())
                    {
                        foreach (Tuple<int, int> t in turningFrameNumbers)
                        {
                            int delta = t.Item2 - t.Item1;
                            turningFrameCount += delta;
                        }
                    }

                    List<int> allInteractingFrameNumbers = new List<int>();
                    if (interactingFrameNumbers != null && interactingFrameNumbers.Any())
                    {
                        foreach (Tuple<int, int> t in interactingFrameNumbers)
                        {
                            int start = t.Item1;
                            int end = t.Item2;

                            for (int i = start; i <= end; i++)
                            {
                                if (!allInteractingFrameNumbers.Contains(i))
                                {
                                    allInteractingFrameNumbers.Add(i);
                                }
                            }
                        }
                    }

                    double movingPercentage = (double)movingFrameCount / frameDelta;
                    double runningPercentage = (double)runningFrameCount / frameDelta;
                    double turningPercentage = (double)turningFrameCount / frameDelta;
                    double stillPercentage = (double)stillFrameCount / frameDelta;

                    int fCount = allInteractingFrameNumbers.Count - 1;
                    if (fCount < 0)
                    {
                        fCount = 0;
                    }
                    double interactionPercentage = (double)fCount / frameDelta;

                    if (centroidWidth > 0)
                    {
                        finalResult.CentroidSize += centroidWidth;
                        counter.CentroidWidth++;
                    }

                    if (distanceTravelled > 0)
                    {
                        finalResult.DistanceTravelled += distanceTravelled;
                        counter.Distance++;
                    }

                    if (maxSpeed > 0)
                    {
                        if (maxSpeed > finalResult.MaxSpeed)
                        {
                            finalResult.MaxSpeed = maxSpeed;
                        }
                        
                        //counter.MaxVelocity++;
                    }

                    if (maxAngVelocity > 0)
                    {
                        if (maxAngVelocity > finalResult.MaxAngularVelocty)
                        {
                            finalResult.MaxAngularVelocty = maxAngVelocity;
                        }
                        
                        //counter.MaxAngVelocity++;
                    }

                    if (avgVelocity > 0)
                    {
                        finalResult.AverageVelocity += avgVelocity;
                        counter.AvgVelocity++;
                    }

                    if (averageAngVelocity > 0)
                    {
                        finalResult.AverageAngularVelocity += averageAngVelocity;
                        counter.AvgAngVelocity++;
                    }

                    if (avgPelvic1 > 0)
                    {
                        finalResult.PelvicArea += avgPelvic1;
                        counter.AvgPelvic1++;
                    }

                    if (avgPelvic2 > 0)
                    {
                        finalResult.PelvicArea2 += avgPelvic2;
                        counter.AvgPelvic2++;
                    }

                    if (avgPelvic3 > 0)
                    {
                        finalResult.PelvicArea3 += avgPelvic3;
                        counter.AvgPelvic3++;
                    }

                    if (avgPelvic4 > 0)
                    {
                        finalResult.PelvicArea4 += avgPelvic4;
                        counter.AvgPelvic4++;
                    }

                    if (avgCentroidVelocity > 0)
                    {
                        finalResult.AverageCentroidVelocity += avgCentroidVelocity;
                        counter.AvgCentroidVelocity++;
                    }

                    if (maxCentroidVelocity > 0)
                    {
                        if (maxCentroidVelocity > finalResult.MaxCentroidSpeed)
                        {
                            finalResult.MaxCentroidSpeed += maxCentroidVelocity;
                        }

                        //counter.MaxCentroidVelocity++;
                    }

                    //if (duration > 0)
                    //{
                        finalResult .Duration += duration;
                        counter.ClipDuration++;
                    //}

                    //if (runningPercentage > 0)
                    //{
                        finalResult.Dummy += runningPercentage;
                        counter.PercentageRunning++;
                    //}

                    //if (movingPercentage > 0)
                    //{
                        finalResult.Dummy2 += movingPercentage;
                        counter.PercentageMoving++;
                    //}

                    //if (turningPercentage > 0)
                    //{
                        finalResult.Dummy3 += turningPercentage;
                        counter.PercentageTurning++;
                    //}

                    //if (stillPercentage > 0)
                    //{
                        finalResult.Dummy4 += stillPercentage;
                        counter.PercentageStill++;
                    //}

                    finalResult.Dummy5 += interactionPercentage;
                    counter.PercentageInteracting++;

                    //rowCounter++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            string fileLocation = FileBrowser.SaveFile("Excel CSV|*.csv");

            if (string.IsNullOrWhiteSpace(fileLocation))
            {
                return;
            }

            foreach (var kvp in sortedResults)
            {
                var key = kvp.Key;
                var counter = sortedCounters[key];
                IMouseDataExtendedResult result = kvp.Value;

                data[rowCounter, 0] = result.Name;
                data[rowCounter, 1] = result.Type;
                data[rowCounter, 2] = result.Age;
                //data[rowCounter, 3] = "Clip";
                data[rowCounter, 4] = result.CentroidSize/counter.CentroidWidth;
                data[rowCounter, 5] = result.DistanceTravelled/counter.Distance;
                data[rowCounter, 6] = result.MaxSpeed;
                data[rowCounter, 7] = result.MaxAngularVelocty;
                data[rowCounter, 8] = result.AverageVelocity/counter.AvgVelocity;
                data[rowCounter, 9] = result.AverageAngularVelocity/counter.AvgAngVelocity;
                data[rowCounter, 10] = result.PelvicArea/counter.AvgPelvic1;
                data[rowCounter, 11] = result.PelvicArea2/counter.AvgPelvic2;
                data[rowCounter, 12] = result.PelvicArea3/counter.AvgPelvic3;
                data[rowCounter, 13] = result.PelvicArea4/counter.AvgPelvic4;
                data[rowCounter, 14] = result.AverageCentroidVelocity/counter.AvgCentroidVelocity;
                data[rowCounter, 15] = result.MaxSpeed;
                data[rowCounter, 16] = result.Duration/counter.ClipDuration;
                data[rowCounter, 17] = result.Dummy/counter.PercentageRunning;
                data[rowCounter, 18] = result.Dummy2/counter.PercentageMoving;
                data[rowCounter, 19] = result.Dummy3/counter.PercentageTurning;
                data[rowCounter, 20] = result.Dummy4/counter.PercentageStill;
                data[rowCounter, 21] = result.Dummy5/counter.PercentageInteracting;
                rowCounter++;
            }
            int rowCount = data.GetLength(0);
            int columnCount = data.GetLength(1);

            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= rowCount; i++)
            {
                StringBuilder sb2 = new StringBuilder();
                for (int j = 1; j <= columnCount; j++)
                {
                    object dat = data[i - 1, j - 1];
                    sb2.Append(dat);

                    if (j < columnCount)
                    {
                        sb2.Append(",");
                    }
                }
                
                sb.AppendLine(sb2.ToString());
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileLocation))
            {
                file.WriteLine(sb.ToString());
            }
        }

        private void Apply()
        {
            
        }

        private class MouseHolder
        {
            public SingleMouseViewModel Mouse
            {
                get;
                set;
            }

            public IMouseDataExtendedResult Result
            {
                get;
                set;
            }

            public string Age
            {
                get;
                set;
            }

            public string Class
            {
                get;
                set;
            }

            public string Type
            {
                get;
                set;
            }

            public string File
            {
                get;
                set;
            }
        }
    }
}
