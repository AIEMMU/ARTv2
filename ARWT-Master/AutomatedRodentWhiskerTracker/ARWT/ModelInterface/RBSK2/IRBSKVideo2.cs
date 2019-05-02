using System;
using System.Collections.Generic;
using System.Drawing;
using ArtLibrary.Model.Events;
using ArtLibrary.ModelInterface.Results;
using ArtLibrary.ModelInterface.Video;
using Emgu.CV;
using Emgu.CV.Structure;
using ARWT.ModelInterface.Results;
using ARWT.ModelInterface.Whiskers;

namespace ARWT.ModelInterface.RBSK2
{
    public interface IRBSKVideo2 : IModelObjectBase, IDisposable
    {
        event RBSKVideoUpdateEventHandler ProgressUpdates;

        IVideo Video
        {
            get;
            set;
        }

        Dictionary<int, ISingleFrameExtendedResults> HeadPoints
        {
            get;
            set;
        }

        //Dictionary<int, IWhiskerCollection> WhiskerResults
        //{
        //    get;
        //    set;
        //}

        //Dictionary<int, IWhiskerCollection> WhiskerResultsAll
        //{
        //    get;
        //    set;
        //} 

        bool FindFoot { get; set; }
        bool FindWhiskers
        {
            get;
            set;
        }

        Dictionary<int, Tuple<PointF[], double>> SecondPassHeadPoints
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

        double MovementDelta
        {
            get;
            set;
        }

        bool Cancelled
        {
            get;
            set;
        }

        bool Paused
        {
            get;
            set;
        }

        Image<Gray, Byte> BackgroundImage
        {
            get;
            set;
        }

        Rectangle Roi
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

        void Process();
        void GetHeadAndBody(Image<Bgr, byte> frame, out PointF[] headPoints, out Point[] body);
        IWhiskerCollection ProcessWhiskersForSingleFrame(Image<Gray, byte> img, PointF[] headPoints, Point[] bodyContour);
    }
}
