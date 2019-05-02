﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.ModelInterface;
using Emgu.CV;
using Emgu.CV.Structure;
using ArtLibrary.ModelInterface.Video;

namespace ArtLibrary.ModelInterface.Motion.MotionBackground
{
    public interface IMotionBackground : IModelObjectBase
    {
        IVideo Video { get; set; }

        int MotionLength { get; set; }

        void GenerateMotionBackground(double threshold, out Image<Gray, Byte> binaryBackground, Rectangle roi, int startFrame = 0);

        void SetVideo(string fileName);
        void SetVideo(IVideo vieo);
    }
}
