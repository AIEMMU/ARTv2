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
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;
using ARWT.Commands;
using ARWT.Extensions;
using ARWT.Model.MWA;
using ARWT.Resolver;
using Brush = System.Drawing.Brush;
using Color = System.Drawing.Color;
using Point = System.Windows.Point;

namespace ARWT.ViewModel.Settings
{
    public interface IMouseClickedViewModel
    {
        void MouseClicked(object sender, MouseButtonEventArgs e);
    }

    public class PickUnitsPointsViewModel : WindowBaseModel, IMouseClickedViewModel
    {
        private ActionCommand m_OkCommand;
        private ActionCommand _CancelCommand;

        private Bitmap m_Image;
        private int m_CurrentPoint = 1;
        private double m_ObservedImageWidth;
        private double m_ObservedImageHeight;

        private ObservableCollection<GenericPointViewModel> m_CanvasChildren = new ObservableCollection<GenericPointViewModel>();
        private readonly ObservableCollection<int> m_PointsList = new ObservableCollection<int>()
        {
            1,
            2,
        };

        public ActionCommand OkCommand
        {
            get
            {
                return m_OkCommand ?? (m_OkCommand = new ActionCommand()
                {
                    ExecuteAction = Ok,
                    CanExecuteAction = CanOk
                });
            }
        }

        public ActionCommand CancelCommand
        {
            get
            {
                return _CancelCommand ?? (_CancelCommand = new ActionCommand()
                {
                    ExecuteAction = Cancel
                });
            }
        }

