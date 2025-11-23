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
                vs.Update(UpdateType.UpdateVideoCollection);
            }
        }
        public bool DisplayDurations
        {
            get => settings.DisplayCustomization.DisplayDurations;
            set
            {
                settings.DisplayCustomization.DisplayDurations = value;
                vs.Update(UpdateType.UpdateVideoCollection);
            }
        }
        public bool DisplayTitles
        {
            get => settings.DisplayCustomization.DisplayTitles;
            set
            {
                settings.DisplayCustomization.DisplayTitles = value;
                vs.Update(UpdateType.UpdateVideoCollection);
            }
        }
        public bool EnableCacheLoading
        {
            get => settings.Organizer.Global.EnableCacheLoading;
            set
            {
                settings.Organizer.Global.EnableCacheLoading = value;
                vs.Update(UpdateType.UpdateVideoCollection);
            }
        }
        public bool EnableCaseSensitiveSearch
        {
            get => settings.Organizer.Global.EnableCaseSensitiveSearch;
            set
            {
                settings.Organizer.Global.EnableCaseSensitiveSearch = value;
                vs.Update(UpdateType.UpdateVideoCollection);
            }
        }
        public bool EnableConcurrentLoading
        {
            get => settings.Organizer.Global.EnableConcurrentLoading;
            set
            {
                settings.Organizer.Global.EnableConcurrentLoading = value;
                vs.Update(UpdateType.UpdateVideoCollection);
            }
        }
        public bool EnableLiveSearch
        {
            get => settings.Organizer.Global.EnableLiveSearch;
            set
            {
                settings.Organizer.Global.EnableLiveSearch = value;
                vs.Update(UpdateType.UpdateSidePanel);
                vs.Update(UpdateType.UpdateVideoCollection);
            }
        }
        public bool EnableSearchSuggestions
        {
            get => settings.Organizer.Global.EnableSearchSuggestions;
            set
            {
                settings.Organizer.Global.EnableSearchSuggestions = value;
                vs.Update(UpdateType.UpdateVideoCollection);
            }
        }
        public bool EnableSystemNotification
        {
            get => settings.Organizer.Global.EnableSystemNotification;
            set
            {
                settings.Organizer.Global.EnableSystemNotification = value;
                vs.Update(UpdateType.UpdateVideoCollection);
            }
        }
        public bool OpenedSidePanel
        {
            get => settings.Organizer.Global.OpenedSidePanel;
            set
            {
                settings.Organizer.Global.OpenedSidePanel = value;
                vs.Update(UpdateType.UpdateSidePanel);
            }
        }
        public bool SaveOrganizerSettings
        {
            get => settings.Organizer.Global.SaveOrganizerSettings;
            set => settings.Organizer.Global.SaveOrganizerSettings = value;
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
