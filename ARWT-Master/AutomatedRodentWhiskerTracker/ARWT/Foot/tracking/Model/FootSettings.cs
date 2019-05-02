using ARWT.Foot.tracking.data;
using System.Collections.ObjectModel;

namespace ARWT.Foot.tracking.Model
{
    public class FootSettings
    {
        public int areaMax { get; set; }
        public int areaMin { get; set; }
        public int area { get; set; }
        public int scaleFactor { get; set; }
        public int contourDistance { get; set; }
        public int eroisionKernel { get; set; }
        public int eroisionIterations { get; set; }
    }
}
