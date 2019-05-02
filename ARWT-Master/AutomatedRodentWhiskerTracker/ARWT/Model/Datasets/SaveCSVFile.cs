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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using ArtLibrary.Extensions;
using ArtLibrary.Model.Behaviours;
using ArtLibrary.Model.Boundries;
using ArtLibrary.Model.XmlClasses;
using ArtLibrary.ModelInterface.Behaviours;
using ArtLibrary.ModelInterface.Boundries;
using ARWT.Foot.centroidTracker;
using ARWT.Model.RBSK2;
using ARWT.Model.Results;
using ARWT.Model.Whiskers;
using ARWT.ModelInterface.Datasets;
using ARWT.ModelInterface.Feet;
using ARWT.ModelInterface.RBSK2;
using ARWT.ModelInterface.Results;
using ARWT.ModelInterface.Whiskers;
using SingleFileResult = ARWT.Model.Results.SingleFileResult;

namespace ARWT.Model.Datasets
{
    internal class SaveCSVFile : ModelObjectBase, iSaveCSVFile
    {
        
        private bool m_type = false; 
        public void saveCSV(string fileLocation, IMouseDataExtendedResult data, bool exportType = false)
        {
            m_type = exportType;
            saveCSV(fileLocation,  data.VideoOutcome, data.Results );
        }


        public void saveCSV(string fileLocation,  SingleFileResult result, Dictionary<int, ISingleFrameExtendedResults> data)
        {
            if (string.IsNullOrWhiteSpace(fileLocation))
            {
                return;
            }
            if (result != SingleFileResult.Ok)
            {
                File.AppendAllText(fileLocation, "");
            }
            if(data == null)
            {
                return;
            }
            StringBuilder csvOutput = new StringBuilder();

            //could add switch type depending on the export type
            if (m_type == false){
                csvOutput.AppendLine("Frame,nosepointX,nosePointY,midPointX,midPointY,centroidPointX,centroidbodyPointY,orientationX,orientationY,avgLeftWhiskerAngle,avgRightWhiskerAngle,leftWhiskerCoords,rightWhiskerCoords,leftWhiskerAngles,rightWhiskerAngles,leftWhiskerAvgIntensities,rightWhiskerAvgIntensities,leftWhiskerLineP1,rightWhiskerLineP1,leftWhiskerLineP2,rightWhiskerLineP2,AllLeftWhiskerCoords,AllRightWhiskerCoords,AllLeftWhiskerAngles,AllRightWhiskerAngles,AllLeftWhiskerAvgIntensities,AllRightWhiskerAvgIntensities,AllLeftWhiskerLineP1,AllRightWhiskerLineP1,AllLeftWhiskerLineP2,AllRightWhiskerLineP2");
            }else
            {
                csvOutput.AppendLine("Frame,nosepointX,nosePointY,midPointX,midPointY,centroidPointX,centroidbodyPointY,orientationX,orientationY,avgLeftWhiskerAngle,avgRightWhiskerAngle,meanleftWhiskerX,meanleftWhiskeryY,meanRightWhiskerX,meanRightWhiskerY,meanLeftWhiskerAngles,meanRightWhiskerAngles,meanleftWhiskerLineP1X,meanleftWhiskerLineP1Y,meanRightWhiskerLineP1X,meanRightWhiskerLineP1Y,meanleftWhiskerLineP2X,meanleftWhiskerLineP2Y,meanRightWhiskerLineP2X,meanRightWhiskerLineP2Y,meanAllleftWhiskerX,meanAllleftWhiskeryY,meanAllRightWhiskerX,meanAllRightWhiskerY,meanAllLeftWhiskerAngles,meanAllRightWhiskerAngles,meanAllleftWhiskerLineP1X,meanAllleftWhiskerLineP1Y,meanAllRightWhiskerLineP1X,meanAllRightWhiskerLineP1Y,meanAllleftWhiskerLineP2X,meanAllleftWhiskerLineP2Y,meanAllRightWhiskerLineP2X,meanAllRightWhiskerLineP2Y,BodyPoints");
            }
            IWhiskerAverageAngles angles = new WhiskerAverageAngles();
            angles.StartFrame = 0;
            angles.EndFrame = data.Count- 1;
            var allAngles = angles.GetWhiskerAngles(data);
            foreach (KeyValuePair<int, ISingleFrameExtendedResults> item in data) 
            {
                var obj = item.Value;
                var left = allAngles[0];
                var right = allAngles[1];
                if (item.Key == 276)
                {
                    int bob = 0;
                }

                string whiskersData = processData(obj.Whiskers);
                string allWhiskerData = processData(obj.AllWhiskers);
                //writing file
                string moo = $"{whiskersData},{allWhiskerData}";
                string headPoints = "\"[]\"";


                csvOutput.AppendLine($"{item.Key},{obj.HeadPoint.X},{obj.MidPoint.X},{obj.MidPoint.Y},{obj.HeadPoint.Y},{obj.Centroid.X},{obj.Centroid.Y},{obj.Orientation.X},{obj.Orientation.Y},{avgAngles(left, item.Key)},{avgAngles(right, item.Key)},{whiskersData},{allWhiskerData},{bodyPoint(obj)}");

            }

            File.WriteAllText(fileLocation, csvOutput.ToString());
        }

