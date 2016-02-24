using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using micdah.LrControlApi.Common;

namespace micdah.LrControl.Gui.Utils
{
    public class ButtonFillValueConverter : IMultiValueConverter
    {
        public Brush OffBrush { get; set; }
        public Brush OnBrush { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2 || !(values[0] is Range) || !(values[1] is int))
                return DependencyProperty.UnsetValue;

            var range = (Range) values[0];
            var value = (int) values[1];

            return value == (int) range.Maximum
                ? OnBrush
                : OffBrush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}