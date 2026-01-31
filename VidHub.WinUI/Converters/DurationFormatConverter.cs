using Microsoft.UI.Xaml.Data;
using System;
using VidHub.Core.Settings;
using Microsoft.Extensions.Logging;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.WinUI.Converters
{
    internal partial class DurationFormatConverter : IValueConverter
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                TimeSpan duration = (TimeSpan)value;
                try
                {
                    string formatted = duration.TotalDays >= 1
                        ? duration.ToString(VidHubSettings.Instance.Dialogs.DisplayFormat.DurationDayFormat)
                        : duration.TotalHours >= 1
                        ? duration.ToString(VidHubSettings.Instance.Dialogs.DisplayFormat.DurationHourFormat)
                        : duration.TotalMinutes >= 1
                        ? duration.ToString(VidHubSettings.Instance.Dialogs.DisplayFormat.DurationMinuteFormat)
                        : duration >= TimeSpan.Zero
                        ? duration.ToString(VidHubSettings.Instance.Dialogs.DisplayFormat.DurationSecondFormat)
                        : "n/a";
                    logger.LogTrace("DurationFormatConverter: Converted {Duration} to {FormattedDuration}", duration, formatted);
                    return formatted;
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "DurationFormatConverter: Failed to format with custom format, using default format for {Duration}", duration);
                    return duration.TotalDays >= 1
                        ? duration.ToString("d' day(s) 'hh':'mm':'ss")
                        : duration.TotalHours >= 1
                        ? duration.ToString("hh':'mm':'ss")
                        : duration.TotalMinutes >= 1
                        ? duration.ToString("mm':'ss")
                        : duration >= TimeSpan.Zero ? duration.ToString("ss' second(s)'") : "n/a";
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DurationFormatConverter: Error converting value {Value}", value);
                return "n/a";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
