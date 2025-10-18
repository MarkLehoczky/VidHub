using VidHub.Services.Base.Interfaces;

namespace VidHub.Services.Connectors.Base.Interfaces
{
    public interface ISidePanelConnector : IUpdateService
    {
        string? CurrentSortOption { get; set; }
        bool EnableLiveSearch { get; }
        DateTimeOffset? EndDate { get; set; }
        string SearchText { get; set; }
        bool FilterDate { get; set; }
        bool FilterDuration { get; set; }
        bool HasActiveTransfer { get; }
        int LoadedFileCount { get; }
        TimeSpan? MaxDuration { get; set; }
        TimeSpan? MinDuration { get; set; }
        bool OpenedSidePanel { get; }
        DateTimeOffset? StartDate { get; set; }
        int TotalFileCount { get; }
        string TransferDescription { get; }
        IEnumerable<string> GetSortOptions();
        IEnumerable<string> Suggestions();
        void UpdateSearchText();
    }
}
