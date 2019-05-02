using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ARWT.ModelInterface.Feet;
using ARWT.ModelInterface.Results;
using ARWT.ModelInterface.Whiskers;

namespace ARWT.Model.Results
{
    internal class SingleFrameExtendedResults : ModelObjectBase, ISingleFrameExtendedResults
    {
        //public ArtLibrary.Model.ModelObjectState ModelObjectState
        //{
        //    get;
        //    private set;
        //}

        public PointF[] HeadPoints
        {
            get;
            set;
        }

        public System.Drawing.Point[] BodyContour
        {
            get;
            set;
        }

        public PointF HeadPoint
        {
            get;
            set;
        }

        public PointF MidPoint
        {
            get;
            set;
        }

        public PointF SmoothedHeadPoint
        {
            get;
            set;
        }

        public Vector Orientation
        {
            get;
            set;
        }

        public double CentroidSize
        {
            get;
            set;
        }

        public double PelvicArea
        {
            get;
            set;
        }

        public double PelvicArea2
        {
            get;
            set;
        }

        public double PelvicArea3
        {
            get;
            set;
        }

        public double PelvicArea4
        {
            get;
            set;
        }

        public double Dummy
        {
            get;
            set;
        }

        public double Velocity
        {
            get;
            set;
        }

        public double AngularVelocity
        {
            get;
            set;
        }

        public double Distance
        {
            get;
            set;
        }

        public PointF Centroid
        {
            get;
            set;
        }

        public double CentroidVelocity
        {
            get;
            set;
        }

        public IWhiskerCollection Whiskers
        {
            get;
            set;
        }

        public IFootCollection FeetCollection
        {
            get;
            set;
        }

        public IWhiskerCollection AllWhiskers
        {
            get;
            set;
        }

        public IWhiskerCollection BestTrackedWhisker
        {
            get;
            set;
        }

        public bool IsInteracting
        {
            get;
            set;
        }

        private double m_CentroidDistance;
        public double CentroidDistance
        {
            get
            {
                return m_CentroidDistance;
            }
            set
            {
                if (Equals(m_CentroidDistance, value))
                {
                    return;
                }

                m_CentroidDistance = value;

                MarkAsDirty();
            }
        }
    }
}
