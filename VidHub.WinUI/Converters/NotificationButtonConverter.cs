using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using VidHub.Core.Notifications.Base;

namespace VidHub.WinUI.Converters
{
    public partial class NotificationButtonConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is ActionNotificationButton actionButton && actionButton != null)
            {
                Button actualButton = new()
                {
                    Content = actionButton.Text,
                    Command = actionButton.Command
                };
                ToolTipService.SetToolTip(actualButton, actionButton.Description);
                return actualButton;
            }
            else if (value is LinkNotificationButton linkButton && linkButton != null)
            {
                HyperlinkButton actualButton = new()
                {
                    Content = linkButton.Text,
                    NavigateUri = new Uri(linkButton.Link)
                };
                ToolTipService.SetToolTip(actualButton, linkButton.Description);
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
