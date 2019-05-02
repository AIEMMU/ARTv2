using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using ARWT.ModelInterface.Masks;

namespace ARWT.Model.Masks
{
    internal class Line : ModelObjectBase, ILine
    {
        private Point[] m_LinePoints;
        private double m_Distance;
        private Image<Gray, byte> m_MaskImage;
        private Image<Gray, byte> m_Image;

        public Point[] LinePoints
        {
            get
            {
                return m_LinePoints;
            }
            set
            {
                if (ReferenceEquals(m_LinePoints, value))
                {
                    return;
                }

                m_LinePoints = value;

                MarkAsDirty();
            }
        }

        public double Distance
        {
            get
            {
                return m_Distance;
            }
            set
            {
                if (Equals(m_Distance, value))
                {
                    return;
                }

                m_Distance = value;

                MarkAsDirty();
            }
        }

        public Image<Gray, byte> MaskImage
        {
            get
            {
                return m_MaskImage;
            }
            set
            {
                if (ReferenceEquals(m_MaskImage, value))
                {
                    return;
                }

                m_MaskImage = value;

                MarkAsDirty();
            }
        }

        public Image<Gray, byte> Image
        {
            get
            {
                return m_Image;
            }
            set
            {
                if (ReferenceEquals(m_Image, value))
                {
                    return;
                }

                m_Image = value;

                MarkAsDirty();
            }
        }
    }
}
