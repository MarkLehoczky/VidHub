using Microsoft.UI.Xaml.Data;
using System;

namespace VidHub.WinUI.Converters
{
    internal partial class DurationFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is TimeSpan duration
                ? duration.TotalHours >= 1
                    ? duration.ToString(@"h\:mm\:ss")
                    : duration > TimeSpan.Zero
                        ? duration.ToString(@"m\:ss")
                        : "n/a"
                : "n/a";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
