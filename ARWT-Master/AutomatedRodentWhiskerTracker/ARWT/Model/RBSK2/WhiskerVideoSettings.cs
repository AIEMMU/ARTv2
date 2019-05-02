using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARWT.ModelInterface.RBSK2;
using ARWT.Resolver;
using Emgu.CV.CvEnum;

namespace ARWT.Model.RBSK2
{
    internal class WhiskerVideoSettings : ModelObjectBase, IWhiskerVideoSettings
    {
        private double _CropScaleFactor;
        public double CropScaleFactor
        {
            get
            {
                return _CropScaleFactor;
            }
            set
            {
                if (Equals(_CropScaleFactor, value))
                {
                    return;
                }

                _CropScaleFactor = value;

                MarkAsDirty();
            }
        }

        private Inter _InterpolationType;
        public Inter InterpolationType
        {
            get
            {
                return _InterpolationType;
            }
            set
            {
                if (Equals(_InterpolationType, value))
                {
                    return;
                }

                _InterpolationType = value;

                MarkAsDirty();
            }
        }

        private float _ResolutionIncreaseScaleFactor;
        public float ResolutionIncreaseScaleFactor
        {
            get
            {
                return _ResolutionIncreaseScaleFactor;
            }
            set
            {
                if (Equals(_ResolutionIncreaseScaleFactor, value))
                {
                    return;
                }

                _ResolutionIncreaseScaleFactor = value;

                MarkAsDirty();
            }
        }

        private double _OrientationResolution;
        public double OrientationResolution
        {
            get
            {
                return _OrientationResolution;
            }
            set
            {
                if (Equals(_OrientationResolution, value))
                {
                    return;
                }

                _OrientationResolution = value;

                MarkAsDirty();
            }
        }

        private bool _RemoveDuds;
        public bool RemoveDuds
        {
            get
            {
                return _RemoveDuds;
            }
            set
            {
                if (Equals(_RemoveDuds, value))
                {
                    return;
                }

                _RemoveDuds = value;

                MarkAsDirty();
            }
        }

        private byte _LineMinIntensity;
        public byte LineMinIntensity
        {
            get
            {
                return _LineMinIntensity;
            }
            set
            {
                if (Equals(_LineMinIntensity, value))
                {
                    return;
                }

                _LineMinIntensity = value;

                MarkAsDirty();
            }
        }

        private int _LowerBound;
        public int LowerBound
        {
            get
            {
                return _LowerBound;
            }
            set
            {
                if (Equals(_LowerBound, value))
                {
                    return;
                }

                _LowerBound = value;

                MarkAsDirty();
            }
        }

        private int _UpperBound;
        public int UpperBound
        {
            get
            {
                return _UpperBound;
            }
            set
            {
                if (Equals(_UpperBound, value))
                {
                    return;
                }

                _UpperBound = value;

                MarkAsDirty();
            }
        }


        public void AssignDefaultValues()
        {
            RemoveDuds = true;
            CropScaleFactor = 4;
            ResolutionIncreaseScaleFactor = 2;
            InterpolationType = Inter.Area;
            OrientationResolution = 1;
            LineMinIntensity = 150;
            LowerBound = 7;
            UpperBound = 10;
        }

        public IWhiskerVideoSettings Clone()
        {
            IWhiskerVideoSettings clone = ModelResolver.Resolve<IWhiskerVideoSettings>();

            clone.CropScaleFactor = CropScaleFactor;
            clone.InterpolationType = InterpolationType;
            clone.LineMinIntensity = LineMinIntensity;
            clone.LowerBound = LowerBound;
            clone.OrientationResolution = OrientationResolution;
            clone.RemoveDuds = RemoveDuds;
            clone.ResolutionIncreaseScaleFactor = ResolutionIncreaseScaleFactor;
            clone.UpperBound = UpperBound;

            return clone;
        }
    }
}
