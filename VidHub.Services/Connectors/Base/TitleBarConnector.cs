using VidHub.Core.Enums;
using VidHub.Core.Settings;
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
            get => settings.DisplayCustomization.DisplayDates;
            set
            {
                settings.DisplayCustomization.DisplayDates = value;
                vs.Update(UpdateType.UPDATEVIDEOCOLLECTION);
            }
        }
        public bool DisplayDurations
        {
            get => settings.DisplayCustomization.DisplayDurations;
            set
            {
                settings.DisplayCustomization.DisplayDurations = value;
                vs.Update(UpdateType.UPDATEVIDEOCOLLECTION);
            }
        }
        public bool DisplayTitles
        {
            get => settings.DisplayCustomization.DisplayTitles;
            set
            {
                settings.DisplayCustomization.DisplayTitles = value;
                vs.Update(UpdateType.UPDATEVIDEOCOLLECTION);
            }
        }
        public bool EnableCacheLoading
        {
            get => settings.Organizer.Global.EnableCacheLoading;
            set
            {
                settings.Organizer.Global.EnableCacheLoading = value;
                vs.Update(UpdateType.UPDATEVIDEOCOLLECTION);
            }
        }
        public bool EnableCaseSensitiveSearch
        {
            get => settings.Organizer.Global.EnableCaseSensitiveSearch;
            set
            {
                settings.Organizer.Global.EnableCaseSensitiveSearch = value;
                vs.Update(UpdateType.UPDATEVIDEOCOLLECTION);
            }
        }
        public bool EnableConcurrentLoading
        {
            get => settings.Organizer.Global.EnableConcurrentLoading;
            set
            {
                settings.Organizer.Global.EnableConcurrentLoading = value;
                vs.Update(UpdateType.UPDATEVIDEOCOLLECTION);
            }
        }
        public bool EnableLiveSearch
        {
            get => settings.Organizer.Global.EnableLiveSearch;
            set
            {
                settings.Organizer.Global.EnableLiveSearch = value;
                vs.Update(UpdateType.UPDATESIDEPANEL);
                vs.Update(UpdateType.UPDATEVIDEOCOLLECTION);
            }
        }
        public bool EnableSearchSuggestions
        {
            get => settings.Organizer.Global.EnableSearchSuggestions;
            set
            {
                settings.Organizer.Global.EnableSearchSuggestions = value;
                vs.Update(UpdateType.UPDATEVIDEOCOLLECTION);
            }
        }
        public bool OpenedSidePanel
        {
            get => settings.Organizer.Global.OpenedSidePanel;
            set
            {
                settings.Organizer.Global.OpenedSidePanel = value;
                vs.Update(UpdateType.UPDATESIDEPANEL);
            }
        }
        public bool SaveOrganizerSettings
        {
            get => settings.Organizer.Global.SaveOrganizerSettings;
            set => settings.Organizer.Global.SaveOrganizerSettings = value;
        }
        public bool DisplayInformationalSystemNotification
        {
            get => settings.Organizer.Global.DisplayInformationalSystemNotification;
            set => settings.Organizer.Global.DisplayInformationalSystemNotification = value;
        }
        public bool DisplaySuccessSystemNotification
        {
            get => settings.Organizer.Global.DisplaySuccessSystemNotification;
            set => settings.Organizer.Global.DisplaySuccessSystemNotification = value;
        }
        public bool DisplayWarningSystemNotification
        {
            get => settings.Organizer.Global.DisplayWarningSystemNotification;
            set => settings.Organizer.Global.DisplayWarningSystemNotification = value;
        }
        public bool DisplayErrorSystemNotification
        {
            get => settings.Organizer.Global.DisplayErrorSystemNotification;
            set => settings.Organizer.Global.DisplayErrorSystemNotification = value;
        }
        public bool DisplayInformationalBarNotification
        {
            get => settings.Organizer.Global.DisplayInformationalBarNotification;
            set => settings.Organizer.Global.DisplayInformationalBarNotification = value;
        }
        public bool DisplaySuccessBarNotification
        {
            get => settings.Organizer.Global.DisplaySuccessBarNotification;
            set => settings.Organizer.Global.DisplaySuccessBarNotification = value;
        }
        public bool DisplayWarningBarNotification
        {
            get => settings.Organizer.Global.DisplayWarningBarNotification;
            set => settings.Organizer.Global.DisplayWarningBarNotification = value;
        }
        public bool DisplayErrorBarNotification
        {
            get => settings.Organizer.Global.DisplayErrorBarNotification;
            set => settings.Organizer.Global.DisplayErrorBarNotification = value;
        }

        public bool DisabledHealthCheck
        {
            get => settings.Organizer.Global.HealthCheck == HealthCheckLevel.NONE;
            set
            {
                if (value)
                {
                    settings.Organizer.Global.HealthCheck = HealthCheckLevel.NONE;
                }
            }
        }
        public bool ExistenceHealthCheck
        {
            get => settings.Organizer.Global.HealthCheck == HealthCheckLevel.EXISTENCECHECK;
            set
            {
                if (value)
                {
                    settings.Organizer.Global.HealthCheck = HealthCheckLevel.EXISTENCECHECK;
                }
            }
        }
        public bool QuickHealthCheck
        {
            get => settings.Organizer.Global.HealthCheck == HealthCheckLevel.QUICKCHECK;
            set
            {
                if (value)
                {
                    settings.Organizer.Global.HealthCheck = HealthCheckLevel.QUICKCHECK;
                }
            }
        }
        public bool FullHealthCheck
        {
            get => settings.Organizer.Global.HealthCheck == HealthCheckLevel.FULLCHECK;
            set
            {
                if (value)
                {
                    settings.Organizer.Global.HealthCheck = HealthCheckLevel.FULLCHECK;
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

        public void SubscribeToUpdateEvent(Action<UpdateType> action)
        {
            vs.SubscribeToUpdateEvent(action);
        }

        public void UnsubscribeFromUpdateEvent(Action<UpdateType> action)
        {
            vs.UnsubscribeFromUpdateEvent(action);
        }

        public void Update(UpdateType type)
        {
            vs.Update(type);
        }
    }
}
