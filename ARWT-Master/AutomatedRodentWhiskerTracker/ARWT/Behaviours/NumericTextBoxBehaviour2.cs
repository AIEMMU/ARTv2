using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace ARWT.Behaviours
{
    public class NumericTextBoxBehaviour2 : Behavior<TextBox>
    {
        public static readonly DependencyProperty NumericOnlyProperty = DependencyProperty.RegisterAttached("NumericOnly", typeof(bool), typeof(NumericTextBoxBehaviour2), new FrameworkPropertyMetadata(false));

        public bool NumericOnly
        {
            get
            {
                return (bool)base.GetValue(NumericOnlyProperty);
            }
            set
            {
                base.SetValue(NumericOnlyProperty, value);
            }
        }

        public static readonly DependencyProperty MaxNumberProperty = DependencyProperty.Register("MaxNumber", typeof(int), typeof(NumericTextBoxBehaviour2), new FrameworkPropertyMetadata(int.MaxValue));

        public int MaxNumber
        {
            get
            {
                return (int)base.GetValue(MaxNumberProperty);
            }
            set
            {
                base.SetValue(MaxNumberProperty, value);
            }
        }

        public static readonly DependencyProperty MinNumberProperty = DependencyProperty.Register("MinNumber", typeof(int), typeof(NumericTextBoxBehaviour2), new FrameworkPropertyMetadata(int.MinValue));

        public int MinNumber
        {
            get
            {
                return (int)base.GetValue(MinNumberProperty);
            }
            set
            {
                base.SetValue(MinNumberProperty, value);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += OnPreviewTextInput;
            DataObject.AddPastingHandler(AssociatedObject, OnPaste);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string text = Convert.ToString(e.DataObject.GetData(DataFormats.Text));

                if (!IsValid(text, true))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        void OnPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(e.Text, false);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= OnPreviewTextInput;
            DataObject.RemovePastingHandler(AssociatedObject, OnPaste);
        }

        private bool IsValid(string newText, bool paste)
        {
            int result;
            if (int.TryParse((newText), out result))
            {
                return result >= MinNumber && result <= MaxNumber;
            }

            return false;
        }
    }
}
