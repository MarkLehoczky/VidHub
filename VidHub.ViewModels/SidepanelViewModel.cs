using CommunityToolkit.Mvvm.Input;
using VidHub.Core.Utilities;
using VidHub.Services.Connectors.Base;
using Microsoft.Extensions.Logging;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.ViewModels
{
    public partial class SidePanelViewModel(ISidePanelConnector connector) : ViewModelTemplate(connector)
    {
        private readonly ILogger logger = VidHubContext.Logger;
        public SidePanelViewModel() : this(VidHubContext.Host.GetService<ISidePanelConnector>()) { }


        public bool OpenPanel => connector.OpenedSidePanel;

        public IEnumerable<string> SortOptions => connector.GetSortOptions();
        public string? SortBy
        {
            get => connector.SortBy;
            set { connector.SortBy = value; logger.LogDebug("SortBy set in viewmodel to {SortBy}", value); }
        }
        public string Orientation => connector.Orientation == "ASC" ? "▲" : "▼";

        public string SearchText
        {
            get => connector.SearchText;
            set
            {
                connector.SearchText = value;
                OnPropertyChanged(nameof(SearchSuggestions));
                logger.LogTrace("SearchText updated in viewmodel");
            }
        }
        public bool UseRealTimeSearch => connector.UseRealTimeSearch;
        public IEnumerable<string> SearchSuggestions => connector.GetSearchSuggestions();

        public bool FilterDate
        {
            get => connector.FilterDate;
            set { connector.FilterDate = value; logger.LogDebug("FilterDate set in viewmodel to {Value}", value); }
        }
        public DateTimeOffset? StartDate
        {
            get => connector.StartDate;
            set { connector.StartDate = value; logger.LogDebug("StartDate set in viewmodel to {Date}", value); }
        }
        public DateTimeOffset? EndDate
        {
            get => connector.EndDate;
            set { connector.EndDate = value; logger.LogDebug("EndDate set in viewmodel to {Date}", value); }
        }

        public bool FilterDuration
        {
            get => connector.FilterDuration;
            set { connector.FilterDuration = value; logger.LogDebug("FilterDuration set in viewmodel to {Value}", value); }
        }
        public TimeSpan? MinDuration
        {
            get => connector.MinDuration;
            set { connector.MinDuration = value; logger.LogDebug("MinDuration set in viewmodel to {Value}", value); }
        }
        public TimeSpan? MaxDuration
        {
            get => connector.MaxDuration;
            set { connector.MaxDuration = value; logger.LogDebug("MaxDuration set in viewmodel to {Value}", value); }
        }


        public bool DisplayMaximumResolutionVideos { get => connector.DisplayMaximumResolutionVideos; set { connector.DisplayMaximumResolutionVideos = value; logger.LogDebug("DisplayMaximumResolutionVideos set in viewmodel to {Value}", value); } }
        public bool DisplayLargeResolutionVideos { get => connector.DisplayLargeResolutionVideos; set { connector.DisplayLargeResolutionVideos = value; logger.LogDebug("DisplayLargeResolutionVideos set in viewmodel to {Value}", value); } }
        public bool DisplayMediumResolutionVideos { get => connector.DisplayMediumResolutionVideos; set { connector.DisplayMediumResolutionVideos = value; logger.LogDebug("DisplayMediumResolutionVideos set in viewmodel to {Value}", value); } }
        public bool DisplayLowResolutionVideos { get => connector.DisplayLowResolutionVideos; set { connector.DisplayLowResolutionVideos = value; logger.LogDebug("DisplayLowResolutionVideos set in viewmodel to {Value}", value); } }
        public bool DisplayMinimumResolutionVideos { get => connector.DisplayMinimumResolutionVideos; set { connector.DisplayMinimumResolutionVideos = value; logger.LogDebug("DisplayMinimumResolutionVideos set in viewmodel to {Value}", value); } }
        public bool DisplayMaximumFramerateVideos { get => connector.DisplayMaximumFramerateVideos; set { connector.DisplayMaximumFramerateVideos = value; logger.LogDebug("DisplayMaximumFramerateVideos set in viewmodel to {Value}", value); } }
        public bool DisplayLargeFramerateVideos { get => connector.DisplayLargeFramerateVideos; set { connector.DisplayLargeFramerateVideos = value; logger.LogDebug("DisplayLargeFramerateVideos set in viewmodel to {Value}", value); } }
        public bool DisplayMediumFramerateVideos { get => connector.DisplayMediumFramerateVideos; set { connector.DisplayMediumFramerateVideos = value; logger.LogDebug("DisplayMediumFramerateVideos set in viewmodel to {Value}", value); } }
        public bool DisplayLowFramerateVideos { get => connector.DisplayLowFramerateVideos; set { connector.DisplayLowFramerateVideos = value; logger.LogDebug("DisplayLowFramerateVideos set in viewmodel to {Value}", value); } }
        public bool DisplayMinimumFramerateVideos { get => connector.DisplayMinimumFramerateVideos; set { connector.DisplayMinimumFramerateVideos = value; logger.LogDebug("DisplayMinimumFramerateVideos set in viewmodel to {Value}", value); } }

        public string TransferDescription => connector.TransferDescription;
        public bool HasActiveTransfer => connector.HasActiveTransfer;
        public int LoadedCount => connector.LoadedFileCount;
        public int TotalCount => connector.TotalFileCount;


        public void UpdateTextFilter()
        {
            logger.LogTrace("UpdateTextFilter invoked");
            connector.UpdateSearchText();
        }

        [RelayCommand]
        public void ChangeOrientation()
        {
            logger.LogTrace("ChangeOrientation invoked");
            connector.ChangeOrientation();
        }


        public override void Update(IEnumerable<UpdateSection> sections)
        {
            logger.LogTrace("SidePanelViewModel.Update entered with sections count={Count}", sections?.Count() ?? 0);
            if (sections.Contains(UpdateSection.FILTERPANEL))
            {
                OnPropertyChanged(nameof(OpenPanel));
                OnPropertyChanged(nameof(Orientation));
                OnPropertyChanged(nameof(UseRealTimeSearch));
                OnPropertyChanged(nameof(FilterDate));
                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(EndDate));
                OnPropertyChanged(nameof(FilterDuration));
                OnPropertyChanged(nameof(MinDuration));
                OnPropertyChanged(nameof(MaxDuration));
            }
            if (sections.Contains(UpdateSection.LOADPANEL))
            {
                OnPropertyChanged(nameof(OpenPanel));
                OnPropertyChanged(nameof(TransferDescription));
                OnPropertyChanged(nameof(HasActiveTransfer));
                OnPropertyChanged(nameof(LoadedCount));
                OnPropertyChanged(nameof(TotalCount));
            }
        }
    }
}
