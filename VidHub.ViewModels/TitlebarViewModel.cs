using CommunityToolkit.Mvvm.Input;
using VidHub.Core.Data;
using VidHub.Core.Utilities.Helper;
using VidHub.Platform;
using VidHub.Services.Connectors.Base.Interfaces;
using VidHub.ViewModels.Base;

namespace VidHub.ViewModels
{
    public partial class TitleBarViewModel(ITitleBarConnector connector) : ViewModelTemplate(connector)
    {
        public TitleBarViewModel() : this(Context.Host.GetService<ITitleBarConnector>()) { }


        public bool DisplayInformationalSystemNotification
        {
            get => connector.DisplayInformationalSystemNotification;
            set => connector.DisplayInformationalSystemNotification = value;
        }
        public bool DisplaySuccessSystemNotification
        {
            get => connector.DisplaySuccessSystemNotification;
            set => connector.DisplaySuccessSystemNotification = value;
        }
        public bool DisplayWarningSystemNotification
        {
            get => connector.DisplayWarningSystemNotification;
            set => connector.DisplayWarningSystemNotification = value;
        }
        public bool DisplayErrorSystemNotification
        {
            get => connector.DisplayErrorSystemNotification;
            set => connector.DisplayErrorSystemNotification = value;
        }

        public bool DisplayInformationalBarNotification
        {
            get => connector.DisplayInformationalBarNotification;
            set => connector.DisplayInformationalBarNotification = value;
        }
        public bool DisplaySuccessBarNotification
        {
            get => connector.DisplaySuccessBarNotification;
            set => connector.DisplaySuccessBarNotification = value;
        }
        public bool DisplayWarningBarNotification
        {
            get => connector.DisplayWarningBarNotification;
            set => connector.DisplayWarningBarNotification = value;
        }
        public bool DisplayErrorBarNotification
        {
            get => connector.DisplayErrorBarNotification;
            set => connector.DisplayErrorBarNotification = value;
        }

        public bool DisabledHealthCheck
        {
            get => connector.DisabledHealthCheck;
            set => connector.DisabledHealthCheck = value;
        }
        public bool ExistenceHealthCheck
        {
            get => connector.ExistenceHealthCheck;
            set => connector.ExistenceHealthCheck = value;
        }
        public bool QuickHealthCheck
        {
            get => connector.QuickHealthCheck;
            set => connector.QuickHealthCheck = value;
        }
        public bool FullHealthCheck
        {
            get => connector.FullHealthCheck;
            set => connector.FullHealthCheck = value;
        }

        public bool UseCacheLoading
        {
            get => connector.UseCacheLoading;
            set => connector.UseCacheLoading = value;
        }
        public bool UseConcurrentLoading
        {
            get => connector.UseConcurrentLoading;
            set => connector.UseConcurrentLoading = value;
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
        public bool DisplayHealths
        {
            get => connector.DisplayHealths;
            set => connector.DisplayHealths = value;
        }

        public bool KeepSidePanelSettings
        {
            get => connector.KeepSidePanelSettings;
            set => connector.KeepSidePanelSettings = value;
        }
        public bool UseRealTimeSearch
        {
            get => connector.UseRealTimeSearch;
            set => connector.UseRealTimeSearch = value;
        }
        public bool UseCaseSensitiveSearch
        {
            get => connector.UseCaseSensitiveSearch;
            set => connector.UseCaseSensitiveSearch = value;
        }
        public bool UseSearchSuggestions
        {
            get => connector.UseSearchSuggestions;
            set => connector.UseSearchSuggestions = value;
        }


        public string Version => VersionData.CurrentVersion;
        public string License => ApplicationLicenseData.ShortCopyright;



        private bool CanOpenSidePanel()
        {
            return !connector.OpenedSidePanel;
        }

        private bool CanCloseSidePanel()
        {
            return connector.OpenedSidePanel;
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

        [RelayCommand]
        private async Task OpenVersionsModalAsync()
        {
            await connector.OpenVersionsModalAsync();
        }

        [RelayCommand]
        private async Task OpenLicensesModalAsync()
        {
            await connector.OpenLicensesModalAsync();
        }


        public override void Update(IEnumerable<UpdateSection> sections) { }
    }
}
