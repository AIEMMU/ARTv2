using System.Xml.Serialization;

namespace ARWT.Model.MWA
{
    public class WhiskerPointXml
    {
        [XmlElement(ElementName = "PointID")]
        public int PointId
        {
            get;
            set;
        }

        [XmlElement(ElementName = "XRatio")]
        public double XRatio
        {
            get;
            set;
        }

        [XmlElement(ElementName = "YRatio")]
        public double YRatio
        {
            get;
            set;
        }

        public WhiskerPointXml()
        {
            
        }

        public WhiskerPointXml(IWhiskerPoint whiskerPoint)
        {
            PointId = whiskerPoint.PointId;
            XRatio = whiskerPoint.XRatio;
            YRatio = whiskerPoint.YRatio;
        }

        public IWhiskerPoint CreateWhiskerPoint(IWhisker parent)
        {
            WhiskerPoint whiskerPoint = new WhiskerPoint();
            whiskerPoint.Parent = parent;
            whiskerPoint.PointId = PointId;
            whiskerPoint.XRatio = XRatio;
            whiskerPoint.YRatio = YRatio;
            whiskerPoint.DataLoadComplete();

            return whiskerPoint;
        }
    }
}
