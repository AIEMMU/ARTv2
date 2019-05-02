using System;
using Emgu.CV;
using Emgu.CV.Structure;
using ARWT.ModelInterface.NonMaxSuppression.Angles;

namespace ARWT.Model.NonMaxSuppression.Angles
{
    internal class NonMax0 : NonMaxBase, INonMax0
    {
        public NonMax0()
        {
            Angle = 0;
        }

        public override void Apply(Image<Gray, float> img)
        {
            int rows = img.Height;

            if (rows%2 != 1)
            {
                throw new Exception("number of rows must be odd!");
            }

            int cRow = (rows + 1)/2;
            int cols = img.Width;

            double cMax = double.MinValue;
            int bestI = -1;
            for (int i = 0; i < cols; i++)
            {
                double intensity = img[cRow, i].Intensity;
                if (intensity > cMax)
                {
                    cMax = intensity;
                    bestI = i;
                }
            }

            for (int i = 0; i < cols; i++)
            {
                if (i == bestI)
                {
                    continue;
                }

                img[cRow, i] = new Gray(0);
            }
        }
    }
}
