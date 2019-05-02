namespace ARWT.Model.MWA
{
    internal abstract class SettingsBase : ModelObjectBase, ISettingsBase
    {
        private string m_Name;

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                if (Equals(m_Name, value))
                {
                    return;
                }

                m_Name = value;

                MarkAsDirty();
            }
        }

        public abstract void LoadSettings();
        public abstract void SaveSettings();
    }
}
