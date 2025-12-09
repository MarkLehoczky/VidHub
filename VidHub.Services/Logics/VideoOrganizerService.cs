using VidHub.Core;
using VidHub.Core.Enums;
using VidHub.Core.Settings;
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
            get => settings.Organizer.Display.CurrentSortOption;
            set
            {
                settings.Organizer.Display.CurrentSortOption = value;
                UpdateOrganizers();
            }
        }

        public string SearchText
        {
            get => settings.Organizer.Display.SearchText;
            set
            {
                if (settings.Organizer.Global.EnableLiveSearch)
                {
                    settings.Organizer.Display.SearchText = value;
                }

                localSearchText = value;
                UpdateOrganizers();
            }
        }

        public bool FilterDate
        {
            get => settings.Organizer.Display.FilterDate;
            set
            {
                settings.Organizer.Display.FilterDate = value;
                UpdateOrganizers();
            }
        }
        public DateTimeOffset? StartDate
        {
            get => settings.Organizer.Display.StartDate;
            set
            {
                settings.Organizer.Display.StartDate = value;
                UpdateOrganizers();
            }
        }
        public DateTimeOffset? EndDate
        {
            get => settings.Organizer.Display.EndDate;
            set
            {
                settings.Organizer.Display.EndDate = value;
                UpdateOrganizers();
            }
        }

        public bool FilterDuration
        {
            get => settings.Organizer.Display.FilterDuration;
            set
            {
                settings.Organizer.Display.FilterDuration = value;
                UpdateOrganizers();
            }
        }
        public TimeSpan? MinDuration
        {
            get => settings.Organizer.Display.MinDuration;
            set
            {
                settings.Organizer.Display.MinDuration = value;
                UpdateOrganizers();
            }
        }
        public TimeSpan? MaxDuration
        {
            get => settings.Organizer.Display.MaxDuration;
            set
            {
                settings.Organizer.Display.MaxDuration = value;
                UpdateOrganizers();
            }
        }


        public IEnumerable<string> GetSortOptions()
        {
            return sortOptions.Keys;
        }

        public void UpdateSearchText()
        {
            settings.Organizer.Display.SearchText = localSearchText;
            UpdateOrganizers();
        }


        private void UpdateOrganizers()
        {
            service.Predicate = settings.Organizer.ValidVideo;
            service.Comparer = sortOptions.GetValueOrDefault(settings.Organizer.Display.CurrentSortOption ?? string.Empty, Comparer<Video>.Default);
            service.Update(UpdateType.UPDATEVIDEOCOLLECTION);
        }

        public IEnumerable<string> Suggestions()
        {
            if (settings.Organizer.Global.EnableSearchSuggestions)
            {
                IEnumerable<string> startsWith = service.Select(v => v.Title).Where(v => v.StartsWith(SearchText, settings.Organizer.SearchComparison));
                IEnumerable<string> contains = service.Select(v => v.Title).Except(startsWith).Where(v => v.Contains(SearchText, settings.Organizer.SearchComparison));
                return startsWith.Union(contains);
            }

            return [];
        }

    }
}
