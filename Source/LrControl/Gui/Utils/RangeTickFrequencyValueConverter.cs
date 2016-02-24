using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using micdah.LrControlApi.Common;

namespace micdah.LrControl.Gui.Utils
{
    public class RangeTickFrequencyValueConverter : IValueConverter
    {
        public int NumberOfTicks { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var range = value as Range;
            if (range != null)
            {
                return (range.Maximum - range.Minimum)/NumberOfTicks;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}