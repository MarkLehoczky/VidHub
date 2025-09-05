namespace VidHub.Services.Settings.Interfaces
{
    public interface ISettingsService
    {
        bool OpenPanel { get; set; }
        bool CacheLoad { get; set; }
        bool ConcurrentVideoLoading { get; set; }
        bool SystemNotifications { get; set; }
    }
}
