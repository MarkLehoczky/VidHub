using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using VidHub.Core.Data;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Connectors.Base;

namespace VidHub.ViewModels
{
    public partial class TitleBarViewModel(ITitleBarConnector connector) : ViewModelTemplate(connector)
    {
        private new readonly ILogger logger = VidHubContext.Logger;

        public TitleBarViewModel() : this(VidHubContext.Host.GetService<ITitleBarConnector>()) { }


        public bool DisplayInformationalSystemNotification
        {
            get => connector.DisplayInformationalSystemNotification;
            set { connector.DisplayInformationalSystemNotification = value; logger.LogDebug("DisplayInformationalSystemNotification set to {Value}", value); }
        }
        public bool DisplaySuccessSystemNotification
        {
            get => connector.DisplaySuccessSystemNotification;
            set { connector.DisplaySuccessSystemNotification = value; logger.LogDebug("DisplaySuccessSystemNotification set to {Value}", value); }
        }
        public bool DisplayWarningSystemNotification
        {
            get => connector.DisplayWarningSystemNotification;
            set { connector.DisplayWarningSystemNotification = value; logger.LogDebug("DisplayWarningSystemNotification set to {Value}", value); }
        }
        public bool DisplayErrorSystemNotification
        {
            get => connector.DisplayErrorSystemNotification;
            set { connector.DisplayErrorSystemNotification = value; logger.LogDebug("DisplayErrorSystemNotification set to {Value}", value); }
        }

        public bool DisplayInformationalBarNotification
        {
            get => connector.DisplayInformationalBarNotification;
            set { connector.DisplayInformationalBarNotification = value; logger.LogDebug("DisplayInformationalBarNotification set to {Value}", value); }
        }
        public bool DisplaySuccessBarNotification
        {
            get => connector.DisplaySuccessBarNotification;
            set { connector.DisplaySuccessBarNotification = value; logger.LogDebug("DisplaySuccessBarNotification set to {Value}", value); }
        }
        public bool DisplayWarningBarNotification
        {
            get => connector.DisplayWarningBarNotification;
            set { connector.DisplayWarningBarNotification = value; logger.LogDebug("DisplayWarningBarNotification set to {Value}", value); }
        }
        public bool DisplayErrorBarNotification
        {
            get => connector.DisplayErrorBarNotification;
            set { connector.DisplayErrorBarNotification = value; logger.LogDebug("DisplayErrorBarNotification set to {Value}", value); }
        }

        public bool DisabledHealthCheck
        {
            get => connector.DisabledHealthCheck;
            set { connector.DisabledHealthCheck = value; logger.LogDebug("DisabledHealthCheck set to {Value}", value); }
        }
        public bool ExistenceHealthCheck
        {
            get => connector.ExistenceHealthCheck;
            set { connector.ExistenceHealthCheck = value; logger.LogDebug("ExistenceHealthCheck set to {Value}", value); }
        }
        public bool QuickHealthCheck
        {
            get => connector.QuickHealthCheck;
            set { connector.QuickHealthCheck = value; logger.LogDebug("QuickHealthCheck set to {Value}", value); }
        }
        public bool FullHealthCheck
        {
            get => connector.FullHealthCheck;
            set { connector.FullHealthCheck = value; logger.LogDebug("FullHealthCheck set to {Value}", value); }
        }

        public bool UseCacheLoading
        {
            get => connector.UseCacheLoading;
            set { connector.UseCacheLoading = value; logger.LogDebug("UseCacheLoading set to {Value}", value); }
        }
        public bool UseConcurrentLoading
        {
            get => connector.UseConcurrentLoading;
            set { connector.UseConcurrentLoading = value; logger.LogDebug("UseConcurrentLoading set to {Value}", value); }
        }
        public bool UseContentHash
        {
            get => connector.UseContentHash;
            set { connector.UseContentHash = value; logger.LogDebug("UseContentHash set to {Value}", value); }
        }

