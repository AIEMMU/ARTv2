using ARWT.Resolver;

namespace ARWT.Model.MWA
{
    internal class WhiskerClipSettings : ModelObjectBase, IWhiskerClipSettings
    {
        private int m_WhiskerId;
        private int m_NumberOfPoints;
        private string m_WhiskerName;
        private bool m_IsGenericPoint;

        public int WhiskerId
        {
            get
            {
                return m_WhiskerId;
            }
            set
            {
                if (Equals(m_WhiskerId, value))
                {
                    return;
                }

                m_WhiskerId = value;

                MarkAsDirty();
            }
        }

        public int NumberOfPoints
        {
            get
            {
                return m_NumberOfPoints;
            }
            set
            {
                if (Equals(m_NumberOfPoints, value))
                {
                    return;
                }

                m_NumberOfPoints = value;

                MarkAsDirty();
            }
        }

        public string WhiskerName
        {
            get
            {
                return m_WhiskerName;
            }
            set
            {
                if (Equals(m_WhiskerName, value))
                {
                    return;
                }

                m_WhiskerName = value;

                MarkAsDirty();
            }
        }

        public bool IsGenericPoint
        {
            get
            {
                return m_IsGenericPoint;
            }
            set
            {
                if (Equals(m_IsGenericPoint, value))
                {
                    return;
                }

                m_IsGenericPoint = value;

                MarkAsDirty();
            }
        }

        public IWhisker CreateWhisker(IMouseFrame mouseFrame)
        {
            IWhisker whisker = ModelResolver.Resolve<IWhisker>();

            whisker.Parent = mouseFrame;
            whisker.WhiskerId = WhiskerId;

            IWhiskerPoint[] whiskerPoints = new IWhiskerPoint[NumberOfPoints];
            whisker.WhiskerPoints = new IWhiskerPoint[NumberOfPoints];
            for (int i = 0; i < NumberOfPoints; i++)
            {
                IWhiskerPoint whiskerPoint = ModelResolver.Resolve<IWhiskerPoint>();
                whiskerPoint.Parent = whisker;
                whiskerPoint.PointId = i + 1;
                whiskerPoints[i] = whiskerPoint;
            }

            whisker.WhiskerPoints = whiskerPoints;
            whisker.IsGenericPoint = IsGenericPoint;
            whisker.WhiskerName = WhiskerName;

            return whisker;
        }
    }
}
