namespace VidHub.Services.Logics.Interfaces
{
    public interface IVideoOrganizerService
    {
        string? CurrentSortOption { get; set; }
        DateTimeOffset? EndDate { get; set; }
        bool FilterDate { get; set; }
        bool FilterDuration { get; set; }
        TimeSpan? MaxDuration { get; set; }
        TimeSpan? MinDuration { get; set; }
        string SearchText { get; set; }
        DateTimeOffset? StartDate { get; set; }
        IEnumerable<string> GetSortOptions();
        IEnumerable<string> Suggestions();
        void UpdateSearchText();
    }
}
