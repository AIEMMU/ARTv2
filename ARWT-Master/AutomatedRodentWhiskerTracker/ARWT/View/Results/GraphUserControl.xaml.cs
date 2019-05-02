using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ARWT.Services;

namespace ARWT.View.Results
{
    /// <summary>
    /// Interaction logic for GraphUserControl.xaml
    /// </summary>
    public partial class GraphUserControl : Window
    {
        public GraphUserControl()
        {
            InitializeComponent();
        }

        private void ExportButton_OnClick(object sender, RoutedEventArgs e)
        {
            string saveLocation = FileBrowser.SaveFile("png|*.png");

            if (string.IsNullOrWhiteSpace(saveLocation))
            {
                return;
            }

            RenderTargetBitmap renderBitmapRight = new RenderTargetBitmap((int)RenderGrid.ActualWidth, (int)RenderGrid.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            renderBitmapRight.Render(RenderGrid);

            //string rightLocation = saveLocation.Replace(".png", "-Right.png");

            using (FileStream outStream = new FileStream(saveLocation, FileMode.Create))
            {
                // Use png encoder for our data
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                // push the rendered bitmap to it
                encoder.Frames.Add(BitmapFrame.Create(renderBitmapRight));
                // save the data to the stream
                encoder.Save(outStream);
            }

            //RenderTargetBitmap renderBitmapRight = new RenderTargetBitmap((int)RightChart.ActualWidth, (int)RightChart.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            //renderBitmapRight.Render(RightChart);

            //string rightLocation = saveLocation.Replace(".png", "-Right.png");

            //using (FileStream outStream = new FileStream(rightLocation, FileMode.Create))
            //{
            //    // Use png encoder for our data
            //    PngBitmapEncoder encoder = new PngBitmapEncoder();
            //    // push the rendered bitmap to it
            //    encoder.Frames.Add(BitmapFrame.Create(renderBitmapRight));
            //    // save the data to the stream
            //    encoder.Save(outStream);
            //}

            //RenderTargetBitmap renderBitmapLeft = new RenderTargetBitmap((int) LeftChart.ActualWidth, (int)LeftChart.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            //renderBitmapLeft.Render(LeftChart);

            //string leftLocation = saveLocation.Replace(".png", "-Left.png");

            //// Create a file stream for saving image
            //using (FileStream outStream = new FileStream(leftLocation, FileMode.Create))
            //{
            //    // Use png encoder for our data
            //    PngBitmapEncoder encoder = new PngBitmapEncoder();
            //    // push the rendered bitmap to it
            //    encoder.Frames.Add(BitmapFrame.Create(renderBitmapLeft));
            //    // save the data to the stream
            //    encoder.Save(outStream);
            //}
        }
    }
}
