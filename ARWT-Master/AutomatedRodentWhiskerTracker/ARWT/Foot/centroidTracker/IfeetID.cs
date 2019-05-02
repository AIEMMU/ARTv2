using ARWT.ModelInterface;
using ARWT.ModelInterface.Feet;

namespace ARWT.Foot.centroidTracker
{
    public interface IfeetID: IModelObjectBase
    {
        string id { get; set; }
        IFootPlacement value { get; set; }
    }
}