using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using LrControl.Api.Common;

namespace LrControl.Ui.Gui.Utils
{
    public class EncoderAngleValueConverter : IMultiValueConverter
    {
        public Range AngleRange { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2 || !(values[0] is Range) || !(values[1] is int))
                return DependencyProperty.UnsetValue;

            var range = (Range) values[0];
            var value = (int) values[1];

            return AngleRange.FromRange(range, System.Convert.ToDouble(value));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}