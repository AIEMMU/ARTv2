namespace ARWT.Model.MWA
{
    internal class FrameRateSettings : SettingsBase, IFrameRateSettings
    {
        private double m_OriginalFrameRate;
        private double m_CurrentFrameRate;
        private double m_ModifierRatio;

        public double OriginalFrameRate
        {
            get
            {
                return m_OriginalFrameRate;
            }
            set
            {
                if (Equals(m_OriginalFrameRate, value))
                {
                    return;
                }

                m_OriginalFrameRate = value;
                CalculateModifierRatio();

                MarkAsDirty();                
            }
        }

        public double CurrentFrameRate
        {
            get
            {
                return m_CurrentFrameRate;
            }
            set
            {
                if (Equals(m_CurrentFrameRate, value))
                {
                    return;
                }

                m_CurrentFrameRate = value;

                MarkAsDirty();                
            }
        }

        public double ModifierRatio
        {
            get
            {
                return m_ModifierRatio;
            }
            set
            {
                if (Equals(m_ModifierRatio, value))
                {
                    return;
                }

                m_ModifierRatio = value;
                CalculateOriginalFrameRate();

                MarkAsDirty();                
            }
        }

        private void CalculateModifierRatio()
        {
            if (OriginalFrameRate == 0 || CurrentFrameRate == 0)
            {
                return;
            }

            m_ModifierRatio = OriginalFrameRate / CurrentFrameRate;
        }

        private void CalculateOriginalFrameRate()
        {
            if (ModifierRatio == 0 || CurrentFrameRate == 0)
            {
                return;
            }

            m_OriginalFrameRate = CurrentFrameRate * ModifierRatio;
        }

        public override void LoadSettings()
        {
            DataLoadComplete();
        }

        public override void SaveSettings()
        {
            DataLoadComplete();
        }
    }
}
