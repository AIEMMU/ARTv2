using System;
using System.Linq;
using ArtLibrary.Model.Resolver;
using ArtLibrary.Model.XmlClasses;
using ArtLibrary.ModelInterface.Boundries;

namespace ArtLibrary.Model.Boundries
{
    [Serializable]
    public class CircleBoundaryXml : BoundaryBaseXml
    {
        public CircleBoundaryXml()
        {
            
        }

        public CircleBoundaryXml(int id, PointXml[] points) : base(id, points)
        {
        }

        public override IBoundaryBase GetBoundary()
        {
            ICircleBoundary boundary = ModelResolver.Resolve<ICircleBoundary>();
            boundary.Id = Id;
            boundary.Points = Points.Select(x => x.GetPoint()).ToArray(); ;
            return boundary;
        }

        
    }
}
