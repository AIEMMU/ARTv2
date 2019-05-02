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
using ArtLibrary.Model.Results;
using ArtLibrary.Model.Video;
using ArtLibrary.Model.XmlClasses;
using ArtLibrary.ModelInterface.Behaviours;
using ArtLibrary.ModelInterface.Boundries;
using ArtLibrary.ModelInterface.Datasets;
using ArtLibrary.Model;
using ArtLibrary.ModelInterface.Results;

namespace ArtLibrary.Model.Datasets
{
    internal class SaveArtFile : ModelObjectBase, ISaveArtFile
    {
        //public void SaveFile(string fileLocation, string videoFileName, SingleFileResult result, Dictionary<int, ISingleFrameResult> headPoints, PointF[] motionTrack, PointF[] smoothedMotionTrack, Vector[] orientationTrack, IEnumerable<IBehaviourHolder> events, IEnumerable<IBoundaryBase> boundaries, Dictionary<IBoundaryBase, IBehaviourHolder[]> interactions, double minInteractionDistance, double gapDistance, int thresholdValue, int thresholdValue2, int startFrame, int endFrame, double frameRate, bool smoothMotion, double smoothingFactor, double centroidSize, double pelvicArea1, double pelvicArea2, double pelvicArea3, double pelvicArea4, string message = "")
        //{
        //    SaveFile(fileLocation, videoFileName, result, headPoints, motionTrack, smoothedMotionTrack, orientationTrack, events, boundaries, interactions, minInteractionDistance, gapDistance, thresholdValue, thresholdValue2, startFrame, endFrame, frameRate, smoothMotion, smoothingFactor, centroidSize, pelvicArea1, pelvicArea2, pelvicArea3, pelvicArea4, message);
        //}

        //public void SaveFile(string fileLocation, string videoFileName, IMouseDataResult data)
        //{
        //    SaveFile(fileLocation, videoFileName, data);
        //}

        public void SaveFile(string fileLocation, string videoFileName, IMouseDataResult data)
        {
            SaveFile(fileLocation, videoFileName, data.VideoOutcome, data.Results, data.MotionTrack, data.SmoothedMotionTrack, data.OrientationTrack, data.Events, data.Boundaries, data.InteractingBoundries, data.MinInteractionDistance, data.GapDistance, data.ThresholdValue, data.ThresholdValue2, data.StartFrame, data.EndFrame, data.FrameRate, data.SmoothMotion, data.SmoothFactor, data.CentroidSize, data.PelvicArea, data.PelvicArea2, data.PelvicArea3, data.PelvicArea4, data.Message);
        }

        public void SaveFile(string fileLocation, string videoFileName, SingleFileResult result, Dictionary<int, ISingleFrameResult> headPoints, PointF[] motionTrack, PointF[] smoothedMotionTrack, Vector[] orientationTrack, IEnumerable<IBehaviourHolder> events, IEnumerable<IBoundaryBase> boundaries, Dictionary<IBoundaryBase, IBehaviourHolder[]> interactions, double minInteractionDistance, double gapDistance, int thresholdValue, int thresholdValue2, int startFrame, int endFrame, double frameRate, bool smoothMotion, double smoothingFactor, double centroidSize, double pelvicArea1, double pelvicArea2, double pelvicArea3, double pelvicArea4, string message = "")
        {
            if (string.IsNullOrWhiteSpace(fileLocation))
            {
                return;
            }

            TrackedVideoXml fileXml;
            XmlSerializer serializer;

            if (result != SingleFileResult.Ok)
            {
                fileXml = new TrackedVideoXml(videoFileName, result, null, null, null, null, null, null, null, minInteractionDistance, gapDistance, thresholdValue, thresholdValue2, startFrame, endFrame, frameRate, smoothMotion, smoothingFactor, centroidSize, pelvicArea1, pelvicArea2, pelvicArea3, pelvicArea4, message);
                serializer = new XmlSerializer(typeof(TrackedVideoXml));

                using (StreamWriter writer = new StreamWriter(fileLocation))
                {
                    serializer.Serialize(writer, fileXml);
                }

                return;
            }

            int headCount = headPoints.Count;
            SingleFrameResultXml[] allPoints = new SingleFrameResultXml[headCount];
            for (int i = 0; i < headCount; i++)
            {
                if (headPoints[i].HeadPoints == null)
                {
                    allPoints[i] = null;
                }
                else
                {
                    allPoints[i] = new SingleFrameResultXml(headPoints[i]);
                }
            }

            DictionaryXml<int, SingleFrameResultXml> headPointsXml = new DictionaryXml<int, SingleFrameResultXml>(headPoints.Keys.ToArray(), allPoints);
            PointFXml[] motionTrackXml = motionTrack.Select(point => new PointFXml(point.X, point.Y)).ToArray();
            PointFXml[] smoothedMotionTrackXml = smoothedMotionTrack.Select(point => new PointFXml(point)).ToArray();
            VectorXml[] orientationTrackXml = orientationTrack.Select(vector => new VectorXml(vector)).ToArray();

            List<BoundaryBaseXml> boundariesXml = boundaries.Select(boundary => boundary.GetData()).ToList();

            List<BehaviourHolderXml> eventsXml = new List<BehaviourHolderXml>();
            foreach (IBehaviourHolder behaviour in events)
            {
                BoundaryBaseXml boundary = null;
                InteractionBehaviour interaction = behaviour.Interaction;
                int frameNumber = behaviour.FrameNumber;
                boundary = behaviour.Boundary.GetData();

                eventsXml.Add(new BehaviourHolderXml(boundary, interaction, frameNumber));
            }

            BoundaryBaseXml[] keys = interactions.Keys.Select(key => key.GetData()).ToArray();
            BehaviourHolderXml[][] values = interactions.Values.Select(value => value.Select(behavHolder => new BehaviourHolderXml(behavHolder.Boundary.GetData(), behavHolder.Interaction, behavHolder.FrameNumber)).ToArray()).ToArray();
            DictionaryXml<BoundaryBaseXml, BehaviourHolderXml[]> interactionBoundries = new DictionaryXml<BoundaryBaseXml, BehaviourHolderXml[]>(keys, values);

            fileXml = new TrackedVideoXml(videoFileName, result, headPointsXml, motionTrackXml, smoothedMotionTrackXml, orientationTrackXml, boundariesXml.ToArray(), eventsXml.ToArray(), interactionBoundries, minInteractionDistance, gapDistance, thresholdValue, thresholdValue2, startFrame, endFrame, frameRate, smoothMotion, smoothingFactor, centroidSize, pelvicArea1, pelvicArea2, pelvicArea3, pelvicArea4, message);
            serializer = new XmlSerializer(typeof(TrackedVideoXml));

            using (StreamWriter writer = new StreamWriter(fileLocation))
            {
                serializer.Serialize(writer, fileXml);
            }
        }
    }
}
