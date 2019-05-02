/*Automated Rodent Tracker - A program to automatically track rodents
Copyright(C) 2015 Brett Michael Hewitt

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.If not, see<http://www.gnu.org/licenses/>.*/

using System;
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

        void GenerateMotionBackground(double threshold, int lineThreshold, out Image<Gray, Byte> binaryBackground, out Image<Gray, Byte> extraImage, Rectangle roi, int startFrame = 0);
        void GenerateMotionBackground(double threshold, int lineThreshold, out Image<Gray, Byte> binaryBackground, Rectangle roi, int startFrame = 0);

        void SetVideo(string fileName);
        void SetVideo(IVideo vieo);
    }
}
