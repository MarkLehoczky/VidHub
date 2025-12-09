using VidHub.Core.Models.Notifications;

namespace VidHub.Core.Settings
{
    public interface IVidHubSettings
    {
        DisplayCustomizationSettings DisplayCustomization { get; set; }
        OrganizerSettings Organizer { get; set; }
        PreviewImageCustomizationSettings PreviewImageCustomization { get; set; }
        TitleCustomizationSettings TitleCustomization { get; set; }
        void Load();
        void Save();
        bool DisplaySystemNotification(SystemNotification notification);
        bool DisplayBarNotification(BarNotification notification);
    }
}
