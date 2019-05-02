using ARWT.Foot.centroidTracker;
using ARWT.ModelInterface.Feet;
using ARWT.Resolver;
using System;
using System.Xml.Serialization;

namespace ARWT.Model.Feet
{
    public class FootplacementCollectionXML
    {
        
        [XmlElement(ElementName = "LeftFrontFootPlacement")]
        public FootPlacementXML leftFrontFeetoPlacements
        {
            get; set;
        }

        [XmlElement(ElementName = "LeftHindFootPlacement")]
        public FootPlacementXML leftHindFeetoPlacements
        {
            get; set;
        }

       
        [XmlElement(ElementName = "RighttFrontFootPlacement")]
        public FootPlacementXML rightFrontFeetoPlacements
        {
            get; set;
        }

        
        [XmlElement(ElementName = "RightHindFootPlacement")]
        public FootPlacementXML rightHindFeetoPlacements
        {
            get; set;
        }

        public FootplacementCollectionXML()
        {

        }
        public FootplacementCollectionXML(IFootCollection footCollection)
        {
            if(footCollection != null)
            {
                if(footCollection.leftfront != null)
                {
                    leftFrontFeetoPlacements = getXMLParams(footCollection.leftfront);

                }

                if (footCollection.leftHind != null)
                {
                    leftHindFeetoPlacements = getXMLParams(footCollection.leftHind);
                }

                if (footCollection.rightHind != null)
                {
                    rightHindFeetoPlacements = getXMLParams(footCollection.rightHind);
                }

                if (footCollection.rightfront != null)
                {
                    rightFrontFeetoPlacements = getXMLParams(footCollection.rightfront);
                }
            }
        }

        private FootPlacementXML getXMLParams(IfeetID footPlacement)
        {
            FootPlacementXML foot = new FootPlacementXML();
            foot.id = footPlacement.id;
            foot.height = footPlacement.value.height;
            foot.width = footPlacement.value.width;
            foot.minX = footPlacement.value.minX;
            foot.minY = footPlacement.value.minY;
            foot.maxY = footPlacement.value.maxY;
            foot.maxX = footPlacement.value.maxX;
            foot.centroidX = footPlacement.value.centroidX;
            foot.centroidY = footPlacement.value.centroidY;
            return foot;
        }

        public IFootCollection GetFootCollection()
        {
            IFootCollection footCollection = ModelResolver.Resolve<IFootCollection>();

            if(rightFrontFeetoPlacements !=null )
            {
                footCollection.rightfront = getparams(rightFrontFeetoPlacements);
                
            }
            if (rightHindFeetoPlacements != null )
            {
                footCollection.rightHind = getparams(rightHindFeetoPlacements); 
            }

            if (leftFrontFeetoPlacements != null )
            {
                footCollection.leftfront = getparams(leftFrontFeetoPlacements);
            }
            if (leftHindFeetoPlacements != null )
            {
                footCollection.leftHind = getparams(leftHindFeetoPlacements);
            }
            footCollection.DataLoadComplete();
            return footCollection;
        }

        private IfeetID getparams(FootPlacementXML footplacement)
        {
            IfeetID foot = new feetID();
            foot.id = footplacement.id;
            foot.value.height = footplacement.height;
            foot.value.width = footplacement.width;
            foot.value.minX = footplacement.minX;
            foot.value.minY = footplacement.minY;
            foot.value.maxY = footplacement.maxY;
            foot.value.maxX = footplacement.maxX;
            foot.value.centroidX = footplacement.centroidX;
            foot.value.centroidY = footplacement.centroidY;
            return foot;
        }
    }
}
