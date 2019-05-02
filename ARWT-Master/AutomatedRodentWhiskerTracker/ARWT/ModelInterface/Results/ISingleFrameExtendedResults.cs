using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ArtLibrary.ModelInterface.Results;
using ARWT.Model.Feet;
using ARWT.ModelInterface.Feet;
using ARWT.ModelInterface.Whiskers;

namespace ARWT.ModelInterface.Results
{
    public interface ISingleFrameExtendedResults : IModelObjectBase
    {
        PointF[] HeadPoints
        {
            get;
            set;
        }

        System.Drawing.Point[] BodyContour
        {
            get;
            set;
        }

        PointF HeadPoint
        {
            get;
            set;
        }

        PointF MidPoint
        {
            get;
            set;
        }

        PointF SmoothedHeadPoint
        {
            get;
            set;
        }

        Vector Orientation
        {
            get;
            set;
        }

        double CentroidSize
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

        double Velocity
        {
            get;
            set;
        }

        double AngularVelocity
        {
            get;
            set;
        }

        double Distance
        {
            get;
            set;
        }

        PointF Centroid
        {
            get;
            set;
        }

        double CentroidVelocity
        {
            get;
            set;
        }

        IWhiskerCollection Whiskers
        {
            get;
            set;
        }
        IFootCollection FeetCollection
        {
            get;
            set;
        }
        IWhiskerCollection AllWhiskers
        {
            get;
            set;
        }

        IWhiskerCollection BestTrackedWhisker
        {
            get;
            set;
        }

        double CentroidDistance
        {
            get;
            set;
        }

        bool IsInteracting
        {
            get;
            set;
        }
    }
}
