using System.Linq;
using System.Xml.Serialization;

namespace ARWT.Model.MWA
{
    public class ClipSettingsXml
    {
        [XmlElement(ElementName = "ClipFilePath")]
        public string ClipFilePath
        {
            get;
            set;
        }

        [XmlElement(ElementName = "StartFrame")]
        public int StartFrame
        {
            get;
            set;
        }

        [XmlElement(ElementName = "EndFrame")]
        public int EndFrame
        {
            get;
            set;
        }

        [XmlElement(ElementName = "FrameInterval")]
        public int FrameInterval
        {
            get;
            set;
        }

        [XmlElement(ElementName = "NumberOfWhiskers")]
        public int NumberOfWhiskers
        {
            get;
            set;
        }

        [XmlElement(ElementName = "NumberOfPointsPerWhisker")]
        public int NumberOfPointsPerWhisker
        {
            get;
            set;
        }

        [XmlElement(ElementName = "IncludeNosePoint")]
        public bool IncludeNosePoint
        {
            get;
            set;
        }

        [XmlElement(ElementName = "IncludeOrientationPoint")]
        public bool IncludeOrientationPoint
        {
            get;
            set;
        }
        
        [XmlElement(ElementName = "NumberOfGenericPoints")]
        public int NumberOfGenericPoints
        {
            get;
            set;
        }

        [XmlArray(ElementName = "Whiskers")]
        [XmlArrayItem(ElementName = "Whisker")]
        public WhiskerClipSettingsXml[] Whiskers
        {
            get;
            set;
        }

        public ClipSettingsXml()
        {
            
        }

        public ClipSettingsXml(IClipSettings clipSettings)
        {
            ClipFilePath = clipSettings.ClipFilePath;
            StartFrame = clipSettings.StartFrame;
            EndFrame = clipSettings.EndFrame;
            FrameInterval = clipSettings.FrameInterval;
            NumberOfWhiskers = clipSettings.NumberOfWhiskers;
            NumberOfPointsPerWhisker = clipSettings.NumberOfPointsPerWhisker;
            IncludeNosePoint = clipSettings.IncludeNosePoint;
            IncludeOrientationPoint = clipSettings.IncludeOrientationPoint;
            NumberOfGenericPoints = clipSettings.NumberOfGenericPoints;
            Whiskers = clipSettings.Whiskers.Select(x => new WhiskerClipSettingsXml(x)).ToArray();
        }

        public IClipSettings GetClipSettings()
        {
            ClipSettings clipSettings = new ClipSettings();
            clipSettings.ClipFilePath = ClipFilePath;
            clipSettings.StartFrame = StartFrame;
            clipSettings.EndFrame = EndFrame;
            clipSettings.FrameInterval = FrameInterval;
            clipSettings.NumberOfWhiskers = NumberOfWhiskers;
            clipSettings.NumberOfPointsPerWhisker = NumberOfPointsPerWhisker;
            clipSettings.IncludeNosePoint = IncludeNosePoint;
            clipSettings.IncludeOrientationPoint = IncludeOrientationPoint;
            clipSettings.NumberOfGenericPoints = NumberOfGenericPoints;
            clipSettings.Whiskers = Whiskers.Select(x => x.CreateWhiskerClipSettings()).ToArray();
            clipSettings.DataLoadComplete();

            return clipSettings;
        }
    }
}
