using Emgu.CV;
using Emgu.CV.Structure;
using ARWT.ModelInterface.NonMaxSuppression.Angles;

namespace ARWT.Model.NonMaxSuppression.Angles
{
    internal abstract class NonMaxBase : ModelObjectBase, INonMaxBase
    {
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

        public abstract void Apply(Image<Gray, float> img);
    }
}
