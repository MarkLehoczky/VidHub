namespace VidHub.Core.Settings
{
    public class NotificationSettings
    {
        public bool DisplayInformationalSystemNotification { get; set; } = false;
        public bool DisplaySuccessSystemNotification { get; set; } = false;
        public bool DisplayWarningSystemNotification { get; set; } = true;
        public bool DisplayErrorSystemNotification { get; set; } = true;
        public bool DisplayInformationalBarNotification { get; set; } = false;
        public bool DisplaySuccessBarNotification { get; set; } = false;
        public bool DisplayWarningBarNotification { get; set; } = true;
        public bool DisplayErrorBarNotification { get; set; } = true;
    }
}
