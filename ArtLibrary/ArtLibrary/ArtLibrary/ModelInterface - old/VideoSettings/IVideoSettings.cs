using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.ModelInterface;
using ArtLibrary.ModelInterface.Boundries;
using Emgu.CV;
using Emgu.CV.Structure;
using ArtLibrary.ModelInterface.Video;

namespace ArtLibrary.ModelInterface.VideoSettings
{
    public interface IVideoSettings : IModelObjectBase
    {
        int ThresholdValue
        {
            get;
            set;
        }

        double MaxThreshold
        {
            get;
            set;
        }

        double MinimumInteractionDistance
        {
            get;
            set;
        }

        double GapDistance
        {
            get;
            set;
        }

        int ThresholdValue2
        {
            get;
            set;
        }

        string FileName
        {
            get;
            set;
        }

        Point[] MousePoints
        {
            get; set; }

        List<Point[]> Artefacts
        {
            get;
            set;
        }

        List<Point[]> Boundries
        {
            get;
            set;
        }

        List<RotatedRect> Boxes
        {
            get;
            set;
        }

        Rectangle Roi
        {
            get;
            set;
        }

        void GeneratePreview(IVideo video, out Image<Gray, Byte> binaryBackground, out IEnumerable<IBoundaryBase> boundaries, int motionLength = 100, int startFrame = 0);
    }
}
