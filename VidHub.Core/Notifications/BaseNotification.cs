using VidHub.Core.Utilities.Internal;

namespace VidHub.Core.Notifications
{
    public class BaseNotification : FocusableObject
    {
        public string Details { get; set; } = string.Empty;
        public NotificationSeverity Severity { get; set; } = NotificationSeverity.INFORMATIONAL;
        public string Title { get; set; } = string.Empty;
    }
}
