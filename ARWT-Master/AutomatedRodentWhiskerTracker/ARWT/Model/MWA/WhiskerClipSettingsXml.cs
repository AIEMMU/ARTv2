using System.Xml.Serialization;

namespace ARWT.Model.MWA
{
    public class WhiskerClipSettingsXml
    {
        [XmlElement(ElementName="WhiskerId")]
        public int WhiskerId
        {
            get;
            set;
        }

        [XmlElement(ElementName = "NumberOfPoints")]
        public int NumberOfPoints
        {
            get;
            set;
        }

        [XmlElement(ElementName = "WhiskerName")]
        public string WhiskerName
        {
            get;
            set;
        }

        [XmlElement(ElementName = "IsGenericPoint")]
        public bool IsGenericPoint
        {
            get;
            set;
        }

        public WhiskerClipSettingsXml()
        {
            
        }

        public WhiskerClipSettingsXml(IWhiskerClipSettings whiskerClipSettings)
        {
            WhiskerId = whiskerClipSettings.WhiskerId;
            WhiskerName = whiskerClipSettings.WhiskerName;
            IsGenericPoint = whiskerClipSettings.IsGenericPoint;
            NumberOfPoints = whiskerClipSettings.NumberOfPoints;
        }

        public IWhiskerClipSettings CreateWhiskerClipSettings()
        {
            WhiskerClipSettings whiskerClipSettings = new WhiskerClipSettings();

            whiskerClipSettings.WhiskerId = WhiskerId;
            whiskerClipSettings.WhiskerName = WhiskerName;
            whiskerClipSettings.NumberOfPoints = NumberOfPoints;
            whiskerClipSettings.IsGenericPoint = IsGenericPoint;
            whiskerClipSettings.DataLoadComplete();

            return whiskerClipSettings;
        }
    }
}
