using CommunityToolkit.Mvvm.ComponentModel;
using VidHub.Platform;
using VidHub.Services.Logics.Interfaces;

namespace VidHub.ViewModels
{
    public partial class SidepanelViewModel(IVideoOrganizeService service) : ObservableRecipient
    {
        public IEnumerable<string> SortOptions => service.GetSortOptions();

        public string? CurrentSortOption
        {
            get => service.CurrentSortOption;
            set => service.CurrentSortOption = value;
        }


        public string? SearchText
        {
            get => service.SearchText;
            set => service.SearchText = value;
        }

        public bool FilterDate
        {
            get => service.FilterDate;
            set => service.FilterDate = value;
        }
        public DateTimeOffset? StartDate
        {
            get => service.StartDate;
            set => service.StartDate = value;
        }
        public DateTimeOffset? EndDate
        {
            get => service.EndDate;
            set => service.EndDate = value;
        }

        public bool FilterDuration
        {
            get => service.FilterDuration;
            set => service.FilterDuration = value;
        }
        public TimeSpan? MinDuration
        {
            get => service.MinDuration;
            set => service.MinDuration = value;
        }
        public TimeSpan? MaxDuration
        {
            get => service.MaxDuration;
            set => service.MaxDuration = value;
        }


        public SidepanelViewModel() : this(Context.MainHost.GetService<IVideoOrganizeService>()) { }
    }
}
