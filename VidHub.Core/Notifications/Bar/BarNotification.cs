using VidHub.Core.Notifications.Base;

namespace VidHub.Core.Notifications.Bar
{
    public class BarNotification : BaseNotification, IBarNotification
    {
        private Func<bool> openCondition = () => false;
        private bool displayNotification = false;
        private bool isClosable = true;
        private NotificationButton? button = null;

        public Func<bool> OpenCondition { get => openCondition; set => SetProperty(ref openCondition, value); }
        public bool DisplayNotification { get => displayNotification; set => SetProperty(ref displayNotification, value); }
        public bool IsClosable { get => isClosable; set => SetProperty(ref isClosable, value); }
        public NotificationButton? Button { get => button; set => SetProperty(ref button, value); }


        protected BarNotification() { }
        protected BarNotification(string title, string message, NotificationSeverity severity) : base(title, message, severity) { }
        public BarNotification(string title, string message, NotificationSeverity severity, bool isClosable) : this(title, message, severity)
        {
            IsClosable = isClosable;
        }
        public BarNotification(string title, string message, NotificationSeverity severity, bool isClosable, Func<bool> openCondition) : this(title, message, severity, isClosable)
        {
            OpenCondition = openCondition;
        }
        public BarNotification(string title, string message, NotificationSeverity severity, bool isClosable, NotificationButton button) : this(title, message, severity, isClosable)
        {
            Button = button;
        }
        public BarNotification(string title, string message, NotificationSeverity severity, bool isClosable, Func<bool> openCondition, NotificationButton button) : this(title, message, severity, isClosable, openCondition)
        {
            Button = button;
        }
    }
}
