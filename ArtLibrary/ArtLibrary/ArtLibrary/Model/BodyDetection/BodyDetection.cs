using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using ArtLibrary.Extensions;
using ArtLibrary.Model.Resolver;
using ArtLibrary.ModelInterface.BodyDetection;
using ArtLibrary.ModelInterface.Skeletonisation;
using ArtLibrary.Services.Mouse;
using ArtLibrary.Services.RBSK;
using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Point = System.Drawing.Point;

namespace ArtLibrary.Model.BodyDetection
{
    internal class BodyDetection : ModelObjectBase, IBodyDetection
    {
        private Image<Gray, Byte> m_BinaryBackground;
        public Image<Gray, Byte> BinaryBackground
        {
            get
            {
                return m_BinaryBackground;
            }
            set
            {
                if (Equals(m_BinaryBackground, value))
                {
                    return;
                }

                if (m_BinaryBackground != null)
                {
                    m_BinaryBackground.Dispose();
                }

                m_BinaryBackground = value;

                MarkAsDirty();
            }
        }

        private CvBlobDetector BlobDetector
        {
            get;
            set;
        }

        private double m_ThresholdValue;
        public double ThresholdValue
        {
            get
            {
                return m_ThresholdValue;
            }
            set
            {
                if (Equals(m_ThresholdValue, value))
                {
                    return;
                }

                m_ThresholdValue = value;

                MarkAsDirty();
            }
        }

        private Services.RBSK.RBSK RBSK
        {
            get;
            set;
        }

        private Image<Gray, Byte> SkelImage
        {
            get;
            set;
        }

        public BodyDetection()
        {
            ThresholdValue = 20;
            //VideoSettings = ModelResolver.ModelResolver.Resolve<IVideoSettings>();
            BlobDetector = new CvBlobDetector();
            RBSK = MouseService.GetStandardMouseRules();
        }

        //public void GetBody(Image<Gray, Byte> filteredImage, out PointF centroid, out Point[] bodyContour)
        //{
        //    bodyContour = null;

        //    using (Image<Gray, Byte> binary = filteredImage.ThresholdBinary(new Gray(ThresholdValue), new Gray(255)))
        //    using (Image<Gray, Byte> backgroundNot = BinaryBackground.Not())
        //    using (Image<Gray, Byte> finalImage = binary.Add(backgroundNot))
        //    using (Image<Gray, Byte> subbed = finalImage.Not())
        //    {
        //        centroid = PointF.Empty;
        //        CvBlobs blobs = new CvBlobs();
        //        BlobDetector.Detect(subbed, blobs);

        //        CvBlob mouseBlob = null;
        //        double maxArea = -1;
        //        foreach (var blob in blobs.Values)
        //        {
        //            if (blob.Area > maxArea)
        //            {
        //                mouseBlob = blob;
        //                maxArea = blob.Area;
        //            }
        //        }

        //        if (mouseBlob != null)
        //        {
        //            centroid = mouseBlob.Centroid;
        //            bodyContour = mouseBlob.GetContour();
        //        }
        //    }
        //}

        public void GetBody(Image<Gray, Byte> filteredImage, out PointF centroid, out Point[] bodyContour)
        {
            
            using (Image<Gray, Byte> binary = filteredImage.ThresholdBinary(new Gray(ThresholdValue), new Gray(255)))
            using (Image<Gray, Byte> backgroundNot = BinaryBackground.Not())
            using (Image<Gray, Byte> finalImage = binary.Add(backgroundNot))
            using (Image<Gray, Byte> subbed = finalImage.Not())
            {
                
                ISkeleton skel = ModelResolver.Resolve<ISkeleton>();
                using (Image<Gray, Byte> skelImage = skel.GetSkeleton(finalImage))
                centroid = PointF.Empty;
                bodyContour = null;
                CvBlobs blobs = new CvBlobs();
                BlobDetector.Detect(subbed, blobs);

                CvBlob mouseBlob = null;
                double maxArea = -1;
                foreach (var blob in blobs.Values)
                {
                    if (blob.Area > maxArea)
                    {
                        mouseBlob = blob;
                        maxArea = blob.Area;
                    }
                }

                if (mouseBlob != null)
                {
                    centroid = mouseBlob.Centroid;

                    bodyContour = mouseBlob.GetContour();
                   
                }
            }
        }

        
        //public void GetBody(Image<Gray, Byte> frame, out PointF centroid, out Point[] bodyContour)
        //{
        //    bodyContour = null;

        //    //using (Image<Gray, Byte> origGray = frame.Convert<Gray, Byte>())
        //    //using (Image<Gray, Byte> filteredImage = origGray.SmoothMedian(13))
        //    //using (Image<Gray, Byte> binary = filteredImage.ThresholdBinary(new Gray(ThresholdValue), new Gray(255)))
        //    //using (Image<Gray, Byte> backgroundNot = BinaryBackground.Not())
        //    //using (Image<Gray, Byte> finalImage = binary.Add(backgroundNot))
        //    //using (Image<Gray, Byte> subbed = finalImage.Not())
        //    {
        //        centroid = PointF.Empty;
        //        CvBlobs blobs = new CvBlobs();
        //        BlobDetector.Detect(frame, blobs);

        //        CvBlob mouseBlob = null;
        //        double maxArea = -1;
        //        foreach (var blob in blobs.Values)
        //        {
        //            if (blob.Area > maxArea)
        //            {
        //                mouseBlob = blob;
        //                maxArea = blob.Area;
        //            }
        //        }

        //        if (mouseBlob != null)
        //        {
        //            centroid = mouseBlob.Centroid;
        //            bodyContour = mouseBlob.GetContour();
        //        }
        //    }
        //}

        public void FindBody(Image<Gray, Byte> filteredImage, out double waistLength, out double waistVolume, out double waistVolume2, out double waistVolume3, out double waistVolume4, out PointF centroid, out Point[] bodyContour)
        {
            
            using (Image<Gray, Byte> binary = filteredImage.ThresholdBinary(new Gray(ThresholdValue), new Gray(255)))
            using (Image<Gray, Byte> backgroundNot = BinaryBackground.Not())
            using (Image<Gray, Byte> finalImage = binary.Add(backgroundNot))
            using (Image<Gray, Byte> subbed = finalImage.Not())
            {
                //ImageViewer.Show(filteredImage);
                //ImageViewer.Show(binary);
                

                //Console.WriteLine(roi);
                CvBlobs blobs = new CvBlobs();
                BlobDetector.Detect(subbed, blobs);

                CvBlob mouseBlob = null;
                double maxArea = -1;
                foreach (var blob in blobs.Values)
                {
                    if (blob.Area > maxArea)
                    {
                        mouseBlob = blob;
                        maxArea = blob.Area;
                    }
                }

                double gapDistance = 50;
                RBSK.Settings.GapDistance = gapDistance;

                centroid = mouseBlob.Centroid;
                bodyContour = mouseBlob.GetContour();
                
                waistLength = -1;
                waistVolume = -1;
                waistVolume2 = -1;
                waistVolume3 = -1;
                waistVolume4 = -1;
            }
        }

        
    }
}