        private string avgAngles(Dictionary<int,double> whisker, int key)
        {
            double value;
            if (whisker.TryGetValue(key, out value))
            {
               return $"{value}";
            }
            return "";
        }
        private string processData(ModelInterface.Whiskers.IWhiskerCollection WhiskersData)
        {
            if (WhiskersData != null)
            {
                (string, string, string, string, string) leftWhiskers = ("", "" ,"", "",""); 
                (string, string, string, string, string) rightWhiskers = ("", "" , "", "", ""); ;
                if (m_type != true)
                {
                    leftWhiskers = processRawWhiskerData(WhiskersData.LeftWhiskers);
                    rightWhiskers = processRawWhiskerData(WhiskersData.RightWhiskers);
                }else
                {
                    leftWhiskers = processMeanWhiskerData(WhiskersData.LeftWhiskers);
                    rightWhiskers = processMeanWhiskerData(WhiskersData.RightWhiskers);
                }
                //return $"\"[{leftWhiskers.Item1}]\",\"[{rightWhiskers.Item1}]\",\"[{leftWhiskers.Item2}]\",\"[{rightWhiskers.Item2}]\",\"[{leftWhiskers.Item3}]\",\"[{rightWhiskers.Item3}]\",\"[{leftWhiskers.Item4}]\",\"[{rightWhiskers.Item4}]\",\"[{leftWhiskers.Item5}]\",\"[{rightWhiskers.Item5}]\"";
                string moo = $"{ leftWhiskers.Item1},{ leftWhiskers.Item2},{ rightWhiskers.Item1},{ rightWhiskers.Item2},{ leftWhiskers.Item3},{ rightWhiskers.Item3},{ leftWhiskers.Item4},{ rightWhiskers.Item4},{ leftWhiskers.Item5},{ rightWhiskers.Item5}";
                return $"{leftWhiskers.Item1},{leftWhiskers.Item2},{rightWhiskers.Item1},{rightWhiskers.Item2},{leftWhiskers.Item3},{rightWhiskers.Item3},{leftWhiskers.Item4},{rightWhiskers.Item4},{leftWhiskers.Item5},{rightWhiskers.Item5}";
            }
            return ",,,,,,,,,,,,,";
        }
        
        private string bodyPoint(ISingleFrameExtendedResults data)
        {
            if (data.BodyContour != null)
            {
                List<string> coords = new List<string>();
                
                foreach (var contour in data.BodyContour)
                {
                    coords.Add($"[[{contour.X},{contour.Y}]]");
                }

                return $"\"[{string.Join(",",coords )}]\"";

            }
            return "\"[]\"";
        }
        private (string, string, string, string, string) processRawWhiskerData(ModelInterface.Whiskers.IWhiskerSegment[] whiskers)
        {

            List<double> angles = new List<double>();
            List<double> avgIntensity = new List<double>();
            List<string> coords = new List<string>();
            List<string> lineP1 = new List<string>();
            List<string> lineP2 = new List<string>();

            if (whiskers != null)
            {
                foreach (ModelInterface.Whiskers.IWhiskerSegment whisk in whiskers)
                {
                    angles.Add(whisk.Angle);
                    avgIntensity.Add(whisk.AvgIntensity);
                    coords.Add($"[{whisk.X},{whisk.Y}]");
                    lineP1.Add($"[{whisk.Line.P1.X},{whisk.Line.P1.Y}]");
                    lineP2.Add($"[{whisk.Line.P2.X},{whisk.Line.P2.Y}]");
                }
                return ($"\"[{string.Join(",", coords)}]\"",$"\"[{string.Join(",", angles)}]\"",$"\"[{string.Join(",", avgIntensity)}]\"",$"\"[{string.Join(",", lineP1)}]\"",$"\"[{string.Join(",", lineP2)}]\"");
            }

            return ("", "", "", "", "");
            
        }

