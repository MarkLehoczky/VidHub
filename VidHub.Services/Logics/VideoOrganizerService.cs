using VidHub.Core;
using VidHub.Core.Settings;
using VidHub.Core.Utilities.Helper;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;

namespace VidHub.Services.Logics
{
    public class VideoOrganizerService(IVideoService service, IVidHubSettings settings) : IVideoOrganizerService
    {
        private string localSearchText = string.Empty;
        private readonly Dictionary<string, Comparer<Video>> sortOptions = new()
            {
                { "Default", Comparer<Video>.Default },
                { "▲ Title", Comparer<Video>.Create((x, y) => string.Compare(x.Title, y.Title, StringComparison.OrdinalIgnoreCase)) },
                { "▼ Title", Comparer<Video>.Create((x, y) => string.Compare(y.Title, x.Title, StringComparison.OrdinalIgnoreCase)) },
                { "▲ Date", Comparer<Video>.Create((x, y) => DateTime.Compare(x.Date, y.Date)) },
                { "▼ Date", Comparer<Video>.Create((x, y) => DateTime.Compare(y.Date, x.Date)) },
                { "▲ Duration", Comparer<Video>.Create((x, y) => TimeSpan.Compare(x.Duration, y.Duration)) },
                { "▼ Duration", Comparer<Video>.Create((x, y) => TimeSpan.Compare(y.Duration, x.Duration)) }
            };


        public string? CurrentSortOption
        {
            get => settings.SidePanel.SortBy;
            set
            {
                settings.SidePanel.SortBy = value;
                UpdateOrganizers();
            }
        }

        public string SearchText
        {
            get => settings.SidePanel.SearchText;
            set
            {
                if (settings.SidePanel.UseRealTimeSearch)
                {
                    settings.SidePanel.SearchText = value;
                }

                localSearchText = value;
                UpdateOrganizers();
            }
        }

        public bool FilterDate
        {
            get => settings.SidePanel.FilterDate;
            set
            {
                settings.SidePanel.FilterDate = value;
                UpdateOrganizers();
            }
        }
        public DateTimeOffset? StartDate
        {
            get => settings.SidePanel.StartDate;
            set
            {
                settings.SidePanel.StartDate = value;
                UpdateOrganizers();
            }
        }
        public DateTimeOffset? EndDate
        {
            get => settings.SidePanel.EndDate;
            set
            {
                settings.SidePanel.EndDate = value;
                UpdateOrganizers();
            }
        }

        public bool FilterDuration
        {
            get => settings.SidePanel.FilterDuration;
            set
            {
                settings.SidePanel.FilterDuration = value;
                UpdateOrganizers();
            }
        }
        public TimeSpan? MinDuration
        {
            get => settings.SidePanel.MinDuration;
            set
            {
                settings.SidePanel.MinDuration = value;
                UpdateOrganizers();
            }
        }
        public TimeSpan? MaxDuration
        {
            get => settings.SidePanel.MaxDuration;
            set
            {
                settings.SidePanel.MaxDuration = value;
                UpdateOrganizers();
            }
        }


        public IEnumerable<string> GetSortOptions()
        {
            return sortOptions.Keys;
        }

        public void UpdateSearchText()
        {
            settings.SidePanel.SearchText = localSearchText;
            UpdateOrganizers();
        }


        private void UpdateOrganizers()
        {
            service.Predicate = settings.ValidVideo;
            service.Comparer = sortOptions.GetValueOrDefault(settings.SidePanel.SortBy ?? string.Empty, Comparer<Video>.Default);
            service.Update(UpdateType.UPDATEVIDEOCOLLECTION);
        }

        public IEnumerable<string> Suggestions()
        {
            if (settings.SidePanel.UseSearchSuggestions)
            {
                IEnumerable<string> startsWith = service.Select(v => v.Title).Where(v => v.StartsWith(SearchText, settings.SearchComparison()));
                IEnumerable<string> contains = service.Select(v => v.Title).Except(startsWith).Where(v => v.Contains(SearchText, settings.SearchComparison()));
                return startsWith.Union(contains);
            }

            return [];
        }

    }
}
