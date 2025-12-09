namespace VidHub.Core.Models.Notifications
{
    public class BarNotification : BaseNotification
    {
        public NotificationButton? Button { get; set; } = null;
        public bool Display { get; set; } = false;
        public Func<bool> DisplayCondition { get; set; } = () => true;
        public bool IsClosable { get; set; } = true;
    }
}
