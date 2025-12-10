using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using VidHub.Core.Notifications;

namespace VidHub.WinUI.Converters
{
    internal class SeverityTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is NotificationSeverity severity
                ? severity switch
                {
                    NotificationSeverity.SUCCESS => InfoBarSeverity.Success,
                    NotificationSeverity.INFORMATIONAL => InfoBarSeverity.Informational,
                    NotificationSeverity.WARNING => InfoBarSeverity.Warning,
                    NotificationSeverity.ERROR => InfoBarSeverity.Error,
                    _ => InfoBarSeverity.Informational,
                }
                : InfoBarSeverity.Informational;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
