using ArtLibrary.Model;
using ArtLibrary.ModelInterface.Datasets.Types;

namespace ArtLibrary.Model.Datasets.Types
{
    internal abstract class TypeBase : ModelObjectBase, ITypeBase
    {
        public abstract string Name
        {
            get;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
