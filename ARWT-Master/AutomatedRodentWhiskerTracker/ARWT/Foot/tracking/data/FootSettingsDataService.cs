using ARWT.Foot.tracking.Model;
using System.Collections.ObjectModel;

namespace ARWT.Foot.tracking.data
{
    public class FootSettingsDataService : IFootSettingsDataService
    {
        public FootSettings GetSettings() => new FootSettings
        {
            areaMin = 1,
            areaMax = 200,
            area = 60,
            contourDistance = 5,
            eroisionKernel = 3,
            scaleFactor = 2,
            eroisionIterations = 14
        };
    }
}
