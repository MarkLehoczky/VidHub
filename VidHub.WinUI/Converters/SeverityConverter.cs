using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using VidHub.Core.Notifications;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.WinUI.Converters
{
    internal partial class SeverityConverter : IValueConverter
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value is NotificationSeverity severity)
                {
                    InfoBarSeverity result = severity switch
                    {
                        NotificationSeverity.SUCCESS => InfoBarSeverity.Success,
                        NotificationSeverity.INFORMATIONAL => InfoBarSeverity.Informational,
                        NotificationSeverity.WARNING => InfoBarSeverity.Warning,
                        NotificationSeverity.ERROR => InfoBarSeverity.Error,
                        _ => InfoBarSeverity.Informational,
                    };
                    logger.LogTrace("SeverityConverter: Converted NotificationSeverity {Severity} to InfoBarSeverity {Result}", severity, result);
                    return result;
                }
                logger.LogTrace("SeverityConverter: Value is not NotificationSeverity, returning default");
                return InfoBarSeverity.Informational;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "SeverityConverter: Error converting value {Value}", value);
                return InfoBarSeverity.Informational;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
