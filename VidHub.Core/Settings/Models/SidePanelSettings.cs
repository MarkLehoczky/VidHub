namespace VidHub.Core.Settings.Models
{
    public class SidePanelSettings
    {
        public string? SortBy { get; set; } = null;
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
    }
}
