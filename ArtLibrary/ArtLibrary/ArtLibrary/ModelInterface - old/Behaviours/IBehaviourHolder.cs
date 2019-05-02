using ArtLibrary.Model.Behaviours;
using ArtLibrary.ModelInterface.Boundries;

namespace ArtLibrary.ModelInterface.Behaviours
{
    public interface IBehaviourHolder : IModelObjectBase
    {
        IBoundaryBase Boundary
        {
            get;
            set;
        }

        InteractionBehaviour Interaction
        {
            get;
            set;
        }

        int FrameNumber
        {
            get;
            set;
        }
    }
}
