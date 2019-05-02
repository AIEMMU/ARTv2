using ARWT.ModelInterface;

namespace ARWT.Model.MWA
{
    public interface IWhisker : IModelObjectBase
    {
        int WhiskerId
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

        IWhiskerPoint[] WhiskerPoints
        {
            get;
            set;
        }

        IMouseFrame Parent
        {
            get;
            set;
        }

        void Initialise(int numberOfPoints);

        bool WhiskerReady();
        IWhisker Clone();
    }
}
