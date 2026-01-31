namespace VidHub.Services.Logics
{
    public interface IVideoOrganizerService
    {
        bool DisplayMaximumResolutionVideos { get; set; }
        bool DisplayLargeResolutionVideos { get; set; }
        bool DisplayMediumResolutionVideos { get; set; }
        bool DisplayLowResolutionVideos { get; set; }
        bool DisplayMinimumResolutionVideos { get; set; }
        bool DisplayMaximumFramerateVideos { get; set; }
        bool DisplayLargeFramerateVideos { get; set; }
        bool DisplayMediumFramerateVideos { get; set; }
        bool DisplayLowFramerateVideos { get; set; }
        bool DisplayMinimumFramerateVideos { get; set; }
        DateTimeOffset? EndDate { get; set; }
        bool FilterDate { get; set; }
        bool FilterDuration { get; set; }
        TimeSpan? MaxDuration { get; set; }
        TimeSpan? MinDuration { get; set; }
        string Orientation { get; set; }
        string SearchText { get; set; }
        string? SortBy { get; set; }
        DateTimeOffset? StartDate { get; set; }

        IEnumerable<string> GetSearchSuggestions();
        IEnumerable<string> GetSortOptions();
        void UpdateSearchText();
    }
}
