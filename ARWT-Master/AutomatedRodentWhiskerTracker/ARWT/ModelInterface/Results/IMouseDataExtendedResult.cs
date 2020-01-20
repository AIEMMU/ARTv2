using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using ArtLibrary.ModelInterface.Behaviours;
using ArtLibrary.ModelInterface.Boundries;
using ArtLibrary.ModelInterface.Results;
using ArtLibrary.ModelInterface.Results.Behaviour.Rotation;
using ARWT.Model.Results;
using ARWT.ModelInterface.Datasets.Types;
using ARWT.ModelInterface.RBSK2;

namespace ARWT.ModelInterface.Results
{
    public interface IMouseDataExtendedResult : IModelObjectBase
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

        double CentroidDistanceTravelled
        {
            get;
            set;
        }

        double Duration
        {
            get;
            set;
        }

        double HeadPointDuration
        {
            get;
            set;
        }

        double CentroidDuration
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

        double Dummy2
        {
            get;
            set;
        }

        double Dummy3
        {
            get;
            set;
        }

        double Dummy4
        {
            get;
            set;
        }

        double Dummy5
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

        PointF[] CentroidMotionTrack
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

        bool ResultsGenerated
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

        void GenerateResults();
        void GenerateResults(string file);
        PointF[] GetMotionTrack();
        void ResetFrames();

        List<Tuple<int, int>> GetFrameNumbersForRunning();

        List<Tuple<int, int>> GetFrameNumbersForMoving();

        List<Tuple<int, int>> GetFrameNumbersForTurning();

        List<Tuple<int, int>> GetFrameNumbersForStill();

        List<Tuple<int, int>> GetFrameNumbesrForInteracting();

        double GetAverageSpeedForMoving();

        double GetAverageAngularSpeedForTurning();

        double GetCentroidWidthForRunning();

        double GetCentroidWidthForPelvic1();

        double GetCentroidWidthForPelvic2();

        double GetCentroidWidthForPelvic3();

        double GetCentroidWidthForPelvic4();

        double GetAverageCentroidSpeedForMoving();
        
        double GetAverageCentroidSpeedForRunning();
        double GetAverageCentroidSpeedForStill();
        double GetAverageCentroidSpeed();
        

        double GetAverageSpeedForRunning();
        double GetAverageSpeedForStill();

        double GetAverageSpeed();
        

        double GetAverageAngularSpeed();

        List<double> CentroidsTest
        {
            get;
            set;
        }

        object[,] GetResults();

        IRotationBehaviour[] RotationBehaviours
        {
            get;
            set;
        }
    }
}
