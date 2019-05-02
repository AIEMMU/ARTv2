using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.ModelInterface;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ArtLibrary.ModelInterface.Video
{
    public interface ILargeMemoryVideo : IModelObjectBase
    {
        Image<Bgr, Byte>[] Frames
        {
            get;
            set;
        }

        int FrameCount
        {
            get;
        }

        void LoadVideo(string filePath);
    }
}
