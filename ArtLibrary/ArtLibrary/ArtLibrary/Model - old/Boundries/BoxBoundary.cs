using System.Linq;
using ArtLibrary.Model.XmlClasses;
using ArtLibrary.ModelInterface.Boundries;

namespace ArtLibrary.Model.Boundries
{
    internal class BoxBoundary : BoundaryBase, IBoxBoundary
    {
        public BoxBoundary() : base(BoundaryType.Box)
        {

        }

        public override BoundaryBaseXml GetData()
        {
            int id = Id;
            PointXml[] points = Points.Select(x => new PointXml(x.X, x.Y )).ToArray();
            BoxBoundaryXml data = new BoxBoundaryXml(id, points);

            return data;
        }
    }
}
