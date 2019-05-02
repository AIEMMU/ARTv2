using ArtLibrary.Model;
using ArtLibrary.ModelInterface.Datasets.Types;

namespace ArtLibrary.Model.Datasets.Types
{
    internal class NonTransgenic : ModelObjectBase, INonTransgenic
    {
        public string Name
        {
            get
            {
                return "Non-Transgenic";
            }
        }
    }
}
