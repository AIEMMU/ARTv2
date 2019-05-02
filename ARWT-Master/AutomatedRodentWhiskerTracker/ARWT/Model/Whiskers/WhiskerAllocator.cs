using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Emgu.CV.Structure;
using ARWT.Comparer;
using ARWT.Extensions;
using ARWT.ModelInterface.Whiskers;
using ARWT.Resolver;

namespace ARWT.Model.Whiskers
{
    internal class WhiskerAllocator : ModelObjectBase, IWhiskerAllocator
    {
        public ITrackSingleWhisker[] InitialiseWhiskers(IEnumerable<IWhiskerSegment> currentWhiskers, PointF nosePoint, PointF midPoint)
        {
            Vector forwardVec = new Vector(nosePoint.X - midPoint.X, nosePoint.Y - midPoint.Y);

            //Extend vector forwards and backwards
            double extensionFactor = 10;
            double forwardX = nosePoint.X + (forwardVec.X*extensionFactor);
            double forwardY = nosePoint.Y + (forwardVec.Y*extensionFactor);
            double rearX = midPoint.X - (forwardVec.X*extensionFactor);
            double rearY = midPoint.Y - (forwardVec.Y*extensionFactor);

            System.Windows.Point newForwardPoint = new System.Windows.Point(forwardX, forwardY);
            System.Windows.Point newRearPoint = new System.Windows.Point(rearX, rearY);

            SortedList<double, IWhiskerSegment> sortedList = new SortedList<double, IWhiskerSegment>(new DuplicateKeyComparer<double>());

            foreach (IWhiskerSegment whisker in currentWhiskers)
            {
                System.Windows.Point centerPoint = GetCenterPoint(whisker.Line);
                System.Windows.Point linePoint;
                MathExtension.FindDistanceToSegmentSquared(centerPoint, newForwardPoint, newRearPoint, out linePoint);

                double dist = linePoint.DistanceSquared(nosePoint);

                sortedList.Add(dist, whisker);
            }

            ITrackSingleWhisker[] trackedWhiskers = new ITrackSingleWhisker[sortedList.Count];
            for (int i = 0; i < sortedList.Count; i++)
            {
                ITrackSingleWhisker singleWhisker = ModelResolver.Resolve<ITrackSingleWhisker>();
                singleWhisker.Initialise(sortedList.Values[i]);
                singleWhisker.PositionId = i;
                trackedWhiskers[i] = singleWhisker;
            }

            return trackedWhiskers;
        }

        private System.Windows.Point GetCenterPoint(LineSegment2D line)
        {
            return new System.Windows.Point((line.P1.X + line.P2.X)/2d, (line.P1.Y + line.P2.Y)/2d);
        }

