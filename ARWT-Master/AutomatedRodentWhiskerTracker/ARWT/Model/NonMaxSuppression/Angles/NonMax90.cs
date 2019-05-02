using System;
using Emgu.CV;
using Emgu.CV.Structure;
using ARWT.ModelInterface.NonMaxSuppression.Angles;

namespace ARWT.Model.NonMaxSuppression.Angles
{
    internal class NonMax90 : NonMaxBase, INonMax90
    {
        public NonMax90()
        {
            Angle = 90;
        }

        public override void Apply(Image<Gray, float> img)
        {
            int cols = img.Width;

            if (cols % 2 != 1)
            {
                throw new Exception("number of cols must be odd!");
            }

            int cCol = (cols + 1) / 2;
            int rows = img.Height;

            double cMax = double.MinValue;
            int bestI = -1;
            for (int i = 0; i < rows; i++)
            {
                double intensity = img[i, cCol].Intensity;
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

                img[i, cCol] = new Gray(0);
            }
        }
    }
}
