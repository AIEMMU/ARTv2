using ARWT.ModelInterface;

namespace ARWT.Model.MWA
{
    public interface IWhiskerPoint : IModelObjectBase
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

        IWhisker Parent
        {
            get;
            set;
        }

        IWhiskerPoint Clone();
    }
}
