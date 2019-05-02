using ARWT.ModelInterface;

namespace ARWT.Model.MWA
{
    public interface IFrameRateSettings : IModelObjectBase
    {
        double OriginalFrameRate
        {
            get;
            set;
        }

        double CurrentFrameRate
        {
            get;
            set;
        }

        double ModifierRatio
        {
            get;
            set;
        }
    }
}
