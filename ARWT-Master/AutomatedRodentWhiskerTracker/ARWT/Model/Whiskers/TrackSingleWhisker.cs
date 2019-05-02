using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARWT.Comparer;
using ARWT.ModelInterface.Whiskers;

namespace ARWT.Model.Whiskers
{
    internal class TrackSingleWhisker : ModelObjectBase, ITrackSingleWhisker
    {
        private static double DistanceThreshold = 4;
        private static double AngleThreshold = 10;

        private Point m_Center;
        public Point Center
        {
            get
            {
                return m_Center;
            }
            private set
            {
                if (Equals(m_Center, value))
                {
                    return;
                }

                m_Center = value;

                MarkAsDirty();
            }
        }

        private double m_Angle;
        public double Angle
        {
            get
            {
                return m_Angle;
            }
            private set
            {
                if (Equals(m_Angle, value))
                {
                    return;
                }

                m_Angle = value;

                MarkAsDirty();
            }
        }

        private IWhiskerSegment m_CurrentWhisker;
        public IWhiskerSegment CurrentWhisker
        {
            get
            {
                return m_CurrentWhisker;
            }
            set
            {
                if (Equals(m_CurrentWhisker, value))
                {
                    return;
                }

                m_CurrentWhisker = value;

                MarkAsDirty();
            }
        }

        private int m_MissingFrameCount = 1;
        public int MissingFrameCount
        {
            get
            {
                return m_MissingFrameCount;
            }
            set
            {
                if (Equals(m_MissingFrameCount, value))
                {
                    return;
                }

                m_MissingFrameCount = value;

                MarkAsDirty();
            }
        }

        private int m_PositionId;
        public int PositionId
        {
            get
            {
                return m_PositionId;
            }
            set
            {
                if (Equals(m_PositionId, value))
                {
                    return;
                }

                m_PositionId = value;

                MarkAsDirty();
            }
        }



        private static int GlobalId = 0;
        private Color m_Color;
        public Color Color
        {
            get
            {
                return m_Color;
            }
            set
            {
                if (Equals(m_Color, value))
                {
                    return;
                }

                m_Color = value;

                MarkAsDirty();
            }
        }

        private Dictionary<int, IWhiskerSegment> m_WhiskerList = new Dictionary<int, IWhiskerSegment>();
        public Dictionary<int, IWhiskerSegment> WhiskerList
        {
            get
            {
                return m_WhiskerList;
            }
            set
            {
                if (Equals(m_WhiskerList, value))
                {
                    return;
                }

                m_WhiskerList = value;

                MarkAsDirty();
            }
        }

        private static Dictionary<int, Color> Colors = new Dictionary<int, Color>();

        static TrackSingleWhisker()
        {
            Colors.Add(0, Color.Red);
            Colors.Add(1, Color.Yellow);
            Colors.Add(2, Color.Green);
            Colors.Add(3, Color.Blue);
            Colors.Add(4, Color.Purple);
            Colors.Add(5, Color.Aqua);
            Colors.Add(6, Color.GreenYellow);
            Colors.Add(7, Color.Violet);
            Colors.Add(8, Color.DarkSalmon);
            Colors.Add(9, Color.Lime);
        }

        public TrackSingleWhisker()
        {
            Color = Colors[GlobalId];
            GlobalId++;

            if (GlobalId == Colors.Count)
            {
                GlobalId = 0;
            }
        }

        public void Initialise(IWhiskerSegment whisker)
        {
            CurrentWhisker = whisker;
        }
        
        public void FindPotentialWhisker(int frameNumber, IEnumerable<IWhiskerSegment> whiskers)
        {
            if (CurrentWhisker == null)
            {
                throw new Exception("Must initalise whisker before finding other potential ones");
            }
            
            SortedList<double, IWhiskerSegment> closeWhiskers = new SortedList<double, IWhiskerSegment>(new DuplicateKeyComparer<double>());
            foreach (IWhiskerSegment whisker in whiskers)
            {
                //Find closest
                double cDist = CurrentWhisker.Distance(whisker);
                if (cDist < (DistanceThreshold * MissingFrameCount))
                {
                    closeWhiskers.Add(cDist, whisker);
                }
            }

            //No close whiskers found
            if (!closeWhiskers.Any())
            {
                MissingFrameCount++;
                return;
            }

            IWhiskerSegment bestWhisker = null;
            for (int i = 0; i < closeWhiskers.Count; i++)
            {
                if (CurrentWhisker.DeltaAngle(closeWhiskers.Values[i]) < (AngleThreshold* MissingFrameCount))
                {
                    //Found it!
                    bestWhisker = closeWhiskers.Values[i];
                    break;
                }
            }

            if (bestWhisker == null)
            {
                MissingFrameCount++;
                return;
            }

            bestWhisker.Color = Color;
            WhiskerList.Add(frameNumber, bestWhisker);
            CurrentWhisker = bestWhisker;
            MissingFrameCount = 1;
        }

        public Dictionary<int, IWhiskerSegment> GetWhiskers()
        {
            return WhiskerList;
        } 

        public void ClearLists()
        {
            WhiskerList = new Dictionary<int, IWhiskerSegment>();
        }

        public void ResetMissingFrameCount()
        {
            //int currentMax = Whi
            //MissingFrameCount = 1;
        }
    }
}
