using System;
using System.Xml.Serialization;
using ArtLibrary.Model.Boundries;
using ArtLibrary.Model.Resolver;
using ArtLibrary.ModelInterface.Behaviours;

namespace ArtLibrary.Model.Behaviours
{
    [Serializable]
    public class BehaviourHolderXml
    {
        [XmlElement(ElementName = "Boundary")]
        public BoundaryBaseXml Boundary
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Interaction")]
        public InteractionBehaviour Interaction
        {
            get;
            set;
        }

        [XmlElement(ElementName = "FrameNumber")]
        public int FrameNumber
        {
            get;
            set;
        }

        public BehaviourHolderXml()
        {
            
        }

        public BehaviourHolderXml(BoundaryBaseXml boundary, InteractionBehaviour interaction, int frameNumber)
        {
            Boundary = boundary;
            Interaction = interaction;
            FrameNumber = frameNumber;
        }

        public IBehaviourHolder GetData()
        {
            IBehaviourHolder data = ModelResolver.Resolve<IBehaviourHolder>();

            data.Boundary = Boundary.GetBoundary();
            data.Interaction = Interaction;
            data.FrameNumber = FrameNumber;

            return data;
        }
    }
}
