using VidHub.Core.Utilities.Helper;
using VidHub.Platform;
using VidHub.Services.Connectors.Base.Interfaces;
using VidHub.ViewModels.Base;

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


        public string TransferDescription => connector.TransferDescription;
        public bool HasActiveTransfer => connector.HasActiveTransfer;
        public int LoadedCount => connector.LoadedFileCount;
        public int TotalCount => connector.TotalFileCount;


        public void UpdateTextFilter()
        {
            connector.UpdateSearchText();
        }


        public override void Update(IEnumerable<UpdateSection> sections)
        {
            if (sections.Contains(UpdateSection.FILTERPANEL))
            {
                OnPropertyChanged(nameof(OpenPanel));
                OnPropertyChanged(nameof(UseRealTimeSearch));
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
