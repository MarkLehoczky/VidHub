using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Core.Notifications;
using VidHub.Services.Base.Interfaces;

namespace VidHub.Services.Connectors.Base.Interfaces
{
    public interface IVideoCollectionConnector : IUpdateService
    {
        bool DisplayDates { get; }
        bool DisplayDurations { get; }
        bool DisplayHealths { get; }
        bool DisplayTitles { get; }
        ObservableCollection<Video> DisplayedVideos { get; }
        ObservableCollection<BarNotification> DisplayedNotifications { get; }
        double PreviewImageWidth { get; }
        double PreviewImageHeight { get; }
        Task OpenAsync(Video video);
        Task OpenFileExplorerAsync(Video video);
        Task RenameAsync(Video video);
        Task CopyFileAsync(Video video);
        Task CopyFilePathAsync(Video video);
        Task CopyPreviewImageAsync(Video video);
        Task RemoveVideoAsync(Video video);
    }
}
