using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace ARWT.Behaviours
{
    public class SliderValueChangedBehavior : Behavior<Slider>
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(double),
            typeof(SliderValueChangedBehavior),
            new PropertyMetadata(default(double), OnValuePropertyChanged));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(SliderValueChangedBehavior),
            new PropertyMetadata(null));

        private int KeysDown
        {
            get;
            set;
        }

        private bool ApplyKeyUpValue
        {
            get;
            set;
        }

        public double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        protected override void OnAttached()
        {
            AssociatedObject.KeyUp += OnKeyUp;
            AssociatedObject.KeyDown += OnKeyDown;
            AssociatedObject.ValueChanged += OnValueChanged;

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.KeyUp -= OnKeyUp;
            AssociatedObject.KeyDown -= OnKeyDown;
            AssociatedObject.ValueChanged -= OnValueChanged;
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = (SliderValueChangedBehavior)d;
            if (me.AssociatedObject != null)
            {
                me.Value = (double)e.NewValue;
            }
        }

        private void OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Mouse.Captured != null)
            {
                AssociatedObject.LostMouseCapture += OnLostMouseCapture;
            }
            else if (KeysDown != 0)
            {
                ApplyKeyUpValue = true;
            }
            else
            {
                ApplyValue();
            }
        }

        private void OnLostMouseCapture(object sender, MouseEventArgs e)
        {
            AssociatedObject.LostMouseCapture -= OnLostMouseCapture;
            ApplyValue();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (KeysDown-- != 0)
            {
                ApplyValue();
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            KeysDown++;
        }

        private void ApplyValue()
        {
            Value = AssociatedObject.Value;

            if (Command != null)
            {
                Command.Execute(Value);
            }
        }
    }
}