using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Core.Notifications;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Connectors.Base;

namespace VidHub.ViewModels
{
    public partial class VideoCollectionViewModel(IVideoCollectionConnector connector) : ViewModelTemplate(connector)
    {
        private new readonly ILogger logger = VidHubContext.Logger;

        public VideoCollectionViewModel() : this(VidHubContext.Host.GetService<IVideoCollectionConnector>()) { }


        public ObservableCollection<BarNotification> Notifications => connector.DisplayedNotifications;
        public ObservableCollection<Video> Videos => connector.DisplayedVideos;

        public bool DisplayTitles => connector.DisplayTitles;
        public bool DisplayDates => connector.DisplayDates;
        public bool DisplayDurations => connector.DisplayDurations;
        public bool DisplayHealths => connector.DisplayHealths;
        public bool DisplayResolutions => connector.DisplayResolutions;
        public bool DisplayFramerates => connector.DisplayFramerates;
        public bool DisplayTags => connector.DisplayTags;

        public double PreviewImageWidth => connector.PreviewImageWidth;
        public double PreviewImageHeight => connector.PreviewImageHeight;


        [RelayCommand]
        private async Task OpenAsync(Video video)
        {
            logger.LogTrace("OpenAsync invoked for {File}", video.FilePath);
            await connector.Open(video);
        }
        [RelayCommand]
        private async Task OpenFileExplorerAsync(Video video)
        {
            logger.LogTrace("OpenFileExplorerAsync invoked for {File}", video.FilePath);
            await connector.OpenFileExplorer(video);
        }

        [RelayCommand]
        private async Task CopyFileAsync(Video video)
        {
            logger.LogTrace("CopyFileAsync invoked for {File}", video.FilePath);
            await connector.CopyFile(video);
        }
        [RelayCommand]
        private async Task CopyFilePathAsync(Video video)
        {
            logger.LogTrace("CopyFilePathAsync invoked for {File}", video.FilePath);
            await connector.CopyFilePath(video);
        }
        [RelayCommand]
        private async Task CopyPreviewImageAsync(Video video)
        {
            logger.LogTrace("CopyPreviewImageAsync invoked for {File}", video.FilePath);
            await connector.CopyPreviewImage(video);
        }

        [RelayCommand]
        private async Task RenameAsync(Video video)
        {
            logger.LogTrace("RenameAsync invoked for {File}", video.FilePath);
            await connector.Rename(video);
        }

        [RelayCommand]
        private async Task RemoveAsync(Video video)
        {
            logger.LogTrace("RemoveAsync invoked for {File}", video.FilePath);
            await connector.Remove(video);
        }

        [RelayCommand]
        private async Task ModifyVideoTagsAsync(Video video)
        {
            logger.LogTrace("ModifyVideoTagsAsync invoked for {File}", video.FilePath);
            await connector.ModifyVideoTags(video);
        }


        public override void Update(IEnumerable<UpdateSection> sections)
        {
            logger.LogTrace("VideoCollectionViewModel.Update entered with sections count={Count}", sections?.Count() ?? 0);
            if (sections.Contains(UpdateSection.VIDEOCOLLECTION))
            {
                OnPropertyChanged(nameof(Videos));
                OnPropertyChanged(nameof(DisplayTitles));
                OnPropertyChanged(nameof(DisplayDates));
                OnPropertyChanged(nameof(DisplayDurations));
                OnPropertyChanged(nameof(DisplayHealths));
                OnPropertyChanged(nameof(DisplayResolutions));
                OnPropertyChanged(nameof(DisplayFramerates));
                OnPropertyChanged(nameof(DisplayTags));
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
