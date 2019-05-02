using System;
using System.Linq;
using ArtLibrary.Model.Resolver;
using ArtLibrary.Model.XmlClasses;
using ArtLibrary.ModelInterface.Boundries;

namespace ArtLibrary.Model.Boundries
{
    [Serializable]
    public class BoxBoundaryXml : BoundaryBaseXml
    {
        public BoxBoundaryXml()
        {
            
        }

        public BoxBoundaryXml(int id, PointXml[] points) : base(id, points)
        {
        }

        public override IBoundaryBase GetBoundary()
        {
            IBoxBoundary boundary = ModelResolver.Resolve<IBoxBoundary>();
            boundary.Id = Id;
            boundary.Points = Points.Select(x => x.GetPoint()).ToArray(); ;
            return boundary;
        }
    }
}
