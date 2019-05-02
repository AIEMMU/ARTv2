using System.Linq;
using System.Xml.Serialization;

namespace ARWT.Model.MWA
{
    public class MouseFrameXml
    {
        [XmlElement(ElementName = "FrameNumber")]
        public int FrameNumber
        {
            get;
            set;
        }

        [XmlElement(ElementName = "IndexNumber")]
        public int IndexNumber
        {
            get;
            set;
        }

        [XmlArray(ElementName = "Whiskers")]
        [XmlArrayItem(ElementName = "Whisker")]
        public WhiskerXml[] Whiskers
        {
            get;
            set;
        }

        public MouseFrameXml()
        {
            
        }

        public MouseFrameXml(IMouseFrame mouseFrame)
        {
            FrameNumber = mouseFrame.FrameNumber;
            IndexNumber = mouseFrame.IndexNumber;
            Whiskers = mouseFrame.Whiskers.Select(x => new WhiskerXml(x)).ToArray();
        }

        public IMouseFrame GetMouseFrame()
        {
            MouseFrame mouseFrame = new MouseFrame();
            mouseFrame.FrameNumber = FrameNumber;
            mouseFrame.IndexNumber = IndexNumber;
            mouseFrame.Whiskers = Whiskers.Select(x => x.GetWhisker(mouseFrame)).ToArray();
            mouseFrame.DataLoadComplete();

            return mouseFrame;
        }
    }
}
