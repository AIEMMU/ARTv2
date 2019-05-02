using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;
using ArtLibrary.Model.Behaviours;
using ArtLibrary.ModelInterface.Behaviours;
using ArtLibrary.ModelInterface.Boundries;
using ARWT.Extensions;
using ARWT.ModelInterface.RBSK2;
using ARWT.ModelInterface.Results;
using ARWT.Resolver;

namespace ARWT.Model.Results
{
    internal class TrackedVideo : ModelObjectBase, ITrackedVideo
    {
        private string m_FileName;
        private Dictionary<int, ISingleFrameExtendedResults> m_Results;
        private PointF[] m_MotionTrack;
        private Vector[] m_OrientationTrack;
        private SingleFileResult m_Result;
        private string m_Message;

        private IBoundaryBase[] m_Boundries;
        private IBehaviourHolder[] m_Events;
        private Dictionary<IBoundaryBase, IBehaviourHolder[]> m_InteractingBoundries;

        private double m_MinInteractionDistance;
        private double m_GapDistance;
        private int m_ThresholdValue;
        private int m_ThresholdValue2;
        private double m_CentroidSize;


        private IWhiskerVideoSettings _WhiskerSettings;
        public IWhiskerVideoSettings WhiskerSettings
        {
            get
            {
                return _WhiskerSettings;
            }
            set
            {
                if (Equals(_WhiskerSettings, value))
                {
                    return;
                }

                _WhiskerSettings = value;

                MarkAsDirty();
            }
        }

        private IFootVideoSettings _FootSettings;
        public IFootVideoSettings FootSettings
        {
            get
            {
                return _FootSettings;
            }
            set
            {
                if (Equals(_FootSettings, value))
                {
                    return;
                }

                _FootSettings = value;

                MarkAsDirty();
            }
        }
        public string FileName
        {
            get
            {
                return m_FileName;
            }
            set
            {
                if (Equals(m_FileName, value))
                {
                    return;
                }

                m_FileName = value;

                MarkAsDirty();
            }
        }

        public Dictionary<int, ISingleFrameExtendedResults> Results
        {
            get
            {
                return m_Results;
            }
            set
            {
                if (ReferenceEquals(m_Results, value))
                {
                    return;
                }

                m_Results = value;
                
                MarkAsDirty();
            }
        }

        private int m_StartFrame;
        public int StartFrame
        {
            get
            {
                return m_StartFrame;
            }
            set
            {
                if (Equals(m_StartFrame, value))
                {
                    return;
                }

                m_StartFrame = value;

                MarkAsDirty();
            }
        }

        private int m_EndFrame;
        public int EndFrame
        {
            get
            {
                return m_EndFrame;
            }
            set
            {
                if (Equals(m_EndFrame, value))
                {
                    return;
                }

                m_EndFrame = value;

                MarkAsDirty();
            }
        }

        public PointF[] MotionTrack
        {
            get
            {
                return m_MotionTrack;
            }
            set
            {
                if (ReferenceEquals(m_MotionTrack, value))
                {
                    return;
                }

                m_MotionTrack = value;

                MarkAsDirty();
            }
        }

        private PointF[] m_SmoothedMotionTrack;
        public PointF[] SmoothedMotionTrack
        {
            get
            {
                return m_SmoothedMotionTrack;
            }
            set
            {
                if (ReferenceEquals(m_SmoothedMotionTrack, value))
                {
                    return;
                }

                m_SmoothedMotionTrack = value;

                MarkAsDirty();
            }
        }

        public Vector[] OrientationTrack
        {
            get
            {
                return m_OrientationTrack;
            }
            set
            {
                if (Equals(m_OrientationTrack, value))
                {
                    return;
                }

                m_OrientationTrack = value;

                MarkAsDirty();
            }
        }

        public IBoundaryBase[] Boundries
        {
            get
            {
                return m_Boundries;
            }
            set
            {
                if (ReferenceEquals(m_Boundries, value))
                {
                    return;
                }

                m_Boundries = value;

                MarkAsDirty();
            }
        }

        public IBehaviourHolder[] Events
        {
            get
            {
                return m_Events;
            }
            set
            {
                if (ReferenceEquals(m_Events, value))
                {
                    return;
                }

                m_Events = value;

                MarkAsDirty();
            }
        }

        public Dictionary<IBoundaryBase, IBehaviourHolder[]> InteractingBoundries
        {
            get
            {
                return m_InteractingBoundries;
            }
            set
            {
                if (ReferenceEquals(m_InteractingBoundries, value))
                {
                    return;
                }

                m_InteractingBoundries = value;

                MarkAsDirty();
            }
        }

