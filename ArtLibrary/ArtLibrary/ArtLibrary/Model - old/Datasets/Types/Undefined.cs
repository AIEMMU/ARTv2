using ArtLibrary.ModelInterface.Datasets.Types;

namespace ArtLibrary.Model.Datasets.Types
{
    internal class Undefined : TypeBase, IUndefined
    {
        public override string Name
        {
            get
            {
                return "Undefined";
            }
        }
    }
}
