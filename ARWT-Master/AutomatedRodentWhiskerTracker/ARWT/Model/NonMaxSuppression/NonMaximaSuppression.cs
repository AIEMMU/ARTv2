using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using ARWT.ModelInterface.NonMaxSuppression;

namespace ARWT.Model.NonMaxSuppression
{
    internal class NonMaximaSuppression : ModelObjectBase, INonMaximaSuppression
    {
        public void Apply(Image<Gray, float> img, Image<Gray, Byte> mask, int kernalSize)
        {
            double[] min, max;
            Point[] minLoc, maxLoc;

            using (Image<Gray, float> holder = new Image<Gray, float>(img.Size))
            {
                img.Copy(holder, mask);
                holder.MinMax(out min, out max, out minLoc, out maxLoc);
            }

            //img.SetValue(new Gray(0));

            for (int i = 0; i < kernalSize; i++)
            {
                for (int j = 0; j < kernalSize; j++)
                {
                    img[j, i] = new Gray(0);
                }
            }

            img[maxLoc[0]] = new Gray(max[0]);
        }

        public void Apply(Image<Gray, byte> img)
        {
            double[] min, max;
            Point[] minLoc, maxLoc;

            //using (Image<Gray, float> holder = new Image<Gray, float>(img.Size))
            //{
                img.MinMax(out min, out max, out minLoc, out maxLoc);
            //}
            //byte[,,] data = img.Data;
            int kernalWidth = img.Width;
            int kernalHeight = img.Height;
            for (int i = 0; i < kernalWidth; i++)
            {
                for (int j = 0; j < kernalHeight; j++)
                {
                    img[j, i] = new Gray(0);
                }
            }
            
            img[maxLoc[0]] = new Gray(255);
        }

        public void Apply(Image<Gray, byte> img, int topPointsToKeep)
        {
            using (Image<Gray, byte> tempImg = img.Clone())
            {
                int counter = 0;

                List<Point> goodPoints = new List<Point>();
                while (counter < topPointsToKeep)
                {
                    double[] min, max;
                    Point[] minLoc, maxLoc;

                    tempImg.MinMax(out min, out max, out minLoc, out maxLoc);

                    goodPoints.Add(maxLoc[0]);
                    tempImg[maxLoc[0]] = new Gray(0);
                    counter++;
                }

                //int kernalWidth = img.Width;
                //int kernalHeight = img.Height;
                //for (int i = 0; i < kernalWidth; i++)
                //{
                //    for (int j = 0; j < kernalHeight; j++)
                //    {
                //        img[j, i] = new Gray(0);
                //    }
                //}
                img.SetValue(new Gray(0));

                //img.SetValue(new Gray(0));

                foreach (var point in goodPoints)
                {
                    img[point] = new Gray(255);
                }
                
            }
        }
    }
}
