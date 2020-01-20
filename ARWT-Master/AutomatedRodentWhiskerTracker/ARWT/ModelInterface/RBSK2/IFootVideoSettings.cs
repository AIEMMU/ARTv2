using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.CvEnum;
namespace ARWT.ModelInterface.RBSK2
{
    public interface IFootVideoSettings: IModelObjectBase
    {
        bool track { get; set; }
        int area { get; set; }
        int contourDistance { get; set; }
        int kernelSize { get; set; }
        int scaleFactor { get; set; }
        int erosionIterations { get; set; }
        void AssignDefaultValues();
        IFootVideoSettings Clone();
    }
}