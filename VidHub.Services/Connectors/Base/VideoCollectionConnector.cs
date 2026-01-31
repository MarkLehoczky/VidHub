using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Core.Notifications;
using VidHub.Core.Settings;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Base;
using VidHub.Services.Logics;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;

namespace VidHub.Services.Connectors.Base
{
    public class VideoCollectionConnector(IVideoService vs, IVidHubSettings settings, IVideoCollectionService service) : ConnectorTemplate(vs), IVideoCollectionConnector
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public bool DisplayDates => settings.Display.DisplayDate;
        public bool DisplayDurations => settings.Display.DisplayDuration;
        public bool DisplayHealths => settings.Display.DisplayHealth;
        public bool DisplayTitles => settings.Display.DisplayTitle;
        public bool DisplayResolutions => settings.Display.DisplayResolution;
        public bool DisplayFramerates => settings.Display.DisplayFramerate;

        public double PreviewImageWidth => settings.Dialogs.DisplayFormat.PreviewImageWidth;
        public double PreviewImageHeight => settings.Dialogs.DisplayFormat.PreviewImageHeight;

        public ObservableCollection<BarNotification> DisplayedNotifications => service.DisplayedNotifications;
        public ObservableCollection<Video> DisplayedVideos => service.DisplayedVideos;


        public async Task Open(Video video)
        {
            logger.LogTrace("Open requested for file={File}", video.FilePath);
            StorageFile file = await StorageFile.GetFileFromPathAsync(video.FilePath);
            _ = await Launcher.LaunchFileAsync(file);
            logger.LogInformation("Opened file {File}", video.FilePath);
        }
        public async Task OpenFileExplorer(Video video)
        {
            logger.LogTrace("OpenFileExplorer requested for file={File}", video.FilePath);
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(Path.GetDirectoryName(video.FilePath) ?? string.Empty);
            _ = await Launcher.LaunchFolderAsync(folder);
            logger.LogInformation("Opened file explorer for folder of {File}", video.FilePath);
        }

        public async Task CopyFile(Video video)
        {
            logger.LogTrace("CopyFile requested for file={File}", video.FilePath);
            StorageFile file = await StorageFile.GetFileFromPathAsync(video.FilePath);

            DataPackage data = new()
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            data.Properties.Title = video.Title;
            data.Properties.Description = $"File '{video.FilePath}' copied to clipboard.";
            data.SetStorageItems([file]);

            Clipboard.SetContent(data);
            logger.LogInformation("Copied file to clipboard: {File}", video.FilePath);
        }
        public async Task CopyFilePath(Video video)
        {
            logger.LogTrace("CopyFilePath requested for file={File}", video.FilePath);
            await Task.Run(() =>
            {
                DataPackage data = new()
                {
                    RequestedOperation = DataPackageOperation.Copy
                };
                data.Properties.Title = video.Title;
                data.Properties.Description = $"Filepath '{video.FilePath}' copied to clipboard.";
                data.SetText(video.FilePath);

                Clipboard.SetContent(data);
            });
            logger.LogInformation("Copied file path to clipboard: {File}", video.FilePath);
        }
        public async Task CopyPreviewImage(Video video)
        {
            logger.LogTrace("CopyPreviewImage requested for file={File}", video.FilePath);
            StorageFile file = await StorageFile.GetFileFromPathAsync(video.PreviewImagePath);

            DataPackage data = new()
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            data.Properties.Title = video.Title;
            data.Properties.Description = $"Preview image of '{video.FilePath}' file copied to clipboard.";
            data.SetBitmap(RandomAccessStreamReference.CreateFromFile(file));

            Clipboard.SetContent(data);
            logger.LogInformation("Copied preview image to clipboard for file={File}", video.FilePath);
        }

        public async Task Rename(Video video)
        {
            logger.LogTrace("Rename requested for file={File}", video.FilePath);
            await VidHubContext.Window.OpenRenameDialog(video);
            logger.LogDebug("Rename dialog completed for file={File}", video.FilePath);
        }

        public async Task Remove(Video video)
        {
            logger.LogTrace("Remove requested for file={File}", video.FilePath);
            _ = await Task.Run(() => vs.Remove(video));
            logger.LogInformation("Removed video {File}", video.FilePath);
        }
    }
}
