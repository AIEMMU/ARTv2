using ArtLibrary.Model;

namespace ArtLibrary.ModelInterface
{
    public interface IModelObjectBase
    {
        string ErrorMessage
        {
            get;
            set;
        }

        ModelObjectState ModelObjectState
        {
            get;
        }

        void Commit();
        void MarkAsDeleted();
        void DataLoadComplete();
    }
}
