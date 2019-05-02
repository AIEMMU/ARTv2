using ArtLibrary.ModelInterface;

namespace ArtLibrary.ModelInterface.Datasets.Types
{
    public interface ITypeBase : IModelObjectBase
    {
        string Name
        {
            get;
        }
    }
}
