using System.Linq;
using System.Xml.Serialization;
using ArtLibrary.Model.XmlClasses;
using ARWT.Extensions;
using ARWT.Model.Feet;
using ARWT.Model.Whiskers;
using ARWT.ModelInterface.Results;
using ARWT.Resolver;

namespace ARWT.Model.Results
{
    public class SingleFrameExtendedResultXml
    {
        [XmlArray(ElementName = "HeadPointsTest")]
        [XmlArrayItem(ElementName = "Point")]
        public PointFXml[] HeadPoints
        {
            get;
            set;
        }

        [XmlArray(ElementName = "BodyContour")]
        [XmlArrayItem(ElementName = "BodyPoint")]
        public PointXml[] BodyContour
        {
            get;
            set;
        }

        [XmlElement(ElementName = "HeadPoint")]
        public PointFXml HeadPoint
        {
            get;
            set;
        }

        [XmlElement(ElementName = "MidPoint")]
        public PointFXml MidPoint
        {
            get;
            set;
        }

        [XmlElement(ElementName = "SmoothedHeadPoint")]
        public PointFXml SmoothedHeadPoint
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Orientation")]
        public VectorXml Orientation
        {
            get;
            set;
        }

        [XmlElement(ElementName = "CentroidSize")]
        public double CentroidSize
        {
            get;
            set;
        }

        [XmlElement(ElementName = "PelvicArea")]
        public double PelvicArea
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Velocity")]
        public double Velocity
        {
            get;
            set;
        }

        [XmlElement(ElementName = "AngularVelocity")]
        public double AngularVelocity
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Distance")]
        public double Distance
        {
            get;
            set;
        }

        [XmlElement(ElementName = "PelvicArea2")]
        public double PelvicArea2
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Centroid")]
        public PointFXml Centroid
        {
            get;
            set;
        }

        [XmlElement(ElementName = "PelvicArea3")]
        public double PelvicArea3
        {
            get;
            set;
        }

        [XmlElement(ElementName = "PelvicArea4")]
        public double PelvicArea4
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Whiskers", IsNullable = true)]
        public WhiskerCollectionXml Whiskers
        {
            get;
            set;
        }

        [XmlElement(IsNullable = true, ElementName = "AllWhiskers")]
        public WhiskerCollectionXml AllWhiskers
        {
            get;
            set;
        }

        [XmlElement(IsNullable = true, ElementName = "FeetPlacement")]
        public FootplacementCollectionXML FootCollections
        {
            get;
            set;
        }

        [XmlElement(IsNullable = true, ElementName = "BestTrackedWhisker")]
        public WhiskerCollectionXml BestTrackedWhisker
        {
            get;
            set;
        }

        public SingleFrameExtendedResultXml()
        {
            
        }

        public SingleFrameExtendedResultXml(ISingleFrameExtendedResults result)
        {

            if (result.HeadPoints == null)
            {
                HeadPoints = null;
                HeadPoint = null;
                MidPoint = null;
            }
            else
            {
                HeadPoints = result.HeadPoints.Select(x => new PointFXml(x.X, x.Y)).ToArray();
                HeadPoint = HeadPoints[2];
                MidPoint = new PointFXml(result.HeadPoints[1].MidPoint(result.HeadPoints[3]));
            }

            if (result.BodyContour != null)
            {
                BodyContour = result.BodyContour.Select(x => new PointXml(x)).ToArray();
            }
            else
            {
                BodyContour = null;
            }

            SmoothedHeadPoint = new PointFXml(result.SmoothedHeadPoint);
            
            Orientation = new VectorXml(result.Orientation);
            CentroidSize = result.CentroidSize;
            PelvicArea = result.PelvicArea;
            Velocity = result.Velocity;
            AngularVelocity = result.AngularVelocity;
            Distance = result.Distance;
            PelvicArea2 = result.PelvicArea2;
            PelvicArea3 = result.PelvicArea3;
            PelvicArea4 = result.PelvicArea4;
            Centroid = new PointFXml(result.Centroid);

            if (result.Whiskers != null)
            {
                Whiskers = new WhiskerCollectionXml(result.Whiskers);
            }
            else
            {
                Whiskers = null;
            }

            if (result.AllWhiskers != null)
            {
                AllWhiskers = new WhiskerCollectionXml(result.AllWhiskers);
            }
            else
            {
                AllWhiskers = null;
            }
            if (result.FeetCollection != null)
            {
                FootCollections = new FootplacementCollectionXML(result.FeetCollection);
            }else
            {
                FootCollections = null;
            }

            if (result.BestTrackedWhisker != null)
            {
                BestTrackedWhisker = new WhiskerCollectionXml(result.BestTrackedWhisker);
            }
            else
            {
                BestTrackedWhisker = null;
            }
        }

        public ISingleFrameExtendedResults GetData()
        {
            ISingleFrameExtendedResults result = ModelResolver.Resolve<ISingleFrameExtendedResults>();

            if (HeadPoints != null)
            {
                result.HeadPoints = HeadPoints.Select(x => x.GetPoint()).ToArray();
            }
            else
            {
                result.HeadPoints = null;
            }

            if (BodyContour != null)
            {
                result.BodyContour = BodyContour.Select(x => x.GetPoint()).ToArray();
            }
            else
            {
                result.BodyContour = null;
            }

            if (HeadPoint != null)
            {
                result.HeadPoint = HeadPoint.GetPoint();
                result.MidPoint = MidPoint.GetPoint();
            }

            if (SmoothedHeadPoint != null)
            {
                result.SmoothedHeadPoint = SmoothedHeadPoint.GetPoint();
            }

            if (Orientation != null)
            {
                result.Orientation = Orientation.GetVector();
            }
            
            result.CentroidSize = CentroidSize;
            result.PelvicArea = PelvicArea;
            result.Velocity = Velocity;
            result.AngularVelocity = AngularVelocity;
            result.Distance = Distance;
            result.PelvicArea2 = PelvicArea2;
            result.PelvicArea3 = PelvicArea3;
            result.PelvicArea4 = PelvicArea4;

            if (Centroid != null)
            {
                result.Centroid = Centroid.GetPoint();
            }

            if (Whiskers != null)
            {
                result.Whiskers = Whiskers.GetWhiskerCollection();
            }
            if (FootCollections != null)
            {
                result.FeetCollection = FootCollections.GetFootCollection();
            }

            if (AllWhiskers != null)
            {
                result.AllWhiskers = AllWhiskers.GetWhiskerCollection();
            }

            if (BestTrackedWhisker != null)
            {
                result.BestTrackedWhisker = BestTrackedWhisker.GetWhiskerCollection();
            }

            result.DataLoadComplete();
            return result;
        }
    }
}
