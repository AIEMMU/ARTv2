/*
Manual Whisker Annotator - A program to manually annotate whiskers and analyse them
Copyright (C) 2015 Brett Michael Hewitt

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ArtLibrary.ModelInterface.Video;
using ARWT.Commands;
using ARWT.Model.MWA;
using ARWT.Resolver;
using ARWT.Services;
using ARWT.ViewModel;
using ARWT.ViewModel.Settings;
using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Office.Interop.Excel;
using RobynsWhiskerTracker.View.Settings;
using RobynsWhiskerTracker.View.Settings.UnitSettings;

namespace RobynsWhiskerTracker.ViewModel.Setings
{
    public class UnitSettingsViewModel : ViewModelBase
    {
        private ActionCommand m_BrowseForUnitVideoCommand;
        private ActionCommand m_RevertToDefaultCommand;
        private Bitmap m_Image;

        private string m_UnitsName;
        private double m_UnitsPerPixel;
        private double m_PixelDistance;
        private double m_UnitsDistance;

        private bool m_UnitsDistanceHasError = false;

        private string m_Name;
        
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                if (Equals(m_Name, value))
                {
                    return;
                }

                m_Name = value;

                NotifyPropertyChanged();
            }
        }

        public ActionCommand BrowseForUnitVideoCommand
        {
            get
            {
                return m_BrowseForUnitVideoCommand ?? (m_BrowseForUnitVideoCommand = new ActionCommand()
                {
                    ExecuteAction = BrowseForUnitVideo,
                });
            }
        }

        public ActionCommand RevertToDefaultCommand
        {
            get
            {
                return m_RevertToDefaultCommand ?? (m_RevertToDefaultCommand = new ActionCommand()
                {
                    ExecuteAction = RevertToDefault,
                });
            }
        }

        public string UnitsName
        {
            get
            {
                return m_UnitsName;
            }
            set
            {
                if (Equals(m_UnitsName, value))
                {
                    return;
                }

                m_UnitsName = value;

                MarkAsDirtyAndNotifyPropertyChange();
            }
        }

        public double UnitsPerPixel
        {
            get
            {
                return m_UnitsPerPixel;
            }
            set
            {
                if (Equals(m_UnitsPerPixel, value))
                {
                    return;
                }

                m_UnitsPerPixel = value;

                MarkAsDirtyAndNotifyPropertyChange();
            }
        }

        public double PixelDistance
        {
            get
            {
                return m_PixelDistance;
            }
            set
            {
                if (Equals(m_PixelDistance, value))
                {
                    return;
                }

                m_PixelDistance = value;

                UpdateUnitsPerPixel();

                MarkAsDirtyAndNotifyPropertyChange();
                NotifyPropertyChanged("Enabled");
            }
        }

        public double UnitsDistance
        {
            get
            {
                return m_UnitsDistance;
            }
            set
            {
                if (Equals(m_UnitsDistance, value))
                {
                    return;
                }

                m_UnitsDistance = value;

                UpdateUnitsPerPixel();

                MarkAsDirtyAndNotifyPropertyChange();
            }
        }

        public bool Enabled
        {
            get
            {
                return PixelDistance > 0;
            }
        }

        public Bitmap Image
        {
            get
            {
                return m_Image;
            }
            set
            {
                if (Equals(m_Image, value))
                {
                    return;
                }

                if (m_Image != null)
                {
                    m_Image.Dispose();
                }

                m_Image = value;

                NotifyPropertyChanged();
            }
        }

        public bool UnitsDistanceHasError
        {
            get
            {
                return m_UnitsDistanceHasError;
            }
            set
            {
                if (Equals(m_UnitsDistanceHasError, value))
                {
                    return;
                }

                m_UnitsDistanceHasError = value;

                NotifyPropertyChanged();
            }
        }

        private IUnitSettings Model
        {
            get;
            set;
        }

        public UnitSettingsViewModel(IUnitSettings model)
        {
            Model = model;
            Name = "Unit Settings";
        }

        public void LoadSettings(object sender, RoutedEventArgs e)
        {
            UnitsName = Model.UnitsName;
            UnitsPerPixel = Model.UnitsPerPixel;
        }

        private void UpdateUnitsPerPixel()
        {
            if (PixelDistance > 0 && UnitsDistance > 0)
            {
                UnitsPerPixel = UnitsDistance / PixelDistance;
            }
        }

        public void SaveSettings()
        {
            Model.UnitsName = UnitsName;
            Model.UnitsPerPixel = UnitsPerPixel;
        }

        private void BrowseForUnitVideo()
        {
            string filePath = FileBrowser.BroseForVideoFiles();

            if (string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }

            using (IVideo video = ModelResolver.Resolve<IVideo>())
            {
                video.SetVideo(filePath);

                using (Image<Bgr, Byte> image = video.GetFrameImage())
                {
                    Image = image.ToBitmap();

                    PickUnitsPointsViewModel viewModel = new PickUnitsPointsViewModel(Image);
                    PickUnitsPointsView view = new PickUnitsPointsView()
                    {
                        DataContext = viewModel,
                    };

                    view.ShowDialog();

                    if (viewModel.OkPressed)
                    {
                        PixelDistance = viewModel.GetDistance();
                    }
                }
            }
        }

        private void RevertToDefault()
        {
            UnitsPerPixel = 1;
            UnitsName = "pixels";
        }

        public bool Validate(ref string message)
        {
            if (UnitsDistanceHasError)
            {
                message += "Units Distance is not valid\r\n";
                return false;
            }

            return true;
        }
    }
}
