using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using VidHub.Core.Models;
using VidHub.Core.Streams;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Connectors.Base;

namespace VidHub.ViewModels
{
    public partial class SidePanelViewModel(ISidePanelConnector connector) : ViewModelTemplate(connector)
    {
        private new readonly ILogger logger = VidHubContext.Logger;
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

        public bool FilterResolution { get => connector.FilterResolution; set { connector.FilterResolution = value; logger.LogDebug("FilterResolution set in viewmodel to {Value}", value); } }
        public ObservableCollection<FixedResolution> Resolutions => connector.Resolutions;

        public bool FilterFramerate { get => connector.FilterFramerate; set { connector.FilterFramerate = value; logger.LogDebug("FilterFramerate set in viewmodel to {Value}", value); } }
        public ObservableCollection<FixedFramerate> Framerates => connector.Framerates;

        public bool FilterTags { get => connector.FilterTags; set { connector.FilterTags = value; logger.LogDebug("FilterTags set in viewmodel to {Value}", value); } }
        public ObservableCollection<Tag> Tags => connector.Tags;

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


        [RelayCommand]
        public async Task OpenResolutionSettingsAsync()
        {
            logger.LogTrace("OpenResolutionSettings invoked");
            await connector.OpenResolutionSettings();
        }

        [RelayCommand]
        public async Task OpenFramerateSettingsAsync()
        {
            logger.LogTrace("OpenFramerateSettings invoked");
            await connector.OpenFramerateSettings();
        }

        [RelayCommand]
        public async Task OpenTagSettingsAsync()
        {
            logger.LogTrace("OpenTagSettings invoked");
            await connector.OpenTagSettings();
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
