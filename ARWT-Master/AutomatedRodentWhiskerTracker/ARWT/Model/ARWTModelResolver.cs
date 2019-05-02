using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.ModelInterface.RBSK;
using ARWT.Model.FileExtension;
using ARWT.Model.Masks;
using ARWT.Model.NonMaxSuppression;
using ARWT.Model.NonMaxSuppression.Angles;
using ARWT.Model.RBSK2;
using ARWT.Model.Whiskers;
using ARWT.ModelInterface.FileExtension;
using ARWT.ModelInterface.Masks;
using ARWT.ModelInterface.NonMaxSuppression;
using ARWT.ModelInterface.NonMaxSuppression.Angles;
using ARWT.ModelInterface.RBSK2;
using ARWT.ModelInterface.Whiskers;
using Line = ArtLibrary.Extensions.Line;

namespace ARWT.Model
{
    public static class ARWTModelResolver
    {
        private static Dictionary<Type, Func<object>> _TypeDictionary = new Dictionary<Type, Func<object>>(); 

        public static T Resolve<T>() where T : class
        {
            return _TypeDictionary[typeof(T)].Invoke() as T;
        }

        static ARWTModelResolver()
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
        }
    }
}
