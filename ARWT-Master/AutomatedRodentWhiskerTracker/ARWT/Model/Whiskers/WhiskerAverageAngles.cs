using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARWT.ModelInterface.Results;
using ARWT.ModelInterface.Whiskers;
using ARWT.Services;

namespace ARWT.Model.Whiskers
{
    internal class WhiskerAverageAngles : ModelObjectBase, IWhiskerAverageAngles
    {
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

                MarkAsDirty();
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

                MarkAsDirty();
            }
        }
        public double[] GetWhiskerSpread(Dictionary<int, ISingleFrameExtendedResults> results, bool onlyForInteracting = false)
        {

            int counter = 0;
            double[] spread = new double[2];
            Dictionary<int, double> leftWhiskerAngles = new Dictionary<int, double>();
            Dictionary<int, double> rightWhiskerAngles = new Dictionary<int, double>();
            for (int i = StartFrame; i < EndFrame; i++)
            {
                var result = results.ElementAt(i);

                if (result.Value == null)
                {
                    counter++;
                    continue;
                }

                if (onlyForInteracting && !result.Value.IsInteracting)
                {
                    counter++;
                    continue;
                }

                if (result.Value.BestTrackedWhisker == null)
                {
                    double langle;
                    int lmin, lmax;
                    bool lfound = GetAverageAngleForleftWhiskersGap(results, counter, out langle, out lmin, out lmax);
                    if (lfound)
                    {
                        leftWhiskerAngles.Add(result.Key, 0);
                    }

                    double angle;
                    int min, max;
                    bool found = GetAverageAngleForRightWhiskersGap(results, counter, out angle, out min, out max);
                    if (found)
                    {
                        rightWhiskerAngles.Add(result.Key, 0);
                    }
                    counter++;
                    continue;
                }

                if (result.Value.BestTrackedWhisker.LeftWhiskers == null || !result.Value.BestTrackedWhisker.LeftWhiskers.Any())
                {
                    double angle;
                    int min, max;
                    bool found = GetAverageAngleForleftWhiskersGap(results, counter, out angle, out min, out max);
                    if (found)
                    {
                        leftWhiskerAngles.Add(result.Key, 0);
                    }
                }
                else
                {
                    double[] angles_ = result.Value.BestTrackedWhisker.LeftWhiskers.Select(x => x.Angle).ToArray();
                    double std = SmoothingFunctions.GetSTD(angles_);
                    leftWhiskerAngles.Add(result.Key, std);
                }

                if (result.Value.BestTrackedWhisker.RightWhiskers == null || !result.Value.BestTrackedWhisker.RightWhiskers.Any())
                {
                    double angle;
                    int min, max;
                    bool found = GetAverageAngleForRightWhiskersGap(results, counter, out angle, out min, out max);
                    if (found)
                    {
                        rightWhiskerAngles.Add(result.Key, 0);
                    }
                }
                else
                {
                    double[] angles_ = result.Value.BestTrackedWhisker.RightWhiskers.Select(x => x.Angle).ToArray();
                    double std = SmoothingFunctions.GetSTD(angles_);
                    rightWhiskerAngles.Add(result.Key, std);
                }

                counter++;
            }
            
            spread[0] = leftWhiskerAngles.Count == 0 ? 0 : leftWhiskerAngles.Average(x => x.Value);
            spread[1] = rightWhiskerAngles.Count ==0 ? 0 : rightWhiskerAngles.Average(x => x.Value);
            return spread;
        }



        public Dictionary<int, double>[] GetWhiskerAngles(Dictionary<int, ISingleFrameExtendedResults> results, bool onlyForInteracting = false)
        {
            int counter = 0;
            Dictionary<int, double> leftWhiskerAngles = new Dictionary<int, double>();
            Dictionary<int, double> rightWhiskerAngles = new Dictionary<int, double>();
            for(int i = StartFrame; i < EndFrame; i++)
            {
                var result = results.ElementAt(i);

                if (result.Value == null)
                {
                    counter++;
                    continue;
                }

                if (onlyForInteracting && !result.Value.IsInteracting)
                {
                    counter++;
                    continue;
                }

                if (result.Value.BestTrackedWhisker == null)
                {
                    double langle;
                    int lmin, lmax;
                    bool lfound = GetAverageAngleForleftWhiskersGap(results, counter, out langle, out lmin, out lmax);
                    if (lfound)
                    {
                        leftWhiskerAngles.Add(result.Key, langle);
                    }

                    double angle;
                    int min, max;
                    bool found = GetAverageAngleForRightWhiskersGap(results, counter, out angle, out min, out max);
                    if (found)
                    {
                        rightWhiskerAngles.Add(result.Key, angle);
                    }
                    counter++;
                    continue;
                }
                
                if (result.Value.BestTrackedWhisker.LeftWhiskers == null || !result.Value.BestTrackedWhisker.LeftWhiskers.Any())
                {
                    double angle;
                    int min, max;
                    bool found = GetAverageAngleForleftWhiskersGap(results, counter, out angle, out min, out max);
                    if (found)
                    {
                        leftWhiskerAngles.Add(result.Key, angle);
                    }
                }
                else
                {
                    leftWhiskerAngles.Add(result.Key, result.Value.BestTrackedWhisker.LeftWhiskers.Average(x => x.Angle));
                }
                
                if (result.Value.BestTrackedWhisker.RightWhiskers == null || !result.Value.BestTrackedWhisker.RightWhiskers.Any())
                {
                    double angle;
                    int min, max;
                    bool found = GetAverageAngleForRightWhiskersGap(results, counter, out angle, out min, out max);
                    if (found)
                    {
                        rightWhiskerAngles.Add(result.Key, angle);
                    }
                }
                else
                {
                    rightWhiskerAngles.Add(result.Key, result.Value.BestTrackedWhisker.RightWhiskers.Average(x => x.Angle));
                }

                counter++;
            }

            return new[] {leftWhiskerAngles, rightWhiskerAngles};
        }

        private bool GetAverageAngleForleftWhiskersGap(Dictionary<int, ISingleFrameExtendedResults> results, int index, out double angle, out int minIndex, out int maxIndex)
        {
            int min = -1, max = -1;
            bool minFound = false, maxFound = false;

            //Find minIndex
            for (int i = index - 1; i > 0; i--)
            {
                if (results[i].BestTrackedWhisker != null && results[i].BestTrackedWhisker.LeftWhiskers != null && results[i].BestTrackedWhisker.LeftWhiskers.Any())
                {
                    min = i;
                    minFound = true;
                    break;
                }
            }

            //Find maxIndex
            for (int i = index + 1; i < results.Count; i++)
            {
                if (results[i].BestTrackedWhisker != null && results[i].BestTrackedWhisker.LeftWhiskers != null && results[i].BestTrackedWhisker.LeftWhiskers.Any())
                {
                    max = i;
                    maxFound = true;
                    break;
                }
            }

            if (!maxFound || !minFound)
            {
                //Can't interpolate
                angle = -1;
                minIndex = -1;
                maxIndex = -1;
                return false;
            }

            minIndex = min;
            maxIndex = max;

            double minAngle = results[minIndex].BestTrackedWhisker.LeftWhiskers.Select(x => x.Angle).Average();
            double maxAngle = results[maxIndex].BestTrackedWhisker.LeftWhiskers.Select(x => x.Angle).Average();

            int deltaIndex = index - minIndex;
            int deltaMaxIndex = maxIndex - minIndex;

            double variation = (double)deltaIndex / deltaMaxIndex;
            double angleVariation = maxAngle - minAngle;
            angle = minAngle + (variation * angleVariation);
            return true;
        }

        private bool GetAverageAngleForRightWhiskersGap(Dictionary<int, ISingleFrameExtendedResults> results, int index, out double angle, out int minIndex, out int maxIndex)
        {
            int min = -1, max = -1;
            bool minFound = false, maxFound = false;

            //Find minIndex
            for (int i = index - 1; i > 0; i--)
            {
                if (results[i].BestTrackedWhisker != null && results[i].BestTrackedWhisker.RightWhiskers != null && results[i].BestTrackedWhisker.RightWhiskers.Any())
                {
                    min = i;
                    minFound = true;
                    break;
                }
            }

            for (int i = index + 1; i < results.Count; i++)
            {
                if (results[i].BestTrackedWhisker != null && results[i].BestTrackedWhisker.RightWhiskers != null && results[i].BestTrackedWhisker.RightWhiskers.Any())
                {
                    max = i;
                    maxFound = true;
                    break;
                }
            }

            if (!maxFound || !minFound)
            {
                //Can't interpolate
                angle = -1;
                minIndex = -1;
                maxIndex = -1;
                return false;
            }

            minIndex = min;
            maxIndex = max;

            double minAngle = results[minIndex].BestTrackedWhisker.RightWhiskers.Select(x => x.Angle).Average();
            double maxAngle = results[maxIndex].BestTrackedWhisker.RightWhiskers.Select(x => x.Angle).Average();

            int deltaIndex = index - minIndex;
            int deltaMaxIndex = maxIndex - minIndex;

            double variation = (double)deltaIndex / deltaMaxIndex;
            double angleVariation = maxAngle - minAngle;
            angle = minAngle + (variation * angleVariation);
            return true;
        }
    }
}
