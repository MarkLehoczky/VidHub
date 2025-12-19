using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Core.Notifications;
using VidHub.Core.Settings;
using VidHub.Platform.Environment;
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
        public bool DisplayDates => settings.Display.DisplayDate;
        public bool DisplayDurations => settings.Display.DisplayDuration;
        public bool DisplayHealths => settings.Display.DisplayHealth;
        public bool DisplayTitles => settings.Display.DisplayTitle;

        public double PreviewImageWidth => settings.Dialogs.DisplayFormat.PreviewImageWidth;
        public double PreviewImageHeight => settings.Dialogs.DisplayFormat.PreviewImageHeight;

        public ObservableCollection<BarNotification> DisplayedNotifications => service.DisplayedNotifications;
        public ObservableCollection<Video> DisplayedVideos => service.DisplayedVideos;


        public async Task Open(Video video)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(video.FilePath);
            _ = await Launcher.LaunchFileAsync(file);
        }
        public async Task OpenFileExplorer(Video video)
        {
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(Path.GetDirectoryName(video.FilePath) ?? string.Empty);
            _ = await Launcher.LaunchFolderAsync(folder);
        }

        public async Task CopyFile(Video video)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(video.FilePath);

            DataPackage data = new()
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            data.Properties.Title = video.Title;
            data.Properties.Description = $"File '{video.FilePath}' copied to clipboard.";
            data.SetStorageItems([file]);

            Clipboard.SetContent(data);
        }
        public async Task CopyFilePath(Video video)
        {
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
        }
        public async Task CopyPreviewImage(Video video)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(video.PreviewImagePath);

            DataPackage data = new()
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            data.Properties.Title = video.Title;
            data.Properties.Description = $"Preview image of '{video.FilePath}' file copied to clipboard.";
            data.SetBitmap(RandomAccessStreamReference.CreateFromFile(file));

            Clipboard.SetContent(data);
        }

        public async Task Rename(Video video)
        {
            await Context.Window.OpenRenameDialog(video);
        }

        public async Task Remove(Video video)
        {
            _ = await Task.Run(() => vs.Remove(video));
        }
    }
}
