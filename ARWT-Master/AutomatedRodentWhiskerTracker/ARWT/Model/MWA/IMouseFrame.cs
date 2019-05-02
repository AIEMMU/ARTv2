using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ARWT.Model.MWA
{
    public interface IMouseFrame : IDisposable
    {
        int FrameNumber
        {
            get;
            set;
        }

        int IndexNumber
        {
            get;
            set;
        }

        Image<Bgr, Byte> Frame
        {
            get;
            set;
        }

        double OriginalWidth
        {
            get;
        }

        double OriginalHeight
        {
            get;
        }

        IWhisker[] Whiskers
        {
            get;
            set;
        }
    }
}
