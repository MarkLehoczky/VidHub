using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using VidHub.Core;
using VidHub.Core.Enums;
using VidHub.Core.Models;
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
        private bool ffmpegInstalled = false;
        public bool FFmpegInstallationFound()
        {
            ProcessStartInfo info = new()
            {
                FileName = "where",
                Arguments = "ffmpeg",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using var process = Process.Start(info);
            string output = process!.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return !string.IsNullOrWhiteSpace(output);
        }


        public bool DisplayDates => settings.DisplayCustomization.DisplayDates;

        public bool DisplayDurations => settings.DisplayCustomization.DisplayDurations;

        public bool DisplayTitles => settings.DisplayCustomization.DisplayTitles;

        public ObservableCollection<Video> DisplayedVideos => service.DisplayedVideos;

        public double PreviewImageWidth => settings.DisplayCustomization.PreviewImageWidth;

        public double PreviewImageHeight => settings.DisplayCustomization.PreviewImageHeight;

        public string LargeCacheDataMessage => FormattedCacheDataSize();

        public bool LargeCacheDataSize => CacheDataSize() > Math.Pow(1024, 3);

        public bool FFmpegNotInstalled => !ffmpegInstalled && !FFmpegInstallationFound();

        public ObservableCollection<Notification> Notifications { get; } =
            [
                new()
                {
                    IsOpen = false,
                    Title = "Large Cache Data Size",
                    Message = FormattedCacheDataSize(),
                    Severity = NotificationSeverity.Warning,
                    Closable = true,
                    Button = new()
                    {
                        Text = "Clear Cache",
                        Tooltip = "Clear the application's cache data to free up space. After clearing the cache, the cache loading will not be available.",
                        Command = new AsyncRelayCommand(async () =>
                        {
                            await Task.Run(() =>
                            {
                                foreach (var item in Directory.GetFiles(Path.Combine(Path.GetTempPath(), "VidHub"), "*", SearchOption.AllDirectories))
                                {
                                    File.Delete(item);
                                }
                            });
                        })
                    }
                },
                new()
                {
                    IsOpen = false,
                    Title = "Large Cache Data Size",
                    Message = FormattedCacheDataSize(),
                    Severity = NotificationSeverity.Warning,
                    Closable = true,
                    Button = new()
                    {
                        Text = "Clear Cache",
                        Tooltip = "Clear the application's cache data to free up space. After clearing the cache, the cache loading will not be available.",
                        Command = new AsyncRelayCommand(async () =>
                        {
                            await Task.Run(() =>
                            {
                                foreach (var item in Directory.GetFiles(Path.Combine(Path.GetTempPath(), "VidHub"), "*", SearchOption.AllDirectories))
                                {
                                    File.Delete(item);
                                }
                            });
                        })
                    }
                }
            ];

        public async Task ClearCacheAsync()
        {
            await Task.Run(() =>
            {
                foreach (var item in Directory.GetFiles(Path.Combine(Path.GetTempPath(), "VidHub"), "*", SearchOption.AllDirectories))
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

        public async Task InstallFFmpegAsync()
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
                    using var process = Process.Start(info);
                    process!.WaitForExit();
                    if (process.ExitCode == 0)
                    {
                        ffmpegInstalled = true;
                    }
                }
                catch { }

                Update(UpdateType.UpdateVideoCollection);
            });
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
            await Context.Window.ShowDialogAsync(ModalType.ChangeVideoTitle, $"Rename '{video.Title}'", "Confirm", video);
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


        private long CacheDataSize()
        {
            DirectoryInfo info = new(Path.Combine(Path.GetTempPath(), "VidHub"));
            return info.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);
        }
        private static string FormattedCacheDataSize()
        {
            DirectoryInfo info = new(Path.Combine(Path.GetTempPath(), "VidHub"));
            long size = info.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);

            if (size > Math.Pow(1024, 4))
            {
                return $"The application's cache data has reached {size / Math.Pow(1024, 4):f2} TB";
            }
            if (size > Math.Pow(1024, 3))
            {
                return $"The application's cache data has reached {size / Math.Pow(1024, 3):f2} GB";
            }
            if (size > Math.Pow(1024, 2))
            {
                return $"The application's cache data has reached {size / Math.Pow(1024, 2):f2} MB";
            }
            if (size > Math.Pow(1024, 1))
            {
                return $"The application's cache data has reached {size / Math.Pow(1024, 1):f2} kB";
            }
            return $"The application's cache data has reached {size} B";
        }
    }
}
