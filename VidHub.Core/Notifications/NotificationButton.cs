using CommunityToolkit.Mvvm.Input;
using VidHub.Core.Utilities.Internal;

namespace VidHub.Core.Notifications
{
    public class NotificationButton : FocusableObject
    {
        public string Details { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }


    public class CustomActionNotificationButton : NotificationButton
    {
        public Action CustomAction { get; set; } = () => { };
        public IRelayCommand Command => new RelayCommand(CustomAction);
    }


    public class HyperlinkNotificationButton : NotificationButton
    {
        public Uri Hyperlink { get; set; } = new Uri("about:blank");
    }
}
