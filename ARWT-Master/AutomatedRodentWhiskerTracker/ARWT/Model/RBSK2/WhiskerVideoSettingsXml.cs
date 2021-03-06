﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ARWT.ModelInterface.RBSK2;
using Emgu.CV.CvEnum;

namespace ARWT.Model.RBSK2
{
    public class WhiskerVideoSettingsXml
    {
        [XmlElement(ElementName = "CropScaleFactor")]
        public double CropScaleFactor
        {
            get;
            set;
        }
        [XmlElement(ElementName = "RepeatSmoothing")]
        public int RepeatSmoothing 
        {
            get;
            set;
        }
        [XmlElement(ElementName = "InterpolationType")]
        public int InterpolationType
        {
            get;
            set;
        }

        [XmlElement(ElementName = "ResolutionIncreaseScaleFactor")]
        public float ResolutionIncreaseScaleFactor
        {
            get;
            set;
        }

        [XmlElement(ElementName = "OrientationResolution")]
        public double OrientationResolution
        {
            get;
            set;
        }

        [XmlElement(ElementName = "RemoveDuds")]
        public bool RemoveDuds
        {
            get;
            set;
        }

        [XmlElement(ElementName = "LineMinIntensity")]
        public byte LineMinIntensity
        {
            get;
            set;
        }

        [XmlElement(ElementName = "LowerBound")]
        public int LowerBound
        {
            get;
            set;
        }

        [XmlElement(ElementName = "UpperBound")]
        public int UpperBound
        {
            get;
            set;
        }
        [XmlElement(ElementName = "track")]
        public bool track
        {
            get;
            set;
        }
        public WhiskerVideoSettingsXml()
        {
            
        }

        public WhiskerVideoSettingsXml(IWhiskerVideoSettings settings)
        {
            track = settings.track;
            CropScaleFactor = settings.CropScaleFactor;
            RepeatSmoothing = settings.RepeatSmooths;
            InterpolationType = (int)settings.InterpolationType;
            LineMinIntensity = settings.LineMinIntensity;
            LowerBound = settings.LowerBound;
            OrientationResolution = settings.OrientationResolution;
            RemoveDuds = settings.RemoveDuds;
            ResolutionIncreaseScaleFactor = settings.ResolutionIncreaseScaleFactor;
            UpperBound = settings.UpperBound;
        }

        public IWhiskerVideoSettings GetWhiskerVideoSettings()
        {
            WhiskerVideoSettings settings = new WhiskerVideoSettings();
            settings.track = track;
            settings.CropScaleFactor = CropScaleFactor;
            settings.RepeatSmooths = RepeatSmoothing;
            settings.InterpolationType = (Inter)InterpolationType;
            settings.LineMinIntensity = LineMinIntensity;
            settings.LowerBound = LowerBound;
            settings.OrientationResolution = OrientationResolution;
            settings.RemoveDuds = RemoveDuds;
            settings.ResolutionIncreaseScaleFactor = ResolutionIncreaseScaleFactor;
            settings.UpperBound = UpperBound;
            settings.DataLoadComplete();
            return settings;
        }
    }
}
