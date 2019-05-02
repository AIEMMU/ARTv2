using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ARWT.Model.MWA
{
    internal class MouseFrame : ModelObjectBase, IMouseFrame, IDisposable
    {
        private int m_FrameNumber;
        private int m_IndexNumber;
        private Image<Bgr, Byte> m_Frame;
        private IWhisker[] m_Whiskers;

        public int FrameNumber
        {
            get
            {
                return m_FrameNumber;
            }
            set
            {
                if (Equals(m_FrameNumber, value))
                {
                    return;
                }

                m_FrameNumber = value;

                MarkAsDirty();
            }
        }

        public int IndexNumber
        {
            get
            {
                return m_IndexNumber;
            }
            set
            {
                if (Equals(m_IndexNumber, value))
                {
                    return;
                }

                m_IndexNumber = value;

                MarkAsDirty();
            }
        }

        public Image<Bgr, Byte> Frame
        {
            get
            {
                return m_Frame;
            }
            set
            {
                if (Equals(m_Frame, value))
                {
                    return;
                }

                m_Frame = value;

                MarkAsDirty();
            }
        }

        public double OriginalWidth
        {
            get
            {
                return Frame.Width;
            }
        }

        public double OriginalHeight
        {
            get
            {
                return Frame.Height;
            }
        }

        public IWhisker[] Whiskers
        {
            get
            {
                return m_Whiskers;
            }
            set
            {
                if (ReferenceEquals(m_Whiskers, value))
                {
                    return;
                }

                m_Whiskers = value;

                MarkAsDirty();
            }
        }

        public void Dispose()
        {
            if (Frame != null)
            {
                Frame.Dispose();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            IMouseFrame mouseFrame = obj as IMouseFrame;

            if (mouseFrame == null)
            {
                return false;
            }

            return Equals(mouseFrame);
        }

        public bool Equals(IMouseFrame mouseFrame)
        {
            return FrameNumber == mouseFrame.FrameNumber;
        }

        public override int GetHashCode()
        {
            return FrameNumber;
        }
    }
}
