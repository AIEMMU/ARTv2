using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ArtLibrary.Model.XmlClasses;
using Emgu.CV.Structure;
using ARWT.ModelInterface.Whiskers;
using ARWT.Resolver;

namespace ARWT.Model.Whiskers
{
    public class WhiskerSegmentXml
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

        [XmlElement(ElementName = "Angle")]
        public double Angle
        {
            get;
            set;
        }

        [XmlElement(ElementName = "LineP1")]
        public PointXml LineP1
        {
            get;
            set;
        }

        [XmlElement(ElementName = "LineP2")]
        public PointXml LineP2
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Color")]
        public string HtmlColor
        {
            get;
            set;
        }

        [XmlElement(ElementName = "AverageIntensity")]
        public double AverageIntensity
        {
            get;
            set;
        }

        public WhiskerSegmentXml()
        {
            
        }

        public WhiskerSegmentXml(IWhiskerSegment whiskerSegment)
        {
            X = whiskerSegment.X;
            Y = whiskerSegment.Y;
            Angle = whiskerSegment.Angle;
            LineP1 = new PointXml(whiskerSegment.Line.P1);
            LineP2 = new PointXml(whiskerSegment.Line.P2);
            AverageIntensity = whiskerSegment.AvgIntensity;

            HtmlColor = ColorTranslator.ToHtml(whiskerSegment.Color);
        }

        public IWhiskerSegment GetSegment()
        {
            IWhiskerSegment whiskerSegment = ModelResolver.Resolve<IWhiskerSegment>();

            whiskerSegment.X = X;
            whiskerSegment.Y = Y;
            whiskerSegment.Angle = Angle;
            whiskerSegment.Line = new LineSegment2D(LineP1.GetPoint(), LineP2.GetPoint());
            whiskerSegment.Color = ColorTranslator.FromHtml(HtmlColor);
            whiskerSegment.AvgIntensity = AverageIntensity;
            whiskerSegment.DataLoadComplete();

            return whiskerSegment;
        }
    }
}
