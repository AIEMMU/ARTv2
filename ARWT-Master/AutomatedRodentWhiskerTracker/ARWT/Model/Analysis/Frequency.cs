using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Properties;
using Complex = System.Numerics.Complex;
using ARWT.Services;
using ARWT.ModelInterface.Analysis;
using MathNet.Numerics.Statistics;





namespace ARWT.Model.Analysis
{
    internal class Frequency : ModelObjectBase, IFrequency
    {
        private double[] _Signal;
        public double[] Signal
        {
            get
            {
                return _Signal;
            }
            set
            {
                if (Equals(_Signal, value))
                {
                    return;
                }

                _Signal = value;

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

                MarkAsDirty();
            }
        }

        public double GetFrequency(out double[] zeroed)
        {
            if (UseDft)
            {
                double bestFrequency;
                var result = BrettFFT.BrettDFT(Signal, out zeroed, out bestFrequency, FrameRate, 4, 16, 0.1);

                return bestFrequency;
            }
            zeroed = null;
            
            return CalculateFrequency(Signal, FrameRate, 1);
        }
        public double [] AutoCorrelation(double[] x)
        {
            double mean = x.Mean();
            double[] autocor = new double[x.Count() ];
            for(int t =0; t < autocor.Count(); t++)
            {
                double n = 0;
                double d = 0;

                for(int i =0; i < x.Count(); i++)
                {
                    double xim = x[i] - mean;
                    n += xim * (x[(i + t) % x.Count()] - mean);
                    d += xim * xim;
                }
                autocor[t] = n / d;
            }

            return autocor;
        }
        private int[] calcLag(double[] x)
        {
            int[] lag = new int[x.Length * 2 - 1];

            for (int i = 0; i < x.Length; i++)
            {
                lag[i] = i + 1;
                lag[(lag.Length-1) - i] = i + 1;
                
            }
            return lag;
        }
        public double[] calcxCor(double[] x)
        {
            int[] lag = calcLag(x);// { 1, 2, 3, 4, 5, 4, 3, 2, 1 };
            double[] xx = new double[x.Length];
            double[] c0 = new double[x.Length* 2 -1];
            double[] c1 = new double[x.Length * 2 - 1];
            double s = x[0];
            
            for(int i = 1; i<x.Length; i++)
            {
                s += x[i];
            }

            s /= x.Length;

            for(int i = 0; i< x.Length; i++)
            {
                xx[i] = x[i] - s;
            }

            for (int k = 0; k < xx.Length; k++)
            {
                s = 0.0;
                for (int i = 0; i <= (xx.Length - 1) - k; i++)
                {
                    s += xx[i] * xx[k + i];
                }
                c1[(xx.Length - 1) - k] = s;
                c1[k + (xx.Length - 1)] = s;

            }
            for (int i =0; i <c1.Length; i++)
            {
                c1[i] /= Convert.ToDouble(lag[i]);
            }
            return c1;
        }
        public double CalculateFrequency(double[] signal, double frameRate, double frameInterval)
        {

            double T = 1.0 / 500;// FrameRate;
            double fq_nyquist = 0.5 / T;
            double fq_min = 4.0;
            double fq_max = 16.0;
            double n_fq_max = Math.Round((1.0 / fq_max) / T);
            double n_smooth = Math.Round(n_fq_max / 4.0);

            var theta = movingAverage(signal,5);
            var unsmooth_acf = calcxCor(theta);
            var acf = movingAverage(unsmooth_acf, Convert.ToInt32(n_smooth));

            double[] b = new double[signal.Count()];
            double[] a = new double[signal.Count()];

            int[] lag = new int[signal.Count()];
            for(int i =0; i < lag.Count(); i++)
            {
                lag[i] = i;
            }
            Array.Copy(unsmooth_acf, signal.Count()-1, a, 0, signal.Count());
            Array.Copy(acf, signal.Count()-1, b, 0, signal.Count());



            //now to find primary freq signal! 

            List<int> optima1 = new List<int>();
            List<int> optima2 = new List<int>();
            double[] aa = { -1, 0, 1, 2, 3 };
            double[] dacf = new double[b.Length-1];
            double work = b[0];
            int x = 1;
            int y = 0;
            for (int m = 0; m < dacf.Count(); m++)
            {
                double tmp1 = work;
                work = b[x];
                tmp1 = b[x] - tmp1;
                x++;
                dacf[m] = tmp1;
            }

            for(int i = 1; i< dacf.Count()-1; i++)
            {
                if(dacf[i+1] == 0)
                {
                    dacf[i + 1] = 1.0e-8;
                }

                if (dacf[i - 1] * dacf[i] < 0)
                {
                    if (dacf[i] >0) {
                        optima1.Add(1);
                        optima2.Add(i);
                    }else
                    {
                        optima1.Add(-1);
                        optima2.Add(i);
                    }
                }
            }

            
            if (optima2.Count() == 0) {
                return 0;
            }
            int t_whisk = optima2[1];
            while (b[t_whisk + 1] > b[t_whisk])
            {
                t_whisk += 1;
                if (t_whisk + 1 >= b.Count())
                {
                    break;
                }
            }
            while(b[t_whisk-1] > b[t_whisk])
            {
                t_whisk -= 1;
            }

            double f = 1 / (lag[t_whisk] * T);
            return f;
            //Smooth signal using box-car filter
            //return f;
            //double[] smoothedSignal = BoxCarFilter(signal);
            
            ////Find peaks from smoothed signal
            //int[] peaks = FindPeaks(smoothedSignal);

            //if (peaks.Length < 2)
            //{
            //    return 0;
            //}

            ////Find peaks from original signal
            //int[] rawPeaks = new int[peaks.Length];

            //for (int i = 0; i < peaks.Length; i++)
            //{
            //    rawPeaks[i] = FindClosestPeak(peaks[i], signal);
            //}

            ////Calculate average frames between peaks
            //int peakCounter = 0;
            //double cumulativePeak = 0;

            //for (int i = 1; i < rawPeaks.Length; i++)
            //{
            //    cumulativePeak += Math.Abs(rawPeaks[i] - rawPeaks[i - 1]);
            //    peakCounter++;
            //}

            //double averageFramesBetweenPeak = cumulativePeak / peakCounter;

            //return (frameRate / frameInterval) / averageFramesBetweenPeak;
        }

