using VidHub.Services.Base.Interfaces;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.Services.Settings
{
    public class SettingsService(IMainService service) : ISettingsService
    {
        private bool openPanel = true;
        private bool concurrentVideoLoading = false;

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

        public bool ConcurrentVideoLoading
        {
            get => concurrentVideoLoading;
            set => concurrentVideoLoading = value;
        }
    }
}
