namespace VidHub.Services.Settings.Interfaces
{
    public interface ISettingsService
    {
        bool OpenPanel { get; set; }
        bool SystemNotifications { get; set; }
        bool CacheLoad { get; set; }
        bool ConcurrentVideoLoading { get; set; }
        bool KeepFilterStatus { get; set; }
        bool CaseSensitiveTextFiltering { get; set; }
        bool LiveTextFiltering { get; set; }
        bool TextSuggestions {  get; set; }
        void Load();
        void Save();
        void Set(ISettingsService service);
    }
}
