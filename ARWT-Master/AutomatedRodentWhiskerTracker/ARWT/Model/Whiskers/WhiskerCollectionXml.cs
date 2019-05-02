using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ARWT.ModelInterface.Whiskers;
using ARWT.Resolver;

namespace ARWT.Model.Whiskers
{
    public class WhiskerCollectionXml
    {
        [XmlArray(ElementName = "LeftWhiskers")]
        [XmlArrayItem(ElementName = "LeftWhisker")]
        public WhiskerSegmentXml[] LeftWhiskers
        {
            get;
            set;
        }

        [XmlArray(ElementName = "RightWhiskers")]
        [XmlArrayItem(ElementName = "RightWhisker")]
        public WhiskerSegmentXml[] RightWhiskers
        {
            get;
            set;
        }

        public WhiskerCollectionXml()
        {
            
        }

        public WhiskerCollectionXml(IWhiskerCollection whiskerCollection)
        {
            if (whiskerCollection != null)
            {
                if (whiskerCollection.LeftWhiskers != null)
                {
                    LeftWhiskers = whiskerCollection.LeftWhiskers.Select(x => new WhiskerSegmentXml(x)).ToArray();
                }
                

                if (whiskerCollection.RightWhiskers != null)
                {
                    RightWhiskers = whiskerCollection.RightWhiskers.Select(x => new WhiskerSegmentXml(x)).ToArray();
                }
                
            }
        }

        public IWhiskerCollection GetWhiskerCollection()
        {
            IWhiskerCollection whiskerCollection = ModelResolver.Resolve<IWhiskerCollection>();

            if (RightWhiskers != null && RightWhiskers.Any())
            {
                whiskerCollection.RightWhiskers = RightWhiskers.Select(x => x.GetSegment()).ToArray();
            }

            if (LeftWhiskers != null && LeftWhiskers.Any())
            {
                whiskerCollection.LeftWhiskers = LeftWhiskers.Select(x => x.GetSegment()).ToArray();
            }
            
            whiskerCollection.DataLoadComplete();
            return whiskerCollection;
        }
    }
}
