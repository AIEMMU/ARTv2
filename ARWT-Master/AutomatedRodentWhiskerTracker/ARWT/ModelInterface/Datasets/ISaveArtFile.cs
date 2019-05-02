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
using System.Windows;
using ArtLibrary.ModelInterface.Behaviours;
using ArtLibrary.ModelInterface.Boundries;
using ARWT.Model.Results;
using ARWT.ModelInterface.RBSK2;
using ARWT.ModelInterface.Results;

namespace ARWT.ModelInterface.Datasets
{
    public interface ISaveArtFile : IModelObjectBase
    {
        void SaveFile(string fileLocation, string videoFileName, IMouseDataExtendedResult data);
        void SaveFile(string fileLocation, string videoFileName, SingleFileResult result, Dictionary<int, ISingleFrameExtendedResults> headPoints, PointF[] motionTrack, PointF[] smoothedMotionTrack, Vector[] orientationTrack, IEnumerable<IBehaviourHolder> events, IEnumerable<IBoundaryBase> boundaries, Dictionary<IBoundaryBase, IBehaviourHolder[]> interactions, double minInteractionDistance, double gapDistance, int thresholdValue, int thresholdValue2, int startFrame, int endFrame, double frameRate, bool smoothMotion, double smoothingFactor, double centroidSize, double pelvicArea1, double pelvicArea2, double pelvicArea3, double pelvicArea4, double unitsToMilimeters, Rectangle roi, IWhiskerVideoSettings whiskerSettings, IFootVideoSettings feetSettings, string message = "");
        
    }
}
