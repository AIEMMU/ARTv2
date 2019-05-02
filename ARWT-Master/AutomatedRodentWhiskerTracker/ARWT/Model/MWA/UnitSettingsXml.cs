using System.Xml.Serialization;

namespace ARWT.Model.MWA
{
    public class UnitSettingsXml
    {
        [XmlElement(ElementName="Units")]
        public string UnitsName
        {
            get;
            set;
        }

        [XmlElement(ElementName="UnitsPerPixel")]
        public double UnitsPerPixel
        {
            get;
            set;
        }

        public UnitSettingsXml()
        {
            
        }

        public UnitSettingsXml(IUnitSettings unitSettings)
        {
            UnitsName = unitSettings.UnitsName;
            UnitsPerPixel = unitSettings.UnitsPerPixel;
        }

        public IUnitSettings GetSettings()
        {
            UnitSettings unitSettings = new UnitSettings();
            unitSettings.UnitsName = UnitsName;
            unitSettings.UnitsPerPixel = UnitsPerPixel;
            unitSettings.DataLoadComplete();

            return unitSettings;
        }
    }
}
