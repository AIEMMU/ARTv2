using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using ARWT.ModelInterface.Masks;

namespace ARWT.Model.Masks
{
    internal class Mask : ModelObjectBase, IMask
    {
        private Point[] m_MaskPoints;
        private double m_LowerDistance;
        private double m_UppderDistance;
        private Image<Gray, byte> m_MaskImage;
        private Image<Gray, byte> m_Image;

        public Point[] MaskPoints
        {
            get
            {
                return m_MaskPoints;
            }
            set
            {
                if (ReferenceEquals(m_MaskPoints, value))
                {
                    return;
                }

                m_MaskPoints = value;

                MarkAsDirty();
            }
        }

        public double LowerDistance
        {
            get
            {
                return m_LowerDistance;
            }
            set
            {
                if (Equals(m_LowerDistance, value))
                {
                    return;
                }

                m_LowerDistance = value;

                MarkAsDirty();
            }
        }

        public double UpperDistance
        {
            get
            {
                return m_UppderDistance;
            }
            set
            {
                if (Equals(m_UppderDistance, value))
                {
                    return;
                }

                m_UppderDistance = value;

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
