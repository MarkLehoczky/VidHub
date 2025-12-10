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
        public bool ShowTitles => connector.DisplayTitles;
        public bool ShowDates => connector.DisplayDates;
        public bool ShowDurations => connector.DisplayDurations;
        public double PreviewWidth => connector.PreviewImageWidth;
        public double PreviewHeight => connector.PreviewImageHeight;
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
                OnPropertyChanged(nameof(ShowTitles));
                OnPropertyChanged(nameof(ShowDates));
                OnPropertyChanged(nameof(ShowDurations));
                OnPropertyChanged(nameof(PreviewWidth));
                OnPropertyChanged(nameof(PreviewHeight));
                OnPropertyChanged(nameof(Notifications));
            }
        }
    }
}
