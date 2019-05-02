namespace ARWT.Model.Analysis
{
    internal class RetractionData : ProtractionRetractionBase
    {
        public override string Name
        {
            get
            {
                return "Retraction";
            }
        }

        protected override double CalculateMean()
        {
            if (DeltaTime == 0)
            {
                return 0;
            }

            double deltaAngle = MinAngle - MaxAngle;

            return deltaAngle/DeltaTime;
        }
    }
}
