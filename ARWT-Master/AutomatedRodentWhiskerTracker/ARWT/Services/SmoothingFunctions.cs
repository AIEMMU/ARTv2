using System;
using System.Collections.Generic;
using System.Linq;
using RobynsWhiskerTracker.Services.Maths;

namespace ARWT.Services
{
    public static class SmoothingFunctions
    {
        public static double[] BoxCarSmooth(double[] signal)
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

        public static double[] BoxCarSmoothLight(double[] signal)
        {
            double[] filter = new double[] { 1d/3d, 1d/3d, 1d/3d };
            double[] result = new double[signal.Length];
            double[] borderedSignal = new double[signal.Length + 2];

            for (int i = -1; i < signal.Length + 1; i++)
            {
                if (i < 0)
                {
                    borderedSignal[i + 1] = signal[0];
                }
                else if (i >= signal.Length)
                {
                    borderedSignal[i + 1] = signal.Last();
                }
                else
                {
                    borderedSignal[i + 1] = signal[i];
                }
            }

            for (int i = 1; i < borderedSignal.Length - 1; i++)
            {
                //Start at 1, finish at length - 1
                result[i - 1] = (borderedSignal[i - 1] * filter[0]) + (borderedSignal[i] * filter[1]) + (borderedSignal[i + 1] * filter[2]);
            }

            return result;
        }

        public static int[] FindPeaks(double[] smoothedSignal, int range = 2)
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

        public static int[][] FindPeaksAndValleys(double[] smoothedSignal, int range = 3)
        {
            List<int> peaks = new List<int>();
            List<int> valleys = new List<int>();

            for (int i = range; i < smoothedSignal.Length - range; i++)
            {
                double currentValue = smoothedSignal[i];

                bool isPeak = true;
                bool isValley = true;

                for (int j = -range; j <= range; j++)
                {
                    if (j == 0)
                    {
                        continue;
                    }

                    double testValue = smoothedSignal[i - j];

                    if (isPeak && testValue > currentValue)
                    {
                        isPeak = false;

                        if (!isValley)
                        {
                            break;
                        }
                    }

                    if (isValley && testValue < currentValue)
                    {
                        isValley = false;

                        if (!isPeak)
                        {
                            break;
                        }
                    }
                }

                if (isPeak)
                {
                    peaks.Add(i);
                }

                if (isValley)
                {
                    valleys.Add(i);
                }
            }

            return new [] { peaks.ToArray(), valleys.ToArray() };
        }

        public static int FindClosestPeak(int index, double[] signal)
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

        private static int ScanLeftForPeak(int index, double[] signal)
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

        private static int ScanRightForPeak(int index, double[] signal)
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

        //---------------------------

        public static int FindClosestValley(int index, double[] signal)
        {
            double currentValue = signal[index];

            //Go left or right?
            double leftValue;
            double rightValue;

            if (index - 1 < 0)
            {
                rightValue = signal[index + 1];

                if (currentValue < rightValue)
                {
                    //This is a valley already
                    return index;
                }
                else
                {
                    //Scan right
                    return ScanRightForValley(index, signal);
                }
            }
            else if (index + 1 >= signal.Length)
            {
                leftValue = signal[index - 1];

                if (currentValue < leftValue)
                {
                    //This is a valley already
                    return index;
                }
                else
                {
                    //Scan left
                    return ScanLeftForValley(index, signal);
                }
            }
            else
            {
                leftValue = signal[index - 1];
                rightValue = signal[index + 1];

                if (currentValue > leftValue && currentValue > rightValue)
                {
                    //We're at a peak, look both ways
                    int leftIndex = ScanLeftForValley(index, signal);
                    int rightIndex = ScanRightForValley(index, signal);

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
                else if (currentValue > leftValue)
                {
                    //Go left
                    return ScanLeftForValley(index, signal);
                }
                else if (currentValue > rightValue)
                {
                    //Go right
                    return ScanRightForValley(index, signal);
                }
                else
                {
                    //We're already at a peak
                    return index;
                }
            }

            return 0;
        }

        private static int ScanLeftForValley(int index, double[] signal)
        {
            int testIndex = index;
            double currentValue = signal[testIndex];
            double testValue = signal[testIndex - 1];

            while (currentValue > testValue)
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

        private static int ScanRightForValley(int index, double[] signal)
        {
            int testIndex = index;
            double currentValue = signal[testIndex];
            double testValue = signal[testIndex + 1];

            while (currentValue > testValue)
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

        public static int FindClosestPeak(double[] data, int location)
        {
            if (location < 0 || location >= data.Length)
            {
                return -1;
            }

            double currentValue = data[location];

            //Look left or right?
            if (location - 1 < 0)
            {
                if (data[location + 1] < currentValue)
                {
                    return location;
                }

                //Look right
                return ScanRightForPeak(location, data);
            }
            else if (location + 1 >= data.Length)
            {
                if (data[location - 1] < currentValue)
                {
                    return location;
                }

                //Look left
                return ScanLeftForPeak(location, data);
            }
            else
            {
                if (data[location - 1] < currentValue && data[location + 1] < currentValue)
                {
                    //Already at a peak
                    return location;
                }
                else if (data[location - 1] < currentValue)
                {
                    //Look right
                    return ScanRightForPeak(location, data);
                }
                else
                {
                    //Look left
                    return ScanLeftForPeak(location, data);
                }
            }
        }

        public static int FindClosestValley(double[] data, int location)
        {
            if (location < 0 || location >= data.Length)
            {
                return -1;
            }

            //Look left or right?
            if (location - 1 < 0)
            {
                //Look right
                return ScanRightForValley(location, data);
            }
            else if (location + 1 >= data.Length)
            {
                //Look left
                return ScanLeftForValley(location, data);
            }
            else
            {
                double currentValue = data[location];

                if (data[location - 1] < currentValue && data[location + 1] < currentValue)
                {
                    //Already at a peak
                    return location;
                }
                else if (data[location - 1] < currentValue)
                {
                    //Look right
                    return ScanRightForValley(location, data);
                }
                else
                {
                    //Look left
                    return ScanLeftForValley(location, data);
                }
            }
        }

        public static double GetRMS(double[] signal)
        {
            if (signal == null)
            {
                return 0;
            }

            if (signal.Length == 0)
            {
                return 0;
            }



            double[] squared = new double[signal.Length];
            
            for (int i = 0; i < squared.Length; i++)
            {
                squared[i] = Math.Pow(signal[i], 2);
            }

            double squaredAverage = squared.Average();

            return Math.Sqrt(squaredAverage);
        }

        public static double GetSTD(double[] signal)
        {
            if (signal == null)
            {
                return 0;
            }

            if (signal.Length == 0)
            {
                return 0;
            }

            double[] zeroMeaned = BrettFFT.ZeroAverageSignal(signal);

            double[] zeroMeanedSquared = new double[zeroMeaned.Length];

            for (int i = 0; i < zeroMeaned.Length; i++)
            {
                zeroMeanedSquared[i] = Math.Pow(zeroMeaned[i], 2);
            }

            double squaredAverage = zeroMeanedSquared.Average();
            var std = zeroMeanedSquared.Sum();
            std = (1.0 / zeroMeanedSquared.Length) * std;
            std = Math.Sqrt(std);
            return std;
        }
    }
}
