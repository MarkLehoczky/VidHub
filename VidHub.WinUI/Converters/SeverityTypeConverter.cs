using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using VidHub.Core.Models;

namespace VidHub.WinUI.Converters
{
    internal class SeverityTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is NotificationSeverity severity)
            {
                return severity switch                 {
                    NotificationSeverity.Success => InfoBarSeverity.Success,
                    NotificationSeverity.Informational => InfoBarSeverity.Informational,
                    NotificationSeverity.Warning => InfoBarSeverity.Warning,
                    NotificationSeverity.Error => InfoBarSeverity.Error,
                    _ => InfoBarSeverity.Informational,
                };
            }

            return InfoBarSeverity.Informational;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
