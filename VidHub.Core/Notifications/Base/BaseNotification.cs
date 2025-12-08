using CommunityToolkit.Mvvm.ComponentModel;

namespace VidHub.Core.Notifications.Base
{
    public class BaseNotification : ObservableObject
    {
        private string title = string.Empty;
        private string message = string.Empty;
        private NotificationSeverity severity;

        public string Title { get => title; set => SetProperty(ref title, value); }
        public string Message { get => message; set => SetProperty(ref message, value); }
        public NotificationSeverity Severity { get => severity; set => SetProperty(ref severity, value); }

        protected BaseNotification() { }
        public BaseNotification(string title, string message, NotificationSeverity severity)
        {
            Title = title;
            Message = message;
            Severity = severity;
        }
    }
}
