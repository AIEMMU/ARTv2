using ArtLibrary.ModelInterface.Datasets.Types;

namespace ArtLibrary.Model.Datasets.Types
{
    internal class Transgenic : TypeBase, ITransgenic
    {
        public override string Name
        {
            get
            {
                return "Transgenic";
            }
        }
    }
}
