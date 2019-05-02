namespace ARWT.Model.MWA
{
    internal class UnitSettings : SettingsBase, IUnitSettings
    {
        private double m_UnitsPerPixel;
        private string m_UnitsName;

        public double UnitsPerPixel
        {
            get
            {
                return m_UnitsPerPixel;
            }
            set
            {
                if (Equals(m_UnitsPerPixel, value))
                {
                    return;
                }

                m_UnitsPerPixel = value;

                MarkAsDirty();
            }
        }

        public string UnitsName
        {
            get
            {
                return m_UnitsName;
            }
            set
            {
                if (Equals(m_UnitsName, value))
                {
                    return;
                }

                m_UnitsName = value;

                MarkAsDirty();
            }
        }

        public override void LoadSettings()
        {
            UnitsName = "pixels";
            UnitsPerPixel = 1;
            DataLoadComplete();
        }

        public override void SaveSettings()
        {
            DataLoadComplete();
        }
    }
}
