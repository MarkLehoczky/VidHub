using Microsoft.UI.Xaml.Data;
using System;

namespace VidHub.WinUI.Converters
{
    internal partial class DateFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is DateTime date
                ? date.ToString("yyyy. MMMM dd.")
                : "n/a";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
