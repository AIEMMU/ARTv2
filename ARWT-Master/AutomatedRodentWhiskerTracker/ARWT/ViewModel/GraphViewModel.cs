using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARWT.Commands;
using ARWT.ModelInterface.Smoothing;
using ARWT.Resolver;
using ARWT.Services;

namespace ARWT.ViewModel
{
    public class GraphViewModel : WindowBaseModel
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
                    ExecuteAction = ExportData
                });
            }
        }

        public GraphViewModel()
        {
            ObservableCollection<ISmoothingBase> smoothingFunctions = new ObservableCollection<ISmoothingBase>();
            smoothingFunctions.Add(ModelResolver.Resolve<iMovingAverage2>());
            smoothingFunctions.Add(ModelResolver.Resolve<iMovingAverage>());
            smoothingFunctions.Add(ModelResolver.Resolve<IGaussianSmoothing>());
            smoothingFunctions.Add(ModelResolver.Resolve<IBoxCarSmoothing>());
            SmoothingFunctions = smoothingFunctions;
            SelectedSmoothingFunction = SmoothingFunctions.First();
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
            Dictionary < int, double> dicSignal = new Dictionary<int, double>();
            for (int i = 0; i < signal.Length; i++)
            {
                dicSignal.Add(i, signal[i]);
            }
            return dicSignal;
        } 

        private KeyValuePair<int, double>[] ConvertToKvp(double[] values)
        {
            KeyValuePair<int, double>[] kvps = new KeyValuePair<int, double>[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                kvps[i] = new KeyValuePair<int, double>(i, values[i]);
            }
            return kvps;
        }

        private double[] GetDoubleValues(Dictionary<int, double> array)
        {
            return array.Select(x => x.Value).ToArray();
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

            double[] smoothedLeft = SelectedSmoothingFunction.Smooth(leftData);
            double[] smootedRight = SelectedSmoothingFunction.Smooth(rightData);


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

        private void ExportData()
        {
            string saveLocation = FileBrowser.SaveFile("Excel|*.xlsx");

            if (string.IsNullOrWhiteSpace(saveLocation))
            {
                return;
            }

            int leftMax = LeftGraphWhiskers.Select(x => x.Key).Max();
            int rightMax = RightGraphWhiskers.Select(x => x.Key).Max();
            int max = leftMax > rightMax ? leftMax : rightMax;

            object[,] data = new object[5, max + 2];

            data[0, 0] = "Frame";
            data[1, 0] = "Left Whisker";
            data[2, 0] = "Left Whisker Smoothed";
            data[3, 0] = "Right Whisker";
            data[4, 0] = "Right Whisker Smoothed";

            //for (int i = 0; i < max + 2; i++)
            //{
            //    data[0, i] = string.Empty;
            //    data[1, i] = string.Empty;
            //    data[2, i] = string.Empty;
            //    data[3, i] = string.Empty;
            //    data[4, i] = string.Empty;
            //}

            for (int i = 0; i < max + 1; i++)
            {
                data[0, i + 1] = i;
            }
            
            for (int i = 0; i < LeftGraphWhiskers.Length; i++)
            {
                
                var t = LeftGraphWhiskers[i];

                if (t.Key == 502 || t.Key == 503 || t.Key == 504)
                {
                    Console.WriteLine("");
                }


                data[1, LeftGraphWhiskers[i].Key + 1] = LeftGraphWhiskers[i].Value;
            }

            for (int i = 0; i < LeftGraphWhiskersSmoothed.Length; i++)
            {
                data[2, LeftGraphWhiskersSmoothed[i].Key + 1] = LeftGraphWhiskersSmoothed[i].Value;
            }

            for (int i = 0; i < RightGraphWhiskers.Length; i++)
            {
                data[3, RightGraphWhiskers[i].Key + 1] = RightGraphWhiskers[i].Value;
                data[4, RightGraphWhiskersSmoothed[i].Key + 1] = RightGraphWhiskersSmoothed[i].Value;
            }

            ExcelService.WriteData(data, saveLocation, true);
        }
    }
}
