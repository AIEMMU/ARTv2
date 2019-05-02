using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ArtLibrary.ModelInterface.Skeletonisation
{
    public interface ISkeleton : IModelObjectBase
    {
        int Iterations
        {
            get;
            set;
        }

        Image<Gray, Byte> GetSkeleton(Image<Gray, Byte> input);
    }
}
