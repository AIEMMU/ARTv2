using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ArtLibrary.Model.Results;
using ArtLibrary.ModelInterface;
using ArtLibrary.ModelInterface.Behaviours;
using ArtLibrary.ModelInterface.Boundries;
using ArtLibrary.ModelInterface.Datasets.Types;

namespace ArtLibrary.ModelInterface.Results
{
    public interface IMouseDataResult : IModelObjectBase
    {
        string Name
        {
            get;
            set;
        }

        int Age
        {
            get;
            set;
        }
        
        ITypeBase Type
        {
            get;
            set;
        }
        
        double CentroidSize
        {
            get;
            set;
        }
        
        double DistanceTravelled
        {
            get;
            set;
        }
        
        double Duration
        {
            get;
            set;
        }
        
        double MaxSpeed
        {
            get;
            set;
        }

        double MaxCentroidSpeed
        {
            get;
            set;
        }

        double MaxAngularVelocty
        {
            get;
            set;
        }

        double AverageWaist
        {
            get;
            set;
        }

        double PelvicArea
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

        double Dummy
        {
            get;
            set;
        }

        Dictionary<int, ISingleFrameResult> Results
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

        SingleFileResult VideoOutcome
        {
            get;
            set;
        }

        double AverageVelocity
        {
            get;
            set;
        }

        double AverageCentroidVelocity
        {
            get;
            set;
        }

        double AverageAngularVelocity
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
        
        IBoundaryBase[] Boundaries
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
        
        double MinInteractionDistance
        {
            get;
            set;
        }
        
        Dictionary<IBoundaryBase, IBehaviourHolder[]> InteractingBoundries
        {
            get;
            set;
        }
        
        IBehaviourHolder[] Events
        {
            get;
            set;
        }

        string MainBehaviour
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

        double SmoothFactor
        {
            get;
            set;
        }

        void GenerateResults();
        PointF[] GetMotionTrack();
        void ResetFrames();

        List<Tuple<int, int>> GetFrameNumbersForRunning();

        List<Tuple<int, int>> GetFrameNumbesrForMoving();

        List<Tuple<int, int>> GetFrameNumbesrForTurning();

        double GetAverageSpeedForMoving();

        double GetAverageAngularSpeedForTurning();

        double GetCentroidWidthForRunning();

        double GetCentroidWidthForPelvic1();

        double GetCentroidWidthForPelvic2();

        double GetCentroidWidthForPelvic3();

        double GetCentroidWidthForPelvic4();

        double GetAverageCentroidSpeedForMoving();

        List<double> CentroidsTest
        {
            get;
            set;
        }
    }
}
