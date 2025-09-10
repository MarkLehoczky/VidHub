using System.Text.Json;
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
        private bool keepFilterStatus = true;
        private bool liveTextFiltering = true;
        private bool caseSensitiveTextFiltering = false;

        public void Load()
        {
            var appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub");
            var appDataSettings = Path.Combine(appDataDirectory, "VidHub.json");

            Directory.CreateDirectory(appDataDirectory);

            if (File.Exists(appDataSettings))
            {
                string json = File.ReadAllText(appDataSettings);
                var settings = JsonSerializer.Deserialize<SettingsLoader>(json);
                Set(settings);
            }
        }

        public void Save()
        {
            var appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub");
            var appDataSettings = Path.Combine(appDataDirectory, "VidHub.json");

            Directory.CreateDirectory(appDataDirectory);

            string json = JsonSerializer.Serialize(this);
            File.WriteAllText(appDataSettings, json);
        }

        public void Set(ISettingsService service)
        {
            openPanel = service.OpenPanel;
            systemNotifications = service.SystemNotifications;
            cacheLoad = service.CacheLoad;
            concurrentVideoLoading = service.ConcurrentVideoLoading;
            keepFilterStatus = service.KeepFilterStatus;
            liveTextFiltering = service.LiveTextFiltering;
            caseSensitiveTextFiltering = service.CaseSensitiveTextFiltering;
        }


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

        public bool KeepFilterStatus
        {
            get => keepFilterStatus;
            set => keepFilterStatus = value;
        }

        public bool LiveTextFiltering
        {
            get => liveTextFiltering;
            set
            {
                if (liveTextFiltering == value) return;
                liveTextFiltering = value;
                service.Update();
            }
        }

        public bool CaseSensitiveTextFiltering
        {
            get => caseSensitiveTextFiltering;
            set => caseSensitiveTextFiltering = value;
        }
    }
}
