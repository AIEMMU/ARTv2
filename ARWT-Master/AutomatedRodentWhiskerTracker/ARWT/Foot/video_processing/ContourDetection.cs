using ARWT.Model.Feet;
using ARWT.ModelInterface.Feet;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ARWT.Foot.video_processing
{
    public class ContourDetection
    {
        
        public static List<VectorOfPointF> detectFootContours(Image<Bgr,double> mask, int scaleFactor, int contourDistance)
        {
            VectorOfVectorOfPoint contours = findMaskcontours(mask, scaleFactor);

            if (contours.Size > 0)
            {
                int length;
                double[] status;
                getLenghtandStatus(contours, out length, out status);
                generateStatus(contours, length, status, contourDistance);
                List<VectorOfPointF> unified = generateUnifiedContours(contours, status, scaleFactor);
                return unified;
            }

            return new List<VectorOfPointF>();
        }

        private static List<VectorOfPointF> generateUnifiedContours(VectorOfVectorOfPoint contours, double[] status, int scaleFactor)
        {
            List<VectorOfPointF> unified = new List<VectorOfPointF>();

            int maxium = Convert.ToInt32(status.Max()) + 1;

            for (int i = 0; i < maxium; i++)
            {
                int[] pos = where(status, i);

                if (pos.Length != 0)
                {
                    List<PointF> cont = new List<PointF>();
                    for (int j = 0; j < pos.Length; j++)
                    {
                        VectorOfPoint contie = contours[pos[j]];

                        for (int k = 0; k < contie.Size; k++)
                        {
                            int factor = Convert.ToInt32(Math.Pow(2, scaleFactor));
                            cont.Add(new PointF(contie[k].X /** 2*/ * factor, contie[k].Y /* 2*/ * factor));
                        }
                    }
                    var footContour = CvInvoke.ConvexHull(cont.ToArray());
                    unified.Add(new VectorOfPointF(footContour));
                }
            }

            return unified;
        }

        private static double[] generateStatus(VectorOfVectorOfPoint contours, int length, double[] status, int contourDistance)
        {
            for (int i = 0; i < contours.Size; i++)
            {
                int x = i;
                var contour1 = contours[i];

                if (i != length - 1)
                {
                    for (int j = 0; j < contours.Size - 1 - i; j++)
                    {
                        x++;
                        var contour2 = contours[i + 1 + j];

                        bool dist = findIfClose(contour1, contour2, contourDistance);

                        if (dist == true)
                        {
                            double value = Math.Min(status[i], status[x]);
                            status[x] = status[i] = value;
                        }
                        else
                        {
                            if (status[x] == status[i])
                            {
                                status[x] = i + 1;
                            }

                        }
                    }
                }
            }
            return status;
        }

        private static void getLenghtandStatus(VectorOfVectorOfPoint contours, out int length, out double[] status)
        {
            length = contours.Size;
            status = new double[length];
        }

        private static VectorOfVectorOfPoint findMaskcontours(Image<Bgr, double> mask, int scaleFactor)
        {
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Image<Gray, byte> downSampled = mask.Clone().Convert<Gray, byte>();
            for(int i = 0; i < scaleFactor; i++)
            {
                downSampled = downSampled.PyrDown();
            }

            Mat heir = new Mat();
            CvInvoke.FindContours(downSampled, contours, heir, RetrType.List, method: ChainApproxMethod.ChainApproxSimple);
            return contours;
        }
        
        //HelperFunctions
        private static int[] where(double[] status, int id)
        {
            List<int> listIndx = new List<int>();
            for (int j = 0; j < status.Length; j++)
            {
                if (status[j] == id)
                {
                    listIndx.Add(j);
                    listIndx.Add(j);
                }
            }
            return listIndx.ToArray();
        }

        private static bool findIfClose(VectorOfPoint contour1, VectorOfPoint contour2, int contourDistance)
        {
            int row1 = contour1.Size;
            int row2 = contour2.Size;

            for (int i = 0; i < row1; i++)
            {
                for (int j = 0; j < row2; j++)
                {
                    double dist = HelperFunctions.getDistance(contour1[i], contour2[j]);

                    if (Math.Abs(dist) < contourDistance)
                    {
                        return true;
                    }
                    else if (i == row1 - 1 && j == row2 - 1)
                    {
                        return false;
                    }
                }

            }
            return false;
        }


    }

}
