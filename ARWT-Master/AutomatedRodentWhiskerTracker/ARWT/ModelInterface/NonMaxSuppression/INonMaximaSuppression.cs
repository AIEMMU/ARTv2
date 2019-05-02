using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ARWT.ModelInterface.NonMaxSuppression
{
    public interface INonMaximaSuppression : IModelObjectBase
    {
        void Apply(Image<Gray, float> img, Image<Gray, Byte> mask, int kernalSize);
        void Apply(Image<Gray, byte> img);

        void Apply(Image<Gray, byte> img, int topPointsToKeep);
    }
}
