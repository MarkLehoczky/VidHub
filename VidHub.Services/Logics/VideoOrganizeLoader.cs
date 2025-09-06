using VidHub.Services.Logics.Interfaces;

namespace VidHub.Services.Logics
{
    internal class VideoOrganizeLoader : IVideoOrganizeService
    {
        public string? CurrentSortOption { get; set; }
        public string? SearchText { get; set; }
        public bool FilterDate { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool FilterDuration { get; set; }
        public TimeSpan? MinDuration { get; set; }
        public TimeSpan? MaxDuration { get; set; }

        public IEnumerable<string> GetSortOptions()
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Set(IVideoOrganizeService service)
        {
            throw new NotImplementedException();
        }

        public void UpdateTextFilter(string text)
        {
            throw new NotImplementedException();
        }
    }
}
