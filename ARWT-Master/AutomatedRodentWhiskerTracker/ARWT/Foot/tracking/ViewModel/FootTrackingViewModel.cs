using ARWT.Resolver;
using ArtLibrary.ModelInterface.Video;
using ArtLibrary.Services.Mouse;
using ArtLibrary.Services.RBSK;
using ARWT.Commands;
using ARWT.Foot.tracking.data;
using ARWT.Foot.tracking.Model;
using ARWT.Foot.video_processing;
using ARWT.ModelInterface.RBSK2;
using ARWT.Services;
using ARWT.ViewModel;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;

namespace ARWT.Foot.tracking.ViewModel
{
    public class FootTrackingViewModel : WindowBaseModel
    {
        private IFootSettingsDataService _settingsDataService;
        private FootSettings _settings;
        private IRBSKVideo2 _rbskVideo;
        private string _videoFileName;


        public FootTrackingViewModel(IFootSettingsDataService settingsDataService,IVideo video,
            IRBSKVideo2 rbskVideo,
            string videoFileName)
        {
            
            _settingsDataService = settingsDataService;
            Settings = new FootSettings();


            _video = video;
            
            _rbskVideo = rbskVideo;
            _videoFileName = videoFileName;
            if(_rbskVideo.FootSettings == null)
            {
                FootVideoSettings = ModelResolver.Resolve<IFootVideoSettings>();
                FootVideoSettings.AssignDefaultValues();
                _rbskVideo.FootSettings = FootVideoSettings;
            }
            else
            {
                FootVideoSettings = _rbskVideo.FootSettings;
                area = FootVideoSettings.area;
                contourDistance = FootVideoSettings.contourDistance;
                KernelSize = FootVideoSettings.kernelSize;
                scaleFactor = FootVideoSettings.scaleFactor;
                erosionIterations = FootVideoSettings.erosionIterations;
            }


            maxFrame = _video.FrameCount-1 ;
            minFrame = 0;
            Frame = 0;
            
            FrameNumberDisplay = "Frame: " + Frame;
        }
        public void loadSettings()
        {

            Settings = _settingsDataService.GetSettings();
            selectedColorSpace = 0;
        }
        private IVideo _Video;
        public IVideo Video
        {
            get
            {
                return _Video;
            }
            set
            {
                if (Equals(_Video, value))
                {
                    return;
                }

                _Video = value;

                NotifyPropertyChanged();
            }
        }
        private int _Frame;

        public int Frame
        {
            get { return _Frame; }
            set
            {
                _Frame = value; NotifyPropertyChanged();
                SliderValueChanged();
            }
        }
        private int _erosionIterations;

        public int erosionIterations
        {
            get { return _erosionIterations; }
            set { _erosionIterations = value;
                NotifyPropertyChanged();
                FootVideoSettings.erosionIterations = Math.Abs(value);

            }
        }

        private IFootVideoSettings footVideoSettings;

        public IFootVideoSettings FootVideoSettings
        {
            get { return footVideoSettings; }
            set { footVideoSettings = value;
                NotifyPropertyChanged(); }
        }

        private BitmapSource _displayImage;

        public BitmapSource DisplayImage
        {
            get { return _displayImage; }
            set
            {
                _displayImage = value;
                NotifyPropertyChanged();
            }
        }
        private Image<Bgr, byte> _CurrentImage;

        public Image<Bgr, byte> CurrentImage
        {
            get { return _CurrentImage; }
            set
            {
                _CurrentImage = value;
                NotifyPropertyChanged();
            }
        }

        private void SliderValueChanged()
        {
            _video.SetFrame(Frame);
            CurrentImage = _video.GetFrameImage();
            UpdateDisplayImage();

            FrameNumberDisplay = "Frame: " + Frame;
        }

        private void Ok()
        {
            ExitResult = WindowExitResult.Ok;
            CloseWindow();
        }

        private void Cancel()
        {
            ExitResult = WindowExitResult.Cancel;
            CloseWindow();
        }
        private Image<Bgr, byte> GetProcessedImage()
        {

            return ColorSpaceProcessing.Process(CurrentImage.Clone(), 1);
            
        }
        private void UpdateDisplayImage()
        {
           _video.SetFrame(Frame);
            using (CurrentImage = _video.GetFrameImage())
            {
                if (CurrentImage == null)
                {
                    return;
                }
                CurrentImage = GetProcessedImage();
    
                DisplayImage = ImageService.ToBitmapSource(CurrentImage);
            }
            Image<Bgr, double> img = CurrentImage.Convert<Bgr, double>().Clone();
            
            DisplayImage = ImageService.ToBitmapSource(img);

        }

