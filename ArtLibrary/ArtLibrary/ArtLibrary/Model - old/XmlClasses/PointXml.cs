using System.Drawing;
using System.Xml.Serialization;

namespace ArtLibrary.Model.XmlClasses
{
    public class PointXml
    {
        [XmlElement(ElementName = "X")]
        public int X
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Y")]
        public int Y
        {
            get;
            set;
        }

        public PointXml()
        {
            
        }

        public PointXml(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point GetPoint()
        {
            return new Point(X, Y);
        }
    }
}
