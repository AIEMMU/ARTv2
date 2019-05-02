using System.Collections.Generic;
using ArtLibrary.ModelInterface;
using ArtLibrary.ModelInterface.Datasets.Types;
using ArtLibrary.ModelInterface.Results;

namespace ArtLibrary.ModelInterface.Datasets
{
    public interface ISingleMouse : IModelObjectBase
    {
        string Name
        {
            get;
            set;
        }

        string Id
        {
            get;
            set;
        }

        ITypeBase Type
        {
            get;
            set;
        }

        List<string> Videos
        {
            get;
            set;
        }

        string Class
        {
            get;
            set;
        }

        int Age
        {
            get;
            set;
        }

        List<ISingleFile> VideoFiles
        {
            get;
            set;
        }

        Dictionary<ISingleFile, IMouseDataResult> Results
        {
            get;
            set;
        }

        void GenerateFiles(string fileLocation);
    }
}