        public bool DisplayTitles
        {
            get => connector.DisplayTitles;
            set { connector.DisplayTitles = value; logger.LogDebug("DisplayTitles set to {Value}", value); }
        }
        public bool DisplayDates
        {
            get => connector.DisplayDates;
            set { connector.DisplayDates = value; logger.LogDebug("DisplayDates set to {Value}", value); }
        }
        public bool DisplayDurations
        {
            get => connector.DisplayDurations;
            set { connector.DisplayDurations = value; logger.LogDebug("DisplayDurations set to {Value}", value); }
        }
        public bool DisplayResolutions
        {
            get => connector.DisplayResolutions;
            set { connector.DisplayResolutions = value; logger.LogDebug("DisplayResolutions set to {Value}", value); }
        }
        public bool DisplayFramerates
        {
            get => connector.DisplayFramerates;
            set { connector.DisplayFramerates = value; logger.LogDebug("DisplayFramerates set to {Value}", value); }
        }
        public bool DisplayHealths
        {
            get => connector.DisplayHealths;
            set { connector.DisplayHealths = value; logger.LogDebug("DisplayHealths set to {Value}", value); }
        }

        public bool KeepSidePanelSettings
        {
            get => connector.KeepSidePanelSettings;
            set { connector.KeepSidePanelSettings = value; logger.LogDebug("KeepSidePanelSettings set to {Value}", value); }
        }
        public bool UseRealTimeSearch
        {
            get => connector.UseRealTimeSearch;
            set { connector.UseRealTimeSearch = value; logger.LogDebug("UseRealTimeSearch set to {Value}", value); }
        }
        public bool UseCaseSensitiveSearch
        {
            get => connector.UseCaseSensitiveSearch;
            set { connector.UseCaseSensitiveSearch = value; logger.LogDebug("UseCaseSensitiveSearch set to {Value}", value); }
        }
        public bool UseSearchSuggestions
        {
            get => connector.UseSearchSuggestions;
            set { connector.UseSearchSuggestions = value; logger.LogDebug("UseSearchSuggestions set to {Value}", value); }
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
            logger.LogTrace("LoadFilesAsync invoked");
            await connector.LoadFiles();
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task LoadSingleFolderAsync()
        {
            logger.LogTrace("LoadSingleFolderAsync invoked");
            await connector.LoadFolders(false);
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task LoadAllFolderAsync()
        {
            logger.LogTrace("LoadAllFolderAsync invoked");
            await connector.LoadFolders(true);
        }

        [RelayCommand]
        private async Task ImportAsync()
        {
            logger.LogTrace("ImportAsync invoked");
            await connector.Import();
        }

        [RelayCommand]
        private async Task ExportAsync()
        {
            logger.LogTrace("ExportAsync invoked");
            await connector.Export();
        }


        [RelayCommand(CanExecute = nameof(CanOpenSidePanel))]
        private void OpenSidePanel()
        {
            connector.OpenedSidePanel = true;
            OpenSidePanelCommand.NotifyCanExecuteChanged();
            CloseSidePanelCommand.NotifyCanExecuteChanged();
            logger.LogDebug("OpenSidePanel executed");
        }

        [RelayCommand(CanExecute = nameof(CanCloseSidePanel))]
        private void CloseSidePanel()
        {
            connector.OpenedSidePanel = false;
            OpenSidePanelCommand.NotifyCanExecuteChanged();
            CloseSidePanelCommand.NotifyCanExecuteChanged();
            logger.LogDebug("CloseSidePanel executed");
        }

        [RelayCommand]
        private async Task OpenDisplayFormatDialogAsync()
        {
            logger.LogTrace("OpenDisplayFormatDialogAsync invoked");
            await connector.OpenDisplayFormatDialog();
        }

        [RelayCommand]
        private async Task OpenPassiveTitleFormatDialogAsync()
        {
            logger.LogTrace("OpenPassiveTitleFormatDialogAsync invoked");
            await connector.OpenPassiveTitleFormatDialog();
        }

        [RelayCommand]
        private async Task OpenPreviewImageFormatDialogAsync()
        {
            logger.LogTrace("OpenPreviewImageFormatDialogAsync invoked");
            await connector.OpenPreviewImageFormatDialog();
        }

        [RelayCommand]
        private async Task OpenVersionsDialogAsync()
        {
            logger.LogTrace("OpenVersionsDialogAsync invoked");
            await connector.OpenVersionsDialog();
        }

        [RelayCommand]
        private async Task OpenLicensesDialogAsync()
        {
            logger.LogTrace("OpenLicensesDialogAsync invoked");
            await connector.OpenLicensesDialog();
        }


        public override void Update(IEnumerable<UpdateSection> sections) { }
    }
}
