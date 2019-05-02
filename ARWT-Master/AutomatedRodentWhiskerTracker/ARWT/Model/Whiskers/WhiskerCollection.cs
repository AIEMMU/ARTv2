using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARWT.ModelInterface.Whiskers;

namespace ARWT.Model.Whiskers
{
    internal class WhiskerCollection : ModelObjectBase, IWhiskerCollection
    {
        private IWhiskerSegment[] m_LeftWhiskers;
        public IWhiskerSegment[] LeftWhiskers
        {
            get
            {
                return m_LeftWhiskers;
            }
            set
            {
                if (Equals(m_LeftWhiskers, value))
                {
                    return;
                }

                m_LeftWhiskers = value;

                MarkAsDirty();
            }
        }

        private IWhiskerSegment[] m_RightWhiskers;
        public IWhiskerSegment[] RightWhiskers
        {
            get
            {
                return m_RightWhiskers;
            }
            set
            {
                if (Equals(m_RightWhiskers, value))
                {
                    return;
                }

                m_RightWhiskers = value;

                MarkAsDirty();
            }
        }
    }
}