        private (string, string, string, string, string) processMeanWhiskerData(ModelInterface.Whiskers.IWhiskerSegment[] whiskers)
        {
           
            float angles = new float();
            float xCoords = new float();
            float yCoords = new float();
            float lineP1X = new float();
            float lineP1Y = new float();
            float lineP2X = new float();
            float lineP2Y = new float();


            if (whiskers != null)
            {
                float count = 0;
                foreach (ModelInterface.Whiskers.IWhiskerSegment whisk in whiskers)
                {
                    angles += (float)whisk.Angle;
                    xCoords += (float)whisk.X;
                    yCoords += (float)whisk.Y;
                    lineP1X += (float)whisk.Line.P1.X;
                    lineP2X += (float)whisk.Line.P2.X;
                    lineP1Y += (float)whisk.Line.P1.Y;
                    lineP2Y += (float)whisk.Line.P2.Y;
                    count++;
                }
                angles /= count;
                xCoords /= count;
                yCoords /= count;
                lineP1X /= count;
                lineP2X /= count;
                lineP1Y /= count;
                lineP2Y /= count;
                return ($"{xCoords}", $"{yCoords}", $"{angles}", $"{lineP1X},{lineP1Y}", $"{lineP2X},{lineP2Y}");
            }

            return ("","","",",",",");
        }

        public void saveFeetCSV(string fileLocation, IMouseDataExtendedResult data, bool exportType = false)
        {
            m_type = exportType;
            saveFeetCSV(fileLocation, data.VideoOutcome, data.Results);
        }

        public void saveFeetCSV(string fileLocation, SingleFileResult result, Dictionary<int, ISingleFrameExtendedResults> data)
        {
            if (string.IsNullOrWhiteSpace(fileLocation))
            {
                return;
            }
            if (result != SingleFileResult.Ok)
            {
                File.AppendAllText(fileLocation, "");
            }
            if (data == null)
            {
                return;
            }
            StringBuilder csvOutput = new StringBuilder();

            csvOutput.AppendLine("Frame,LFCentroidX,LFCentroidX,LFWidth,LFHeight,LFMinX,LFMinY,LFMaxX,LFMaxY,LHCentroidX,LHCentroidX,LHWidth,LHHeight,LHMinX,LHMinY,LHMaxX,LHMaxY,RFCentroidX,RFCentroidX,RFWidth,RFHeight,RFMinX,RFMinY,RFMaxX,RFMaxY,RHCentroidX,RHCentroidX,RHWidth,RHHeight,RHMinX,RHMinY,RHMaxX,RHMaxY,");
            foreach(KeyValuePair<int, ISingleFrameExtendedResults> kvp in data)
            {
                IFootCollection feet = kvp.Value.FeetCollection;
                csvOutput.AppendLine($"{kvp.Key}, {getFootData(feet.leftfront)}, {getFootData(feet.leftHind)}, {getFootData(feet.rightfront)}, {getFootData(feet.rightHind)}");
            }
            File.WriteAllText(fileLocation, csvOutput.ToString());
        }

        private object getFootData(IfeetID foot)
        {
            if (foot == null)
            {
                return ",,,,,,,";
            }else
            {
                IFootPlacement item = foot.value;
                return $"{item.centroidX},{item.centroidY},{item.width},{item.height},{item.minX},{item.minY},{item.maxX},{item.maxY}";
            }
        }
    }
}
