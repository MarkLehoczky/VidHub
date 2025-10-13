using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Core.Helpers;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;
using VidHub.Services.Settings.Interfaces;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;

namespace VidHub.ViewModels
{
    public partial class VideoCollectionViewModel(IVideoCollectionService service, ISettingsService settings) : ObservableRecipient
    {
        public ObservableCollection<Video> Videos => service.DisplayedVideos;
        public bool ShowTitles => settings.DisplayCustomization.DisplayTitles;
        public bool ShowDates => settings.DisplayCustomization.DisplayDates;
        public bool ShowDurations => settings.DisplayCustomization.DisplayDurations;
        public double PreviewWidth => settings.DisplayCustomization.PreviewImageWidth;
        public double PreviewHeight => settings.DisplayCustomization.PreviewImageHeight;


        public VideoCollectionViewModel() : this(Context.Host.GetService<IVideoCollectionService>(),
            Context.Host.GetService<ISettingsService>())
        {
            Context.Host.GetService<IVideoService>().SubscribeToUpdateEvent(UpdateProperties);
        }

        ~VideoCollectionViewModel()
        {
            Context.Host.GetService<IVideoService>().UnsubscribeFromUpdateEvent(UpdateProperties);
        }

        private void UpdateProperties(UpdateType type)
        {
            if (type == UpdateType.UpdateVideoCollection || type == UpdateType.ForceUpdateVideoCollection)
            {
                OnPropertyChanged(nameof(Videos));
                OnPropertyChanged(nameof(ShowTitles));
                OnPropertyChanged(nameof(ShowDates));
                OnPropertyChanged(nameof(ShowDurations));
                OnPropertyChanged(nameof(PreviewWidth));
                OnPropertyChanged(nameof(PreviewHeight));
            }
        }



        [RelayCommand]
        private async Task OpenAsync(Video video)
        {
            var file = await StorageFile.GetFileFromPathAsync(video.FilePath);
            await Launcher.LaunchFileAsync(file);
        }

        [RelayCommand]
        private async Task OpenFileExplorerAsync(Video video)
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(Path.GetDirectoryName(video.FilePath) ?? string.Empty);
            await Launcher.LaunchFolderAsync(folder);
        }

        [RelayCommand]
        private async Task RenameAsync(Video video)
        {
            await Context.Window.ShowDialogAsync(ModalType.ChangeVideoTitle, $"Rename '{video.Title}'", "Confirm", video);
            Context.Host.GetService<IVideoService>().Update(UpdateType.ForceUpdateVideoCollection);
        }

        [RelayCommand]
        private async Task CopyVideoAsync(Video video)
        {
            var file = await StorageFile.GetFileFromPathAsync(video.FilePath);

            var data = new DataPackage();
            data.RequestedOperation = DataPackageOperation.Copy;
            data.Properties.Title = video.Title;
            data.Properties.Description = $"File '{video.FilePath}' copied to clipboard.";
            data.SetStorageItems([file]);

            Clipboard.SetContent(data);
        }

        [RelayCommand]
        private void CopyFilePath(Video video)
        {
            var data = new DataPackage();
            data.RequestedOperation = DataPackageOperation.Copy;
            data.Properties.Title = video.Title;
            data.Properties.Description = $"Filepath '{video.FilePath}' copied to clipboard.";
            data.SetText(video.FilePath);

            Clipboard.SetContent(data);
        }

        [RelayCommand]
        private async Task CopyThumbnailAsync(Video video)
        {
            var file = await StorageFile.GetFileFromPathAsync(video.PreviewImagePath);

            var data = new DataPackage();
            data.RequestedOperation = DataPackageOperation.Copy;
            data.Properties.Title = video.Title;
            data.Properties.Description = $"Thumbnail of '{video.FilePath}' file copied to clipboard.";
            data.SetBitmap(RandomAccessStreamReference.CreateFromFile(file));

            Clipboard.SetContent(data);
        }

        [RelayCommand]
        private void RemoveVideo(Video video)
        {
            Context.Host.GetService<IVideoService>().Remove(video);
        }
    }
}
