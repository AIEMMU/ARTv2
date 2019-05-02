using System.Xml.Serialization;

namespace ArtLibrary.Model.Behaviours
{
    public enum InteractionBehaviour
    {
        [XmlEnum(Name="Started")]
        Started,
        [XmlEnum(Name="Ended")]
        Ended,
    }
}
