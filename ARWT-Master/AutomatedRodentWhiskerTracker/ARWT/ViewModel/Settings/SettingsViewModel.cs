/*Automated Rodent Tracker - A program to automatically track rodents
Copyright(C) 2015 Brett Michael Hewitt

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.If not, see<http://www.gnu.org/licenses/>.*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using ArtLibrary.ModelInterface.Boundries;
using ArtLibrary.ModelInterface.Video;
using ArtLibrary.ModelInterface.VideoSettings;
using ArtLibrary.Services.Mouse;
using ArtLibrary.Services.RBSK;
using ARWT.Commands;
using ARWT.Foot.tracking;
using ARWT.Foot.tracking.data;
using ARWT.Foot.tracking.Model;
using ARWT.Foot.tracking.ViewModel;
using ARWT.ModelInterface.RBSK2;
using ARWT.Resolver;
using ARWT.Services;
using ARWT.View.CropImage;
using ARWT.View.NewSession;
using ARWT.View.Settings;
using ARWT.ViewModel.CropImage;
using ARWT.ViewModel.Datasets;
using ARWT.ViewModel.NewSession;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ARWT.ViewModel.Settings
{
    public class SettingsViewModel : WindowBaseModel
    {
        private ActionCommand m_OkCommand;
        private ActionCommand m_CancelCommand;
        private ActionCommand _WhiskerSettingsCommand;
        private ActionCommand _FeetSettingsCommand;


        public ActionCommand FeetSettingsCommand
        {
            get
            {
                return _FeetSettingsCommand ?? (_FeetSettingsCommand = new ActionCommand()
                {
                    ExecuteAction = ShowFeetSettings,
                });
            }
        }
        public ActionCommand OkCommand
        {
            get
            {
                return m_OkCommand ?? (m_OkCommand = new ActionCommand()
                {
                    ExecuteAction = Ok,
                });
            }
        }

        public ActionCommand CancelCommand
        {
            get
            {
                return m_CancelCommand ?? (m_CancelCommand = new ActionCommand()
                {
                    ExecuteAction = Cancel,
                });
            }
        }

        public ActionCommand WhiskerSettingsCommand
        {
            get
            {
                return _WhiskerSettingsCommand ?? (_WhiskerSettingsCommand = new ActionCommand()
                {
                    ExecuteAction = ShowWhiskerSettings
                });
            }
        }

        private ObservableCollection<SingleFileViewModel> m_Mice;
        public ObservableCollection<SingleFileViewModel> Mice
        {
            get
            {
                return m_Mice;
            }
            set
            {
                if (ReferenceEquals(m_Mice, value))
                {
                    return;
                }

                m_Mice = value;

                NotifyPropertyChanged();
            }
        }

        private SingleFileViewModel m_SelectedMouse;
        public SingleFileViewModel SelectedMouse
        {
            get
            {
                return m_SelectedMouse;
            }
            set
            {
                if (Equals(m_SelectedMouse, value))
                {
                    return;
                }

                m_SelectedMouse = value;

                SelectedMouseChanged();
                NotifyPropertyChanged();
            }
        }

        private BitmapSource m_Image;
        public BitmapSource DisplayImage
        {
            get
            {
                return m_Image;
            }
            set
            {
                if (ReferenceEquals(m_Image, value))
                {
                    return;
                }

                m_Image = value;

                NotifyPropertyChanged();
            }
        }

        private int m_SliderValue;
        public int SliderValue
        {
            get
            {
                return m_SliderValue;
            }
            set
            {
                if (Equals(m_SliderValue, value))
                {
                    return;
                }

                m_SliderValue = value;

                NotifyPropertyChanged();
                SliderValueChanged();
            }
        }

        private int m_Minimum;
        public int Minimum
        {
            get
            {
                return m_Minimum;
            }
            set
            {
                if (Equals(m_Minimum, value))
                {
                    return;
                }

                m_Minimum = value;

                NotifyPropertyChanged();
            }
        }

        private int m_Maximum;
        public int Maximum
        {
            get
            {
                return m_Maximum;
            }
            set
            {
                if (Equals(m_Maximum, value))
                {
                    return;
                }

                m_Maximum = value;

                NotifyPropertyChanged();
            }
        }

        private IVideo m_Video;
        public IVideo Video
        {
            get
            {
                return m_Video;
            }
            set
            {
                if (Equals(m_Video, value))
                {
                    return;
                }

                if (m_Video != null)
                {
                    m_Video.Dispose();
                }

                m_Video = value;

                NotifyPropertyChanged();
            }
        }

        private bool m_ShowVideo;
        public bool ShowVideo
        {
            get
            {
                return m_ShowVideo;
            }
            set
            {
                if (Equals(m_ShowVideo, value))
                {
                    return;
                }

                m_ShowVideo = value;

                NotifyPropertyChanged();
            }
        }

        private double m_GapDistance;
        public double GapDistance
        {
            get
            {
                return m_GapDistance;
            }
            set
            {
                if (Equals(m_GapDistance, value))
                {
                    return;
                }

                m_GapDistance = value;

                NotifyPropertyChanged();
                UpdateGapDistance();
            }
        }

        private double m_GapDistanceMin;
        public double GapDistanceMin
        {
            get
            {
                return m_GapDistanceMin;
            }
            set
            {
                if (Equals(m_GapDistanceMin, value))
                {
                    return;
                }

                m_GapDistanceMin = value;

                NotifyPropertyChanged();
            }
        }

        private double m_GapDistanceMax;
        public double GapDistanceMax
        {
            get
            {
                return m_GapDistanceMax;
            }
            set
            {
                if (Equals(m_GapDistanceMax))
                {
                    return;
                }

                m_GapDistanceMax = value;

                NotifyPropertyChanged();
            }
        }

        private int m_BinaryThreshold;
        public int BinaryThreshold
        {
            get
            {
                return m_BinaryThreshold;
            }
            set
            {
                if (Equals(m_BinaryThreshold, value))
                {
                    return;
                }

                m_BinaryThreshold = value;

                NotifyPropertyChanged();
                UpdateBinaryThreshold();
            }
        }

        private int m_BinaryThresholdMax;
        public int BinaryThresholdMax
        {
            get
            {
                return m_BinaryThresholdMax;
            }
            set
            {
                if (Equals(m_BinaryThresholdMax, value))
                {
                    return;
                }

                m_BinaryThresholdMax = value;

                NotifyPropertyChanged();
            }
        }

        private int m_BinaryThresholdMin;
        public int BinaryThresholdMin
        {
            get
            {
                return m_BinaryThresholdMin;
            }
            set
            {
                if (Equals(m_BinaryThresholdMin, value))
                {
                    return;
                }

                m_BinaryThresholdMin = value;

                NotifyPropertyChanged();
            }
        }

        private int m_BinaryThreshold2;
        public int BinaryThreshold2
        {
            get
            {
                return m_BinaryThreshold2;
            }
            set
            {
                if (Equals(m_BinaryThreshold2, value))
                {
                    return;
                }

                m_BinaryThreshold2 = value;

                NotifyPropertyChanged();
                UpdateBinaryThreshold2();
            }
        }

        private int m_BinaryThreshold2Max;
        public int BinaryThreshold2Max
        {
            get
            {
                return m_BinaryThreshold2Max;
            }
            set
            {
                if (Equals(m_BinaryThreshold2Max, value))
                {
                    return;
                }

                m_BinaryThreshold2Max = value;

                NotifyPropertyChanged();
            }
        }

        private int m_BinaryThreshold2Min;
        public int BinaryThreshold2Min
        {
            get
            {
                return m_BinaryThreshold2Min;
            }
            set
            {
                if (Equals(m_BinaryThreshold2Min, value))
                {
                    return;
                }

                m_BinaryThreshold2Min = value;

                NotifyPropertyChanged();
            }
        }

        private Image<Bgr, Byte> m_CurrentImage;
        public Image<Bgr, Byte> CurrentImage
        {
            get
            {
                return m_CurrentImage;
            }
            set
            {
                if (Equals(m_CurrentImage, value))
                {
                    return;
                }

                if (m_CurrentImage != null)
                {
                    m_CurrentImage.Dispose();
                }

                m_CurrentImage = value;

                NotifyPropertyChanged();
            }
        }

        private bool m_VideoSelected;
        public bool VideoSelected
        {
            get
            {
                return m_VideoSelected;
            }
            set
            {
                if (Equals(m_VideoSelected, value))
                {
                    return;
                }

                m_VideoSelected = value;

                NotifyPropertyChanged();
            }
        }

        private bool m_SmoothMotion;
        public bool SmoothMotion
        {
            get
            {
                return m_SmoothMotion;
            }
            set
            {
                if (Equals(m_SmoothMotion, value))
                {
                    return;
                }

                m_SmoothMotion = value;

                NotifyPropertyChanged();
            }
        }

        private double m_FrameRate;
        public double FrameRate
        {
            get
            {
                return m_FrameRate;
            }
            set
            {
                if (Equals(m_FrameRate, value))
                {
                    return;
                }

                m_FrameRate = value;

                NotifyPropertyChanged();
            }
        }

        private Rectangle _ROI;
        public Rectangle ROI
        {
            get
            {
                return _ROI;
            }
            set
            {
                if (Equals(_ROI, value))
                {
                    return;
                }

                _ROI = value;

                NotifyPropertyChanged();
            }
        }

        private ActionCommand _SetRoiCommand;
        public ActionCommand SetRoiCommand
        {
            get
            {
                return _SetRoiCommand ?? (_SetRoiCommand = new ActionCommand()
                {
                    ExecuteAction = SetRoi
                });
            }
        }

        private ActionCommand _RemoveRoiCommand;
        public ActionCommand RemoveRoiCommand
        {
            get
            {
                return _RemoveRoiCommand ?? (_RemoveRoiCommand = new ActionCommand()
                {
                    ExecuteAction = RemoveRoi
                });
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

                NotifyPropertyChanged();
            }
        }


        private IFootVideoSettings _footVideoSettings;
        public IFootVideoSettings FootVideoSettings
        {
            get
            {
                return _footVideoSettings;
            }
            set
            {
                if (Equals(_footVideoSettings, value))
                {
                    return;
                }

                _footVideoSettings = value;

                NotifyPropertyChanged();
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

                NotifyPropertyChanged();
            }
        }


        public SettingsViewModel(IEnumerable<SingleMouseViewModel> mice)
        {
            GapDistanceMin = 5;
            GapDistanceMax = 300;
            BinaryThresholdMin = 0;
            BinaryThresholdMax = 255;
            BinaryThreshold2Min = 0;
            BinaryThreshold2Max = 255;
            
            m_GapDistance = 35;
            m_BinaryThreshold = 20;
            m_BinaryThreshold2 = 10;
            SmoothMotion = true;

            SingleMouseViewModel firstMouse = mice.FirstOrDefault();
            if (firstMouse != null)
            {
                m_GapDistance = firstMouse.GapDistance;
                m_BinaryThreshold = firstMouse.ThresholdValue;
                m_BinaryThreshold2 = firstMouse.ThresholdValue2;
                SmoothMotion = firstMouse.SmoothMotion;
                WhiskerSettings = firstMouse.WhiskerSettings;
                FootVideoSettings = firstMouse.FootSettings;
                
            }

            ObservableCollection<SingleFileViewModel> singleFiles = new ObservableCollection<SingleFileViewModel>();
            foreach (var mouse in mice)
            {
                foreach (var file in mouse.VideoFiles)
                {
                    singleFiles.Add(new SingleFileViewModel(file, ""));
                }
            }

            Mice = singleFiles;

            using (IVideo video = ModelResolver.Resolve<IVideo>())
            {
                video.SetVideo(Mice.First().VideoFileName);
                FrameRate = video.FrameRate;
            }

            SelectedMouse = Mice.First();
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

        private void SliderValueChanged()
        {
           
            Video.SetFrame(SliderValue);
            CurrentImage = Video.GetFrameImage();
            CurrentImage.ROI = ROI;
            
            UpdateDisplayImage();
        }

        private void UpdateDisplayImage()
        {
            RBSK rbsk = MouseService.GetStandardMouseRules();
            rbsk.Settings.GapDistance = GapDistance;
            rbsk.Settings.BinaryThreshold = BinaryThreshold;
            
            PointF[] result = RBSKService.RBSK(CurrentImage, rbsk);
            using (Image<Bgr, Byte> img = CurrentImage.Clone())
            {
                //if (!ROI.IsEmpty)
                //{
                //    img.ROI = ROI;
                //}

                if (result != null)
                {
                    foreach (PointF point in result)
                    {
                        img.Draw(new CircleF(point, 2), new Bgr(Color.Yellow), 2);
                    }
                }

                DisplayImage = ImageService.ToBitmapSource(img);
            }
           
        }

        private void UpdateGapDistance()
        {
            RBSK rbsk = MouseService.GetStandardMouseRules();
            rbsk.Settings.GapDistance = GapDistance;
            rbsk.Settings.BinaryThreshold = BinaryThreshold;
            PointF[] result = RBSKService.RBSK(CurrentImage, rbsk);
            using (Image<Bgr, Byte> img = CurrentImage.Clone())
            {
                if (result != null)
                {
                    foreach (PointF point in result)
                    {
                        img.Draw(new CircleF(point, 2), new Bgr(Color.Yellow), 2);
                    }
                }

                DisplayImage = ImageService.ToBitmapSource(img);
            }
        }

        private void UpdateBinaryThreshold()
        {
            using (Image<Gray, Byte> grayImage = CurrentImage.Convert<Gray, Byte>())
            using (Image<Gray, Byte> binaryImage = grayImage.ThresholdBinary(new Gray(BinaryThreshold), new Gray(255)))
            {
                DisplayImage = ImageService.ToBitmapSource(binaryImage);
            }
        }

        private void UpdateBinaryThreshold2()
        {
            using (Image<Gray, Byte> grayImage = CurrentImage.Convert<Gray, Byte>())
            using (Image<Gray, Byte> binaryImage = grayImage.ThresholdBinary(new Gray(BinaryThreshold2), new Gray(255)))
            {
                DisplayImage = ImageService.ToBitmapSource(binaryImage);
            }
        }

        private void SelectedMouseChanged()
        {
            if (SelectedMouse == null)
            {
                Video = null;
                ShowVideo = false;
                return;
            }

            Video = ModelResolver.Resolve<IVideo>();
            Video.SetVideo(SelectedMouse.VideoFileName);
            Maximum = Video.FrameCount - 1;
            Minimum = 0;
            m_SliderValue = -1;
            SliderValue = 0;
            ShowVideo = true;
        }

        private void SetRoi()
        {
            CropImageViewModel viewModel = new CropImageViewModel();
            viewModel.DisplayImage = ImageService.ToBitmapSource(CurrentImage);
            viewModel.AssignRegionOfInterestToValues(new Rect(ROI.X, ROI.Y, ROI.Width, ROI.Height));
            CropImageView view = new CropImageView();
            view.DataContext = viewModel;
            view.ShowDialog();

            if (viewModel.ExitResult != WindowExitResult.Ok)
            {
                return;
            }

            Rect roiRect = viewModel.GetRoi();
            if (roiRect.Width == 0 || roiRect.Height == 0)
            {
                ROI = Rectangle.Empty;
                CurrentImage.ROI = Rectangle.Empty;
                return;
            }

            ROI = new Rectangle((int)roiRect.X, (int)roiRect.Y, (int)roiRect.Width, (int)roiRect.Height);
            CurrentImage.ROI = ROI;
            UpdateDisplayImage();
        }

        protected override void WindowClosing(CancelEventArgs closingEventArgs)
        {
            if (Video != null)
            {
                Video.Dispose();
            }

            if (CurrentImage != null)
            {
                CurrentImage.Dispose();
            }
        }

        private void RemoveRoi()
        {
            CurrentImage.ROI = Rectangle.Empty;
        }

        private void ShowWhiskerSettings()
        {
            if (!PreviewGenerated)
            {
                GeneratePreview();
            }

            IRBSKVideo2 rbskVideo = ModelResolver.Resolve<IRBSKVideo2>();
            rbskVideo.Video = Video;
            rbskVideo.BackgroundImage = BinaryBackground;
            rbskVideo.ThresholdValue = BinaryThreshold;
            rbskVideo.Roi = ROI;
            rbskVideo.GapDistance = GapDistance;
            rbskVideo.WhiskerSettings = WhiskerSettings;

            NewWhiskerSessionView view = new NewWhiskerSessionView();
            NewWhiskerSessionViewModel vm = new NewWhiskerSessionViewModel(rbskVideo, SelectedMouse.VideoFileName);
            view.DataContext = vm;
            view.ShowDialog();
        }

        private void ShowFeetSettings()
        {
            if (!PreviewGenerated)
            {
                GeneratePreview();
            }

            IRBSKVideo2 rbskVideo = ModelResolver.Resolve<IRBSKVideo2>();
            rbskVideo.Video = Video;
            rbskVideo.BackgroundImage = BinaryBackground;
            rbskVideo.ThresholdValue = BinaryThreshold;
            rbskVideo.Roi = ROI;
            rbskVideo.GapDistance = GapDistance;
            rbskVideo.WhiskerSettings = WhiskerSettings;
            rbskVideo.FootSettings = FootVideoSettings;

            NewFootTrackingSessionView view = new NewFootTrackingSessionView(new FootTrackingViewModel(new FootSettingsDataService(), Video, rbskVideo, SelectedMouse.VideoFileName));
            
            view.ShowDialog();
        }

        private void GeneratePreview()
        {
            IVideoSettings videoSettings = ModelResolver.Resolve<IVideoSettings>();
            videoSettings.FileName = SelectedMouse.VideoFileName;
            videoSettings.ThresholdValue = BinaryThreshold;
            videoSettings.Roi = ROI;
            Image<Gray, Byte> binaryBackground;
            IEnumerable<IBoundaryBase> boundaries;
            videoSettings.GeneratePreview(Video, out binaryBackground, out boundaries);
            BinaryBackground = binaryBackground;
            VideoSettings = videoSettings;
            PreviewGenerated = true;
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

                NotifyPropertyChanged();
            }
        }


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

                NotifyPropertyChanged();
            }
        }

    }
}
