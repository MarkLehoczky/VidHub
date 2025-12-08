namespace VidHub.Core.Notifications.Bar
{
    public interface IBarNotification
    {
        Func<bool> OpenCondition { get; set; }
        bool IsClosable { get; set; }
        bool DisplayNotification { get; }
    }
}
