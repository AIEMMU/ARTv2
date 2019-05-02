using System;
using System.Collections.Generic;
using ArtLibrary.Model.Behaviours;
using ArtLibrary.Model.Boundries;
using ArtLibrary.Model.Results;
using ArtLibrary.Model.Results.Behaviour;
using ArtLibrary.Model.Results.Behaviour.BodyOption;
using ArtLibrary.Model.Results.Behaviour.Movement;
using ArtLibrary.Model.Skeletonisation;
using ArtLibrary.Model.Smoothing;
using ArtLibrary.Model.Video;
using ArtLibrary.ModelInterface.Behaviours;
using ArtLibrary.ModelInterface.BodyDetection;
using ArtLibrary.ModelInterface.Boundries;
using ArtLibrary.ModelInterface.Results.Behaviour;
using ArtLibrary.ModelInterface.Results.Behaviour.BodyOption;
using ArtLibrary.ModelInterface.Results.Behaviour.Movement;
using ArtLibrary.ModelInterface.Skeletonisation;
using ArtLibrary.ModelInterface.Smoothing;
using ArtLibrary.ModelInterface.Video;
using ArtLibrary.Model.Datasets;
using ArtLibrary.Model.Datasets.Types;
using ArtLibrary.Model.Motion.BackgroundSubtraction;
using ArtLibrary.Model.Motion.MotionBackground;
using ArtLibrary.Model.RBSK;
using ArtLibrary.Model.Results.Behaviour.Rotation;
using ArtLibrary.ModelInterface.Datasets;
using ArtLibrary.ModelInterface.Datasets.Types;
using ArtLibrary.ModelInterface.Motion.BackgroundSubtraction;
using ArtLibrary.ModelInterface.Motion.MotionBackground;
using ArtLibrary.ModelInterface.RBSK;
using ArtLibrary.ModelInterface.Results;
using ArtLibrary.ModelInterface.Results.Behaviour.Rotation;
using ArtLibrary.ModelInterface.VideoSettings;

namespace ArtLibrary.Model.Resolver
{
    public static class ModelResolver
    {
        private static Dictionary<Type, Func<object>> _TypeDictionary = new Dictionary<Type, Func<object>>(); 

        public static T Resolve<T>() where T : class
        {
            return _TypeDictionary[typeof(T)].Invoke() as T;
        }

        static ModelResolver()
        {
            _TypeDictionary.Add(typeof(IVideo), () => new Video.Video());
            _TypeDictionary.Add(typeof(IMotionBackgroundSubtraction), () => new MotionBackgroundSubtraction());
            _TypeDictionary.Add(typeof(IVideoSettings), () => new VideoSettings.VideoSettings());
            _TypeDictionary.Add(typeof(IGenerateBoundries), () => new GenerateBoundries());
            _TypeDictionary.Add(typeof(IMotionBackground), () => new MotionBackground());
            _TypeDictionary.Add(typeof(IRBSKVideo), () => new RBSKVideo());
            //_TypeDictionary.Add(typeof(ILargeMemoryVideo), () => new LargeMemoryVideo());
            _TypeDictionary.Add(typeof(ISkeleton), () => new Skeleton());
            _TypeDictionary.Add(typeof(ISpineFinding), () => new SpineFinding());
            _TypeDictionary.Add(typeof(ITailFinding), () => new TailFinding());
            //_TypeDictionary.Add(typeof(ILabbookConverter), () => new LabbookConverter());
            //_TypeDictionary.Add(typeof(ILabbookData), () => new LabbookData());
            _TypeDictionary.Add(typeof(ISingleFile), () => new SingleFile());
            _TypeDictionary.Add(typeof(INonTransgenic), () => new NonTransgenic());
            _TypeDictionary.Add(typeof(ITransgenic), () => new Transgenic());
            _TypeDictionary.Add(typeof(IUndefined), () => new Undefined());
            _TypeDictionary.Add(typeof(ISingleMouse), () => new SingleMouse());
            _TypeDictionary.Add(typeof(IBodyDetection), () => new BodyDetection.BodyDetection());
            _TypeDictionary.Add(typeof(IMouseDataResult), () => new MouseDataResult());
            _TypeDictionary.Add(typeof(IArtefactsBoundary), () => new ArtefactsBoundary());
            _TypeDictionary.Add(typeof(IBoxBoundary), () => new BoxBoundary());
            _TypeDictionary.Add(typeof(ICircleBoundary), () => new CircleBoundary());
            _TypeDictionary.Add(typeof(IOuterBoundary), () => new OuterBoundary());
            _TypeDictionary.Add(typeof(ITrackedVideo), () => new TrackedVideo());
            _TypeDictionary.Add(typeof(IBehaviourHolder), () => new BehaviourHolder());
            _TypeDictionary.Add(typeof(ISaveArtFile), () => new SaveArtFile());
            _TypeDictionary.Add(typeof(ISingleFrameResult), () => new SingleFrameResult());
            _TypeDictionary.Add(typeof(ITrackSmoothing), () => new TrackSmoothing());

            _TypeDictionary.Add(typeof(IBehaviourSpeedDefinitions), () => new BehaviourSpeedDefinitions());
            _TypeDictionary.Add(typeof(IStill), () => new Still());
            _TypeDictionary.Add(typeof(IWalking), () => new Walking());
            _TypeDictionary.Add(typeof(IRunning), () => new Running());
            _TypeDictionary.Add(typeof(INoRotation), () => new NoRotation());
            _TypeDictionary.Add(typeof(ISlowTurning), () => new SlowTurning());
            _TypeDictionary.Add(typeof(IFastTurning), () => new FastTurning());
            _TypeDictionary.Add(typeof(IShaking), () => new Shaking());
            _TypeDictionary.Add(typeof(IHeadVisible), () => new HeadVisible());
            _TypeDictionary.Add(typeof(IBodyVisible), () => new BodyVisible());
            _TypeDictionary.Add(typeof(ITailVisible), () => new TailVisible());
            _TypeDictionary.Add(typeof(IHeadBodyTailVisible), () => new HeadBodyTailVisible());
        }

        public static void AddModels(Dictionary<Type, Func<object>> dictionary)
        {
            foreach (var kvp in _TypeDictionary)
            {
                dictionary.Add(kvp.Key, kvp.Value);
            }
        }
    }
}
