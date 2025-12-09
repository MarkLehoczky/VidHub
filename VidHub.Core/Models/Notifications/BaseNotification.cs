using CommunityToolkit.Mvvm.ComponentModel;

namespace VidHub.Core.Models.Notifications
{
    public class BaseNotification : ObservableObject
    {
        public string Details { get; set; } = string.Empty;
        public NotificationSeverity Severity { get; set; } = NotificationSeverity.INFORMATIONAL;
        public string Title { get; set; } = string.Empty;
    }
}
