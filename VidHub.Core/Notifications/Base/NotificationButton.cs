using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace VidHub.Core.Notifications.Base
{
    public enum ButtonType
    {
        BUTTON,
        HYPERLINK
    }


    public class NotificationButton : ObservableObject
    {
        private string text = string.Empty;
        private string description = string.Empty;

        public string Text { get => text; set => SetProperty(ref text, value); }
        public string Description { get => description; set => SetProperty(ref description, value); }

        protected NotificationButton() { }
        public NotificationButton(string text)
        {
            Text = text;
        }
        public NotificationButton(string text, string description)
        {
            Text = text;
            Description = description;
        }
    }


    public class ActionNotificationButton : NotificationButton
    {
        private Action action = () => { };

        public Action Action { get => action; set => SetProperty(ref action, value); }

        public IRelayCommand Command => new RelayCommand(() =>
        {
            Action?.Invoke();
        });

        protected ActionNotificationButton() { }
        public ActionNotificationButton(string text, Action action)
        {
            Text = text;
            Action = action;
        }
        public ActionNotificationButton(string text, Action action, string description)
        {
            Text = text;
            Action = action;
            Description = description;
        }
    }


    public class LinkNotificationButton : NotificationButton
    {
        private string link = string.Empty;

        public string Link { get => link; set => SetProperty(ref link, value); }

        protected LinkNotificationButton() { }
        public LinkNotificationButton(string text, string link)
        {
            Text = text;
            Link = link;
        }
        public LinkNotificationButton(string text, string link, string description)
        {
            Text = text;
            Link = link;
            Description = description;
        }
    }
}
