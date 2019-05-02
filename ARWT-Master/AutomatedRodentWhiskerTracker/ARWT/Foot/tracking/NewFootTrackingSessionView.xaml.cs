using ARWT.Foot.tracking.ViewModel;
using ARWT.ModelInterface.RBSK2;
using System.Windows;

namespace ARWT.Foot.tracking
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class NewFootTrackingSessionView : Window
    {
        private FootTrackingViewModel _foottrackingviewModel;


        public NewFootTrackingSessionView(FootTrackingViewModel foottrackingviewModel)
        {
            InitializeComponent();
            _foottrackingviewModel = foottrackingviewModel;
            DataContext = _foottrackingviewModel;
            Loaded += NewFootTrackingSessionView_Loaded;
        }

       

        private void NewFootTrackingSessionView_Loaded(object sender, RoutedEventArgs e)
        {
            _foottrackingviewModel.loadSettings();
        }

    }
}
