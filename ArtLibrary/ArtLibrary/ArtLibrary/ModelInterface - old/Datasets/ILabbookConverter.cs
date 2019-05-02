using System.Collections.Generic;
using ArtLibrary.ModelInterface;

namespace ArtLibrary.ModelInterface.Datasets
{
    public interface ILabbookConverter : IModelObjectBase
    {
        List<ISingleMouse> GenerateLabbookData(string[] lines, string fileLocation, int age = -1);
    }
}
