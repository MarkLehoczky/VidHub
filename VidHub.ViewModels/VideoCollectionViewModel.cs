using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.ViewModels
{
    public class VideoCollectionViewModel(IVideoCollectionService service, ISettingsService settings) : ObservableRecipient
    {
        public ObservableCollection<Video> Videos => service.DisplayedVideos;
        public bool ShowTitles => settings.ShowTitles;
        public bool ShowDates => settings.ShowDates;
        public bool ShowDurations => settings.ShowDurations;


        public VideoCollectionViewModel() : this(Context.MainHost.GetService<IVideoCollectionService>(),
            Context.MainHost.GetService<ISettingsService>())
        {
            Context.MainHost.GetService<IMainService>().SubscribeToUpdateEvent(UpdateProperties);
        }

        ~VideoCollectionViewModel()
        {
            Context.MainHost.GetService<IMainService>().UnsubscribeFromUpdateEvent(UpdateProperties);
        }

        private void UpdateProperties()
        {
            OnPropertyChanged(nameof(Videos));
            OnPropertyChanged(nameof(ShowTitles));
            OnPropertyChanged(nameof(ShowDates));
            OnPropertyChanged(nameof(ShowDurations));
        }
    }
}
