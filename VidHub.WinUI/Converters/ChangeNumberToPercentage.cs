using Microsoft.UI.Xaml.Data;
using System;

namespace VidHub.WinUI.Converters
{
    public partial class ChangeNumberToPercentage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double number)
            {
                return number switch
                {
                    0 => $"First frame",
                    50 => $"Middle frame",
                    100 => $"Last frame",
                    _ => $"{(int)number}%",
                };
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
