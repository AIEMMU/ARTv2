using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ArtLibrary.ModelInterface.Skeletonisation
{
    public interface ISpineFinding : IModelObjectBase
    {
        int NumberOfIterations
        {
            get;
            set;
        }

        int NumberOfCycles
        {
            get;
            set;
        }

        Image<Gray, Byte> SkeletonImage
        {
            get;
            set;
        }

        PointF[] GenerateSpine(PointF headPoint, PointF tailPoint);

        RotatedRect RotatedRectangle
        {
            get;
            set;
        }
    }
}