        public Dictionary<IWhiskerSegment, ITrackSingleWhisker> AllocateWhiskers(int frameNumber, IEnumerable<ITrackSingleWhisker> singleWhiskers, IEnumerable<IWhiskerSegment> currentWhiskers, PointF nosePoint, PointF midPoint)
        {
            List<ITrackSingleWhisker> whiskers = singleWhiskers.ToList();
            List<HolderClass> holders = new List<HolderClass>();
            Vector forwardVec = new Vector(nosePoint.X - midPoint.X, nosePoint.Y - midPoint.Y);
            double extensionFactor = 10;
            double forwardX = nosePoint.X + (forwardVec.X * extensionFactor);
            double forwardY = nosePoint.Y + (forwardVec.Y * extensionFactor);
            double rearX = midPoint.X - (forwardVec.X * extensionFactor);
            double rearY = midPoint.Y - (forwardVec.Y * extensionFactor);

            System.Windows.Point newForwardPoint = new System.Windows.Point(forwardX, forwardY);
            System.Windows.Point newRearPoint = new System.Windows.Point(rearX, rearY);
            foreach (IWhiskerSegment foundWhisker in currentWhiskers)
            {
                //Find most likely tracked whisker
                foreach (ITrackSingleWhisker tWhisker in whiskers)
                {
                    double dist = tWhisker.CurrentWhisker.Distance(foundWhisker);
                    double angle = tWhisker.CurrentWhisker.DeltaAngle(foundWhisker);

                    if (dist < (6*tWhisker.MissingFrameCount) && angle < (20*tWhisker.MissingFrameCount))
                    {
                        HolderClass holder = new HolderClass();
                        holder.Whisker = foundWhisker;
                        holder.TrackedWhisker = tWhisker;
                        holder.Score = dist;
                        holder.PotentialTracks.Add(tWhisker, dist);

                        System.Windows.Point centerPoint = GetCenterPoint(foundWhisker.Line);
                        System.Windows.Point linePoint;
                        MathExtension.FindDistanceToSegmentSquared(centerPoint, newForwardPoint, newRearPoint, out linePoint);

                        double forwardVecDist = linePoint.DistanceSquared(nosePoint);

                        holder.DistanceFromNose = forwardVecDist;

                        holders.Add(holder);
                    }
                }
            }

            //We now have every whisker associated with a tracked whisker, need to find lowest values
            //HolderClass[] newHolders = holders.OrderBy(x => x.Score).ThenBy(x => x.DistanceFromNose).ToArray();


            List<ITrackSingleWhisker> usedTrackedWhiskers = new List<ITrackSingleWhisker>();
            List<IWhiskerSegment> usedWhiskers = new List<IWhiskerSegment>();
            Dictionary<IWhiskerSegment, ITrackSingleWhisker> finalWhiskers = new Dictionary<IWhiskerSegment, ITrackSingleWhisker>();
            

            int maxPos = singleWhiskers.Select(x => x.PositionId).Max();
            double currentDistFromNose = 0;
            for (int currentPos = 0; currentPos <= maxPos; currentPos++)
            {
                //Get holders for position index
                IEnumerable<HolderClass> bestHolders = holders.Where(x => x.TrackedWhisker.PositionId == currentPos && x.DistanceFromNose > currentDistFromNose).OrderBy(x => x.Score);

                if (!bestHolders.Any())
                {
                    continue;
                }

                HolderClass bestHolder = bestHolders.First();

                currentDistFromNose = bestHolder.DistanceFromNose;

                ITrackSingleWhisker tSingle = bestHolder.TrackedWhisker;
                IWhiskerSegment tWhisker = bestHolder.Whisker;

                if (usedTrackedWhiskers.Contains(tSingle))
                {
                    continue;
                }

                if (usedWhiskers.Contains(tWhisker))
                {
                    continue;
                }

                finalWhiskers.Add(tWhisker, tSingle);
                tWhisker.Color = tSingle.Color;
                usedTrackedWhiskers.Add(tSingle);
                usedWhiskers.Add(tWhisker);
                tSingle.MissingFrameCount = 1;
            }

            //for (int i = 0; i < newHolders.Length; i++)
            //{
            //    HolderClass cHolder = newHolders[i];
            //    ITrackSingleWhisker tSingle = cHolder.TrackedWhisker;

            //    IWhiskerSegment tWhisker = cHolder.Whisker;
            //    if (usedTrackedWhiskers.Contains(tSingle))
            //    {
            //        continue;
            //    }

            //    if (usedWhiskers.Contains(tWhisker))
            //    {
            //        continue;
            //    }

            //    finalWhiskers.Add(tWhisker, tSingle);
            //    tWhisker.Color = tSingle.Color;
            //    usedTrackedWhiskers.Add(tSingle);
            //    usedWhiskers.Add(tWhisker);
            //    tSingle.MissingFrameCount = 1;
            //}

            foreach (ITrackSingleWhisker tWhisker in whiskers)
            {
                if (!usedTrackedWhiskers.Contains(tWhisker))
                {
                    tWhisker.MissingFrameCount++;
                }
            }

            return finalWhiskers;
        }

        private class HolderClass
        {
            public IWhiskerSegment Whisker
            {
                get;
                set;
            }

            public ITrackSingleWhisker TrackedWhisker
            {
                get;
                set;
            }

            public double Score
            {
                get;
                set;
            }

            public double DistanceFromNose
            {
                get;
                set;
            }

            public Dictionary<ITrackSingleWhisker, double> PotentialTracks = new Dictionary<ITrackSingleWhisker, double>();

            public List<KeyValuePair<ITrackSingleWhisker, double>> GetSortedDictionary()
            {
                var myList = PotentialTracks.ToList();

                myList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                return myList;
            } 
        }
    }
}
