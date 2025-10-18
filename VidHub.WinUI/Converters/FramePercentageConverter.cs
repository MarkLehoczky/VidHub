using Microsoft.UI.Xaml.Data;
using System;

namespace VidHub.WinUI.Converters
{
    public partial class FramePercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is double number
                ? number switch
                {
                    0 => $"First frame",
                    50 => $"Middle frame",
                    100 => $"Last frame",
                    _ => $"{(int)number}%",
                }
                : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
