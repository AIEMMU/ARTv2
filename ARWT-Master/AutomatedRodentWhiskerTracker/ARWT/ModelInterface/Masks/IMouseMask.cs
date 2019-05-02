using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ARWT.ModelInterface.Masks
{
    public interface IMouseMask : IModelObjectBase
    {
        IMaskHolder GetMasks(Point[] body, PointF headPoint, PointF leftPoint, PointF rightPoint, Size maxSize, double headDistanceThreshold, double[] distances, double[] lineDistances);
        IMaskHolder GetMasks(Point[] body, PointF headPoint, PointF leftPoint, PointF rightPoint, Size maxSize, double headDistanceThreshold, double[] distances, double[] lineDistances, Image<Bgr, byte> debugImg);
    }
}
