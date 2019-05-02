using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ARWT.Behaviours
{
    public class NumericTextBoxBehaviour
    {
        private static int MinNumber = int.MinValue;
        private static int MaxNumber = int.MaxValue;
        private static int LastGoodValue = 1;
        private static double MinDouble = double.MinValue;
        private static double MaxDouble = double.MaxValue;

        public static readonly DependencyProperty NumericOnlyProperty =
            DependencyProperty.RegisterAttached("NumericOnly",
            typeof(bool), typeof(NumericTextBoxBehaviour),
            new UIPropertyMetadata(false, OnNumericOnlyChanged));

        public static bool GetNumericOnly(DependencyObject source)
        {
            return (bool)source.GetValue(NumericOnlyProperty);
        }

        public static void SetNumericOnly(DependencyObject source, bool value)
        {
            source.SetValue(NumericOnlyProperty, value);
        }

        private static void OnNumericOnlyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox == null)
            {
                throw new Exception("Textbox behvaiour can only be used on a textbox");
            }

            bool isNumericOnly = (e.NewValue is bool) && (bool)e.NewValue;
            if (isNumericOnly)
            {
                textBox.PreviewTextInput += TextBoxOnPreviewTextInput;
                textBox.TextChanged += TextBoxOnTextChanged;
                textBox.PreviewKeyDown += TextBoxOnPreviewKeyDown;
            }
            else
            {
                textBox.PreviewTextInput -= TextBoxOnPreviewTextInput;
                textBox.TextChanged += TextBoxOnTextChanged;
                textBox.PreviewKeyDown -= TextBoxOnPreviewKeyDown;
            }
        }

        private static void TextBoxOnPreviewKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            Key key = keyEventArgs.Key;
            if (!(key == Key.Delete || key == Key.Back))
            {
                return;
            }

            TextBox textBox = sender as TextBox;

            if (textBox == null)
            {
                throw new Exception("Numeric text box behaviour must be used on a textbox");
            }

            if (textBox.Text.Length == 1)
            {
                textBox.Text = MinNumber.ToString();
                textBox.CaretIndex = 1;
                keyEventArgs.Handled = true;
            }
        }

        private static void TextBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox == null)
            {
                throw new Exception("Numeric text box behaviour must be used on a textbox");
            }

            int result;
            if (int.TryParse((textBox.Text), out result))
            {
                if (result < MinNumber)
                {
                    textBox.Text = MinNumber.ToString();
                    LastGoodValue = MinNumber;
                    textBox.CaretIndex = 1;
                    e.Handled = true;
                    return;
                }

                if (result > MaxNumber)
                {
                    textBox.Text = MaxNumber.ToString();
                    LastGoodValue = MaxNumber;
                    textBox.CaretIndex = MaxNumber.ToString().Length;
                    e.Handled = true;
                    return;
                }

                LastGoodValue = result;
            }
            else
            {
                textBox.Text = LastGoodValue.ToString();
                e.Handled = true;
            }
        }

        private static void TextBoxOnPreviewTextInput(object sender, TextCompositionEventArgs textCompositionEventArgs)
        {
            TextBox textBox = sender as TextBox;

            if (textBox == null)
            {
                throw new Exception("Numeric text box behaviour must be used on a textbox");
            }

            //Check for illegal characters
            int result;
            if (!int.TryParse((textBox.Text + textCompositionEventArgs.Text), out result))
            {
                textBox.Text = LastGoodValue.ToString();
                textCompositionEventArgs.Handled = true;
            }
        }

        public static readonly DependencyProperty MinNumberProperty =
            DependencyProperty.RegisterAttached("MinNumber",
            typeof(int), typeof(NumericTextBoxBehaviour),
            new UIPropertyMetadata(MinNumber, OnMinNumberChanged));

        private static void OnMinNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MinNumber = (int)e.NewValue;
        }

        public static int GetMinNumber(DependencyObject source)
        {
            return (int)source.GetValue(MinNumberProperty);
        }

        public static void SetMinNumber(DependencyObject source, int value)
        {
            source.SetValue(MinNumberProperty, value);
        }

        public static readonly DependencyProperty MaxNumberProperty =
            DependencyProperty.RegisterAttached("MaxNumber",
            typeof(int), typeof(NumericTextBoxBehaviour),
            new UIPropertyMetadata(MaxNumber, OnMaxNumberChanged));

        private static void OnMaxNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MaxNumber = (int)e.NewValue;
        }

        public static int GetMaxNumber(DependencyObject source)
        {
            return (int)source.GetValue(MaxNumberProperty);
        }

        public static void SetMaxNumber(DependencyObject source, int value)
        {
            source.SetValue(MaxNumberProperty, value);
        }

        public static readonly DependencyProperty DoubleOnlyProperty =
            DependencyProperty.RegisterAttached("DoubleOnly",
            typeof(bool), typeof(NumericTextBoxBehaviour),
            new UIPropertyMetadata(false, OnDoubleOnlyChanged));

        public static bool GetDoubleOnly(DependencyObject source)
        {
            return (bool)source.GetValue(NumericOnlyProperty);
        }

        public static void SetDoubleOnly(DependencyObject source, bool value)
        {
            source.SetValue(NumericOnlyProperty, value);
        }

        private static void OnDoubleOnlyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox == null)
            {
                throw new Exception("Textbox behvaiour can only be used on a textbox");
            }

            bool isDoubleOnly = (e.NewValue is bool) && (bool)e.NewValue;
            if (isDoubleOnly)
            {
                textBox.PreviewTextInput += TextBoxOnPreviewTextInputForDouble;
            }
            else
            {
                textBox.PreviewTextInput -= TextBoxOnPreviewTextInputForDouble;
            }
        }

        private static void TextBoxOnPreviewTextInputForDouble(object sender, TextCompositionEventArgs textCompositionEventArgs)
        {
            TextBox textBox = sender as TextBox;

            if (textBox == null)
            {
                throw new Exception("Double only text box behaviour must be used on a textbox");
            }

            double result;
            if (double.TryParse((textBox.Text + textCompositionEventArgs.Text), out result))
            {
                if (result < MinDouble)
                {
                    textBox.Text = MinNumber.ToString();
                    textCompositionEventArgs.Handled = true;
                }

                if (result > MaxDouble)
                {
                    textBox.Text = MaxNumber.ToString();
                    textCompositionEventArgs.Handled = true;
                }
            }
        }
    }
}
