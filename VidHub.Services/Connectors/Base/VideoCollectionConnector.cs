using System.Collections.ObjectModel;
using System.Diagnostics;
using VidHub.Core;
using VidHub.Core.Enums;
using VidHub.Core.Models;
using VidHub.Core.Settings;
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
    public class VideoCollectionConnector(IVideoService vs, ISettingsService settings, IVideoCollectionService service) : IVideoCollectionConnector
    {
        public bool DisplayDates => settings.DisplayCustomization.DisplayDates;

        public bool DisplayDurations => settings.DisplayCustomization.DisplayDurations;

        public bool DisplayTitles => settings.DisplayCustomization.DisplayTitles;

        public ObservableCollection<Video> DisplayedVideos => service.DisplayedVideos;

        public double PreviewImageWidth => settings.DisplayCustomization.PreviewImageWidth;

        public double PreviewImageHeight => settings.DisplayCustomization.PreviewImageHeight;

        public ObservableCollection<Notification> Notifications { get; } =
            [
                new SingleInteractionNotification()
                {
                    OpenCondition = () => {
                        DirectoryInfo info = new(Path.Combine(Path.GetTempPath(), "VidHub"));
                        return info.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length) > Math.Pow(1024, 3);
                    },
                    Title = "Large Cache Size",
                    Message = FormattedCacheDataSize(),
                    Severity = NotificationSeverity.Warning,
                    IsClosable = true,
                    Button = new AsyncNotificationButton()
                    {
                        Content = "Clear Cache",
                        Tooltip = "Clears cached files to recover disk space. With cache loading enabled, all previously cached videos has to be extracted again.",
                        Action = async () =>
                        {
                            await Task.Run(() =>
                            {
                                foreach (string item in Directory.GetFiles(Path.Combine(Path.GetTempPath(), "VidHub"), "*", SearchOption.AllDirectories))
                                {
                                    File.Delete(item);
                                }
                            });
                        }
                    }
                },
                new SingleInteractionNotification()
                {
                    OpenCondition = () => {
                        ProcessStartInfo info = new()
                        {
                            FileName = "where",
                            Arguments = "ffmpeg",
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        using Process? process = Process.Start(info);
                        string output = process!.StandardOutput.ReadToEnd();
                        process.WaitForExit();
                        return string.IsNullOrWhiteSpace(output);
                    },
                    Title = "FFmpeg Not Detected",
                    Message = "FFmpeg is required for generating video thumbnails and rendering previews. It was not found in the system PATH.",
                    Severity = NotificationSeverity.Error,
                    IsClosable = true,
                    Button = new AsyncNotificationButton()
                    {
                        Content = "Install FFmpeg",
                        Tooltip = "Installs FFmpeg using Winget",
                        Action = async () =>
                        {
                            await Task.Run(() =>
                            {
                                try
                                {
                                    ProcessStartInfo info = new()
                                    {
                                        FileName = "winget",
                                        Arguments = "install ffmpeg",
                                        UseShellExecute = true,
                                        CreateNoWindow = true
                                    };
                                    using Process? process = Process.Start(info);
                                    process!.WaitForExit();
                                }
                                catch { }
                            });
                        }
                    }
                }
            ];

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
            Context.Host.GetService<IVideoService>().Update(UpdateType.ForceUpdateVideoCollection);
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


        private static string FormattedCacheDataSize()
        {
            DirectoryInfo info = new(Path.Combine(Path.GetTempPath(), "VidHub"));
            long size = info.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);

            if (size > Math.Pow(1024, 4))
            {
                return $"The application's cache data has reached {size / Math.Pow(1024, 4):f2} TB";
            }
            return size > Math.Pow(1024, 3)
                ? $"The application's cache data has reached {size / Math.Pow(1024, 3):f2} GB"
                : size > Math.Pow(1024, 2)
                ? $"The application's cache data has reached {size / Math.Pow(1024, 2):f2} MB"
                : size > Math.Pow(1024, 1)
                ? $"The application's cache data has reached {size / Math.Pow(1024, 1):f2} kB"
                : $"The application's cache data has reached {size} B";
        }
    }
}
