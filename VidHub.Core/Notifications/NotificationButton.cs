using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace VidHub.Core.Notifications
{
    public class NotificationButton : ObservableObject
    {
        public string Details { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }


    public class CustomActionNotificationButton : NotificationButton
    {
        public Action CustomAction { get; set; } = () => { };
        public IRelayCommand Command => new RelayCommand(CustomAction.Invoke);
    }


    public class HyperlinkNotificationButton : NotificationButton
    {
        public Uri Hyperlink { get; set; } = new Uri("about:blank");
    }
}
