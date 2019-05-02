using ARWT.ModelInterface.Feet;

namespace ARWT.Model.Feet
{
    internal class FootPlacement : ModelObjectBase, IFootPlacement
    {

        private int _centroidX;
        public int centroidX
        {
            get { return _centroidX; }
            set
            {
                _centroidX = value;
                MarkAsDirty();
            }
        }

        private int _centroidY;
        public int centroidY
        {
            get { return _centroidY; }
            set
            {
                _centroidY = value;
                MarkAsDirty();
            }
        }
        private  int _minX;
        public  int minX
        {
            get { return _minX;}
            set { _minX = value;
                MarkAsDirty();
            }
        }
        private int _minY;
        public int minY
        {
            get { return _minY; }
            set
            {
                _minY = value;
                MarkAsDirty();
            }
        }

        private int _maxX;
        public int maxX
        {
            get { return _maxX; }
            set
            {
                _maxX = value;
                MarkAsDirty();
            }
        }
        private int _width;
        public int width
        {
            get { return _width; }
            set
            {
                _width = value;
                MarkAsDirty();
            }
        }

        private int _height;
        public int height
        {
            get { return _height; }
            set
            {
                _height = value;
                MarkAsDirty();
            }
        }
        private int _maxY;
        public int maxY
        {
            get { return _maxY; }
            set
            {
                _maxY = value;
                MarkAsDirty();
            }
        }
    }
}
