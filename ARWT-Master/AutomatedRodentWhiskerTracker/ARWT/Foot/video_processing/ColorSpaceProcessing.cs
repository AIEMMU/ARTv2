using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARWT.Foot.video_processing
{
    public class ColorSpaceProcessing
    {
        public static Image<Bgr, byte> Process(Image<Bgr, byte> img, int? colorSpace)
        {
            if (colorSpace == null)
            {
                colorSpace = 0;
            }

            switch (colorSpace)
            {
                case 0:
                    //return GammaCorrection(img, brightness, gain);
                case 1:
                case 2:
                    return LogSpace(img);
            }
            return img;
        }

        public static Image<Bgr, byte> LogSpace(Image<Bgr, byte> img)
        {
            if (img == null)
            {
                return img;
            }
            return processLogSpace(img);
        }

        public static Image<Bgr, byte> processLogSpace(Image<Bgr, byte> img)
        {
            var image = img.Convert<Bgr, Double>();
            image = image.Log();
            return image.Convert<Bgr, byte>() * 112;
        }

        private static Image<Bgr, byte> LogSpace2(Image<Bgr, byte> img)
        {
            var image = img.Convert<Bgr, Double>();
            image = image.Log();
            return img.Convert<Bgr, Byte>().Log();
        }

        private static Image<Bgr,byte> GammaCorrection(Image<Bgr, byte> img, int gamma, int alpha)
        {   if(img == null)
            {
                return img;
            }
            CvInvoke.ConvertScaleAbs(img, img, Convert.ToDouble(gamma)/255.0, Convert.ToDouble(alpha));
            return img;
        }
    }
}
