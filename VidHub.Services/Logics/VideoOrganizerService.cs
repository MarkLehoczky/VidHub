using VidHub.Core;
using VidHub.Core.Settings;
using VidHub.Core.Utilities;
using VidHub.Services.Base;

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


        public string? SortBy
        {
            get => settings.SidePanel.SortBy;
            set
            {
                settings.SidePanel.SortBy = value;
                UpdateDisplayedVideos();
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
                UpdateDisplayedVideos();
            }
        }

        public bool FilterDate
        {
            get => settings.SidePanel.FilterDate;
            set
            {
                settings.SidePanel.FilterDate = value;
                service.Update(UpdateSection.FILTERPANEL);
                UpdateDisplayedVideos();
            }
        }
        public DateTimeOffset? StartDate
        {
            get => settings.SidePanel.StartDate;
            set
            {
                settings.SidePanel.StartDate = value;
                UpdateDisplayedVideos();
            }
        }
        public DateTimeOffset? EndDate
        {
            get => settings.SidePanel.EndDate;
            set
            {
                settings.SidePanel.EndDate = value;
                UpdateDisplayedVideos();
            }
        }

        public bool FilterDuration
        {
            get => settings.SidePanel.FilterDuration;
            set
            {
                settings.SidePanel.FilterDuration = value;
                service.Update(UpdateSection.FILTERPANEL);
                UpdateDisplayedVideos();
            }
        }
        public TimeSpan? MinDuration
        {
            get => settings.SidePanel.MinDuration;
            set
            {
                settings.SidePanel.MinDuration = value;
                UpdateDisplayedVideos();
            }
        }
        public TimeSpan? MaxDuration
        {
            get => settings.SidePanel.MaxDuration;
            set
            {
                settings.SidePanel.MaxDuration = value;
                UpdateDisplayedVideos();
            }
        }


        public IEnumerable<string> GetSortOptions()
        {
            return sortOptions.Keys;
        }

        public IEnumerable<string> GetSearchSuggestions()
        {
            if (settings.SidePanel.UseSearchSuggestions)
            {
                IEnumerable<string> startsWith = service.Select(v => v.Title).Where(v => v.StartsWith(SearchText, settings.SearchComparison()));
                IEnumerable<string> contains = service.Select(v => v.Title).Except(startsWith).Where(v => v.Contains(SearchText, settings.SearchComparison()));
                return startsWith.Union(contains);
            }

            return [];
        }

        public void UpdateSearchText()
        {
            settings.SidePanel.SearchText = localSearchText;
            UpdateDisplayedVideos();
        }


        private void UpdateDisplayedVideos()
        {
            service.Predicate = settings.ValidVideo;
            service.Comparer = sortOptions.TryGetValue(settings.SidePanel.SortBy ?? string.Empty, out Comparer<Video>? comparer) ? comparer : Comparer<Video>.Default;
            service.Update(UpdateSection.VIDEOCOLLECTION);
        }
    }
}
