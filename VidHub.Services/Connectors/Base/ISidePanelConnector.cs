using VidHub.Services.Base;

namespace VidHub.Services.Connectors.Base
{
    public interface ISidePanelConnector : IUpdateService
    {
        bool OpenedSidePanel { get; }
        string? SortBy { get; set; }
        string Orientation { get; set; }
        string SearchText { get; set; }
        bool UseRealTimeSearch { get; }
        bool FilterDate { get; set; }
        DateTimeOffset? StartDate { get; set; }
        DateTimeOffset? EndDate { get; set; }
        bool FilterDuration { get; set; }
        TimeSpan? MaxDuration { get; set; }
        TimeSpan? MinDuration { get; set; }
        bool HasActiveTransfer { get; }
        int LoadedFileCount { get; }
        int TotalFileCount { get; }
        string TransferDescription { get; }

        void ChangeOrientation();
        IEnumerable<string> GetSearchSuggestions();
        IEnumerable<string> GetSortOptions();
        void UpdateSearchText();
    }
}
