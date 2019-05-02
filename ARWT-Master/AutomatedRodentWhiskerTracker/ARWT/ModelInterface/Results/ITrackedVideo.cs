using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using ArtLibrary.Model.Results;
using ArtLibrary.ModelInterface.Behaviours;
using ArtLibrary.ModelInterface.Boundries;
using ARWT.ModelInterface.RBSK2;

namespace ARWT.ModelInterface.Results
{
    public interface ITrackedVideo : IModelObjectBase
    {
        string FileName
        {
            get;
            set;
        }

        Dictionary<int, ISingleFrameExtendedResults> Results
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

        PointF[] MotionTrack
        {
            get;
            set;
        }

        PointF[] SmoothedMotionTrack
        {
            get;
            set;
        }

        Vector[] OrientationTrack
        {
            get;
            set;
        }

        IBoundaryBase[] Boundries
        {
            get;
            set;
        }

        IBehaviourHolder[] Events
        {
            get;
            set;
        }

        Dictionary<IBoundaryBase, IBehaviourHolder[]> InteractingBoundries
        {
            get;
            set;
        }

        double MinInteractionDistance
        {
            get;
            set;
        }

        double GapDistance
        {
            get;
            set;
        }

        int ThresholdValue
        {
            get;
            set;
        }

        int ThresholdValue2
        {
            get;
            set;
        }

        Model.Results.SingleFileResult Result
        {
            get;
            set;
        }

        string Message
        {
            get;
            set;
        }

        bool SmoothMotion
        {
            get;
            set;
        }

        double FrameRate
        {
            get;
            set;
        }

        double PelvicArea1
        {
            get;
            set;
        }

        double PelvicArea2
        {
            get;
            set;
        }

        double PelvicArea3
        {
            get;
            set;
        }

        double PelvicArea4
        {
            get;
            set;
        }

        double CentroidSize
        {
            get;
            set;
        }

        double UnitsToMilimeters
        {
            get;
            set;
        }

        Rectangle ROI
        {
            get;
            set;
        }

        IWhiskerVideoSettings WhiskerSettings
        {
            get;
            set;
        }
        IFootVideoSettings FootSettings
        {
            get;
            set;
        }
        void UpdateTrack();
    }
}
