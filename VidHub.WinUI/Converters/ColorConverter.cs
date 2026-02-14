using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.UI;

namespace VidHub.WinUI.Converters
{
    internal class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is Color color ? new SolidColorBrush(color) : new SolidColorBrush(Colors.DimGray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
