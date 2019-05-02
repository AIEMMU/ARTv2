﻿using System.Linq;
using ArtLibrary.Model.XmlClasses;
using ArtLibrary.ModelInterface.Boundries;

namespace ArtLibrary.Model.Boundries
{
    internal class OuterBoundary : BoundaryBase, IOuterBoundary
    {
        public OuterBoundary() : base(BoundaryType.Outer)
        {

        }

        public override BoundaryBaseXml GetData()
        {
            int id = Id;
            PointXml[] points = Points.Select(x => new PointXml(x.X, x.Y)).ToArray();

            OuterBoundaryXml data = new OuterBoundaryXml(id, points);

            return data;
        }
    }
}
