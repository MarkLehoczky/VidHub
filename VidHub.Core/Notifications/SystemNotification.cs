using Microsoft.Toolkit.Uwp.Notifications;
using VidHub.Core.Settings;
using Windows.UI.Notifications;

namespace VidHub.Core.Notifications
{
    public class SystemNotification : BaseNotification
    {
        public void Display()
        {
            if (!VidHubSettings.Instance.DisplayNotification(this))
            {
                return;
            }
            ToastContentBuilder content = new();
            _ = content.AddText(Title);
            _ = content.AddText(Details);
            _ = content.GetToastContent();
            ToastNotification toast = new(content.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
