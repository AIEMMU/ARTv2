﻿using System;
using System.Xml.Serialization;
using ArtLibrary.Model.XmlClasses;
using ArtLibrary.ModelInterface.Boundries;

namespace ArtLibrary.Model.Boundries
{
    [XmlInclude(typeof(BoxBoundaryXml))]
    [XmlInclude(typeof(ArtefactsBoundaryXml))]
    [XmlInclude(typeof(CircleBoundaryXml))]
    [XmlInclude(typeof(OuterBoundaryXml))]
    [Serializable]
    public abstract class BoundaryBaseXml
    {
        [XmlElement(ElementName = "ID")]
        public int Id
        {
            get;
            set;
        }

        [XmlArray(ElementName = "Points")]
        [XmlArrayItem(ElementName = "Point")]
        public PointXml[] Points
        {
            get;
            set;
        }

        protected BoundaryBaseXml()
        {
            
        }

        protected BoundaryBaseXml(int id, PointXml[] points)
        {
            Id = id;
            Points = points;
        }

        public abstract IBoundaryBase GetBoundary();
    }
}
