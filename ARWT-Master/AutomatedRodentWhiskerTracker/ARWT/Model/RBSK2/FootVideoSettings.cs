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
    internal class FootVideoSettings : ModelObjectBase, IFootVideoSettings
    {
        private int _area;

        public int area
        {
            get { return _area; }
            set { _area = value;
                MarkAsDirty();
            }
        }
        private int _contourDistance;

        public int contourDistance
        {
            get { return _contourDistance; }
            set { _contourDistance = value;
                MarkAsDirty(); }
        }
        private int _kernelSize;

        public int kernelSize
        {
            get { return _kernelSize; }
            set
            {
                _kernelSize = value;
                MarkAsDirty();
            }
        }
        private int _scaleFactor;

        public int scaleFactor
        {
            get { return _scaleFactor; }
            set
            {
                _scaleFactor = value;
                MarkAsDirty();
            }
        }
        private int _erosionIterations;

        public int erosionIterations
        {
            get { return _erosionIterations; }
            set
            {
                _erosionIterations = value;
                MarkAsDirty();
            }
        }
        public void AssignDefaultValues()
        {
            area = 10;
            contourDistance = 5;
            scaleFactor = 2;
            kernelSize = 3;
            erosionIterations = 14;
        }

        public IFootVideoSettings Clone()
        {
            IFootVideoSettings clone = ModelResolver.Resolve<IFootVideoSettings>();
            clone.area = area;
            clone.contourDistance = contourDistance;
            clone.scaleFactor = scaleFactor;
            clone.kernelSize = kernelSize;
            return clone;
        }
    }
}
