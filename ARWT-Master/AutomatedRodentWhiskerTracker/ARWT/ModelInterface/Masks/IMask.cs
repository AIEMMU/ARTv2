using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ARWT.ModelInterface.Masks
{
    public interface IMask : IModelObjectBase
    {
        Point[] MaskPoints
        {
            get;
            set;
        }

        double LowerDistance
        {
            get;
            set;
        }

        double UpperDistance
        {
            get;
            set;
        }

        Image<Gray, byte> MaskImage
        {
            get;
            set;
        }

        Image<Gray, byte> Image
        {
            get;
            set;
        }
    }
}
