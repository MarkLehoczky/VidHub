using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Core.Notifications;
using VidHub.Services.Base;

namespace VidHub.Services.Connectors.Base
{
    public interface IVideoCollectionConnector : IUpdateService
    {
        bool DisplayDates { get; }
        bool DisplayDurations { get; }
        bool DisplayHealths { get; }
        bool DisplayTitles { get; }
        bool DisplayResolutions { get; }
        bool DisplayFramerates { get; }
        ObservableCollection<Video> DisplayedVideos { get; }
        ObservableCollection<BarNotification> DisplayedNotifications { get; }
        double PreviewImageWidth { get; }
        double PreviewImageHeight { get; }
        Task CopyFile(Video video);
        Task CopyFilePath(Video video);
        Task CopyPreviewImage(Video video);
        Task Open(Video video);
        Task OpenFileExplorer(Video video);
        Task Remove(Video video);
        Task Rename(Video video);
    }
}
