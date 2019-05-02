
namespace ARWT.Model.Analysis
{
    internal class ProtractionData : ProtractionRetractionBase
    {
        public override string Name
        {
            get
            {
                return "Protraction";
            }
        }

        protected override double CalculateMean()
        {
            if (DeltaTime == 0)
            {
                return 0;
            }

            double deltaAngle = MaxAngle - MinAngle;

            return deltaAngle / DeltaTime;
        }
    }
}
