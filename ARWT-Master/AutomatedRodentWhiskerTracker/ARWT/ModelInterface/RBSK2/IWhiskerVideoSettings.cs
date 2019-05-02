using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.CvEnum;

namespace ARWT.ModelInterface.RBSK2
{
    public interface IWhiskerVideoSettings : IModelObjectBase
    {
        double CropScaleFactor
        {
            get;
            set;
        }

        Inter InterpolationType
        {
            get;
            set;
        }

        float ResolutionIncreaseScaleFactor
        {
            get;
            set;
        }

        double OrientationResolution
        {
            get;
            set;
        }

        bool RemoveDuds
        {
            get;
            set;
        }

        byte LineMinIntensity
        {
            get;
            set;
        }
        
        int LowerBound
        {
            get;
            set;
        }
        
        int UpperBound
        {
            get;
            set;
        }

        void AssignDefaultValues();
        IWhiskerVideoSettings Clone();
    }
}
