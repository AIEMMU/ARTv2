
using ARWT.Foot.centroidTracker;
using ARWT.ModelInterface.Feet;

namespace ARWT.Model.Feet
{
    internal class FootPlacementCollection : ModelObjectBase, IFootCollection
    {
        public FootPlacementCollection()
        {
            leftfront = null;
            rightfront = null;
            leftHind = null;
            rightHind = null;
        }
        private IfeetID _leftFront;
        public IfeetID leftfront
        {
            get { return _leftFront; }
            set { _leftFront = value;
                MarkAsDirty();
            }
        }
        private IfeetID _leftHind;
        public IfeetID leftHind
        {
            get { return _leftHind; }
            set
            {
                _leftHind = value;
                MarkAsDirty();
            }
        }
        private IfeetID _rightFront;
        public IfeetID rightfront
        {
            get { return _rightFront; }
            set
            {
                _rightFront = value;
                MarkAsDirty();
            }
        }
        private IfeetID _rightHind;
        public IfeetID rightHind
        {
            get { return _rightHind; }
            set
            {
                _rightHind = value;
                MarkAsDirty();
            }
        }

    }
}
