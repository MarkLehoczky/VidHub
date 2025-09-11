namespace VidHub.Services.Logics.Interfaces
{
    public interface IVideoOrganizeService
    {
        string? CurrentSortOption { get; set; }
        string? SearchText { get; set; }
        bool FilterDate { get; set; }
        DateTimeOffset? StartDate { get; set; }
        DateTimeOffset? EndDate { get; set; }
        bool FilterDuration { get; set; }
        TimeSpan? MinDuration { get; set; }
        TimeSpan? MaxDuration { get; set; }
        IEnumerable<string> GetSortOptions();
        void UpdateTextFilter(string text);
        void Load();
        void Save();
        void Set(IVideoOrganizeService service);
        IEnumerable<string> Suggestions();
    }
}
