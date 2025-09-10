using CommunityToolkit.Mvvm.ComponentModel;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.ViewModels
{
    public partial class SidepanelViewModel(IVideoOrganizeService organizeService, IVideoLoadService loadService, ISettingsService settingsService) : ObservableRecipient
    {
        public bool OpenPanel => settingsService.OpenPanel;

        #region Organizer section
        public IEnumerable<string> SortOptions => organizeService.GetSortOptions();

        public string? CurrentSortOption
        {
            get => organizeService.CurrentSortOption;
            set => organizeService.CurrentSortOption = value;
        }


        public string? SearchText
        {
            get => organizeService.SearchText;
            set => organizeService.SearchText = value;
        }
        public bool LiveTextFiltering => settingsService.LiveTextFiltering;

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
        public bool HasTransfer => loadService.HasTransfer;
        public bool HasActiveTransfer => loadService.HasActiveTransfer;
        public int LoadedCount => loadService.LoadedCount;
        public int TotalCount => loadService.TotalCount;
        public bool Indeterminate => TotalCount - LoadedCount == 0;
        #endregion


        public SidepanelViewModel() : this(
            Context.MainHost.GetService<IVideoOrganizeService>(),
            Context.MainHost.GetService<IVideoLoadService>(),
            Context.MainHost.GetService<ISettingsService>())
        {
            Context.MainHost.GetService<IMainService>().SubscribeToUpdateEvent(UpdateProperties);
        }

        ~SidepanelViewModel()
        {
            Context.MainHost.GetService<IMainService>().UnsubscribeFromUpdateEvent(UpdateProperties);
        }


        public void UpdateTextFilter(string text)
        {
            organizeService.UpdateTextFilter(text);
        }


        private void UpdateProperties()
        {
            OnPropertyChanged(nameof(OpenPanel));
            OnPropertyChanged(nameof(LiveTextFiltering));
            OnPropertyChanged(nameof(TransferDescription));
            OnPropertyChanged(nameof(HasTransfer));
            OnPropertyChanged(nameof(HasActiveTransfer));
            OnPropertyChanged(nameof(LoadedCount));
            OnPropertyChanged(nameof(TotalCount));
            OnPropertyChanged(nameof(Indeterminate));
        }
    }
}
