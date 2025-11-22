using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VidHub.Core.Enums;

namespace VidHub.Core.Models
{
    public class Notification : ObservableObject
    {
        public bool IsOpen { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationSeverity Severity { get; set; }
        public bool Closable { get; set; }
        public NotificationButton Button { get; set; }
    }

    public class NotificationButton : ObservableObject
    {
        public string Text { get; set; }
        public string Tooltip { get; set; }
        public IRelayCommand Command { get; set; }
    }
}
