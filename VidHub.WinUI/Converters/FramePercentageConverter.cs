using Microsoft.UI.Xaml.Data;
using System;
using Microsoft.Extensions.Logging;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.WinUI.Converters
{
    public partial class FramePercentageConverter : IValueConverter
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value is double number)
                {
                    string result = number switch
                    {
                        0 => $"First frame",
                        50 => $"Middle frame",
                        100 => $"Last frame",
                        _ => $"{(int)number}%",
                    };
                    logger.LogTrace("FramePercentageConverter: Converted {Number} to {Result}", number, result);
                    return result;
                }
                logger.LogTrace("FramePercentageConverter: Value is not double, returning as-is: {Value}", value);
                return value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "FramePercentageConverter: Error converting value {Value}", value);
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
