using System.Linq;
using System.Xml.Serialization;

namespace ARWT.Model.MWA
{
    public class WhiskerXml
    {
        [XmlElement(ElementName="WhiskerId")]
        public int WhiskerId
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

        [XmlArray(ElementName = "WhiskerPoints")]
        [XmlArrayItem(ElementName = "WhiskerPoint")]
        public WhiskerPointXml[] WhiskerPoints
        {
            get;
            set;
        }

        public WhiskerXml()
        {
            
        }

        public WhiskerXml(IWhisker whisker)
        {
            WhiskerId = whisker.WhiskerId;
            WhiskerName = whisker.WhiskerName;
            IsGenericPoint = whisker.IsGenericPoint;
            WhiskerPoints = whisker.WhiskerPoints.Select(x => new WhiskerPointXml(x)).ToArray();
        }

        public IWhisker GetWhisker(IMouseFrame parentFrame)
        {
            Whisker whisker = new Whisker();
            whisker.Parent = parentFrame;
            whisker.WhiskerId = WhiskerId;
            whisker.WhiskerName = WhiskerName;
            whisker.IsGenericPoint = IsGenericPoint;
            whisker.WhiskerPoints = WhiskerPoints.Select(x => x.CreateWhiskerPoint(whisker)).ToArray();
            whisker.DataLoadComplete();

            return whisker;
        }
    }
}
