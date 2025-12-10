using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Core.Notifications;
using VidHub.Core.Settings;
using VidHub.Core.Utilities.Helper;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Connectors.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;

namespace VidHub.Services.Connectors.Base
{
    public class VideoCollectionConnector(IVideoService vs, IVidHubSettings settings, IVideoCollectionService service) : IVideoCollectionConnector
    {
        public bool DisplayDates => settings.Display.DisplayDates;

        public bool DisplayDurations => settings.Display.DisplayDurations;

        public bool DisplayTitles => settings.Display.DisplayTitles;

        public ObservableCollection<Video> DisplayedVideos => service.DisplayedVideos;

        public double PreviewImageWidth => settings.Modals.DisplayFormat.PreviewImageWidth;

        public double PreviewImageHeight => settings.Modals.DisplayFormat.PreviewImageHeight;

        public ObservableCollection<BarNotification> DisplayedNotifications => service.DisplayedNotifications;

        public async Task ClearCacheAsync()
        {
            await Task.Run(() =>
            {
                foreach (string item in Directory.GetFiles(Path.Combine(Path.GetTempPath(), "VidHub"), "*", SearchOption.AllDirectories))
                {
                    File.Delete(item);
                }
            });
        }

        public async Task CopyFileAsync(Video video)
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

        public async Task CopyFilePathAsync(Video video)
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

        public async Task CopyPreviewImageAsync(Video video)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(video.PreviewImagePath);

            DataPackage data = new()
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            data.Properties.Title = video.Title;
            data.Properties.Description = $"Thumbnail of '{video.FilePath}' file copied to clipboard.";
            data.SetBitmap(RandomAccessStreamReference.CreateFromFile(file));

            Clipboard.SetContent(data);
        }

        public async Task OpenAsync(Video video)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(video.FilePath);
            _ = await Launcher.LaunchFileAsync(file);
        }

        public async Task OpenFileExplorerAsync(Video video)
        {
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(Path.GetDirectoryName(video.FilePath) ?? string.Empty);
            _ = await Launcher.LaunchFolderAsync(folder);
        }

        public async Task RemoveVideoAsync(Video video)
        {
            _ = await Task.Run(() => vs.Remove(video));
        }

        public async Task RenameAsync(Video video)
        {
            await Context.Window.ShowDialogAsync("ChangeVideoTitle", $"Rename '{video.Title}'", "Confirm", video);
            Context.Host.GetService<IVideoService>().Update(UpdateType.FORCEUPDATEVIDEOCOLLECTION);
        }

        public void SubscribeToUpdateEvent(Action<UpdateType> action)
        {
            vs.SubscribeToUpdateEvent(action);
        }

        public void UnsubscribeFromUpdateEvent(Action<UpdateType> action)
        {
            vs.UnsubscribeFromUpdateEvent(action);
        }

        public void Update(UpdateType type)
        {
            vs.Update(type);
        }
    }
}
