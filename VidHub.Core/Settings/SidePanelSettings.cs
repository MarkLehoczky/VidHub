namespace VidHub.Core.Settings
{
    public class SidePanelSettings
    {
        public string? SortBy { get; set; } = null;
        public string Orientation { get; set; } = "ASC";
        public bool UseRealTimeSearch { get; set; } = true;
        public bool UseCaseSensitiveSearch { get; set; } = true;
        public bool UseSearchSuggestions { get; set; } = true;
        public string SearchText { get; set; } = string.Empty;
        public bool FilterDate { get; set; } = false;
        public DateTimeOffset? StartDate { get; set; } = null;
        public DateTimeOffset? EndDate { get; set; } = null;
        public bool FilterDuration { get; set; } = false;
        public TimeSpan? MinDuration { get; set; } = null;
        public TimeSpan? MaxDuration { get; set; } = null;
        public bool DisplayMaximumResolutionVideos { get; set; } = false;
        public bool DisplayLargeResolutionVideos { get; set; } = false;
        public bool DisplayMediumResolutionVideos { get; set; } = false;
        public bool DisplayLowResolutionVideos { get; set; } = false;
        public bool DisplayMinimumResolutionVideos { get; set; } = false;
        public bool DisplayMaximumFramerateVideos { get; set; } = false;
        public bool DisplayLargeFramerateVideos { get; set; } = false;
        public bool DisplayMediumFramerateVideos { get; set; } = false;
        public bool DisplayLowFramerateVideos { get; set; } = false;
        public bool DisplayMinimumFramerateVideos { get; set; } = false;
    }
}
