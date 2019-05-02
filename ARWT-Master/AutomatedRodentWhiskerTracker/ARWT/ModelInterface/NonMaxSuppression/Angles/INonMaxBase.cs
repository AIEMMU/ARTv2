using Emgu.CV;
using Emgu.CV.Structure;

namespace ARWT.ModelInterface.NonMaxSuppression.Angles
{
    public interface INonMaxBase : IModelObjectBase
    {
        double Angle
        {
            get;
            set;
        }

        void Apply(Image<Gray, float> img);
    }
}
