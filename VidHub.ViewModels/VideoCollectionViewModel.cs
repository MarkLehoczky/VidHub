using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Core.Notifications;
using VidHub.Core.Utilities;
using VidHub.Platform.Environment;
using VidHub.Services.Connectors.Base;

namespace VidHub.ViewModels
{
    public partial class VideoCollectionViewModel(IVideoCollectionConnector connector) : ViewModelTemplate(connector)
    {
        public VideoCollectionViewModel() : this(Context.Host.GetService<IVideoCollectionConnector>()) { }


        public ObservableCollection<BarNotification> Notifications => connector.DisplayedNotifications;
        public ObservableCollection<Video> Videos => connector.DisplayedVideos;

        public bool DisplayTitles => connector.DisplayTitles;
        public bool DisplayDates => connector.DisplayDates;
        public bool DisplayDurations => connector.DisplayDurations;
        public bool DisplayHealths => connector.DisplayHealths;
        
        public double PreviewImageWidth => connector.PreviewImageWidth;
        public double PreviewImageHeight => connector.PreviewImageHeight;


        [RelayCommand]
        private async Task OpenAsync(Video video)
        {
            await connector.Open(video);
        }
        [RelayCommand]
        private async Task OpenFileExplorerAsync(Video video)
        {
            await connector.OpenFileExplorer(video);
        }

        [RelayCommand]
        private async Task CopyFileAsync(Video video)
        {
            await connector.CopyFile(video);
        }
        [RelayCommand]
        private async Task CopyFilePathAsync(Video video)
        {
            await connector.CopyFilePath(video);
        }
        [RelayCommand]
        private async Task CopyPreviewImageAsync(Video video)
        {
            await connector.CopyPreviewImage(video);
        }

        [RelayCommand]
        private async Task RenameAsync(Video video)
        {
            await connector.Rename(video);
        }
        
        [RelayCommand]
        private async Task RemoveAsync(Video video)
        {
            await connector.Remove(video);
        }


        public override void Update(IEnumerable<UpdateSection> sections)
        {
            if (sections.Contains(UpdateSection.VIDEOCOLLECTION))
            {
                OnPropertyChanged(nameof(Videos));
                OnPropertyChanged(nameof(DisplayTitles));
                OnPropertyChanged(nameof(DisplayDates));
                OnPropertyChanged(nameof(DisplayDurations));
                OnPropertyChanged(nameof(DisplayHealths));
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
