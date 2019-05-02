using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using ARWT.Services;
using Emgu.CV;

namespace ARWT.View.Image
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ImageShower : Window
    {
        public ImageShower()
        {
            InitializeComponent();
        }

        public static void ShowImage(IImage image)
        {
            ImageShower view = new ImageShower();
            view.ImageDisplay.Source = ImageService.ToBitmapSource(image);
            view.ShowDialog();
        }
    }
}
