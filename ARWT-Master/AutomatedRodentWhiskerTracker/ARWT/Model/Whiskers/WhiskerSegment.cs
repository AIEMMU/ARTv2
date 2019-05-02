using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using ARWT.ModelInterface.Whiskers;

namespace ARWT.Model.Whiskers
{
    internal class WhiskerSegment : ModelObjectBase, IWhiskerSegment
    {
        private int m_X;
        public int X
        {
            get
            {
                return m_X;
            }
            set
            {
                if (Equals(m_X, value))
                {
                    return;
                }

                m_X = value;

                MarkAsDirty();
            }
        }

        private int m_Y;
        public int Y
        {
            get
            {
                return m_Y;
            }
            set
            {
                if (Equals(m_Y, value))
                {
                    return;
                }

                m_Y = value;

                MarkAsDirty();
            }
        }

        private double m_Angle;
        public double Angle
        {
            get
            {
                return m_Angle;
            }
            set
            {
                if (Equals(m_Angle, value))
                {
                    return;
                }

                m_Angle = value;

                MarkAsDirty();
            }
        }

        private LineSegment2D m_Line;
        public LineSegment2D Line
        {
            get
            {
                return m_Line;
            }
            set
            {
                if (Equals(m_Line, value))
                {
                    return;
                }

                m_Line = value;

                MarkAsDirty();
            }
        }

        private Color m_Color;
        public Color Color
        {
            get
            {
                return m_Color;
            }
            set
            {
                if (Equals(m_Color, value))
                {
                    return;
                }

                m_Color = value;

                MarkAsDirty();
            }
        }

        private double _AvgIntensity;
        public double AvgIntensity
        {
            get
            {
                return _AvgIntensity;
            }
            set
            {
                if (Equals(_AvgIntensity, value))
                {
                    return;
                }

                _AvgIntensity = value;

                MarkAsDirty();
            }
        }


        public double Distance(IWhiskerSegment whisker)
        {
            double deltaX = whisker.X - X;
            double deltaY = whisker.Y - Y;

            double xSquared = Math.Pow(deltaX, 2);
            double ySquared = Math.Pow(deltaY, 2);

            return Math.Sqrt(xSquared + ySquared);
        }

        public double DistanceSquared(IWhiskerSegment whisker)
        {
            double deltaX = whisker.X - X;
            double deltaY = whisker.Y - Y;

            double xSquared = Math.Pow(deltaX, 2);
            double ySquared = Math.Pow(deltaY, 2);

            return xSquared + ySquared;
        }

        public double DeltaAngle(IWhiskerSegment whisker)
        {
            return Math.Abs(Angle - whisker.Angle);
        }
    }
}
