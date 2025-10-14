using VidHub.Core.Helpers;
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
        public string? CurrentSortOption
        {
            get => connector.CurrentSortOption;
            set => connector.CurrentSortOption = value;
        }

        public string SearchText
        {
            get => connector.SearchText;
            set
            {
                connector.SearchText = value;
                OnPropertyChanged(nameof(Suggestions));
            }
        }
        public bool EnableLiveSearch => connector.EnableLiveSearch;
        public IEnumerable<string> Suggestions => connector.Suggestions();

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
        public bool Indeterminate => TotalCount - LoadedCount == 0;


        public void UpdateTextFilter()
        {
            connector.UpdateSearchText();
        }


        override public void Update(UpdateType type)
        {
            if (type == UpdateType.UpdateSidePanel || type == UpdateType.ForceUpdateSidePanel)
            {
                OnPropertyChanged(nameof(OpenPanel));
                OnPropertyChanged(nameof(EnableLiveSearch));
                OnPropertyChanged(nameof(TransferDescription));
                OnPropertyChanged(nameof(HasActiveTransfer));
                OnPropertyChanged(nameof(LoadedCount));
                OnPropertyChanged(nameof(TotalCount));
                OnPropertyChanged(nameof(Indeterminate));
            }
        }
    }
}
