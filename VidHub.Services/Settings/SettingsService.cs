using VidHub.Services.Base.Interfaces;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.Services.Settings
{
    public class SettingsService(IMainService service) : ISettingsService
    {
        private bool openPanel = true;
        private bool cacheLoad = true;
        private bool concurrentVideoLoading = false;
        private bool systemNotifications = true;

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

        public bool SystemNotifications
        {
            get => systemNotifications;
            set => systemNotifications = value;
        }
    }
}
