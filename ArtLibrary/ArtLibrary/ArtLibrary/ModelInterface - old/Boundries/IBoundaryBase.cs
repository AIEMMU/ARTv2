using System.Drawing;
using ArtLibrary.Model.Boundries;

namespace ArtLibrary.ModelInterface.Boundries
{
    public interface IBoundaryBase : IModelObjectBase
    {
        int Id
        {
            get;
            set;
        }
        
        Point[] Points
        {
            get;
            set;
        }

        double GetMinimumDistance(PointF point);
        BoundaryBaseXml GetData();
    }
}
