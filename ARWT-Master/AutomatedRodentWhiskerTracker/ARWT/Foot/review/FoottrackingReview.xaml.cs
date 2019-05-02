using ARWT.Foot.review.viewmodel;
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

namespace ARWT.Foot.review
{
    /// <summary>
    /// Interaction logic for FoottrackingReview.xaml
    /// </summary>
    public partial class FoottrackingReview : Window
    {
        private object _foottrackingviewModel;

        public FoottrackingReview(FootReviewViewModel footReviewViewModel)
        {
            InitializeComponent();
            _foottrackingviewModel = footReviewViewModel;
            DataContext = _foottrackingviewModel;
        }
    }
}