        public double MinInteractionDistance
        {
            get
            {
                return m_MinInteractionDistance;
            }
            set
            {
                if (Equals(m_MinInteractionDistance, value))
                {
                    return;
                }

                m_MinInteractionDistance = value;

                MarkAsDirty();
            }
        }

        public double GapDistance
        {
            get
            {
                return m_GapDistance;
            }
            set
            {
                if (Equals(m_GapDistance, value))
                {
                    return;
                }

                m_GapDistance = value;

                MarkAsDirty();
            }
        }

        public int ThresholdValue
        {
            get
            {
                return m_ThresholdValue;
            }
            set
            {
                if (Equals(m_ThresholdValue))
                {
                    return;
                }

                m_ThresholdValue = value;

                MarkAsDirty();
            }
        }

        public int ThresholdValue2
        {
            get
            {
                return m_ThresholdValue2;
            }
            set
            {
                if (Equals(m_ThresholdValue2))
                {
                    return;
                }

                m_ThresholdValue2 = value;

                MarkAsDirty();
            }
        }

        public SingleFileResult Result
        {
            get
            {
                return m_Result;
            }
            set
            {
                if (Equals(m_Result, value))
                {
                    return;
                }

                m_Result = value;

                MarkAsDirty();
            }
        }

        public string Message
        {
            get
            {
                return m_Message;
            }
            set
            {
                if (Equals(m_Message, value))
                {
                    return;
                }

                m_Message = value;

                MarkAsDirty();
            }
        }

        private bool m_SmoothMotion;
        public bool SmoothMotion
        {
            get
            {
                return m_SmoothMotion;
            }
            set
            {
                if (Equals(m_SmoothMotion, value))
                {
                    return;
                }

                m_SmoothMotion = value;

                MarkAsDirty();
            }
        }

        private double m_FrameRate;
        public double FrameRate
        {
            get
            {
                return m_FrameRate;
            }
            set
            {
                if (Equals(m_FrameRate, value))
                {
                    return;
                }

                m_FrameRate = value;

                MarkAsDirty();
            }
        }

        public double CentroidSize
        {
            get
            {
                return m_CentroidSize;
            }
            set
            {
                if (Equals(m_CentroidSize, value))
                {
                    return;
                }

                m_CentroidSize = value;

                MarkAsDirty();
            }
        }

        private double m_PelvicArea1;
        public double PelvicArea1
        {
            get
            {
                return m_PelvicArea1;
            }
            set
            {
                if (Equals(m_PelvicArea1, value))
                {
                    return;
                }

                m_PelvicArea1 = value;

                MarkAsDirty();
            }
        }

        private double m_PelvicArea2;
        public double PelvicArea2
        {
            get
            {
                return m_PelvicArea2;
            }
            set
            {
                if (Equals(m_PelvicArea2, value))
                {
                    return;
                }

                m_PelvicArea2 = value;

                MarkAsDirty();
            }
        }

        private double m_PelvicArea3;
        public double PelvicArea3
        {
            get
            {
                return m_PelvicArea3;
            }
            set
            {
                if (Equals(m_PelvicArea3, value))
                {
                    return;
                }

                m_PelvicArea3 = value;

                MarkAsDirty();
            }
        }

        private double m_PelvicArea4;
        public double PelvicArea4
        {
            get
            {
                return m_PelvicArea4;
            }
            set
            {
                if (Equals(m_PelvicArea4, value))
                {
                    return;
                }

                m_PelvicArea4 = value;

                MarkAsDirty();
            }
        }

        private double m_UnitsToMilimeters = 1;
        public double UnitsToMilimeters
        {
            get
            {
                return m_UnitsToMilimeters;
            }
            set
            {
                if (Equals(m_UnitsToMilimeters, value))
                {
                    return;
                }

                m_UnitsToMilimeters = value;

                MarkAsDirty();
            }
        }

        private Rectangle m_ROI;
        public Rectangle ROI
        {
            get
            {
                return m_ROI;
            }
            set
            {
                if (Equals(m_ROI, value))
                {
                    return;
                }

                m_ROI = value;

                MarkAsDirty();
            }
        }


