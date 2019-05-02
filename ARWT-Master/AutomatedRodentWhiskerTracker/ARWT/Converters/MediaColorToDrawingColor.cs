using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using DColor = System.Drawing.Color;
using MColor = System.Windows.Media.Color;

namespace ARWT.Converters
{
    public class MediaColorToDrawingColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (!(value is DColor))
            {
                return null;
            }

            DColor originalColor = (DColor)value;

            return MColor.FromArgb(originalColor.A, originalColor.R, originalColor.G, originalColor.B);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (!(value is MColor))
            {
                return null;
            }

            MColor originalColor = (MColor)value;

            return DColor.FromArgb(originalColor.A, originalColor.R, originalColor.G, originalColor.B);
        }
    }
}
