using System.Text.Json;
using VidHub.Core;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.Services.Logics
{
    public class VideoOrganizeService(IMainService service, ISettingsService settings) : IVideoOrganizeService
    {
        private readonly object locker = new();
        private string? currentSortOption = null;
        private string? searchText = null;
        private string activeSearchText = string.Empty;
        private bool filterDate = false;
        private DateTimeOffset? startDate = null;
        private DateTimeOffset? endDate = null;
        private bool filterDuration = false;
        private TimeSpan? minDuration = null;
        private TimeSpan? maxDuration = null;
        private readonly Dictionary<string, Comparer<Video>> sortOptions = new()
            {
                { "Default", Comparer<Video>.Create((x, y) => x.CompareTo(y)) },
                { "▲ Title", Comparer<Video>.Create((x, y) => string.Compare(x.Title, y.Title, StringComparison.OrdinalIgnoreCase)) },
                { "▼ Title", Comparer<Video>.Create((x, y) => string.Compare(y.Title, x.Title, StringComparison.OrdinalIgnoreCase)) },
                { "▲ Date", Comparer<Video>.Create((x, y) => DateTime.Compare(x.Date, y.Date)) },
                { "▼ Date", Comparer<Video>.Create((x, y) => DateTime.Compare(y.Date, x.Date)) },
                { "▲ Duration", Comparer<Video>.Create((x, y) => TimeSpan.Compare(x.Duration, y.Duration)) },
                { "▼ Duration", Comparer<Video>.Create((x, y) => TimeSpan.Compare(y.Duration, x.Duration)) }
            };


        public string? CurrentSortOption
        {
            get => currentSortOption;
            set
            {
                if (currentSortOption == value) return;
                currentSortOption = value;
                UpdateOrganizers();
            }
        }

        public string? SearchText
        {
            get => searchText;
            set
            {
                if (searchText == value) return;
                if (settings.LiveTextFiltering) activeSearchText = value;
                searchText = value;
                UpdateOrganizers();
            }
        }

        public bool FilterDate
        {
            get => filterDate;
            set
            {
                if (filterDate == value) return;
                filterDate = value;
                UpdateOrganizers();
            }
        }
        public DateTimeOffset? StartDate
        {
            get => startDate;
            set
            {
                if (startDate == value) return;
                startDate = value;
                UpdateOrganizers();
            }
        }
        public DateTimeOffset? EndDate
        {
            get => endDate;
            set
            {
                if (endDate == value) return;
                endDate = value;
                UpdateOrganizers();
            }
        }

        public bool FilterDuration
        {
            get => filterDuration;
            set
            {
                if (filterDuration == value) return;
                filterDuration = value;
                UpdateOrganizers();
            }
        }
        public TimeSpan? MinDuration
        {
            get => minDuration;
            set
            {
                if (minDuration == value) return;
                minDuration = value;
                UpdateOrganizers();
            }
        }
        public TimeSpan? MaxDuration
        {
            get => maxDuration;
            set
            {
                if (maxDuration == value) return;
                maxDuration = value;
                UpdateOrganizers();
            }
        }


        public IEnumerable<string> GetSortOptions() => sortOptions.Keys;

        public void UpdateTextFilter(string text)
        {
            activeSearchText = text;
            UpdateOrganizers();
        }


        private void UpdateOrganizers(bool updateUI = true)
        {
            service.Predicate = video =>
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    if (!video.Title.Contains(activeSearchText, settings.CaseSensitiveTextFiltering ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase)) return false;
                }
                if (FilterDate)
                {
                    if (StartDate.HasValue && video.Date < StartDate.Value) return false;
                    if (EndDate.HasValue && video.Date > EndDate.Value) return false;
                }
                if (FilterDuration)
                {
                    if (MinDuration.HasValue && video.Duration < MinDuration.Value) return false;
                    if (MaxDuration.HasValue && video.Duration > MaxDuration.Value) return false;
                }
                return true;
            };

            service.Comparer = sortOptions.GetValueOrDefault(currentSortOption ?? string.Empty, Comparer<Video>.Default);

            if (updateUI)
            {
                service.Update();
            }
        }

        public void Load()
        {
            var appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub");
            var appDataSettings = Path.Combine(appDataDirectory, "VidHub_organizing.json");

            Directory.CreateDirectory(appDataDirectory);

            if (File.Exists(appDataSettings) && settings.KeepFilterStatus)
            {
                string json = File.ReadAllText(appDataSettings);
                var organizer = JsonSerializer.Deserialize<VideoOrganizeLoader>(json);
                Set(organizer);
            }
        }

        public void Save()
        {
            var appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VidHub");
            var appDataSettings = Path.Combine(appDataDirectory, "VidHub_organizing.json");

            Directory.CreateDirectory(appDataDirectory);

            string json = JsonSerializer.Serialize(this);
            File.WriteAllText(appDataSettings, json);
        }

        public void Set(IVideoOrganizeService service)
        {
            currentSortOption = service.CurrentSortOption;
            searchText = service.SearchText;
            filterDate = service.FilterDate;
            startDate = service.StartDate;
            endDate = service.EndDate;
            filterDuration = service.FilterDuration;
            minDuration = service.MinDuration;
            maxDuration = service.MaxDuration;

            UpdateOrganizers(false);
        }

        public IEnumerable<string> Suggestions()
        {
            var startsWith = service.GetAllVideos().Select(v => v.Title).Where(v => v.StartsWith(SearchText ?? string.Empty, settings.CaseSensitiveTextFiltering ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase));
            var contains = service.GetAllVideos().Select(v => v.Title).Except(startsWith).Where(v => v.Contains(SearchText ?? string.Empty, settings.CaseSensitiveTextFiltering ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase));
            return startsWith.Union(contains);
        }
    }
}
