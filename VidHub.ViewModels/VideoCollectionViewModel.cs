using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Core.Notifications;
using VidHub.Core.Utilities.Helper;
using VidHub.Platform;
using VidHub.Services.Connectors.Base.Interfaces;
using VidHub.ViewModels.Base;

namespace VidHub.ViewModels
{
    public partial class VideoCollectionViewModel(IVideoCollectionConnector connector) : ViewModelTemplate(connector)
    {
        public VideoCollectionViewModel() : this(Context.Host.GetService<IVideoCollectionConnector>()) { }


        public ObservableCollection<Video> Videos => connector.DisplayedVideos;
        public bool DisplayTitles => connector.DisplayTitles;
        public bool DisplayDates => connector.DisplayDates;
        public bool DisplayDurations => connector.DisplayDurations;
        public double PreviewImageWidth => connector.PreviewImageWidth;
        public double PreviewImageHeight => connector.PreviewImageHeight;
        public ObservableCollection<BarNotification> Notifications => connector.DisplayedNotifications;


        [RelayCommand]
        private async Task OpenAsync(Video video)
        {
            await connector.OpenAsync(video);
        }

        [RelayCommand]
        private async Task OpenFileExplorerAsync(Video video)
        {
            await connector.OpenFileExplorerAsync(video);
        }

        [RelayCommand]
        private async Task RenameAsync(Video video)
        {
            await connector.RenameAsync(video);
        }

        [RelayCommand]
        private async Task CopyFileAsync(Video video)
        {
            await connector.CopyFileAsync(video);
        }

        [RelayCommand]
        private async Task CopyFilePathAsync(Video video)
        {
            await connector.CopyFilePathAsync(video);
        }

        [RelayCommand]
        private async Task CopyPreviewImageAsync(Video video)
        {
            await connector.CopyPreviewImageAsync(video);
        }

        [RelayCommand]
        private async Task RemoveVideoAsync(Video video)
        {
            await connector.RemoveVideoAsync(video);
        }


        public override void Update(IEnumerable<UpdateSection> sections)
        {
            if (sections.Contains(UpdateSection.VIDEOCOLLECTION))
            {
                OnPropertyChanged(nameof(Videos));
                OnPropertyChanged(nameof(DisplayTitles));
                OnPropertyChanged(nameof(DisplayDates));
                OnPropertyChanged(nameof(DisplayDurations));
                OnPropertyChanged(nameof(PreviewImageWidth));
                OnPropertyChanged(nameof(PreviewImageHeight));
            }
            if (sections.Contains(UpdateSection.NOTIFICATIONS))
            {
                OnPropertyChanged(nameof(Notifications));
            }
        }
    }
}
