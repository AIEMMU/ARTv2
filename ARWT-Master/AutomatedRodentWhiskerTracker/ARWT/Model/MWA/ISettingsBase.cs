using ARWT.ModelInterface;

namespace ARWT.Model.MWA
{
    public interface ISettingsBase : IModelObjectBase
    {
        string Name
        {
            get;
            set;
        }

        void LoadSettings();
        void SaveSettings();
    }
}
