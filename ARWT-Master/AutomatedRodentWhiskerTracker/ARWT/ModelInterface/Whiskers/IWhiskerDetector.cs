using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ArtLibrary.ModelInterface.Results;
using ARWT.ModelInterface.RBSK2;
using Emgu.CV;
using Emgu.CV.Structure;
using ARWT.ModelInterface.Results;
using Point = System.Drawing.Point;

namespace ARWT.ModelInterface.Whiskers
{
    public interface IWhiskerDetector : IModelObjectBase
    {
        //double LineLength
        //{
        //    get;
        //    set;
        //}

        double OrientationResolution
        {
            get;
            set;
        }


        //Dictionary<double, Vector> OrientationTable
        //{
        //    get;
        //}

        List<IWhiskerSegment>[] FindWhiskers(Image<Gray, byte> grayImage, Image<Gray, byte> binaryBackground, PointF[] headPoints, Point[] bodyContour, int sizeRatio, ISingleFrameExtendedResults results, IWhiskerVideoSettings settings);
        List<IWhiskerSegment>[] FindWhiskers(Image<Gray, byte> grayImage, Image<Gray, byte> binaryBackground, PointF[] headPoints, Point[] bodyContour, int sizeRatio, ISingleFrameExtendedResults results, IWhiskerVideoSettings settings, IWhiskerSegment leftSeg, IWhiskerSegment rightSeg);
        void Debug(Image<Gray, byte> frame, Image<Gray, byte> backgroundImage, int videoWidth, int videoHeight, ISingleFrameExtendedResults currentFrame, string folderLoc, IWhiskerVideoSettings settings);

        List<IWhiskerSegment>[] FindWhiskersDebug(Image<Gray, byte> grayImage, Image<Gray, byte> binaryBackground, PointF[] headPoints, Point[] bodyContour, int sizeRatio, string folderLoc, ISingleFrameExtendedResults results, IWhiskerVideoSettings settings, bool showImages = true);
        void RemoveDudWhiskers(PointF midPoint, List<IWhiskerSegment> whiskers, Vector orientation, double minAngleDelta, bool isLeft);
    }
}
