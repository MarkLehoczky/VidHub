using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using VidHub.Core.Models.Notifications;

namespace VidHub.WinUI.Converters
{
    public partial class NotificationButtonConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is CustomActionNotificationButton actionButton && actionButton != null)
            {
                Button actualButton = new()
                {
                    Content = actionButton.Label,
                    Command = actionButton.Command
                };
                ToolTipService.SetToolTip(actualButton, actionButton.Details);
                return actualButton;
            }
            else if (value is HyperlinkNotificationButton linkButton && linkButton != null)
            {
                HyperlinkButton actualButton = new()
                {
                    Content = linkButton.Label,
                    NavigateUri = linkButton.Hyperlink
                };
                ToolTipService.SetToolTip(actualButton, linkButton.Details);
                return actualButton;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
