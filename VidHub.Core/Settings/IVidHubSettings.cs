using VidHub.Core.Models.Notifications;
using VidHub.Core.Settings.Models;

namespace VidHub.Core.Settings
{
    public interface IVidHubSettings
    {
        DisplaySettings Display { get; set; }
        GeneralSettings General { get; set; }
        ModalSettings Modals { get; set; }
        NotificationSettings Notifications { get; set; }
        PerformanceSettings Performance { get; set; }
        SidePanelSettings SidePanel { get; set; }
        VideoHealthSettings VideoHealth { get; set; }

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