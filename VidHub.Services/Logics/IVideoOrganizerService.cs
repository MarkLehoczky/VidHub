namespace VidHub.Services.Logics
{
    public interface IVideoOrganizerService
    {
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
