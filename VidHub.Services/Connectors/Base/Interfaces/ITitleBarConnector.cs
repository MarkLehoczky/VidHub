using VidHub.Services.Base.Interfaces;
using Windows.Storage;

namespace VidHub.Services.Connectors.Base.Interfaces
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
        bool UseRealTimeSearch { get; set; }
        bool UseSearchSuggestions { get; set; }
        bool OpenedSidePanel { get; set; }
        bool KeepSidePanelSettings { get; set; }
        Task CustomizeVideoDisplayingAsync();
        Task CustomizeVideoLoadingAsync();
        Task CustomizeVideoPreviewImageAsync();
        Task ExportCollectionAsync();
        Task ImportCollectionAsync();
        Task LoadItems(IEnumerable<IStorageItem> items, bool includeSubfolders);
        Task LoadFilesAsync();
        Task LoadFoldersAsync(bool includeSubfolders);
        Task OpenVersionsModalAsync();
        Task OpenLicensesModalAsync();
    }
}
