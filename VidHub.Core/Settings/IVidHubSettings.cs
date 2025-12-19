using VidHub.Core.Notifications;

namespace VidHub.Core.Settings
{
    public interface IVidHubSettings
    {
        DialogSettings Dialogs { get; set; }
        DisplaySettings Display { get; set; }
        GeneralSettings General { get; set; }
        HealthSettings Health { get; set; }
        NotificationSettings Notifications { get; set; }
        PerformanceSettings Performance { get; set; }
        SidePanelSettings SidePanel { get; set; }

        void CustomizeVideoTitle(ref Video video);
        bool DisplayNotification(BarNotification notification);
        bool DisplayNotification(BaseNotification notification);
        bool DisplayNotification(SystemNotification notification);
        string GetCustomizedVideoTitle(Video video);
        string GetCustomizedVideoTitle(Video video, bool useRegex);
        TimeSpan GetPreviewImageTime(TimeSpan duration);
        TimeSpan GetPreviewImageTime(Video video);
        void Load();
        void Save();
        StringComparison SearchComparison();
        bool ValidVideo(Video video);
    }
}