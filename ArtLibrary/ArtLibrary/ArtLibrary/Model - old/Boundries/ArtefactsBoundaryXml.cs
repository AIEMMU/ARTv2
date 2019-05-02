using System;
using System.Linq;
using ArtLibrary.Model.Resolver;
using ArtLibrary.Model.XmlClasses;
using ArtLibrary.ModelInterface.Boundries;

namespace ArtLibrary.Model.Boundries
{
    [Serializable]
    public class ArtefactsBoundaryXml : BoundaryBaseXml
    {
        public ArtefactsBoundaryXml()
        {
            
        }

        public ArtefactsBoundaryXml(int id, PointXml[] points) : base(id, points)
        {
        }

        public override IBoundaryBase GetBoundary()
        {
            IArtefactsBoundary boundary = ModelResolver.Resolve<IArtefactsBoundary>();
            boundary.Id = Id;
            boundary.Points = Points.Select(x => x.GetPoint()).ToArray();
            return boundary;
        }
    }
}
