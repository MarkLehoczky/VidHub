using Microsoft.Extensions.Logging;
using VidHub.Core;
using VidHub.Core.Settings;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Base;

namespace VidHub.Services.Logics
{
    public class VideoOrganizerService(IVideoService service, IVidHubSettings settings) : IVideoOrganizerService
    {
        private readonly ILogger logger = VidHubContext.Logger;
        private string localSearchText = string.Empty;
        private readonly Dictionary<string, Comparer<Video>> sortOptions = new()
            {
                { "Default", Comparer<Video>.Default },
                { "Title", Comparer<Video>.Create((x, y) => string.Compare(x.Title, y.Title, StringComparison.OrdinalIgnoreCase)) },
                { "Date", Comparer<Video>.Create((x, y) => DateTime.Compare(x.Date, y.Date)) },
                { "Duration", Comparer<Video>.Create((x, y) => TimeSpan.Compare(x.Duration, y.Duration)) },
                { "Resolution", Comparer<Video>.Create((x, y) =>
                {
                    int left = x.Metadata.DefaultVideoStream?.Resolution.Value ?? -1;
                    int right = y.Metadata.DefaultVideoStream?.Resolution.Value ?? -1;
                    return left.CompareTo(right);
                }) },
                { "Framerate", Comparer<Video>.Create((x, y) =>
                {
                    double left = x.Metadata.DefaultVideoStream?.Framerate.Value ?? -1;
                    double right = y.Metadata.DefaultVideoStream?.Framerate.Value ?? -1;
                    return left.CompareTo(right);
                }) },
            };


        public string? SortBy
        {
            get => settings.SidePanel.SortBy;
            set
            {
                logger.LogDebug("SortBy property changed to: {SortBy}", value);
                settings.SidePanel.SortBy = value;
                UpdateDisplayedVideos();
            }
        }

        public string Orientation
        {
            get => settings.SidePanel.Orientation;
            set
            {
                logger.LogDebug("Orientation property changed to: {Orientation}", value);
                settings.SidePanel.Orientation = value;
                service.Update(UpdateSection.FILTERPANEL);
                UpdateDisplayedVideos();
            }
        }

        public string SearchText
        {
            get => settings.SidePanel.SearchText;
            set
            {
                logger.LogTrace("SearchText property changed to: {SearchText}", value);
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
                logger.LogDebug("FilterDate property changed to: {FilterDate}", value);
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
                logger.LogDebug("StartDate property changed to: {StartDate}", value);
                settings.SidePanel.StartDate = value;
                UpdateDisplayedVideos();
            }
        }
        public DateTimeOffset? EndDate
        {
            get => settings.SidePanel.EndDate;
            set
            {
                logger.LogDebug("EndDate property changed to: {EndDate}", value);
                settings.SidePanel.EndDate = value;
                UpdateDisplayedVideos();
            }
        }

        public bool FilterDuration
        {
            get => settings.SidePanel.FilterDuration;
            set
            {
                logger.LogDebug("FilterDuration property changed to: {FilterDuration}", value);
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
                logger.LogDebug("MinDuration property changed to: {MinDuration}", value);
                settings.SidePanel.MinDuration = value;
                UpdateDisplayedVideos();
            }
        }
        public TimeSpan? MaxDuration
        {
            get => settings.SidePanel.MaxDuration;
            set
            {
                logger.LogDebug("MaxDuration property changed to: {MaxDuration}", value);
                settings.SidePanel.MaxDuration = value;
                UpdateDisplayedVideos();
            }
        }

