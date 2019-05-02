using ARWT.Model;
using ARWT.Model.Feet;
using ARWT.ModelInterface.Feet;

namespace ARWT.Foot.centroidTracker
{
    internal class feetID : ModelObjectBase, IfeetID
    {
        public string id { get; set; }
        public IFootPlacement value { get; set; }
        public feetID()
        {
            value = new FootPlacement();
            id = "";
        }
    }
   
}
