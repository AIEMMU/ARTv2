using System.Linq;
using ArtLibrary.Model.XmlClasses;
using ArtLibrary.ModelInterface.Boundries;

namespace ArtLibrary.Model.Boundries
{
    internal class CircleBoundary : BoundaryBase, ICircleBoundary
    {
        public CircleBoundary() : base(BoundaryType.Circle)
        {
        }

        public override BoundaryBaseXml GetData()
        {
            int id = Id;
            PointXml[] points = Points.Select(x => new PointXml(x.X, x.Y)).ToArray();
            CircleBoundaryXml data = new CircleBoundaryXml(id, points);

            return data;
        }
    }
}
