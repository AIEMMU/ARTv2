using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System;

namespace ARWT.View.CropImage
{
    /// <summary>
    /// Interaction logic for CropImageView.xaml
    /// </summary>
    public partial class CropImageView : Window
    {
        private int ImagePixelWidth
        {
            get;
            set;
        }

        private int ImagePixelHeight
        {
            get;
            set;
        }

        private double StartX
        {
            get;
            set;
        }

        private double StartY
        {
            get;
            set;
        }

        private bool Dragging
        {
            get;
            set;
        }
        private bool mouseUp
        {
            get;
            set;
        }

        public CropImageView()
        {
            InitializeComponent();
            mouseUp = true;
        }

        private void Grid_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Dragging)
            {
                Rectangle.Width = 0;
                Rectangle.Height = 0;
                Dragging = false;
            }

            DrawRectFromPixelRect();
        }

        private void Image_OnTargetUpdated(object sender, DataTransferEventArgs e)
        {
            BitmapSource bitmapImage = Image.Source as BitmapSource;
            if (bitmapImage != null)
            {
                ImagePixelWidth = bitmapImage.PixelWidth;
                ImagePixelHeight = bitmapImage.PixelHeight;
            }
        }

        private void Image_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            if (checkPos(e.GetPosition(Image)))
            {
                
                Rectangle.Width = 0;
                Rectangle.Height = 0;
                Point mousePosition = e.GetPosition(Canvas);
                Point imagePoint = e.GetPosition(Image);
                Console.WriteLine(mousePosition + " " + imagePoint);
                StartX = mousePosition.X;
                StartY = mousePosition.Y;

                Rectangle.SetValue(Canvas.LeftProperty, StartX);
                Rectangle.SetValue(Canvas.TopProperty, StartY);
                Dragging = true;
            }
           // if (mouseUp)
           // {
           //     mouseUp = false;
          //      Console.WriteLine("meep");
          //  }
         //   else if (!mouseUp)
        //    {
        //        mouseUp = true;
        //        Console.WriteLine("moop");
        //    }
            
           // mouseUp = false;
        }

        private bool checkPos(Point point)
        {
           if (point.X <0 || point.X > Image.Width)
            {
                return false;
            }
           if (point.Y < 0 || point.Y > Image.Height)
            {
                return false;
            }
            return true;
        }

        private void Image_OnMouseMove(object sender, MouseEventArgs e)
        {
        //Console.WriteLine(Dragging+" "+mouseUp);
        if (!Dragging)
            {
                return;
            }

            Point mousePosition = e.GetPosition(Canvas);

            if (mousePosition.X < StartX)
            {
                Rectangle.SetValue(Canvas.LeftProperty, mousePosition.X);
                Rectangle.Width = StartX - mousePosition.X;
            }
            else
            {
                Rectangle.Width = mousePosition.X - StartX;
            }

            if (mousePosition.Y < StartY)
            {
                Rectangle.SetValue(Canvas.TopProperty, mousePosition.Y);
                Rectangle.Height = StartY - mousePosition.Y;
            }
            else
            {
                Rectangle.Height = mousePosition.Y - StartY;
            }

        }

        private void Image_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!checkPos(e.GetPosition(Image)))
            {
                return;
            }
            Dragging = false;
         
            UpdatePixelRect();
            
        }

        private void UpdatePixelRect()
        {
            Point imageStartPoint = Grid.TranslatePoint(new Point(StartX, StartY), Image);

            double pixelStartX = (imageStartPoint.X / Image.ActualWidth) * ImagePixelWidth;
            double pixelStartY = (imageStartPoint.Y / Image.ActualHeight) * ImagePixelHeight;
            double pixelWidth = (Rectangle.Width / Image.ActualWidth) * ImagePixelWidth;
            double pixelHeight = (Rectangle.Height / Image.ActualHeight) * ImagePixelHeight;

            PixelRectangle.SetValue(Canvas.LeftProperty, pixelStartX);
            PixelRectangle.SetValue(Canvas.TopProperty, pixelStartY);
            PixelRectangle.Width = pixelWidth;
            PixelRectangle.Height = pixelHeight;
        }

        private void DrawRectFromPixelRect()
        {
            if (PixelRectangle.Width == 0 || PixelRectangle.Height == 0)
            {
                Rectangle.Width = 0;
                Rectangle.Height = 0;
                return;
            }

            //Get Size in relation to current image size
            double? pixelX = PixelRectangle.GetValue(Canvas.LeftProperty) as double?;
            double? pixelY = PixelRectangle.GetValue(Canvas.TopProperty) as double?;

            if (!pixelX.HasValue || !pixelY.HasValue)
            {
                return;
            }

            Point imagePoint = Image.TranslatePoint(new Point(0, 0), Grid);

            double pixelWidth = PixelRectangle.Width;
            double pixelHeight = PixelRectangle.Height;

            double screenX = ((pixelX.Value / ImagePixelWidth) * Image.ActualWidth) + imagePoint.X;
            double screenY = ((pixelY.Value / ImagePixelHeight) * Image.ActualHeight) + imagePoint.Y;
            double screenWidth = (pixelWidth / ImagePixelWidth) * Image.ActualWidth;
            double screenHeight = (pixelHeight / ImagePixelHeight) * Image.ActualHeight;

            Rectangle.SetValue(Canvas.LeftProperty, screenX);
            Rectangle.SetValue(Canvas.TopProperty, screenY);
            Rectangle.Width = screenWidth;
            Rectangle.Height = screenHeight;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            PixelRectangle.SetValue(Canvas.LeftProperty, 0d);
            PixelRectangle.SetValue(Canvas.TopProperty, 0d);
            PixelRectangle.Width = ImagePixelWidth;
            PixelRectangle.Height = ImagePixelHeight;

            DrawRectFromPixelRect();
        }
    }
}
