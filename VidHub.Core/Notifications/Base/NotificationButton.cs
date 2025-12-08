using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace VidHub.Core.Notifications.Base
{
    public class NotificationButton : ObservableObject
    {
        private Action action = () => { };
        private string text = string.Empty;
        private string description = string.Empty;

        public Action Action { get => action; set => SetProperty(ref action, value); }
        public string Text { get => text; set => SetProperty(ref text, value); }
        public string Description { get => description; set => SetProperty(ref description, value); }

        public IRelayCommand Command => new RelayCommand(() =>
        {
            Action?.Invoke();
        });

        protected NotificationButton() { }
        public NotificationButton(string text, Action action)
        {
            Text = text;
            Action = action;
        }
        public NotificationButton(string text, Action action, string description)
        {
            Text = text;
            Action = action;
            Description = description;
        }
    }
}
