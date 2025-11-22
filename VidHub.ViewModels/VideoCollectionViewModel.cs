using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using VidHub.Core;
using VidHub.Core.Enums;
using VidHub.Core.Models;
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
        public bool LargeCacheDataSize => connector.LargeCacheDataSize;
        public string LargeCacheDataMessage => connector.LargeCacheDataMessage;
        public bool FFmpegNotInstalled => connector.FFmpegNotInstalled;
        public ObservableCollection<Notification> Notifications => connector.Notifications;


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


        [RelayCommand]
        private async Task ClearCacheAsync()
        {
            await connector.ClearCacheAsync();
        }

        [RelayCommand]
        private async Task InstallFFmpegAsync()
        {
            await connector.InstallFFmpegAsync();
        }


        public override void Update(UpdateType type)
        {
            if (type is UpdateType.UpdateVideoCollection or UpdateType.ForceUpdateVideoCollection)
            {
                OnPropertyChanged(nameof(Videos));
                OnPropertyChanged(nameof(ShowTitles));
                OnPropertyChanged(nameof(ShowDates));
                OnPropertyChanged(nameof(ShowDurations));
                OnPropertyChanged(nameof(PreviewWidth));
                OnPropertyChanged(nameof(PreviewHeight));
                OnPropertyChanged(nameof(LargeCacheDataSize));
                OnPropertyChanged(nameof(LargeCacheDataMessage));
                OnPropertyChanged(nameof(FFmpegNotInstalled));
            }
        }
    }
}
