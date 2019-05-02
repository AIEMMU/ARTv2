using ARWT.ModelInterface;

namespace ARWT.Model.MWA
{
    public interface IClipSettings : IModelObjectBase
    {
        string ClipFilePath
        {
            get;
            set;
        }

        int StartFrame
        {
            get;
            set;
        }

        int EndFrame
        {
            get;
            set;
        }

        int FrameInterval
        {
            get;
            set;
        }

        int NumberOfWhiskers
        {
            get;
            set;
        }

        int NumberOfPointsPerWhisker
        {
            get;
            set;
        }

        bool IncludeNosePoint
        {
            get;
            set;
        }

        bool IncludeOrientationPoint
        {
            get;
            set;
        }

        int NumberOfGenericPoints
        {
            get;
            set;
        }

        int TotalNumberOfPoints
        {
            get;
        }

        IWhiskerClipSettings[] Whiskers
        {
            get;
            set;
        }

        IWhisker[] CreateEmptyWhiskers(IMouseFrame mouseFrame);
    }
}
