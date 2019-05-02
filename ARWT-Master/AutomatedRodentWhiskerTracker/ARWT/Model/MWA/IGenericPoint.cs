using ARWT.ModelInterface;

namespace ARWT.Model.MWA
{
    public interface IGenericPoint : IModelObjectBase
    {
        double XRatio
        {
            get;
            set;
        }

        double YRatio
        {
            get;
            set;
        }

        int PointId
        {
            get;
            set;
        }
    }
}
