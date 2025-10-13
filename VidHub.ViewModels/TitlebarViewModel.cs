using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VidHub.Core.Helpers;
using VidHub.Platform;
using VidHub.Services.Logics.Interfaces;
using VidHub.Services.Modals.Interfaces;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.ViewModels
{
    public partial class TitleBarViewModel(IVideoLoadService service, ISettingsService settings) : ObservableRecipient
    {
        private bool CanOpenSidePanel() => !settings.Organizer.Global.OpenedSidePanel;
        private bool CanCloseSidePanel() => settings.Organizer.Global.OpenedSidePanel;


        public bool EnableSystemNotification
        {
            get => settings.Organizer.Global.EnableSystemNotification;
            set => settings.Organizer.Global.EnableSystemNotification = value;
        }

        public bool EnableCacheLoading
        {
            get => settings.Organizer.Global.EnableCacheLoading;
            set => settings.Organizer.Global.EnableCacheLoading = value;
        }

        public bool EnableConcurrentLoading
        {
            get => settings.Organizer.Global.EnableConcurrentLoading;
            set => settings.Organizer.Global.EnableConcurrentLoading = value;
        }

        public bool SaveOrganizerSettings
        {
            get => settings.Organizer.Global.SaveOrganizerSettings;
            set => settings.Organizer.Global.SaveOrganizerSettings = value;
        }

        public bool EnableLiveSearch
        {
            get => settings.Organizer.Global.EnableLiveSearch;
            set => settings.Organizer.Global.EnableLiveSearch = value;
        }

        public bool EnableCaseSensitiveSearch
        {
            get => settings.Organizer.Global.EnableCaseSensitiveSearch;
            set => settings.Organizer.Global.EnableCaseSensitiveSearch = value;
        }

        public bool EnableSearchSuggestions
        {
            get => settings.Organizer.Global.EnableSearchSuggestions;
            set => settings.Organizer.Global.EnableSearchSuggestions = value;
        }

        public bool DisplayTitles
        {
            get => settings.DisplayCustomization.DisplayTitles;
            set => settings.DisplayCustomization.DisplayTitles = value;
        }
        public bool DisplayDates
        {
            get => settings.DisplayCustomization.DisplayDates;
            set => settings.DisplayCustomization.DisplayDates = value;
        }
        public bool DisplayDurations
        {
            get => settings.DisplayCustomization.DisplayDurations;
            set => settings.DisplayCustomization.DisplayDurations = value;
        }


        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task LoadFilesAsync()
        {
            await service.LoadFilesAsync();
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task LoadSingleFolderAsync()
        {
            await service.LoadFoldersAsync(false);
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task LoadAllFolderAsync()
        {
            await service.LoadFoldersAsync(true);
        }

        [RelayCommand]
        private async Task ImportCollectionAsync()
        {
            await service.ImportCollectionAsync();
        }

        [RelayCommand]
        private async Task ExportCollectionAsync()
        {
            await service.ExportCollectionAsync();
        }


        [RelayCommand(CanExecute = nameof(CanOpenSidePanel))]
        private void OpenSidePanel()
        {
            settings.Organizer.Global.OpenedSidePanel = true;
            OpenSidePanelCommand.NotifyCanExecuteChanged();
            CloseSidePanelCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanCloseSidePanel))]
        private void CloseSidePanel()
        {
            settings.Organizer.Global.OpenedSidePanel = false;
            OpenSidePanelCommand.NotifyCanExecuteChanged();
            CloseSidePanelCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private async Task CustomizeVideoDisplayingAsync()
        {
            await Context.Window.ShowDialogAsync(ModalType.CustomizeVideoDisplayFormat, "Customize video displaying", "Confirm");
        }

        [RelayCommand]
        private async Task CustomizeVideoLoadingAsync()
        {
            await Context.Window.ShowDialogAsync(ModalType.CustomizeTitleFormat, "Customize video title", "Confirm", new Tuple<bool, IEnumerable<int>>(true, []));
        }

        [RelayCommand]
        private async Task CustomizeThumbnailAsync()
        {
            await Context.Window.ShowDialogAsync(ModalType.CustomizePreviewImageFrame, "Customize video preview image", "Confirm");
        }


        public TitleBarViewModel() : this(
            Context.Host.GetService<IVideoLoadService>(),
            Context.Host.GetService<ISettingsService>())
        { }
    }
}
