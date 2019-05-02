using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using ARWT.ModelInterface.Masks;
using ARWT.Resolver;

namespace ARWT.Model.Masks
{
    internal class MaskHolder : ModelObjectBase, IMaskHolder
    {
        private List<IMask> m_LeftMasks = new List<IMask>();
        private List<IMask> m_RightMasks = new List<IMask>();
        private int m_MaskCount = 0;

        private List<ILine> m_LeftLines = new List<ILine>();
        private List<ILine> m_RightLines = new List<ILine>();

        public List<Point[]> LeftPoints
        {
            get;
            set;
        }

        public List<Point[]> RightPoints
        {
            get;
            set;
        }

        public List<IMask> LeftMasks
        {
            get
            {
                return m_LeftMasks;
            }
            private set
            {
                if (ReferenceEquals(m_LeftMasks, value))
                {
                    return;
                }

                m_LeftMasks = value;

                MarkAsDirty();
            }
        }

        public List<IMask> RightMasks
        {
            get
            {
                return m_RightMasks;
            }
            private set
            {
                if (ReferenceEquals(m_RightMasks, value))
                {
                    return;
                }

                m_RightMasks = value;

                MarkAsDirty();
            }
        }

        public List<ILine> LeftLines
        {
            get
            {
                return m_LeftLines;
            }
            private set
            {
                if (ReferenceEquals(m_LeftLines, value))
                {
                    return;
                }

                m_LeftLines = value;

                MarkAsDirty();
            }
        }

        public List<ILine> RightLines
        {
            get
            {
                return m_RightLines;
            }
            private set
            {
                if (ReferenceEquals(m_RightLines, value))
                {
                    return;
                }

                m_RightLines = value;

                MarkAsDirty();
            }
        }

        public int MaskCount
        {
            get
            {
                return m_MaskCount;
            }
            private set
            {
                if (Equals(m_MaskCount, value))
                {
                    return;
                }

                m_MaskCount = value;
                
                MarkAsDirty();
            }
        }

        public void AddMask(Point[] leftPoints, Point[] rightPoints, double lowerDist, double upperDist)
        {
            MaskCount++;
            IMask leftMask = ModelResolver.Resolve<IMask>();
            IMask rightMask = ModelResolver.Resolve<IMask>();

            leftMask.MaskPoints = leftPoints;
            leftMask.LowerDistance = lowerDist;
            leftMask.UpperDistance = upperDist;

            rightMask.MaskPoints = rightPoints;
            rightMask.LowerDistance = lowerDist;
            rightMask.UpperDistance = upperDist;

            LeftMasks.Add(leftMask);
            RightMasks.Add(rightMask);
        }

        public void AddLine(Point[] leftPoints, Point[] rightPoints, double dist)
        {
            ILine leftLine = ModelResolver.Resolve<ILine>();
            ILine rightLine = ModelResolver.Resolve<ILine>();

            leftLine.LinePoints = leftPoints;
            leftLine.Distance = dist;

            rightLine.LinePoints = rightPoints;
            rightLine.Distance = dist;

            LeftLines.Add(leftLine);
            RightLines.Add(rightLine);
        }

        public int GetMaskIndexForDistance(double dist)
        {
            for (int i = 0; i < LeftMasks.Count; i++)
            {
                IMask mask = LeftMasks[i];

                if (dist > mask.LowerDistance && dist < mask.UpperDistance)
                {
                    return i;
                }
            }

            return -1;
        }

        public void PerformActionOnImages(Action<Image<Gray, byte>> action)
        {
            foreach (var image in LeftMasks)
            {
                action(image.Image);
            }

            foreach (var image in RightMasks)
            {
                action(image.Image);
            }
        }
    }
}
