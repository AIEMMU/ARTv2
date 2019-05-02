namespace ARWT.Model.MWA
{
    public interface IUnitSettings : ISettingsBase
    {
        double UnitsPerPixel
        {
            get;
            set;
        }

        string UnitsName
        {
            get;
            set;
        }
    }
}
