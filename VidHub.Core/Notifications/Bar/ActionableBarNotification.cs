using VidHub.Core.Notifications.Base;
using VidHub.Platform;

namespace VidHub.Core.Notifications.Bar
{
    public class ActionableBarNotification : BarNotification, IActionableNotification
    {
        private NotificationButton button = null;

        public NotificationButton Button { get => button; set => SetProperty(ref button, value); }


        protected ActionableBarNotification() { }
        protected ActionableBarNotification(string title, string message, NotificationSeverity severity) : base(title, message, severity) { }
        protected ActionableBarNotification(string title, string message, NotificationSeverity severity, bool isClosable) : base(title, message, severity, isClosable) { }
        protected ActionableBarNotification(string title, string message, NotificationSeverity severity, bool isClosable, Func<bool> openCondition) : base(title, message, severity, isClosable, openCondition) { }
        public ActionableBarNotification(string title, string message, NotificationSeverity severity, bool isClosable, NotificationButton button) : this(title, message, severity, isClosable)
        {
            Button = button;
        }
        public ActionableBarNotification(string title, string message, NotificationSeverity severity, bool isClosable, Func<bool> openCondition, NotificationButton button) : this(title, message, severity, isClosable, openCondition)
        {
            Button = button;
        }
    }
}
