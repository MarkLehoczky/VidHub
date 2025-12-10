using VidHub.Core.Settings;
using VidHub.Core.Utilities.Helper;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Connectors.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;
using Windows.Storage;

namespace VidHub.Services.Connectors.Base
{
    public class TitleBarConnector(IVideoService vs, IVidHubSettings settings, IVideoLoadService load) : ITitleBarConnector
    {
        public bool DisplayDates
        {
            get => settings.Display.DisplayDates;
            set
            {
                settings.Display.DisplayDates = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public bool DisplayDurations
        {
            get => settings.Display.DisplayDurations;
            set
            {
                settings.Display.DisplayDurations = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public bool DisplayTitles
        {
            get => settings.Display.DisplayTitles;
            set
            {
                settings.Display.DisplayTitles = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public bool EnableCacheLoading
        {
            get => settings.Performance.UseCacheLoading;
            set
            {
                settings.Performance.UseCacheLoading = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public bool EnableCaseSensitiveSearch
        {
            get => settings.SidePanel.UseCaseSensitiveSearch;
            set
            {
                settings.SidePanel.UseCaseSensitiveSearch = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public bool EnableConcurrentLoading
        {
            get => settings.Performance.UseConcurrentLoading;
            set
            {
                settings.Performance.UseConcurrentLoading = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public bool EnableLiveSearch
        {
            get => settings.SidePanel.UseRealTimeSearch;
            set
            {
                settings.SidePanel.UseRealTimeSearch = value;
                vs.Update(UpdateSections.SIDEPANEL);
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public bool EnableSearchSuggestions
        {
            get => settings.SidePanel.UseSearchSuggestions;
            set
            {
                settings.SidePanel.UseSearchSuggestions = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public bool OpenedSidePanel
        {
            get => settings.General.OpenedSidePanel;
            set
            {
                settings.General.OpenedSidePanel = value;
                vs.Update(UpdateSections.SIDEPANEL);
            }
        }
        public bool SaveOrganizerSettings
        {
            get => settings.General.KeepSidePanelSettings;
            set => settings.General.KeepSidePanelSettings = value;
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
            get => settings.VideoHealth.Type == VideoHealthCheckType.NONE;
            set
            {
                if (value)
                {
                    settings.VideoHealth.Type = VideoHealthCheckType.NONE;
                }
            }
        }
        public bool ExistenceHealthCheck
        {
            get => settings.VideoHealth.Type == VideoHealthCheckType.EXISTENCECHECK;
            set
            {
                if (value)
                {
                    settings.VideoHealth.Type = VideoHealthCheckType.EXISTENCECHECK;
                }
            }
        }
        public bool QuickHealthCheck
        {
            get => settings.VideoHealth.Type == VideoHealthCheckType.QUICKCHECK;
            set
            {
                if (value)
                {
                    settings.VideoHealth.Type = VideoHealthCheckType.QUICKCHECK;
                }
            }
        }
        public bool FullHealthCheck
        {
            get => settings.VideoHealth.Type == VideoHealthCheckType.FULLCHECK;
            set
            {
                if (value)
                {
                    settings.VideoHealth.Type = VideoHealthCheckType.FULLCHECK;
                }
            }
        }

        public async Task CustomizeVideoDisplayingAsync()
        {
            await Context.Window.ShowDialogAsync("CustomizeVideoDisplayFormat", "Customize video displaying", "Confirm");
        }

        public async Task CustomizeVideoLoadingAsync()
        {
            await Context.Window.ShowDialogAsync("CustomizeTitleFormat", "Customize video title", "Confirm", new Tuple<bool, IEnumerable<int>>(true, vs.Select(v => v.ID)));
        }

        public async Task CustomizeVideoPreviewImageAsync()
        {
            await Context.Window.ShowDialogAsync("CustomizePreviewImageFrame", "Customize video preview image", "Confirm");
        }

        public async Task ExportCollectionAsync()
        {
            await load.ExportCollectionAsync();
        }

        public async Task ImportCollectionAsync()
        {
            await load.ImportCollectionAsync();
        }

        public async Task LoadFilesAsync()
        {
            await load.LoadFilesAsync();
        }

        public async Task LoadFoldersAsync(bool includeSubfolders)
        {
            await load.LoadFoldersAsync(includeSubfolders);
        }

        public async Task LoadItems(IEnumerable<IStorageItem> items, bool includeSubfolders)
        {
            await load.LoadItems(items, includeSubfolders);
        }

        public async Task OpenLicensesModalAsync()
        {
            await Context.Window.ShowDialogAsync("DisplayLicenseInformation", "COPYRIGHT", "Close");
        }

        public async Task OpenVersionsModalAsync()
        {
            await Context.Window.ShowDialogAsync("DisplayVersionInformation", "VERSION CHANGES", "Close");
        }

        public void SubscribeToUpdateEvent(Action<IEnumerable<UpdateSection>> action)
        {
            vs.SubscribeToUpdateEvent(action);
        }

        public void UnsubscribeFromUpdateEvent(Action<IEnumerable<UpdateSection>> action)
        {
            vs.UnsubscribeFromUpdateEvent(action);
        }

        public void Update(IEnumerable<UpdateSection> sections)
        {
            vs.Update(sections);
        }

        public void Update(params UpdateSection[] sections)
        {
            vs.Update(sections);
        }
    }
}
