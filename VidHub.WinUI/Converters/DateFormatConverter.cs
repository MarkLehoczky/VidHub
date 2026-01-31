using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Data;
using System;
using VidHub.Core.Settings;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.WinUI.Converters
{
    internal partial class DateFormatConverter : IValueConverter
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                DateTime date = (DateTime)value;
                try
                {
                    string formatted = date.ToString(VidHubSettings.Instance.Dialogs.DisplayFormat.DateFormat);
                    logger.LogTrace("DateFormatConverter: Converted {Date} to {FormattedDate}", date, formatted);
                    return formatted;
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "DateFormatConverter: Failed to format with custom format, using default format for {Date}", date);
                    return date.ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DateFormatConverter: Error converting value {Value}", value);
                return "n/a";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
