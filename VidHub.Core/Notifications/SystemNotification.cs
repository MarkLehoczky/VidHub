using Microsoft.Toolkit.Uwp.Notifications;
using VidHub.Core.Settings;
using Windows.UI.Notifications;
using Microsoft.Extensions.Logging;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.Core.Notifications
{
    public class SystemNotification : BaseNotification
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public void Display()
        {
            logger.LogTrace("SystemNotification Display called for notification: {Title}", Title);
            if (!VidHubSettings.Instance.DisplayNotification(this))
            {
                logger.LogDebug("SystemNotification skipped due to user settings: {Title}", Title);
                return;
            }
            try
            {
                ToastContent content = new ToastContentBuilder().AddText(Title).AddText(Details).GetToastContent();
                ToastNotification toast = new(content.GetXml());
                ToastNotificationManager.CreateToastNotifier().Show(toast);
                logger.LogInformation("SystemNotification displayed: {Title}", Title);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error displaying SystemNotification: {Title}", Title);
            }
        }
    }
}
