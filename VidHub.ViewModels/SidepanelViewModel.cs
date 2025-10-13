using CommunityToolkit.Mvvm.ComponentModel;
using VidHub.Core.Helpers;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.ViewModels
{
    public partial class SidePanelViewModel(IVideoOrganizerService organizeService, IVideoLoadService loadService, ISettingsService settingsService) : ObservableRecipient
    {
        public bool OpenPanel => settingsService.Organizer.Global.OpenedSidePanel;

        #region Organizer section
        public IEnumerable<string> SortOptions => organizeService.GetSortOptions();

        public string? CurrentSortOption
        {
            get => organizeService.CurrentSortOption;
            set => organizeService.CurrentSortOption = value;
        }


        public string SearchText
        {
            get => organizeService.SearchText;
            set
            {
                organizeService.SearchText = value;
                OnPropertyChanged(nameof(Suggestions));
            }
        }
        public bool EnableLiveSearch => settingsService.Organizer.Global.EnableLiveSearch;
        public IEnumerable<string> Suggestions => organizeService.Suggestions();

        public bool FilterDate
        {
            get => organizeService.FilterDate;
            set => organizeService.FilterDate = value;
        }
        public DateTimeOffset? StartDate
        {
            get => organizeService.StartDate;
            set => organizeService.StartDate = value;
        }
        public DateTimeOffset? EndDate
        {
            get => organizeService.EndDate;
            set => organizeService.EndDate = value;
        }

        public bool FilterDuration
        {
            get => organizeService.FilterDuration;
            set => organizeService.FilterDuration = value;
        }
        public TimeSpan? MinDuration
        {
            get => organizeService.MinDuration;
            set => organizeService.MinDuration = value;
        }
        public TimeSpan? MaxDuration
        {
            get => organizeService.MaxDuration;
            set => organizeService.MaxDuration = value;
        }
        #endregion


        #region Transfer section
        public string TransferDescription => loadService.TransferDescription;
        public bool HasActiveTransfer => loadService.HasActiveTransfer;
        public int LoadedCount => loadService.LoadedFileCount;
        public int TotalCount => loadService.TotalFileCount;
        public bool Indeterminate => TotalCount - LoadedCount == 0;
        #endregion


        public SidePanelViewModel() : this(
            Context.Host.GetService<IVideoOrganizerService>(),
            Context.Host.GetService<IVideoLoadService>(),
            Context.Host.GetService<ISettingsService>())
        {
            Context.Host.GetService<IVideoService>().SubscribeToUpdateEvent(UpdateProperties);
        }

        ~SidePanelViewModel()
        {
            Context.Host.GetService<IVideoService>().UnsubscribeFromUpdateEvent(UpdateProperties);
        }


        public void UpdateTextFilter()
        {
            organizeService.UpdateSearchText();
        }


        private void UpdateProperties(UpdateType type)
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
