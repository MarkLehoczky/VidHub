using CommunityToolkit.Mvvm.Input;
using VidHub.Core.Data;
using VidHub.Core.Utilities;
using VidHub.Platform.Environment;
using VidHub.Services.Connectors.Base;

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
        public bool UseContentHash
        {
            get => connector.UseContentHash;
            set => connector.UseContentHash = value;
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
        public bool DisplayResolutions
        {
            get => connector.DisplayResolutions;
            set => connector.DisplayResolutions = value;
        }
        public bool DisplayFramerates
        {
            get => connector.DisplayFramerates;
            set => connector.DisplayFramerates = value;
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
        public string License => ApplicationLicenseData.Copyright;


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
            await connector.LoadFiles();
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task LoadSingleFolderAsync()
        {
            await connector.LoadFolders(false);
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task LoadAllFolderAsync()
        {
            await connector.LoadFolders(true);
        }

        [RelayCommand]
        private async Task ImportAsync()
        {
            await connector.Import();
        }

        [RelayCommand]
        private async Task ExportAsync()
        {
            await connector.Export();
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
        private async Task OpenDisplayFormatDialogAsync()
        {
            await connector.OpenDisplayFormatDialog();
        }

        [RelayCommand]
        private async Task OpenPassiveTitleFormatDialogAsync()
        {
            await connector.OpenPassiveTitleFormatDialog();
        }

        [RelayCommand]
        private async Task OpenPreviewImageFormatDialogAsync()
        {
            await connector.OpenPreviewImageFormatDialog();
        }

        [RelayCommand]
        private async Task OpenVersionsDialogAsync()
        {
            await connector.OpenVersionsDialog();
        }

        [RelayCommand]
        private async Task OpenLicensesDialogAsync()
        {
            await connector.OpenLicensesDialog();
        }


        public override void Update(IEnumerable<UpdateSection> sections) { }
    }
}
