using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Core.Helpers;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Connectors.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;
using VidHub.Services.Settings.Interfaces;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;

namespace VidHub.Services.Connectors.Base
{
    public class VideoCollectionConnector(IVideoService vs, ISettingsService settings, IVideoCollectionService service) : IVideoCollectionConnector
    {
        public bool DisplayDates => settings.DisplayCustomization.DisplayDates;

        public bool DisplayDurations => settings.DisplayCustomization.DisplayDurations;

        public bool DisplayTitles => settings.DisplayCustomization.DisplayTitles;

        public ObservableCollection<Video> DisplayedVideos => service.DisplayedVideos;

        public double PreviewImageWidth => settings.DisplayCustomization.PreviewImageWidth;

        public double PreviewImageHeight => settings.DisplayCustomization.PreviewImageHeight;

        public async Task CopyFileAsync(Video video)
        {
            var file = await StorageFile.GetFileFromPathAsync(video.FilePath);

            var data = new DataPackage
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
                var data = new DataPackage
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
            var file = await StorageFile.GetFileFromPathAsync(video.PreviewImagePath);

            var data = new DataPackage
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
            var file = await StorageFile.GetFileFromPathAsync(video.FilePath);
            await Launcher.LaunchFileAsync(file);
        }

        public async Task OpenFileExplorerAsync(Video video)
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(Path.GetDirectoryName(video.FilePath) ?? string.Empty);
            await Launcher.LaunchFolderAsync(folder);
        }

        public async Task RemoveVideoAsync(Video video)
        {
            await Task.Run(() => vs.Remove(video));
        }

        public async Task RenameAsync(Video video)
        {
            await Context.Window.ShowDialogAsync(ModalType.ChangeVideoTitle, $"Rename '{video.Title}'", "Confirm", video);
            Context.Host.GetService<IVideoService>().Update(UpdateType.ForceUpdateVideoCollection);
        }

        public void SubscribeToUpdateEvent(Action<UpdateType> action) => vs.SubscribeToUpdateEvent(action);

        public void UnsubscribeFromUpdateEvent(Action<UpdateType> action) => vs.UnsubscribeFromUpdateEvent(action);

        public void Update(UpdateType type) => vs.Update(type);
    }
}
