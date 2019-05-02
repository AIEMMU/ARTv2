using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.ModelInterface.RBSK;
using ARWT.Foot.centroidTracker;
using ARWT.Model.Datasets;
using ARWT.Model.Datasets.Types;
using ARWT.Model.Feet;
using ARWT.Model.FileExtension;
using ARWT.Model.Masks;
using ARWT.Model.MWA;
using ARWT.Model.NonMaxSuppression;
using ARWT.Model.NonMaxSuppression.Angles;
using ARWT.Model.RBSK2;
using ARWT.Model.Results;
using ARWT.Model.Smoothing;
using ARWT.Model.Whiskers;
using ARWT.ModelInterface.Datasets;
using ARWT.ModelInterface.Datasets.Types;
using ARWT.ModelInterface.Feet;
using ARWT.ModelInterface.FileExtension;
using ARWT.ModelInterface.Masks;
using ARWT.ModelInterface.NonMaxSuppression;
using ARWT.ModelInterface.NonMaxSuppression.Angles;
using ARWT.ModelInterface.RBSK2;
using ARWT.ModelInterface.Results;
using ARWT.ModelInterface.Smoothing;
using ARWT.ModelInterface.Whiskers;

namespace ARWT.Resolver
{
    public static class ModelResolver
    {
        private static Dictionary<Type, Func<object>> _TypeDictionary = new Dictionary<Type, Func<object>>(); 

        public static T Resolve<T>() where T : class
        {
            try
            {
                return _TypeDictionary[typeof(T)].Invoke() as T;
            }
            catch (KeyNotFoundException e)
            {
                throw new Exception("The key: " + typeof(T) + "does not exist in the resolver");
            }
        }

        static ModelResolver()
        {
            ArtLibrary.Model.Resolver.ModelResolver.AddModels(_TypeDictionary);

            _TypeDictionary.Add(typeof(IDetermineExtension), () => new DetermineExtension());
            _TypeDictionary.Add(typeof(ILine), () => new Line());
            _TypeDictionary.Add(typeof(IMask), () => new Mask());
            _TypeDictionary.Add(typeof(IMaskHolder), () => new MaskHolder());
            _TypeDictionary.Add(typeof(IMouseMask), () => new MouseMask());

            _TypeDictionary.Add(typeof(INonMaximaSuppression), () => new NonMaximaSuppression());
            _TypeDictionary.Add(typeof(INonMax0), () => new NonMax0());
            _TypeDictionary.Add(typeof(INonMax45), () => new NonMax45());
            _TypeDictionary.Add(typeof(INonMax90), () => new NonMax90());
            _TypeDictionary.Add(typeof(INonMax135), () => new NonMax135());

            _TypeDictionary.Add(typeof(IRBSKVideo2), () => new RBSKVideo2());

            _TypeDictionary.Add(typeof(IWhiskerDetector), () => new WhiskerDetector());
            _TypeDictionary.Add(typeof(IWhiskerSegment), () => new WhiskerSegment());
            _TypeDictionary.Add(typeof(IWhiskerCollection), () => new WhiskerCollection());
            _TypeDictionary.Add(typeof(IFootCollection), () => new FootPlacementCollection());
            _TypeDictionary.Add(typeof(IFootPlacement), () => new FootPlacement());
            _TypeDictionary.Add(typeof(IfeetID), () => new feetID());
           
            _TypeDictionary.Add(typeof(ITrackSingleWhisker), () => new TrackSingleWhisker());
            _TypeDictionary.Add(typeof(IWhiskerAllocator), () => new WhiskerAllocator());
            _TypeDictionary.Add(typeof(IWhiskerAverageAngles), () => new WhiskerAverageAngles());

            _TypeDictionary.Add(typeof(ITrackedVideo), () => new TrackedVideo());
            _TypeDictionary.Add(typeof(ISingleFrameExtendedResults), () => new SingleFrameExtendedResults());
            _TypeDictionary.Add(typeof(IMouseDataExtendedResult), () => new MouseDataExtendedResult());

            _TypeDictionary.Add(typeof(IWhiskerVideoSettings), () => new WhiskerVideoSettings());

            _TypeDictionary.Add(typeof(IBoxCarSmoothing), () => new BoxCarSmoothing());
            _TypeDictionary.Add(typeof(iMovingAverage), () => new MovingAverage());
            _TypeDictionary.Add(typeof(iMovingAverage2), () => new MovingAverage2());
            _TypeDictionary.Add(typeof(IGaussianSmoothing), () => new GaussianSmoothing());

            _TypeDictionary.Add(typeof(ISingleMouse), () => new SingleMouse());
            _TypeDictionary.Add(typeof(IUndefined), () => new Undefined());
            _TypeDictionary.Add(typeof(ITransgenic), () => new Transgenic());
            _TypeDictionary.Add(typeof(INonTransgenic), () => new NonTransgenic());

            _TypeDictionary.Add(typeof(ISingleFile), () => new SingleFile());
            _TypeDictionary.Add(typeof(ISaveArtFile), () => new SaveArtFile());
            _TypeDictionary.Add(typeof(iSaveCSVFile), () => new SaveCSVFile());
            _TypeDictionary.Add(typeof(IFootVideoSettings), () => new FootVideoSettings());
            _TypeDictionary.Add(typeof (IGenericPoint), () => new GenericPoint());
        }
    }
}
