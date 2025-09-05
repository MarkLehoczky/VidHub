using VidHub.Services.Base.Interfaces;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.Services.Settings
{
    public class SettingsService(IMainService service) : ISettingsService
    {
        private bool openPanel = true;
        private bool systemNotifications = true;
        private bool cacheLoad = true;
        private bool concurrentVideoLoading = false;
        private bool caseSensitiveTextFiltering = false;
        private bool liveTextFiltering = true;

        public bool OpenPanel
        {
            get => openPanel;
            set
            {
                if (openPanel == value) return;
                openPanel = value;
                service.Update();
            }
        }


        public bool SystemNotifications
        {
            get => systemNotifications;
            set => systemNotifications = value;
        }

        public bool CacheLoad
        {
            get => cacheLoad;
            set => cacheLoad = value;
        }

        public bool ConcurrentVideoLoading
        {
            get => concurrentVideoLoading;
            set => concurrentVideoLoading = value;
        }

        public bool CaseSensitiveTextFiltering
        {
            get => caseSensitiveTextFiltering;
            set => caseSensitiveTextFiltering = value;
        }

        public bool LiveTextFiltering
        {
            get => liveTextFiltering;
            set => liveTextFiltering = value;
        }
    }
}
