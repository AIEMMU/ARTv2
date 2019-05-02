using System.Xml.Serialization;

namespace ARWT.Model.MWA
{
    public class FrameRateSettingsXml
    {
        [XmlElement(ElementName = "OriginalFrameRate")]
        public double OriginalFrameRate
        {
            get;
            set;
        }

        [XmlElement(ElementName = "CurrentFrameRate")]
        public double CurrentFrameRate
        {
            get;
            set;
        }

        [XmlElement(ElementName = "ModifierRatio")]
        public double ModifierRatio
        {
            get;
            set;
        }

        public FrameRateSettingsXml()
        {

        }

        public FrameRateSettingsXml(IFrameRateSettings settings)
        {
            CurrentFrameRate = settings.CurrentFrameRate;
            OriginalFrameRate = settings.OriginalFrameRate;            
        }

        public IFrameRateSettings GetSettings()
        {
            IFrameRateSettings settings = new FrameRateSettings();
            settings.CurrentFrameRate = CurrentFrameRate;
            settings.OriginalFrameRate = OriginalFrameRate;

            return settings;
        }
    }
}
