using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.Model.Events;
using ArtLibrary.ModelInterface;
using ArtLibrary.ModelInterface.Results;
using Emgu.CV;
using Emgu.CV.Structure;
using ArtLibrary.ModelInterface.Video;

namespace ArtLibrary.ModelInterface.RBSK
{
    public interface IRBSKVideo : IModelObjectBase, IDisposable
    {
        event RBSKVideoUpdateEventHandler ProgressUpdates;

        IVideo Video
        {
            get;
            set;
        }

        Dictionary<int, ISingleFrameResult> HeadPoints
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

        void Process();
    }
}
