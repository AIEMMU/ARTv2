using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtLibrary.ModelInterface.Boundries;
using ArtLibrary.ModelInterface.Video;
using ArtLibrary.ModelInterface.VideoSettings;
using ARWT.ModelInterface.RBSK2;
using ARWT.ModelInterface.Results;
using ARWT.Resolver;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ARWT.Model.Datasets
{
    internal class MouseRun : ModelObjectBase
    {
        private IVideoSettings _VideoSettings;
        public IVideoSettings VideoSettings
        {
            get
            {
                return _VideoSettings;
            }
            set
            {
                if (Equals(_VideoSettings, value))
                {
                    return;
                }

                _VideoSettings = value;

                MarkAsDirty();
            }
        }

        private IWhiskerVideoSettings _WhiskerSettings;
        public IWhiskerVideoSettings WhiskerSettings
        {
            get
            {
                return _WhiskerSettings;
            }
            set
            {
                if (Equals(_WhiskerSettings, value))
                {
                    return;
                }

                _WhiskerSettings = value;

                MarkAsDirty();
            }
        }

        private Image<Gray, byte> _BinaryBackground;
        public Image<Gray, byte> BinaryBackground
        {
            get
            {
                return _BinaryBackground;
            }
            set
            {
                if (Equals(_BinaryBackground, value))
                {
                    return;
                }

                _BinaryBackground = value;

                MarkAsDirty();
            }
        }

        private bool _PreviewGenerated;
        public bool PreviewGenerated
        {
            get
            {
                return _PreviewGenerated;
            }
            set
            {
                if (Equals(_PreviewGenerated, value))
                {
                    return;
                }

                _PreviewGenerated = value;

                MarkAsDirty();
            }
        }

        private IMouseDataExtendedResult _DataResult;
        public IMouseDataExtendedResult DataResult
        {
            get
            {
                return _DataResult;
            }
            set
            {
                if (Equals(_DataResult, value))
                {
                    return;
                }

                _DataResult = value;

                MarkAsDirty();
            }
        }

        private string _FileName;
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                if (Equals(_FileName, value))
                {
                    return;
                }

                _FileName = value;

                MarkAsDirty();
            }
        }


        public void Go()
        {
            using (IVideo video = ModelResolver.Resolve<IVideo>())
            {
                video.SetVideo(FileName);
                IMouseDataExtendedResult result = ModelResolver.Resolve<IMouseDataExtendedResult>();

                if (!PreviewGenerated)
                {
                    GeneratePreview(video);
                }

                result.FrameRate = video.FrameRate;

                IRBSKVideo2 rbskVideo = ModelResolver.Resolve<IRBSKVideo2>();
                rbskVideo.Video = video;
                rbskVideo.BackgroundImage = BinaryBackground;
                rbskVideo.ThresholdValue = VideoSettings.ThresholdValue;
            }
        }

        private void GeneratePreview(IVideo video)
        {
            Image<Gray, Byte> binaryBackground;
            IEnumerable<IBoundaryBase> boundaries;
            VideoSettings.MotionLength = 100;
            VideoSettings.StartFrame = 0;
            VideoSettings.GeneratePreview(video, out binaryBackground, out boundaries);
            BinaryBackground = binaryBackground;
        }
    }
}
