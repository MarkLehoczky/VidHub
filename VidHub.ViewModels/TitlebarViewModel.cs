using CommunityToolkit.Mvvm.Input;
using VidHub.Core.Helpers;
using VidHub.ViewModels.Base;
using VidHub.Services.Connectors.Base.Interfaces;
using VidHub.Platform;

namespace VidHub.ViewModels
{
    public partial class TitleBarViewModel(ITitleBarConnector connector) : ViewModelTemplate(connector)
    {
        public TitleBarViewModel() : this(Context.Host.GetService<ITitleBarConnector>()) { }


        private bool CanOpenSidePanel() => !connector.OpenedSidePanel;
        private bool CanCloseSidePanel() => connector.OpenedSidePanel;


        public bool EnableSystemNotification
        {
            get => connector.EnableSystemNotification;
            set => connector.EnableSystemNotification = value;
        }

        public bool EnableCacheLoading
        {
            get => connector.EnableCacheLoading;
            set => connector.EnableCacheLoading = value;
        }

        public bool EnableConcurrentLoading
        {
            get => connector.EnableConcurrentLoading;
            set => connector.EnableConcurrentLoading = value;
        }

        public bool SaveOrganizerSettings
        {
            get => connector.SaveOrganizerSettings;
            set => connector.SaveOrganizerSettings = value;
        }

        public bool EnableLiveSearch
        {
            get => connector.EnableLiveSearch;
            set => connector.EnableLiveSearch = value;
        }

        public bool EnableCaseSensitiveSearch
        {
            get => connector.EnableCaseSensitiveSearch;
            set => connector.EnableCaseSensitiveSearch = value;
        }

        public bool EnableSearchSuggestions
        {
            get => connector.EnableSearchSuggestions;
            set => connector.EnableSearchSuggestions = value;
        }

        public bool DisplayTitles
        {
            get => connector.DisplayTitles;
            set => connector.DisplayTitles = value;
        }
        public bool DisplayDates
        {
            get => connector.DisplayDates;
            set => connector.DisplayDates = value;
        }
        public bool DisplayDurations
        {
            get => connector.DisplayDurations;
            set => connector.DisplayDurations = value;
        }


        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task LoadFilesAsync()
        {
            await connector.LoadFilesAsync();
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task LoadSingleFolderAsync()
        {
            await connector.LoadFoldersAsync(false);
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task LoadAllFolderAsync()
        {
            await connector.LoadFoldersAsync(true);
        }

        [RelayCommand]
        private async Task ImportCollectionAsync()
        {
            await connector.ImportCollectionAsync();
        }

        [RelayCommand]
        private async Task ExportCollectionAsync()
        {
            await connector.ExportCollectionAsync();
        }


        [RelayCommand(CanExecute = nameof(CanOpenSidePanel))]
        private void OpenSidePanel()
        {
            connector.OpenedSidePanel = true;
            OpenSidePanelCommand.NotifyCanExecuteChanged();
            CloseSidePanelCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanCloseSidePanel))]
        private void CloseSidePanel()
        {
            connector.OpenedSidePanel = false;
            OpenSidePanelCommand.NotifyCanExecuteChanged();
            CloseSidePanelCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private async Task CustomizeVideoDisplayingAsync()
        {
            await connector.CustomizeVideoDisplayingAsync();
        }

        [RelayCommand]
        private async Task CustomizeVideoLoadingAsync()
        {
            await connector.CustomizeVideoLoadingAsync();
        }

        [RelayCommand]
        private async Task CustomizeThumbnailAsync()
        {
            await connector.CustomizeVideoPreviewImageAsync();
        }


        override public void Update(UpdateType type) { }
    }
}