        private int[] FindPeaks(double[] smoothedSignal, int range = 2)
        {
            List<int> peaks = new List<int>();

            for (int i = range; i < smoothedSignal.Length - range; i++)
            {
                double currentValue = smoothedSignal[i];

                bool isPeak = true;

                for (int j = -range; j <= range; j++)
                {
                    if (j == 0)
                    {
                        continue;
                    }

                    double testValue = smoothedSignal[i - j];

                    if (testValue > currentValue)
                    {
                        isPeak = false;
                        break;
                    }
                }

                if (isPeak)
                {
                    peaks.Add(i);
                }
            }

            return peaks.ToArray();
        }

        private double[] BoxCarFilter(double[] signal)
        {
            double[] filter = new double[] { 0.2, 0.2, 0.2, 0.2, 0.2 };
            double[] result = new double[signal.Length];
            double[] borderedSignal = new double[signal.Length + 4];

            for (int i = -2; i < signal.Length + 2; i++)
            {
                if (i < 0)
                {
                    borderedSignal[i + 2] = signal[0];
                }
                else if (i >= signal.Length)
                {
                    borderedSignal[i + 2] = signal.Last();
                }
                else
                {
                    borderedSignal[i + 2] = signal[i];
                }
            }

            for (int i = 2; i < borderedSignal.Length - 2; i++)
            {
                //Start at 2, finish at length - 2
                result[i - 2] = (borderedSignal[i - 2] * filter[0]) + (borderedSignal[i - 1] * filter[1]) + (borderedSignal[i] * filter[2]) + (borderedSignal[i + 1] * filter[3]) + (borderedSignal[i + 2] * filter[4]);
            }

            return result;
        }
        private double[] movingAverage(double[] x, int windowSize)
        {
            
            double[] ma = new double[x.Length];
            int period = windowSize;
            ma[0] = x[0];
            ma[x.Length - 1] = x.Last();
            for(int i =1; i <x.Length-1; i++)
            {
                if (i - ( period/2) < 0 || (i+(period/2) > x.Length-1))
                {
                    period = windowSize - 2;
                    if (i - (period / 2) < 0 || (i + (period / 2) > x.Length-1))
                    {
                        period -= 2;
                        if (i - (period / 2) < 0 || (i + (period / 2) > x.Length-1))
                        {
                            period -= 2;
                        }
                    }
                }
                double total = 0.0;
                for(int j = 0; j<period; j++)
                {
                    int meow = Convert.ToInt32(j - (period / 2));
                    total += x[i + (j-period/2)];
                }
                ma[i] = total / period;
                period = windowSize;
            }

            return ma;
        }
        private int FindClosestPeak(int index, double[] signal)
        {
            double currentValue = signal[index];

            //Go left or right?
            double leftValue;
            double rightValue;

            if (index - 1 < 0)
            {
                rightValue = signal[index + 1];

                if (currentValue > rightValue)
                {
                    //This is a peak already
                    return index;
                }
                else
                {
                    //Scan right
                    return ScanRightForPeak(index, signal);
                }
            }
            else if (index + 1 >= signal.Length)
            {
                leftValue = signal[index - 1];

                if (currentValue > leftValue)
                {
                    //This is a peak already
                    return index;
                }
                else
                {
                    //Scan left
                    return ScanLeftForPeak(index, signal);
                }
            }
            else
            {
                leftValue = signal[index - 1];
                rightValue = signal[index + 1];

                if (currentValue < leftValue && currentValue < rightValue)
                {
                    //We're at a valley, look both ways maybe?
                    int leftIndex = ScanLeftForPeak(index, signal);
                    int rightIndex = ScanRightForPeak(index, signal);

                    int leftDelta = Math.Abs(index - leftIndex);
                    int rightDelta = Math.Abs(index - rightIndex);

                    if (leftDelta <= rightDelta)
                    {
                        return leftIndex;
                    }
                    else
                    {
                        return rightIndex;
                    }
                }
                else if (currentValue < leftValue)
                {
                    //Go left
                    return ScanLeftForPeak(index, signal);
                }
                else if (currentValue < rightValue)
                {
                    //Go right
                    return ScanRightForPeak(index, signal);
                }
                else
                {
                    //We're already at a peak
                    return index;
                }
            }

            return 0;
        }

        private int ScanLeftForPeak(int index, double[] signal)
        {
            int testIndex = index;
            double currentValue = signal[testIndex];
            double testValue = signal[testIndex - 1];

            while (currentValue < testValue)
            {
                testIndex--;

                if (testIndex <= 0)
                {
                    return testIndex;
                }

                currentValue = signal[testIndex];
                testValue = signal[testIndex - 1];
            }

            return testIndex;
        }

        private int ScanRightForPeak(int index, double[] signal)
        {
            int testIndex = index;
            double currentValue = signal[testIndex];
            double testValue = signal[testIndex + 1];

            while (currentValue < testValue)
            {
                testIndex++;

                if (testIndex >= signal.Length - 1)
                {
                    return testIndex;
                }

                currentValue = signal[testIndex];
                testValue = signal[testIndex + 1];
            }

            return testIndex;
        }
    }
}
