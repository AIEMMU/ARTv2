using ARWT.ModelInterface.RBSK2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ARWT.Model.RBSK2
{
    public class FootVideoSettingsXML
    {
        [XmlElement(ElementName ="FootArea")]
        public int FootArea
        {
            get;
            set;
        }

        [XmlElement(ElementName = "ContourDistance")]
        public int contourDistance
        {
            get;
            set;
        }

        [XmlElement(ElementName = "KernelSize")]
        public int kernelSize
        {
            get;
            set;
        }

        [XmlElement(ElementName = "ScaleFactor")]
        public int scaleFactor
        {
            get;
            set;
        }
        [XmlElement(ElementName = "track")]
        public bool track
        {
            get;
            set;
        }
        [XmlElement(ElementName = "ErosionIterations")]
        public int erosionIterations
        {
            get;
            set;
        }
        public FootVideoSettingsXML()
        {

        }
        public FootVideoSettingsXML(IFootVideoSettings settings)
        {
            FootArea = settings.area;
            scaleFactor = settings.scaleFactor;
            kernelSize = settings.kernelSize;
            contourDistance = settings.contourDistance;
            erosionIterations = settings.erosionIterations;
            track = settings.track;
        }
        public IFootVideoSettings getFootVideoSettings()
        {
            FootVideoSettings settings = new FootVideoSettings();
            settings.area = FootArea;
            settings.contourDistance = contourDistance;
            settings.scaleFactor = scaleFactor;
            settings.erosionIterations = erosionIterations;
            settings.kernelSize = kernelSize;
            settings.track = track;

            return settings;
        }
    }
}
