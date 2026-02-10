using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using VidHub.Core.Models;
using VidHub.Core.Settings;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Base;
using VidHub.Services.Logics;
using Windows.Storage;

namespace VidHub.Services.Connectors.Base
{
    public class TitleBarConnector(IVideoService vs, IVidHubSettings settings, IVideoLoadService load) : ConnectorTemplate(vs), ITitleBarConnector
    {
        public bool OpenedSidePanel
        {
            get => settings.General.OpenedSidePanel;
            set
            {
                settings.General.OpenedSidePanel = value;
                vs.Update(UpdateSections.SIDEPANEL);
            }
        }
        public bool KeepSidePanelSettings
        {
            get => settings.General.KeepSidePanelSettings;
            set => settings.General.KeepSidePanelSettings = value;
        }
        public bool UseRealTimeSearch
        {
            get => settings.SidePanel.UseRealTimeSearch;
            set
            {
                settings.SidePanel.UseRealTimeSearch = value;
                vs.Update(UpdateSections.SIDEPANEL);
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public bool UseCaseSensitiveSearch
        {
            get => settings.SidePanel.UseCaseSensitiveSearch;
            set
            {
                settings.SidePanel.UseCaseSensitiveSearch = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public bool UseSearchSuggestions
        {
            get => settings.SidePanel.UseSearchSuggestions;
            set
            {
                settings.SidePanel.UseSearchSuggestions = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }

        public bool DisplayDates
        {
            get => settings.Display.DisplayDate;
            set
            {
                settings.Display.DisplayDate = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public bool DisplayDurations
        {
            get => settings.Display.DisplayDuration;
            set
            {
                settings.Display.DisplayDuration = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public bool DisplayFramerates
        {
            get => settings.Display.DisplayFramerate;
            set
            {
                settings.Display.DisplayFramerate = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public bool DisplayHealths
        {
            get => settings.Display.DisplayHealth;
            set
            {
                settings.Display.DisplayHealth = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public bool DisplayResolutions
        {
            get => settings.Display.DisplayResolution;
            set
            {
                settings.Display.DisplayResolution = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public bool DisplayTitles
        {
            get => settings.Display.DisplayTitle;
            set
            {
                settings.Display.DisplayTitle = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }

        public bool UseCacheLoading
        {
            get => settings.Performance.UseCacheLoading;
            set => settings.Performance.UseCacheLoading = value;
        }
        public bool UseConcurrentLoading
        {
            get => settings.Performance.UseConcurrentLoading;
            set => settings.Performance.UseConcurrentLoading = value;
        }
        public bool UseContentHash
        {
            get => settings.General.UseFileContentHash;
            set => settings.General.UseFileContentHash = value;
        }

        public bool DisplayInformationalSystemNotification
        {
            get => settings.Notifications.DisplayInformationalSystemNotification;
            set => settings.Notifications.DisplayInformationalSystemNotification = value;
        }
        public bool DisplaySuccessSystemNotification
        {
            get => settings.Notifications.DisplaySuccessSystemNotification;
            set => settings.Notifications.DisplaySuccessSystemNotification = value;
        }
        public bool DisplayWarningSystemNotification
        {
            get => settings.Notifications.DisplayWarningSystemNotification;
            set => settings.Notifications.DisplayWarningSystemNotification = value;
        }
        public bool DisplayErrorSystemNotification
        {
            get => settings.Notifications.DisplayErrorSystemNotification;
            set => settings.Notifications.DisplayErrorSystemNotification = value;
        }

        public bool DisplayInformationalBarNotification
        {
            get => settings.Notifications.DisplayInformationalBarNotification;
            set => settings.Notifications.DisplayInformationalBarNotification = value;
        }
        public bool DisplaySuccessBarNotification
        {
            get => settings.Notifications.DisplaySuccessBarNotification;
            set => settings.Notifications.DisplaySuccessBarNotification = value;
        }
        public bool DisplayWarningBarNotification
        {
            get => settings.Notifications.DisplayWarningBarNotification;
            set => settings.Notifications.DisplayWarningBarNotification = value;
        }
        public bool DisplayErrorBarNotification
        {
            get => settings.Notifications.DisplayErrorBarNotification;
            set => settings.Notifications.DisplayErrorBarNotification = value;
        }

        public bool DisabledHealthCheck
        {
            get => settings.Health.Type == HealthType.NONE;
            set
            {
                if (value)
                {
                    settings.Health.Type = HealthType.NONE;
                }
            }
        }
        public bool ExistenceHealthCheck
        {
            get => settings.Health.Type == HealthType.EXISTENCECHECK;
            set
            {
                if (value)
                {
                    settings.Health.Type = HealthType.EXISTENCECHECK;
                }
            }
        }
        public bool QuickHealthCheck
        {
            get => settings.Health.Type == HealthType.QUICKCHECK;
            set
            {
                if (value)
                {
                    settings.Health.Type = HealthType.QUICKCHECK;
                }
            }
        }
        public bool FullHealthCheck
        {
            get => settings.Health.Type == HealthType.FULLCHECK;
            set
            {
                if (value)
                {
                    settings.Health.Type = HealthType.FULLCHECK;
                }
            }
        }

        public string CacheSize => GetCacheSize();
        private string GetCacheSize()
        {
            long size = Directory.EnumerateFiles(Path.Combine(Path.GetTempPath(), "VidHub"), "*", SearchOption.AllDirectories).Sum(f => new FileInfo(f).Length);
            return $"Clear Cache ({FormatSize(size):0.##})";
        }

        public string LogSize => GetLogSize();
        private string GetLogSize()
        {
            long size = Directory.EnumerateFiles(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub", "Logs"), "*", SearchOption.AllDirectories).Order().SkipLast(1).Sum(f => new FileInfo(f).Length);
            return $"Clear Log ({FormatSize(size):0.##})";
        }


        public async Task Import()
        {
            await load.Import();
        }
        public async Task Export()
        {
            await load.Export();
        }

        public async Task LoadFiles()
        {
            await load.LoadFiles();
        }
        public async Task LoadFolders(bool includeSubfolders)
        {
            await load.LoadFolders(includeSubfolders);
        }
        public async Task LoadItems(IEnumerable<IStorageItem> items, bool includeSubfolders)
        {
            await load.LoadItems(items, includeSubfolders);
        }

        public async Task OpenLicensesDialog()
        {
            await VidHubContext.Window.OpenLicensesDialog();
        }
        public async Task OpenDisplayFormatDialog()
        {
            await VidHubContext.Window.OpenDisplayFormatDialog();
        }
        public async Task OpenPassiveTitleFormatDialog()
        {
            await VidHubContext.Window.OpenPassiveTitleFormatDialog();
        }
        public async Task OpenPreviewImageFormatDialog()
        {
            await VidHubContext.Window.OpenPreviewImageFormatDialog();
        }
        public async Task OpenVersionsDialog()
        {
            await VidHubContext.Window.OpenVersionsDialog();
        }
        public async Task OpenTagsDialog()
        {
            await VidHubContext.Window.OpenTagsDialog();
        }

        public async Task ClearCache()
        {
            await Task.Run(() =>
            {
                foreach (var file in Directory.EnumerateFiles(Path.Combine(Path.GetTempPath(), "VidHub"), "*", SearchOption.AllDirectories))
                {
                    try
                    {
                        VidHubContext.Logger.LogTrace("Deleting cache file: {File}", file);
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        VidHubContext.Logger.LogError(ex, "Failed to delete cache file: {File}", file);
                    }
                }
            });
        }
        public async Task ClearLogs()
        {
            await Task.Run(() =>
            {
                foreach (var file in Directory.EnumerateFiles(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub", "Logs"), "*", SearchOption.AllDirectories).Order().SkipLast(1))
                {
                    try
                    {
                        VidHubContext.Logger.LogTrace("Deleting log file: {File}", file);
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        VidHubContext.Logger.LogError(ex, "Failed to delete log file: {File}", file);
                    }
                }
            });
        }


        private static string FormatSize(long size)
        {
            if (size > 1024 * 1024 * 1024)
            {
                return $"{size / (1024 * 1024 * 1024)} GiB";
            }
            else if (size > 1024 * 1024)
            {
                return $"{size / (1024 * 1024)} MiB";
            }
            else if (size > 1024)
            {
                return $"{size / 1024} kiB";
            }
            else
            {
                return $"{size} B";
            }
        }
    }
}
