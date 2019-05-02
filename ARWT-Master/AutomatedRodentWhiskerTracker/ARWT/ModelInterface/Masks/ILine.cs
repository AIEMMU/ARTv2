using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ARWT.ModelInterface.Masks
{
    public interface ILine : IModelObjectBase
    {
        Point[] LinePoints
        {
            get;
            set;
        }

        double Distance
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
