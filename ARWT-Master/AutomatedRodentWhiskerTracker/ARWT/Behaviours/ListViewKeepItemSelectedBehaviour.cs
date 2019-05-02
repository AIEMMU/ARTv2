using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ARWT.Behaviours
{
    public static class ListViewKeepItemSelectedBehaviour
    {
        public static readonly DependencyProperty DisableDeselectProperty =
            DependencyProperty.RegisterAttached("DisableDeselect",
            typeof(bool), typeof(ListViewKeepItemSelectedBehaviour),
            new UIPropertyMetadata(false, OnDisableDeselectChanged));

        public static bool GetDisableDeselect(DependencyObject source)
        {
            return (bool)source.GetValue(DisableDeselectProperty);
        }

        public static void SetDisableDeselect(DependencyObject source, bool value)
        {
            source.SetValue(DisableDeselectProperty, value);
        }

        private static void OnDisableDeselectChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ListView listView = sender as ListView;

            if (listView == null)
            {
                return;
            }

            bool disable = (e.NewValue is bool) && (bool)e.NewValue;

            if (disable)
            {
                listView.SelectionChanged += ListVIewOnSelectionChanged;
            }
            else
            {
                listView.SelectionChanged -= ListVIewOnSelectionChanged;
            }
        }

        private static void ListVIewOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = (ListView)sender;

            if (listView.SelectedItem == null)
            {
                if (e.RemovedItems.Count > 0)
                {
                    object itemToReselect = e.RemovedItems[0];
                    if (listView.Items.Contains(itemToReselect))
                    {
                        listView.SelectedItem = itemToReselect;
                    }
                }
            }
        }
    }
}
