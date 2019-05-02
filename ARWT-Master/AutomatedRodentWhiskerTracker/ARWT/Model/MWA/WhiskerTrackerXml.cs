using System.Xml.Serialization;

namespace ARWT.Model.MWA
{
    public class WhiskerTrackerXml
    {
        [XmlElement(ElementName = "ClipSettings")]
        public ClipSettingsXml ClipSettings
        {
            get;
            set;
        }

        [XmlArray(ElementName = "Frames")]
        [XmlArrayItem(ElementName = "Frame")]
        public MouseFrameXml[] Frames
        {
            get;
            set;
        }

        [XmlElement(ElementName="UnitSettings")]
        public UnitSettingsXml UnitSettings
        {
            get;
            set;
        }

        [XmlElement(ElementName = "FrameRateSettings")]
        public FrameRateSettingsXml FrameRateSettings
        {
            get;
            set;
        }

        public WhiskerTrackerXml()
        {
            
        }

        public WhiskerTrackerXml(ClipSettingsXml clipSettings, MouseFrameXml[] frames, UnitSettingsXml unitSettings, FrameRateSettingsXml frameRateSettings)
        {
            ClipSettings = clipSettings;
            Frames = frames;
            UnitSettings = unitSettings;
            FrameRateSettings = frameRateSettings;
        }
    }
}
