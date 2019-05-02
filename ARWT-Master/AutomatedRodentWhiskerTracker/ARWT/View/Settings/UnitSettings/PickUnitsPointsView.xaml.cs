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
using ARWT.ViewModel.Settings;

namespace RobynsWhiskerTracker.View.Settings.UnitSettings
{
    /// <summary>
    /// Interaction logic for PickUnitsPointsView.xaml
    /// </summary>
    public partial class PickUnitsPointsView : Window
    {
        public PickUnitsPointsView()
        {
            InitializeComponent();
        }

        private void Image_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            IMouseClickedViewModel viewModel = DataContext as IMouseClickedViewModel;

            if (viewModel != null)
            {
                viewModel.MouseClicked(sender, e);
            }
        }
    }
}
