using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace VidHub.Core.Models
{
    public class Notification : ObservableObject
    {
        public virtual bool IsOpen => OpenCondition?.Invoke() ?? false;
        public Func<bool> OpenCondition { get; set; } = () => false;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationSeverity Severity { get; set; }
        public bool IsClosable { get; set; }

        private NotificationButton button = new();
        public NotificationButton Button
        {
            get => button;
            set
            {
                if (button == value)
                {
                    return;
                }

                if (button is not null)
                {
                    button.PropertyChanged -= Button_PropertyChanged;
                    button = value;
                    button.PropertyChanged += Button_PropertyChanged;
                }

                OnPropertyChanged(nameof(Button));
                OnPropertyChanged(nameof(IsOpen));
            }
        }

        private void Button_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NotificationButton.CommandCompleted))
            {
                OnPropertyChanged(nameof(IsOpen));
            }
        }
    }

    public class SingleInteractionNotification : Notification
    {
        public override bool IsOpen => base.IsOpen && !(Button?.CommandCompleted ?? false);
    }

    public class NotificationButton : ObservableObject
    {
        public string Content { get; set; } = string.Empty;
        public string Tooltip { get; set; } = string.Empty;

        public virtual IRelayCommand Command => new RelayCommand(() =>
        {
            Action?.Invoke();
            CommandCompleted = true;
        });

        public Action Action { get; set; } = () => { };

        private bool commandCompleted;
        public bool CommandCompleted
        {
            get => commandCompleted;
            set => SetProperty(ref commandCompleted, value);
        }
    }

    public class AsyncNotificationButton : NotificationButton
    {
        public override IRelayCommand Command => new AsyncRelayCommand(async () =>
        {
            await Task.Run(Action);
            CommandCompleted = true;
        });
    }

    public enum NotificationSeverity
    {
        Informational,
        Success,
        Warning,
        Error
    }
}