        public bool DisplayMaximumResolutionVideos
        {
            get => settings.SidePanel.DisplayMaximumResolutionVideos;
            set
            {
                logger.LogDebug("DisplayMaximumResolutionVideos property changed to: {Value}", value);
                settings.SidePanel.DisplayMaximumResolutionVideos = value;
                UpdateDisplayedVideos();
            }
        }
        public bool DisplayLargeResolutionVideos
        {
            get => settings.SidePanel.DisplayLargeResolutionVideos;
            set
            {
                logger.LogDebug("DisplayLargeResolutionVideos property changed to: {Value}", value);
                settings.SidePanel.DisplayLargeResolutionVideos = value;
                UpdateDisplayedVideos();
            }
        }
        public bool DisplayMediumResolutionVideos
        {
            get => settings.SidePanel.DisplayMediumResolutionVideos;
            set
            {
                logger.LogDebug("DisplayMediumResolutionVideos property changed to: {Value}", value);
                settings.SidePanel.DisplayMediumResolutionVideos = value;
                UpdateDisplayedVideos();
            }
        }
        public bool DisplayLowResolutionVideos
        {
            get => settings.SidePanel.DisplayLowResolutionVideos;
            set
            {
                logger.LogDebug("DisplayLowResolutionVideos property changed to: {Value}", value);
                settings.SidePanel.DisplayLowResolutionVideos = value;
                UpdateDisplayedVideos();
            }
        }
        public bool DisplayMinimumResolutionVideos
        {
            get => settings.SidePanel.DisplayMinimumResolutionVideos;
            set
            {
                logger.LogDebug("DisplayMinimumResolutionVideos property changed to: {Value}", value);
                settings.SidePanel.DisplayMinimumResolutionVideos = value;
                UpdateDisplayedVideos();
            }
        }
        public bool DisplayMaximumFramerateVideos
        {
            get => settings.SidePanel.DisplayMaximumFramerateVideos;
            set
            {
                logger.LogDebug("DisplayMaximumFramerateVideos property changed to: {Value}", value);
                settings.SidePanel.DisplayMaximumFramerateVideos = value;
                UpdateDisplayedVideos();
            }
        }
        public bool DisplayLargeFramerateVideos
        {
            get => settings.SidePanel.DisplayLargeFramerateVideos;
            set
            {
                logger.LogDebug("DisplayLargeFramerateVideos property changed to: {Value}", value);
                settings.SidePanel.DisplayLargeFramerateVideos = value;
                UpdateDisplayedVideos();
            }
        }
        public bool DisplayMediumFramerateVideos
        {
            get => settings.SidePanel.DisplayMediumFramerateVideos;
            set
            {
                logger.LogDebug("DisplayMediumFramerateVideos property changed to: {Value}", value);
                settings.SidePanel.DisplayMediumFramerateVideos = value;
                UpdateDisplayedVideos();
            }
        }
        public bool DisplayLowFramerateVideos
        {
            get => settings.SidePanel.DisplayLowFramerateVideos;
            set
            {
                logger.LogDebug("DisplayLowFramerateVideos property changed to: {Value}", value);
                settings.SidePanel.DisplayLowFramerateVideos = value;
                UpdateDisplayedVideos();
            }
        }
        public bool DisplayMinimumFramerateVideos
        {
            get => settings.SidePanel.DisplayMinimumFramerateVideos;
            set
            {
                logger.LogDebug("DisplayMinimumFramerateVideos property changed to: {Value}", value);
                settings.SidePanel.DisplayMinimumFramerateVideos = value;
                UpdateDisplayedVideos();
            }
        }


        public IEnumerable<string> GetSortOptions()
        {
            logger.LogTrace("GetSortOptions called, returning {Count} options", sortOptions.Count);
            return sortOptions.Keys;
        }

        public IEnumerable<string> GetSearchSuggestions()
        {
            logger.LogTrace("GetSearchSuggestions called with SearchText={SearchText}", SearchText);
            if (settings.SidePanel.UseSearchSuggestions)
            {
                IEnumerable<string> startsWith = service.Select(v => v.Title).Where(v => v.StartsWith(SearchText, settings.SearchComparison()));
                IEnumerable<string> contains = service.Select(v => v.Title).Except(startsWith).Where(v => v.Contains(SearchText, settings.SearchComparison()));
                IEnumerable<string> result = startsWith.Union(contains);
                logger.LogDebug("Search suggestions generated: {Count} results for SearchText={SearchText}", result.Count(), SearchText);
                return result;
            }

            logger.LogTrace("Search suggestions disabled");
            return [];
        }

        public void UpdateSearchText()
        {
            logger.LogDebug("UpdateSearchText called, updating settings with localSearchText={SearchText}", localSearchText);
            settings.SidePanel.SearchText = localSearchText;
            UpdateDisplayedVideos();
        }


        private void UpdateDisplayedVideos()
        {
            logger.LogTrace("UpdateDisplayedVideos called");
            Comparer<Video> selectedComparer = sortOptions.TryGetValue(settings.SidePanel.SortBy ?? string.Empty, out Comparer<Video>? comparer) ? comparer : Comparer<Video>.Default;
            Comparer<Video> tempComparer = selectedComparer;
            if (Orientation != "ASC")
            {
                selectedComparer = Comparer<Video>.Create((x, y) => tempComparer.Compare(y, x));
            }
            service.Comparer = selectedComparer;
            service.Update(UpdateSection.VIDEOCOLLECTION);
            logger.LogDebug("Video collection updated with SortBy={SortBy}, Orientation={Orientation}", settings.SidePanel.SortBy, Orientation);
        }
    }
}
