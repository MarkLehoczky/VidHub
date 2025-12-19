using VidHub.Services.Base;
using Windows.Storage;

namespace VidHub.Services.Connectors.Base
{
    public interface ITitleBarConnector : IUpdateService
    {
        bool DisplayDates { get; set; }
        bool DisplayDurations { get; set; }
        bool DisplayHealths { get; set; }
        bool DisplayTitles { get; set; }

        bool DisplayInformationalSystemNotification { get; set; }
        bool DisplaySuccessSystemNotification { get; set; }
        bool DisplayWarningSystemNotification { get; set; }
        bool DisplayErrorSystemNotification { get; set; }

        bool DisplayInformationalBarNotification { get; set; }
        bool DisplaySuccessBarNotification { get; set; }
        bool DisplayWarningBarNotification { get; set; }
        bool DisplayErrorBarNotification { get; set; }

        bool DisabledHealthCheck { get; set; }
        bool ExistenceHealthCheck { get; set; }
        bool QuickHealthCheck { get; set; }
        bool FullHealthCheck { get; set; }

        bool UseCacheLoading { get; set; }
        bool UseCaseSensitiveSearch { get; set; }
        bool UseConcurrentLoading { get; set; }
        bool UseContentHash { get; set; }
        bool UseRealTimeSearch { get; set; }
        bool UseSearchSuggestions { get; set; }
        bool OpenedSidePanel { get; set; }
        bool KeepSidePanelSettings { get; set; }

        Task Export();
        Task Import();
        Task LoadItems(IEnumerable<IStorageItem> items, bool includeSubfolders);
        Task LoadFiles();
        Task LoadFolders(bool includeSubfolders);
        Task OpenDisplayFormatDialog();
        Task OpenLicensesDialog();
        Task OpenPassiveTitleFormatDialog();
        Task OpenPreviewImageFormatDialog();
        Task OpenVersionsDialog();
    }
}