        private void Preview()
        {

            PointF[] headPoints;
            Point[] bodyPoints;
            _video.SetFrame(Frame);
            CurrentImage = _video.GetFrameImage();
            _rbskVideo.GetHeadAndBody(CurrentImage, out headPoints, out bodyPoints);
            CurrentImage = GetProcessedImage();
            Image<Bgr, byte> mask_filter = ColorSpaceProcessing.Process(CurrentImage.Clone(), 1);
            Image<Bgr, byte> img = CurrentImage.Clone();
            if (bodyPoints != null)
            {
                img.DrawPolyline(bodyPoints, true, new Bgr(Color.Yellow));

                var mask = MaskSegmentation.segmentMask(bodyPoints, ColorSpaceProcessing.processLogSpace(mask_filter).Convert<Bgr, double>(), FootVideoSettings.kernelSize, FootVideoSettings.erosionIterations);

                List<VectorOfPointF> unified = ContourDetection.detectFootContours(mask.Clone(), FootVideoSettings.scaleFactor, FootVideoSettings.contourDistance);

                foreach (var c in unified)
                {
                    if (CvInvoke.ContourArea(c) > Settings.area)
                    {
                        var rect = CvInvoke.BoundingRectangle(c);
                        img.Draw(rect, new Bgr(Color.Aquamarine));
                    }
                }

            }

            if (headPoints != null)
            {
                img.Draw(new CircleF(headPoints[2], 2), new Bgr(Color.Red));

            }
            DisplayImage = ImageService.ToBitmapSource(img);

        }


        protected override void WindowClosing(CancelEventArgs closingEventArgs)
        {
            if (CurrentImage != null)
            {
                CurrentImage.Dispose();
            }
        }
        private string _FrameNumberDisplay;
        public string FrameNumberDisplay
        {
            get { return _FrameNumberDisplay; }
            set { _FrameNumberDisplay = value; NotifyPropertyChanged(); }
        }

        private int _minFrame;
        public int minFrame
        {
            get { return _minFrame; }
            set { _minFrame = value; NotifyPropertyChanged(); }
        }
        private bool _colorSelected;

        public bool colorSelected
        {
            get { return _colorSelected; }
            set
            {
                _colorSelected = value;
                NotifyPropertyChanged();
            }
        }


        private int _maxFrame;
        public int maxFrame
        {
            get { return _maxFrame; }
            set { _maxFrame = value; NotifyPropertyChanged(); }
        }

        public FootSettings Settings
        {
            get { return _settings; }
            set
            {
                _settings = value;
                NotifyPropertyChanged();
            }
        }

        private IVideo _video;
        private int? _selectedColorSpace;

        public int? selectedColorSpace
        {
            get
            {
                return _selectedColorSpace;
            }
            set
            {
                _selectedColorSpace = value;
                NotifyPropertyChanged();
                sliderBarsenabled();
            }
        }

        private int _gain;
        public int gain
        {
            get { return _gain; }
            set
            {
                _gain = value;
                NotifyPropertyChanged();
                
                SliderValueChanged();
            }
        }
        private int _scaleFactor;

        public int scaleFactor
        {
            get { return _scaleFactor; }
            set { _scaleFactor = value;
                NotifyPropertyChanged();
                Settings.scaleFactor = _scaleFactor;
                FootVideoSettings.scaleFactor = Math.Abs(value);
            }
        }
        private int _kernelSize;

        public int KernelSize
        {
            get { return _kernelSize; }
            set
            {
                if (value % 2 == 0)
                {
                    _kernelSize = value + 1;
                }
                else
                {
                    _kernelSize = value;
                }

                NotifyPropertyChanged();
                Settings.eroisionKernel = _kernelSize;
                FootVideoSettings.kernelSize = _kernelSize;
            }
        }
        private int _contourDistance;

        public int contourDistance
        {
            get { return _contourDistance; }
            set { _contourDistance = value;
                NotifyPropertyChanged();
                Settings.contourDistance = value;
                FootVideoSettings.contourDistance = Math.Abs(value);
            }
        }
        
        private int _area;
        public int area
        {
            get { return _area; }
            set
            {
                _area = value;
                NotifyPropertyChanged();
                Settings.area = value;
                FootVideoSettings.area = value;
                SliderValueChanged();
            }
        }

        private void sliderBarsenabled()
        {
            switch (selectedColorSpace)
            {
                case 0:
                    colorSelected = true;
                    break;
                case 1:
                case 2:
                    colorSelected = false;
                    break;
            }
            UpdateDisplayImage();


        }

        private ActionCommand _okCommand;
        public ActionCommand OkCommand
        {
            get
            {
                return _okCommand ?? (_okCommand = new ActionCommand()
                {
                    ExecuteAction = Ok
                });
            }
        }

        private ActionCommand _cancelCommand;
        public ActionCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new ActionCommand()
                {
                    ExecuteAction = Cancel
                });
            }
        }

        private ActionCommand _previewCommand;
        public ActionCommand PreviewCommand
        {
            get
            {
                return _previewCommand ?? (_previewCommand = new ActionCommand()
                {
                    ExecuteAction = Preview
                });
            }
        }

    }
}
