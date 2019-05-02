using System.Linq;

namespace ARWT.Model.MWA
{
    internal class ClipSettings : ModelObjectBase, IClipSettings
    {
        private string m_ClipFilePath;
        private int m_StartFrame;
        private int m_EndFrame;
        private int m_FrameInterval;
        private int m_NumberOfWhiskers;
        private int m_NumberOfPointsPerWhisker;
        private bool m_IncludeNosePoint;
        private bool m_IncludeOrientationPoint;
        private int m_NumberOfGenericPoints;
        private IWhiskerClipSettings[] m_Whiskers;

        public string ClipFilePath
        {
            get
            {
                return m_ClipFilePath;
            }
            set
            {
                if (Equals(m_ClipFilePath, value))
                {
                    return;
                }

                m_ClipFilePath = value;

                MarkAsDirty();
            }
        }

        public int StartFrame
        {
            get
            {
                return m_StartFrame;
            }
            set
            {
                if (Equals(m_StartFrame, value))
                {
                    return;
                }

                m_StartFrame = value;

                MarkAsDirty();
            }
        }

        public int EndFrame
        {
            get
            {
                return m_EndFrame;
            }
            set
            {
                if (Equals(m_EndFrame, value))
                {
                    return;
                }

                m_EndFrame = value;

                MarkAsDirty();
            }
        }

        public int FrameInterval
        {
            get
            {
                return m_FrameInterval;
            }
            set
            {
                if (Equals(m_FrameInterval, value))
                {
                    return;
                }

                m_FrameInterval = value;

                MarkAsDirty();
            }
        }

        public int NumberOfWhiskers
        {
            get
            {
                return m_NumberOfWhiskers;
            }
            set
            {
                if (Equals(m_NumberOfWhiskers, value))
                {
                    return;
                }

                m_NumberOfWhiskers = value;

                MarkAsDirty();
            }
        }
        public int NumberOfPointsPerWhisker
        {
            get
            {
                return m_NumberOfPointsPerWhisker;
            }
            set
            {
                if (Equals(m_NumberOfPointsPerWhisker, value))
                {
                    return;
                }

                m_NumberOfPointsPerWhisker = value;

                MarkAsDirty();
            }
        }

        public bool IncludeNosePoint
        {
            get
            {
                return m_IncludeNosePoint;
            }
            set
            {
                if (Equals(m_IncludeNosePoint, value))
                {
                    return;
                }

                m_IncludeNosePoint = value;

                if (!value)
                {
                    IncludeOrientationPoint = false;
                }

                MarkAsDirty();
            }
        }

        public bool IncludeOrientationPoint
        {
            get
            {
                return m_IncludeOrientationPoint;
            }
            set
            {
                if (Equals(m_IncludeOrientationPoint, value))
                {
                    return;
                }

                m_IncludeOrientationPoint = value;

                MarkAsDirty();
            }
        }

        public int NumberOfGenericPoints
        {
            get
            {
                return m_NumberOfGenericPoints;
            }
            set
            {
                if (Equals(m_NumberOfGenericPoints, value))
                {
                    return;
                }

                m_NumberOfGenericPoints = value;

                MarkAsDirty();
            }
        }

        public int TotalNumberOfPoints
        {
            get
            {
                return Whiskers.Sum(whisker => whisker.NumberOfPoints);
            }
        }

        public IWhiskerClipSettings[] Whiskers
        {
            get
            {
                return m_Whiskers;
            }
            set
            {
                m_Whiskers = value;
            }
        }

        public IWhisker[] CreateEmptyWhiskers(IMouseFrame mouseFrame)
        {
            return Whiskers.Select(x => x.CreateWhisker(mouseFrame)).ToArray();
        }
    }
}
