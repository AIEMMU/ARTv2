using System.Linq;
using ArtLibrary.Model.XmlClasses;
using ArtLibrary.ModelInterface.Boundries;

namespace ArtLibrary.Model.Boundries
{
    internal class ArtefactsBoundary : BoundaryBase, IArtefactsBoundary
    {
        public ArtefactsBoundary() : base(BoundaryType.Artefact)
        {

        }

        public override BoundaryBaseXml GetData()
        {
            int id = Id;
            PointXml[] points = Points.Select(x => new PointXml(x.X, x.Y)).ToArray();

            ArtefactsBoundaryXml data = new ArtefactsBoundaryXml(id, points);

            return data;
        }
    }
}
