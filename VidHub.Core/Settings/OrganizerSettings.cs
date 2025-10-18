using System.Text.Json.Serialization;

namespace VidHub.Core.Settings
{
    public class OrganizerSettings
    {
        public GlobalOrganizerSettings Global { get; set; } = new();
        public DisplayOrganizerSettings Display { get; set; } = new();

        [JsonIgnore]
        public StringComparison SearchComparison => Global.EnableCaseSensitiveSearch
            ? StringComparison.Ordinal
            : StringComparison.OrdinalIgnoreCase;


        public bool ValidVideo(Video video)
        {
            if (!string.IsNullOrEmpty(Display.SearchText))
            {
                if (!video.Title.Contains(Display.SearchText, SearchComparison))
                {
                    return false;
                }
            }

            if (Display.FilterDate)
            {
                if (Display.StartDate.HasValue && video.Date < Display.StartDate.Value)
                {
                    return false;
                }
            }

            if (Display.EndDate.HasValue && video.Date > Display.EndDate.Value)
            {
                return false;
            }

            if (Display.FilterDuration)
            {
                if (Display.MinDuration.HasValue && video.Duration < Display.MinDuration.Value)
                {
                    return false;
                }
            }

            return !Display.MaxDuration.HasValue || video.Duration <= Display.MaxDuration.Value;
        }
    }

    public class GlobalOrganizerSettings
    {
        public bool OpenedSidePanel { get; set; } = true;
        public bool SaveOrganizerSettings { get; set; } = true;
        public bool EnableSystemNotification { get; set; } = true;
        public bool EnableCacheLoading { get; set; } = true;
        public bool EnableConcurrentLoading { get; set; } = false;
        public bool EnableLiveSearch { get; set; } = true;
        public bool EnableCaseSensitiveSearch { get; set; } = true;
        public bool EnableSearchSuggestions { get; set; } = true;
    }

    public class DisplayOrganizerSettings
    {
        public string? CurrentSortOption { get; set; } = null;
        public string SearchText { get; set; } = string.Empty;
        public bool FilterDate { get; set; } = false;
        public DateTimeOffset? StartDate { get; set; } = null;
        public DateTimeOffset? EndDate { get; set; } = null;
        public bool FilterDuration { get; set; } = false;
        public TimeSpan? MinDuration { get; set; } = null;
        public TimeSpan? MaxDuration { get; set; } = null;
    }
}
