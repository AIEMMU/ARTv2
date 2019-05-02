using System;
using Emgu.CV;
using Emgu.CV.Structure;
using ARWT.ModelInterface.NonMaxSuppression.Angles;

namespace ARWT.Model.NonMaxSuppression.Angles
{
    internal class NonMax135 : NonMaxBase, INonMax135
    {
        public NonMax135()
        {
            Angle = 135;
        }

        public override void Apply(Image<Gray, float> img)
        {
            int cols = img.Width;
            int rows = img.Height;

            if (cols % 2 != 1)
            {
                throw new Exception("Number of cols must be odd!");
            }

            if (rows != cols)
            {
                throw new Exception("Number of rows must match number of columns");
            }

            double cMax = double.MinValue;
            int bestI = -1;
            for (int i = 0; i < rows; i++)
            {
                double intensity = img[i, cols - i- 1].Intensity;
                if (intensity > cMax)
                {
                    cMax = intensity;
                    bestI = i;
                }
            }

            for (int i = 0; i < rows; i++)
            {
                if (i == bestI)
                {
                    continue;
                }

                img[i, cols - i - 1] = new Gray(0);
            }
        }
    }
}
