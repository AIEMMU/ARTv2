using System;
using System.Collections.Generic;
using System.Linq;
using ARWT.Model.MWA;
using ARWT.Services;

using RobynsWhiskerTracker.Services.Maths;

namespace ARWT.Model.Analysis
{
    internal class SingleWhiskerProtractionRetraction : ModelObjectBase
    {
        private double[] m_AngleSignal;
        private double m_MeanAngularVelocity;
        private IWhisker m_Whisker;
        private double m_MeanProtractionVelocity;
        private double m_MeanRetractionVelocity;
        private double m_MaxAmplitude;
        private List<ProtractionRetractionBase> m_ProtractionRetractionData;

        public Dictionary<int, ProtractionRetractionBase> ProtractionRetractionDictionary
        {
            get;
            set;
        }

        public double[] AngleSignal
        {
            get
            {
                return m_AngleSignal;
            }
            set
            {
                if (ReferenceEquals(m_AngleSignal, value))
                {
                    return;
                }

                m_AngleSignal = value;

                CreateAngularVelocitySignal();
                MarkAsDirty();
            }
        }

        public double MeanAngularVelocity
        {
            get
            {
                return m_MeanAngularVelocity;
            }
            set
            {
                if (ReferenceEquals(m_MeanAngularVelocity, value))
                {
                    return;
                }

                m_MeanAngularVelocity = value;

                MarkAsDirty();
            }
        }
        private double m_avgAmp;
        public double avgAmp
        {
            get
            {
                return m_avgAmp;
            }
            set
            {
                if (ReferenceEquals(m_avgAmp, value))
                {
                    return;
                }

                m_avgAmp = value;

                MarkAsDirty();
            }
        }
        public double MeanProtractionVelocity
        {
            get
            {
                return m_MeanProtractionVelocity;
            }
            private set
            {
                if (Equals(m_MeanProtractionVelocity, value))
                {
                    return;
                }

                m_MeanProtractionVelocity = value;

                MarkAsDirty();
            }
        }

        public double MeanRetractionVelocity
        {
            get
            {
                return m_MeanRetractionVelocity;
            }
            private set
            {
                if (Equals(m_MeanRetractionVelocity, value))
                {
                    return;
                }

                m_MeanRetractionVelocity = value;

                MarkAsDirty();
            }
        }

        public double MaxAmplitude
        {
            get
            {
                return m_MaxAmplitude;
            }
            private set
            {
                if (Equals(m_MaxAmplitude, value))
                {
                    return;
                }

                m_MaxAmplitude = value;

                MarkAsDirty();
            }
        }


        private double _FrameRate;
        public double FrameRate
        {
            get
            {
                return _FrameRate;
            }
            set
            {
                if (Equals(_FrameRate, value))
                {
                    return;
                }

                _FrameRate = value;

                MarkAsDirty();
            }
        }
        
        public List<ProtractionRetractionBase> ProtractionRetractionData
        {
            get
            {
                return m_ProtractionRetractionData;
            }
            set
            {
                if (ReferenceEquals(m_ProtractionRetractionData, value))
                {
                    return;
                }

                m_ProtractionRetractionData = value;

                MarkAsDirty();
            }
        }

