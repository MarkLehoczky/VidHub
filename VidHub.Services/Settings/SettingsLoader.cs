using VidHub.Services.Settings.Interfaces;

namespace VidHub.Services.Settings
{
    internal class SettingsLoader : ISettingsService
    {
        public bool OpenPanel { get; set; }
        public bool SystemNotifications { get; set; }
        public bool CacheLoad { get; set; }
        public bool ConcurrentVideoLoading { get; set; }
        public bool KeepFilterStatus { get; set; }
        public bool CaseSensitiveTextFiltering { get; set; }
        public bool LiveTextFiltering { get; set; }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Set(ISettingsService service)
        {
            throw new NotImplementedException();
        }
    }
}