        public ObservableCollection<GenericPointViewModel> CanvasChildren
        {
            get
            {
                return m_CanvasChildren;
            }
            set
            {
                if (ReferenceEquals(m_CanvasChildren, value))
                {
                    return;
                }

                m_CanvasChildren = value;

                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<int> PointsList
        {
            get
            {
                return m_PointsList;
            }
        }

        public int CurrentPoint
        {
            get
            {
                return m_CurrentPoint;
            }
            set
            {
                if (Equals(m_CurrentPoint, value))
                {
                    return;
                }

                m_CurrentPoint = value;

                NotifyPropertyChanged();
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
                m_Image = value;

                NotifyPropertyChanged();
            }
        }

        public bool OkPressed
        {
            get;
            set;
        }

        public double ObservedImageWidth
        {
            get
            {
                return m_ObservedImageWidth;
            }
            set
            {                
                if (Equals(m_ObservedImageWidth, value))
                {
                    return;
                }

                m_ObservedImageWidth = value;
                
                foreach (GenericPointViewModel genericPointViewModel in CanvasChildren)
                {
                    genericPointViewModel.CanvasWidth = m_ObservedImageWidth;
                }

                NotifyPropertyChanged();
            }
        }

        public double ObservedImageHeight
        {
            get
            {
                return m_ObservedImageHeight;
            }
            set
            {
                if (Equals(m_ObservedImageHeight, value))
                {
                    return;
                }

                m_ObservedImageHeight = value;
                foreach (GenericPointViewModel genericPointViewModel in CanvasChildren)
                {
                    genericPointViewModel.CanvasHeight = m_ObservedImageHeight;
                }

                NotifyPropertyChanged();
            }
        }

        private double _Length;
        public double Length
        {
            get
            {
                return _Length;
            }
            set
            {
                if (Equals(_Length, value))
                {
                    return;
                }

                _Length = value;

                NotifyPropertyChanged();
                OkCommand.RaiseCanExecuteChangedNotification();
            }
        }


        public PickUnitsPointsViewModel(Bitmap bitmap, IWhiskerPoint point1 = null, IWhiskerPoint point2 = null)
        {
            Image = bitmap;

            IGenericPoint genericPoint1 = ModelResolver.Resolve<IGenericPoint>();
            IGenericPoint genericPoint2 = ModelResolver.Resolve<IGenericPoint>();

            genericPoint1.PointId = 1;
            genericPoint2.PointId = 2;

            GenericPointViewModel viewModel1 = new GenericPointViewModel(genericPoint1);
            GenericPointViewModel viewModel2 = new GenericPointViewModel(genericPoint2);

            //IWhisker whisker = ModelResolver.Resolve<IWhisker>();
            //whisker.WhiskerId = 1;
            
            //IWhiskerPoint whiskerPoint1 = point1 ?? ModelResolver.Resolve<IWhiskerPoint>();
            //whiskerPoint1.Parent = whisker;
            ////whiskerPoint1.WhiskerId = 0;

            //IWhiskerPoint whiskerPoint2 = point2 ?? ModelResolver.Resolve<IWhiskerPoint>();
            //whiskerPoint2.Parent = whisker;
            ////whiskerPoint2.WhiskerId = 1;
            //whisker.WhiskerPoints = new IWhiskerPoint[] { whiskerPoint1, whiskerPoint2 };

            //WhiskerViewModel whiskerViewModel = new WhiskerViewModel(whisker);
            //WhiskerPointViewModel viewModel1 = new WhiskerPointViewModel(whiskerPoint1, whiskerViewModel);
            //WhiskerPointViewModel viewModel2 = new WhiskerPointViewModel(whiskerPoint2, whiskerViewModel);

            CanvasChildren.Add(viewModel1);
            CanvasChildren.Add(viewModel2);
        }

        public double UnitsToMm
        {
            get;
            set;
        }

        private void Ok()
        {
            double actualImageWidth = Image.Width;
            double actualImageHeight = Image.Height;
            double actualXPos1 = CanvasChildren[0].XRatio*actualImageWidth;
            double actualYPos1 = CanvasChildren[0].YRatio*actualImageHeight;
            double actualXPos2 = CanvasChildren[1].XRatio*actualImageWidth;
            double actualYPos2 = CanvasChildren[1].YRatio*actualImageHeight;
            Point p1 = new Point(actualXPos1, actualYPos1);
            Point p2 = new Point(actualXPos2, actualYPos2);
            double dist = Distance(p1, p2);
            UnitsToMm = Length/dist;
            ExitResult = WindowExitResult.Ok;
            OkPressed = true;
            Close = true;
        }

        private void Cancel()
        {
            ExitResult = WindowExitResult.Cancel;
            CloseWindow();
        }

        public double Distance(Point point, Point p)
        {
            double xDiff = Math.Abs(point.X - p.X);
            double yDiff = Math.Abs(point.Y - p.Y);
            return Math.Sqrt((xDiff * xDiff) + (yDiff * yDiff));
        }

        private bool CanOk()
        {
            return CanvasChildren.Count == 2 && Length > 0;
        }

        public void MouseClicked(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image image = (System.Windows.Controls.Image)sender;
            
            Point point = e.GetPosition(image);

            double xRatio = point.X/image.ActualWidth;
            double yRatio = point.Y/image.ActualHeight;

            GenericPointViewModel currentPoint = CanvasChildren[CurrentPoint - 1];
            currentPoint.XRatio = xRatio;
            currentPoint.YRatio = yRatio;

            if (CurrentPoint == 1)
            {
                CurrentPoint++;
            }

            OkCommand.RaiseCanExecuteChangedNotification();
        }

        public double GetDistance()
        {
            GenericPointViewModel genericPoint1 = CanvasChildren[0];
            GenericPointViewModel genericPoint2 = CanvasChildren[1];

            PointF relativePoint1 = new PointF((float)genericPoint1.XRatio * Image.Width, (float)genericPoint1.YRatio * Image.Height);
            PointF relativePoint2 = new PointF((float)genericPoint2.XRatio * Image.Width, (float)genericPoint2.YRatio * Image.Height);

            double dist = relativePoint1.Distance(relativePoint2);

            return dist;
        }
    }

    public class GenericPointViewModel : ViewModelBase
    {
        private const float Size = 5;

        private double m_CanvasXPosition;
        private double m_CanvasYPosition;
        private double m_CanvasWidth;
        private double m_CanvasHeight;

        public IGenericPoint Model
        {
            get;
            set;
        }

        public int PointId
        {
            get
            {
                return Model.PointId;
            }
            set
            {
                if (Equals(PointId, value))
                {
                    return;
                }

                Model.PointId = value;

                NotifyPropertyChanged();
            }
        }

        public double XRatio
        {
            get
            {
                return Model.XRatio;
            }
            set
            {
                if (Equals(XRatio, value))
                {
                    return;
                }

                Model.XRatio = value;
                CanvasXPosition = CanvasWidth * XRatio;

                NotifyPropertyChanged();
                NotifyPropertyChanged("XCanvas");
                NotifyPropertyChanged("PointPlaced");
            }
        }

        public double YRatio
        {
            get
            {
                return Model.YRatio;
            }
            set
            {
                if (Equals(YRatio, value))
                {
                    return;
                }

                Model.YRatio = value;
                CanvasYPosition = CanvasHeight * YRatio;

                NotifyPropertyChanged();
                NotifyPropertyChanged("YCanvas");
                NotifyPropertyChanged("PointPlaced");
            }
        }

        public double CanvasXPosition
        {
            get
            {
                return m_CanvasXPosition;
            }
            private set
            {
                if (Equals(m_CanvasXPosition, value))
                {
                    return;
                }

                m_CanvasXPosition = value;

                NotifyPropertyChanged();
                NotifyPropertyChanged("XCanvas");
            }
        }

        public double CanvasYPosition
        {
            get
            {
                return m_CanvasYPosition;
            }
            private set
            {
                if (Equals(m_CanvasYPosition, value))
                {
                    return;
                }

                m_CanvasYPosition = value;

                NotifyPropertyChanged();
                NotifyPropertyChanged("YCanvas");
            }
        }

        public double XCanvas
        {
            get
            {
                return CanvasXPosition - (Width / 2);
            }
        }

        public double YCanvas
        {
            get
            {
                return CanvasYPosition - (Height / 2);
            }
        }

        public double CanvasWidth
        {
            get
            {
                return m_CanvasWidth;
            }
            set
            {
                m_CanvasWidth = value;
                CanvasXPosition = XRatio * m_CanvasWidth;
            }
        }

        public double CanvasHeight
        {
            get
            {
                return m_CanvasHeight;
            }
            set
            {
                m_CanvasHeight = value;
                CanvasYPosition = YRatio * m_CanvasHeight;
            }
        }

        public bool PointPlaced
        {
            get
            {
                return Model.XRatio != 0 && Model.YRatio != 0;
            }
        }

        public float Width
        {
            get
            {
                return Size;
            }
        }

        public float Height
        {
            get
            {
                return Size;
            }
        }

        public System.Windows.Media.Brush Color
        {
            get;
            set;
        }

        public GenericPointViewModel(IGenericPoint model, int color = 1)
        {
            Model = model;
            Color = new SolidColorBrush(GetColorFromDrawingColor(System.Drawing.Color.Red));
        }

        private System.Windows.Media.Color GetColorFromDrawingColor(System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromRgb(color.R, color.G, color.B);
        }

        public void UpdatePositions(double imageWidth, double imageHeight)
        {
            CanvasWidth = imageWidth;
            CanvasHeight = imageHeight;
        }
    }
}
