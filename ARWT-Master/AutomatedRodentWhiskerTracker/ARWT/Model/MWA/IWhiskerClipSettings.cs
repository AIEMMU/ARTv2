using ARWT.ModelInterface;

namespace ARWT.Model.MWA
{
    public interface IWhiskerClipSettings : IModelObjectBase
    {
        int WhiskerId
        {
            get;
            set;
        }

        int NumberOfPoints
        {
            get;
            set;
        }

        string WhiskerName
        {
            get;
            set;
        }

        bool IsGenericPoint
        {
            get;
            set;
        }

        IWhisker CreateWhisker(IMouseFrame mouseFrame);
    }
}
