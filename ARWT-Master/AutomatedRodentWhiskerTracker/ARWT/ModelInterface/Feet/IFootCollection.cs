

using ARWT.Foot.centroidTracker;

namespace ARWT.ModelInterface.Feet
{
    public interface IFootCollection : IModelObjectBase
    {
        IfeetID leftfront { get; set; }
        IfeetID leftHind { get; set; }
        IfeetID rightfront { get; set; }
        IfeetID rightHind { get; set; }
    }
}