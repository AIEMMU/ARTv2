using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARWT.Foot.video_processing
{
    public class MaskSegmentation
    {
        public static int i = 0;
        public static Image<Bgr, double> segmentMask(Point[] bodyPoints, Image<Bgr, double> image, int kernelSize, int iterations)
        {
            var img = image.Convert<Bgr, double>();
            
            Image<Bgr, double> mask = new Image<Bgr, double>(img.Width, img.Height);
            mask.Draw(bodyPoints, new Bgr(Color.White), -1);

            //mask = mask.Erode(7);
            var element = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(kernelSize, kernelSize), new Point(-1, -1));
            //CvInvoke.Imwrite($"mask_not_eroded{i:D4}.png", mask);
            CvInvoke.Erode(mask, mask, element, new Point(-1, -1), iterations, BorderType.Reflect, default(MCvScalar));
            CvInvoke.Multiply(mask, img, img);
            i++;
            return img;
            
        }
    }
}
