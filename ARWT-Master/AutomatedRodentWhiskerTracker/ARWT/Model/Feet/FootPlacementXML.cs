using ARWT.Resolver;
using ARWT.ModelInterface.Feet;
using System.Xml.Serialization;
using ARWT.Foot.centroidTracker;

namespace ARWT.Model.Feet
{
    public class FootPlacementXML
    {
        [XmlElement(ElementName = "id")]
        public string id { get; set; }

        [XmlElement(ElementName ="CentroidX")]
        public int centroidX{ get; set; }

        [XmlElement(ElementName = "CentroidY")]
        public int centroidY { get; set; }

        [XmlElement(ElementName = "minX")]
        public int minX { get; set; }

        [XmlElement(ElementName = "maxX")]
        public int maxX { get; set; }

        [XmlElement(ElementName = "minY")]
        public int minY { get; set; }

        [XmlElement(ElementName = "maxY")]
        public int maxY { get; set; }
        [XmlElement(ElementName = "width")]
        public int width { get; set; }

        [XmlElement(ElementName = "height")]
        public int height { get; set; }

        public FootPlacementXML()
        {

        }
        public FootPlacementXML(IfeetID footPlacement)
        {
            id = footPlacement.id;
            minX = footPlacement.value.minX;
            minY = footPlacement.value.minY;
            maxX = footPlacement.value.maxX;
            maxY = footPlacement.value.maxY;
            width = footPlacement.value.width;
            height = footPlacement.value.height;
            centroidX = footPlacement.value.centroidX;
            centroidY = footPlacement.value.centroidY;
        }

        public IfeetID GetFootPlacement()
        {
            IfeetID footPlacement = ModelResolver.Resolve<IfeetID>();
            footPlacement.id = id;
            footPlacement.value.minX = minX;
            footPlacement.value.minY = minY;
            footPlacement.value.maxY = maxY;
            footPlacement.value.maxX = maxX;
            footPlacement.value.width = width;
            footPlacement.value.height  = height; 
            footPlacement.DataLoadComplete();
            return footPlacement;
        }
    }
}
