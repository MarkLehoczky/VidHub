using VidHub.Services.Base.Interfaces;
using Windows.Storage;

namespace VidHub.Services.Connectors.Base.Interfaces
{
    public interface ITitleBarConnector : IUpdateService
    {
        bool DisplayDates { get; set; }
        bool DisplayDurations { get; set; }
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

        bool EnableCacheLoading { get; set; }
        bool EnableCaseSensitiveSearch { get; set; }
        bool EnableConcurrentLoading { get; set; }
        bool EnableLiveSearch { get; set; }
        bool EnableSearchSuggestions { get; set; }
        bool OpenedSidePanel { get; set; }
        bool SaveOrganizerSettings { get; set; }
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