        public void UpdateTrack()
        {
            if (Results == null || Results.Count == 0)
            {
                return;
            }

            List<PointF> motionTrack = new List<PointF>();
            List<Vector> orientationTrack = new List<Vector>();
            int startOffset = Results.First(x => x.Value != null).Key;
            
            int frameCount = Results.Count;
            for (int i = 0; i < frameCount; i++)
            {
                ISingleFrameExtendedResults currentFrameResult = Results[i];

                if (currentFrameResult == null)
                {
                    continue;
                }

                PointF[] headPoints = currentFrameResult.HeadPoints;
                if (headPoints != null)
                {
                    PointF midPoint = headPoints[1].MidPoint(headPoints[3]);
                    //Vector up = new Vector(0,1);
                    Vector dir = new Vector(headPoints[2].X - midPoint.X, headPoints[2].Y - midPoint.Y);
                    orientationTrack.Add(dir);

                    motionTrack.Add(headPoints[2]);
                }
            }

            MotionTrack = motionTrack.ToArray();
            //OrientationTrack = orientationTrack.ToArray();

            if (Boundries != null)
            {
                GenerateBehaviourAnalysis(MotionTrack, Boundries, startOffset);
            }
        }

        private void GenerateBehaviourAnalysis(PointF[] motionTrack, IEnumerable<IBoundaryBase> objects, int startOffset)
        {
            Dictionary<IBoundaryBase, IBehaviourHolder[]> interactingBoundries = new Dictionary<IBoundaryBase, IBehaviourHolder[]>();
            int trackCount = motionTrack.Length;
            foreach (IBoundaryBase boundary in objects)
            {
                //Manually handle first point
                PointF currentPoint = motionTrack[0];
                double distance = boundary.GetMinimumDistance(currentPoint);
                IBehaviourHolder previousHolder;
                List<IBehaviourHolder> behaviours = new List<IBehaviourHolder>();
                if (distance < MinInteractionDistance)
                {
                    //Interaction
                    previousHolder = ModelResolver.Resolve<IBehaviourHolder>(); //new IBehaviourHolder(boundary, InteractionBehaviour.Started, 0);
                    previousHolder.Boundary = boundary;
                    previousHolder.Interaction = InteractionBehaviour.Started;
                    previousHolder.FrameNumber = 0;
                    //interactingBoundries[boundary].Add(previousHolder);
                    behaviours.Add(previousHolder);
                }
                else
                {
                    //No Interaction
                    previousHolder = ModelResolver.Resolve<IBehaviourHolder>();
                    previousHolder.Boundary = boundary;
                    previousHolder.Interaction = InteractionBehaviour.Ended;
                    previousHolder.FrameNumber = 0;
                    //interactingBoundries[boundary].Add(previousHolder);
                    behaviours.Add(previousHolder);
                }

                
                for (int i = 1; i < trackCount; i++)
                {
                    currentPoint = motionTrack[i];
                    distance = boundary.GetMinimumDistance(currentPoint);

                    if (distance < MinInteractionDistance)
                    {
                        //Interaction
                        if (previousHolder.Interaction != InteractionBehaviour.Started)
                        {
                            previousHolder = ModelResolver.Resolve<IBehaviourHolder>();
                            previousHolder.Boundary = boundary;
                            previousHolder.Interaction = InteractionBehaviour.Started;
                            previousHolder.FrameNumber = i + startOffset;
                            //previousHolder = new IBehaviourHolder(boundary, InteractionBehaviour.Started, i + startOffset);
                            behaviours.Add(previousHolder);
                            //interactingBoundries[boundary].Add(previousHolder);
                        }
                    }
                    else
                    {
                        //No Interaction
                        if (previousHolder.Interaction != InteractionBehaviour.Ended)
                        {
                            previousHolder = ModelResolver.Resolve<IBehaviourHolder>();
                            previousHolder.Boundary = boundary;
                            previousHolder.Interaction = InteractionBehaviour.Ended;
                            previousHolder.FrameNumber = i + startOffset;
                            //previousHolder = new IBehaviourHolder(boundary, InteractionBehaviour.Ended, i + startOffset);
                            behaviours.Add(previousHolder);
                            //interactingBoundries[boundary].Add(previousHolder);
                        }
                    }
                }

                interactingBoundries.Add(boundary, behaviours.ToArray());
            }

            InteractingBoundries = interactingBoundries;

            IEnumerable<IBehaviourHolder> events = interactingBoundries.SelectMany(boundary => boundary.Value);

            Events = events.OrderBy(x => x.FrameNumber).ToArray();
        }
    }

    public enum SingleFileResult
    {
        [XmlEnum(Name = "Ok")]
        Ok = 0,
        [XmlEnum(Name = "LowDetectionRate")]
        LowDetectionRate = 1,
        [XmlEnum(Name = "FrameCountTooLow")]
        FrameCountTooLow = 2,
        [XmlEnum(Name = "Error")]
        Error = 3,
    }
}
