using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using VidHub.Core.Notifications;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.WinUI.Converters
{
    public partial class NotificationButtonConverter : IValueConverter
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value is CustomActionNotificationButton actionButton && actionButton != null)
                {
                    logger.LogTrace("NotificationButtonConverter: Converting CustomActionNotificationButton with label {Label}", actionButton.Label);
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
                    logger.LogTrace("NotificationButtonConverter: Converting HyperlinkNotificationButton with label {Label}", linkButton.Label);
                    HyperlinkButton actualButton = new()
                    {
                        Content = linkButton.Label,
                        NavigateUri = linkButton.Hyperlink
                    };
                    ToolTipService.SetToolTip(actualButton, linkButton.Details);
                    return actualButton;
                }

                logger.LogTrace("NotificationButtonConverter: Value is not a recognized notification button type");
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "NotificationButtonConverter: Error converting value {Value}", value);
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
