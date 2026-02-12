using System.Collections.ObjectModel;
using VidHub.Core.Models;
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
        bool FilterResolution { get; set; }
        bool DisplayMaximumResolutionVideos { get; set; }
        bool DisplayLargeResolutionVideos { get; set; }
        bool DisplayMediumResolutionVideos { get; set; }
        bool DisplayLowResolutionVideos { get; set; }
        bool FilterFramerate { get; set; }
        bool DisplayMinimumResolutionVideos { get; set; }
        bool DisplayMaximumFramerateVideos { get; set; }
        bool DisplayLargeFramerateVideos { get; set; }
        bool DisplayMediumFramerateVideos { get; set; }
        bool DisplayLowFramerateVideos { get; set; }
        bool DisplayMinimumFramerateVideos { get; set; }
        bool FilterTags { get; set; }
        ObservableCollection<Tag> Tags { get; }
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
