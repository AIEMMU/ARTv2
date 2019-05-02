using System;

namespace ARWT.Model.Analysis
{
    internal abstract class ProtractionRetractionBase : ModelObjectBase
    {
        private double m_MinAngle;
        private double m_MaxAngle;
        private double m_DeltaTime;
        private double m_MeanAngularVelocity;

        public abstract string Name
        {
            get;
        }

        public double MinAngle
        {
            get
            {
                return m_MinAngle;
            }
            private set
            {
                if (Equals(m_MinAngle, value))
                {
                    return;
                }

                m_MinAngle = value;

                MarkAsDirty();
            }
        }

        public double MaxAngle
        {
            get
            {
                return m_MaxAngle;
            }
            private set
            {
                if (Equals(m_MaxAngle, value))
                {
                    return;
                }

                m_MaxAngle = value;

                MarkAsDirty();
            }
        }

        public double DeltaTime
        {
            get
            {
                return m_DeltaTime;
            }
            private set
            {
                if (Equals(m_DeltaTime, value))
                {
                    return;
                }

                m_DeltaTime = value;

                MarkAsDirty();
            }
        }

        public double MeanAngularVelocity
        {
            get
            {
                return m_MeanAngularVelocity;
            }
        }

        public double Amplitude
        {
            get
            {
                return Math.Abs(MaxAngle - MinAngle);
            }
        }

        public virtual void UpdateData(double minAngle, double maxAngle, double deltaTime)
        {
            MinAngle = minAngle;
            MaxAngle = maxAngle;
            DeltaTime = deltaTime;
            m_MeanAngularVelocity = CalculateMean();
        }

        protected abstract double CalculateMean();
    }
}