        private void CreateAngularVelocitySignal()
        {
            if (AngleSignal == null || AngleSignal.Length == 0)
            {
                return;
            }

            bool error = false;

            double[] smoothedAngleSignal = SmoothingFunctions.BoxCarSmooth(AngleSignal);

            if (smoothedAngleSignal == null || smoothedAngleSignal.Length == 0)
            {
                return;
            }

            int[][] peaksAndValleys = SmoothingFunctions.FindPeaksAndValleys(smoothedAngleSignal/*AngleSignal*/);
            int[] peaks = peaksAndValleys[0];
            int[] valleys = peaksAndValleys[1];

            if (peaks.Length == 0 || valleys.Length == 0)
            {
                return;
            }

            int[] rawPeaks = new int[peaks.Length];
            for (int i = 0; i < peaks.Length; i++)
            {
                int closestPeak = SmoothingFunctions.FindClosestPeak(peaks[i], AngleSignal);

                if (rawPeaks.Contains(closestPeak))
                {
                    ErrorOccured("The signal is too noisy, accurate results can not be guaranteed");
                    error = true;
                }

                rawPeaks[i] = closestPeak;
            }

            int[] rawValleys = new int[valleys.Length];
            for (int i = 0; i < valleys.Length; i++)
            {
                int closestValley = SmoothingFunctions.FindClosestValley(valleys[i], AngleSignal);

                if (rawValleys.Contains(closestValley))
                {
                    ErrorOccured("The signal is too noisy, accurate results can not be guaranteed");
                    error = true;
                }

                rawValleys[i] = closestValley;
            }

            List<ProtractionRetractionBase> data = new List<ProtractionRetractionBase>();

            int peakCounter = 0;
            int valleyCounter = 0;
            int currentPeak = rawPeaks[peakCounter];
            int currentValley = rawValleys[valleyCounter];
            bool protract = currentPeak > currentValley;
            Dictionary<int, ProtractionRetractionBase> protractionRetractionDictionary = new Dictionary<int, ProtractionRetractionBase>();
            while (true)
            {
                if (protract)
                {
                    double deltaTime = Math.Abs(currentPeak - currentValley) / FrameRate;
                    ProtractionRetractionBase protraction = new ProtractionData();
                    protraction.UpdateData(AngleSignal[currentValley], AngleSignal[currentPeak], deltaTime);
                    data.Add(protraction);

                    for (int i = currentValley; i < currentPeak; i++)
                    {
                        if (!protractionRetractionDictionary.ContainsKey(i))
                        {
                            protractionRetractionDictionary.Add(i, protraction);
                        }
                        else
                        {
                            ErrorOccured("The signal is too noisy, accurate results can not be guaranteed");
                            error = true;
                        }
                    }

                    valleyCounter++;

                    if (valleyCounter >= rawValleys.Length)
                    {
                        break;
                    }

                    currentValley = rawValleys[valleyCounter];

                    protract = false;
                }
                else
                {
                    double deltaTime = Math.Abs(currentPeak - currentValley) / FrameRate;
                    ProtractionRetractionBase retraction = new RetractionData();
                    retraction.UpdateData(AngleSignal[currentValley], AngleSignal[currentPeak], deltaTime);
                    data.Add(retraction);

                    for (int i = currentPeak; i < currentValley; i++)
                    {
                        if (!protractionRetractionDictionary.ContainsKey(i))
                        {
                            protractionRetractionDictionary.Add(i, retraction);
                        }
                        else
                        {
                            ErrorOccured("The signal is too noisy, accurate results can not be guaranteed");
                            error = true;
                        }
                    }

                    peakCounter++;

                    if (peakCounter >= rawPeaks.Length)
                    {
                        break;
                    }

                    currentPeak = rawPeaks[peakCounter];

                    protract = true;
                }
            }

            ProtractionRetractionData = data;
            ProtractionRetractionDictionary = protractionRetractionDictionary;
            avgAmp = ProtractionRetractionData.Where(x => x.Amplitude != 0).Average(protractionRetraction => protractionRetraction.Amplitude);

            MeanProtractionVelocity = Math.Abs(ProtractionRetractionData.Where(x => x is ProtractionData).Select(x => x.MeanAngularVelocity).Mean());
            MeanRetractionVelocity = Math.Abs(ProtractionRetractionData.Where(x => x is RetractionData).Select(x => x.MeanAngularVelocity).Mean());
            MeanAngularVelocity = Math.Abs(ProtractionRetractionData.Select(x => Math.Abs(x.MeanAngularVelocity)).Mean());

            double maxAngle = ProtractionRetractionData.Select(x => x.MaxAngle).Max();
            double minAngle = ProtractionRetractionData.Select(x => x.MinAngle).Min();

            MaxAmplitude = Math.Abs(maxAngle - minAngle);

            if (!error)
            {
                ModelObjectState = ModelObjectState.New;
            }
        }

        public ProtractionRetractionBase GetCurrentProtractionRetraction(int frame)
        {
            if (ProtractionRetractionDictionary == null)
            {
                return null;
            }

            if (ProtractionRetractionDictionary.ContainsKey(frame))
            {
                return ProtractionRetractionDictionary[frame];
            }

            return null;
        }
    }
}
