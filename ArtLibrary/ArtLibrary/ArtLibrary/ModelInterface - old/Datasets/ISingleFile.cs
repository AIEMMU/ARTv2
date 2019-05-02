using ArtLibrary.ModelInterface;

namespace ArtLibrary.ModelInterface.Datasets
{
    public interface ISingleFile : IModelObjectBase
    {
        string VideoFileName
        {
            get;
            set;
        }

        string VideoNumber
        {
            get;
            set;
        }
    }
}
