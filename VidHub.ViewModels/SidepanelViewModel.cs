using CommunityToolkit.Mvvm.Input;
using VidHub.Core.Utilities;
using VidHub.Platform.Environment;
using VidHub.Services.Connectors.Base;

namespace VidHub.ViewModels
{
    public partial class SidePanelViewModel(ISidePanelConnector connector) : ViewModelTemplate(connector)
    {
        public SidePanelViewModel() : this(Context.Host.GetService<ISidePanelConnector>()) { }


        public bool OpenPanel => connector.OpenedSidePanel;

        public IEnumerable<string> SortOptions => connector.GetSortOptions();
        public string? SortBy
        {
            get => connector.SortBy;
            set => connector.SortBy = value;
        }
        public string Orientation => connector.Orientation == "ASC" ? "▲" : "▼";

        public string SearchText
        {
            get => connector.SearchText;
            set
            {
                connector.SearchText = value;
                OnPropertyChanged(nameof(SearchSuggestions));
            }
        }
        public bool UseRealTimeSearch => connector.UseRealTimeSearch;
        public IEnumerable<string> SearchSuggestions => connector.GetSearchSuggestions();

        public bool FilterDate
        {
            get => connector.FilterDate;
            set => connector.FilterDate = value;
        }
        public DateTimeOffset? StartDate
        {
            get => connector.StartDate;
            set => connector.StartDate = value;
        }
        public DateTimeOffset? EndDate
        {
            get => connector.EndDate;
            set => connector.EndDate = value;
        }

        public bool FilterDuration
        {
            get => connector.FilterDuration;
            set => connector.FilterDuration = value;
        }
        public TimeSpan? MinDuration
        {
            get => connector.MinDuration;
            set => connector.MinDuration = value;
        }
        public TimeSpan? MaxDuration
        {
            get => connector.MaxDuration;
            set => connector.MaxDuration = value;
        }


        public bool DisplayMaximumResolutionVideos { get => connector.DisplayMaximumResolutionVideos; set => connector.DisplayMaximumResolutionVideos = value; }
        public bool DisplayLargeResolutionVideos { get => connector.DisplayLargeResolutionVideos; set => connector.DisplayLargeResolutionVideos = value; }
        public bool DisplayMediumResolutionVideos { get => connector.DisplayMediumResolutionVideos; set => connector.DisplayMediumResolutionVideos = value; }
        public bool DisplayLowResolutionVideos { get => connector.DisplayLowResolutionVideos; set => connector.DisplayLowResolutionVideos = value; }
        public bool DisplayMinimumResolutionVideos { get => connector.DisplayMinimumResolutionVideos; set => connector.DisplayMinimumResolutionVideos = value; }
        public bool DisplayMaximumFramerateVideos { get => connector.DisplayMaximumFramerateVideos; set => connector.DisplayMaximumFramerateVideos = value; }
        public bool DisplayLargeFramerateVideos { get => connector.DisplayLargeFramerateVideos; set => connector.DisplayLargeFramerateVideos = value; }
        public bool DisplayMediumFramerateVideos { get => connector.DisplayMediumFramerateVideos; set => connector.DisplayMediumFramerateVideos = value; }
        public bool DisplayLowFramerateVideos { get => connector.DisplayLowFramerateVideos; set => connector.DisplayLowFramerateVideos = value; }
        public bool DisplayMinimumFramerateVideos { get => connector.DisplayMinimumFramerateVideos; set => connector.DisplayMinimumFramerateVideos = value; }

        public string TransferDescription => connector.TransferDescription;
        public bool HasActiveTransfer => connector.HasActiveTransfer;
        public int LoadedCount => connector.LoadedFileCount;
        public int TotalCount => connector.TotalFileCount;


        public void UpdateTextFilter()
        {
            connector.UpdateSearchText();
        }

        [RelayCommand]
        public void ChangeOrientation()
        {
            connector.ChangeOrientation();
        }


        public override void Update(IEnumerable<UpdateSection> sections)
        {
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
