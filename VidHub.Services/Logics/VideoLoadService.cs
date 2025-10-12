using System.Collections.Concurrent;
using VidHub.Core;
using VidHub.Core.Helpers;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;
using VidHub.Services.Settings.Interfaces;
using VidHub.Services.System.Interfaces;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace VidHub.Services.Logics
{
    public class VideoLoadService(IMainService service, ISettingsService settings, ISystemManager manager, IVideoCustomizationService customization) : IVideoLoadService
    {
        private readonly object locker = new();
        private readonly ConcurrentQueue<Transfer> transfers = [];

        public bool HasTransfer => !transfers.IsEmpty;
        public bool HasActiveTransfer => transfers.Any(t => t.IsActive);

        public string TransferDescription =>
            transfers.Where(t => t.IsActive).All(t => t.IsCollecting) ? "Collecting videos" :
            transfers.Where(t => t.IsActive).All(t => !t.IsCollecting) ? "Loading videos" :
            "Collecting and loading videos";

        public int LoadedCount => transfers.Sum(t => t.LoadedCount);

        public int TotalCount => transfers.Sum(t => t.TotalCount);


        public async Task LoadFilesAsync()
        {
            IReadOnlyList<StorageFile> files = await PickFilesOpen("Load", Video.ExtensionTypes);

            if (files.Count > 0)
            {
                await Task.Run(async () =>
                {
                    var index = transfers.Count;
                    var transfer = new Transfer();
                    transfer.AddTotalCount(files.Count);
                    transfers.Enqueue(transfer);
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidepanel);

                    AddFilesToVideoCollection(index, files);

                    transfers.ElementAt(index).IsActive = false;
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidepanel);
                    await TransferCleanup();
                });
            }
        }

        public async Task LoadFoldersAsync(bool includeSubfolders)
        {
            StorageFolder? folder = await PickFolderOpen("Load");

            if (folder != null)
            {
                await Task.Run(async () =>
                {
                    var index = transfers.Count;
                    var transfer = new Transfer();
                    transfer.IsCollecting = true;
                    transfers.Enqueue(transfer);
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidepanel);

                    transfers.ElementAt(index).IsCollecting = true;
                    var files = await CollectFilesAsync(folder, includeSubfolders, Video.ExtensionTypes);
                    transfers.ElementAt(index).IsCollecting = false;
                    transfers.ElementAt(index).AddTotalCount(files.Count());
                    AddFilesToVideoCollection(index, files);

                    transfers.ElementAt(index).IsActive = false;
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidepanel);
                    await TransferCleanup();
                });
            }
        }

        public async Task LoadExternal(IEnumerable<IStorageItem> items)
        {
            if (items.Any())
            {
                await Task.Run(async () =>
                {
                    var index = transfers.Count;
                    var transfer = new Transfer();
                    transfers.Enqueue(transfer);
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidepanel);

                    transfers.ElementAt(index).IsCollecting = true;
                    var files = items.OfType<StorageFile>().Where(f => Video.ExtensionTypes.Contains(f.FileType)).ToList();
                    foreach (var folder in items.OfType<StorageFolder>())
                    {
                        files.AddRange(await CollectFilesAsync(folder, true, Video.ExtensionTypes));
                    }
                    if (files.Count > 0)
                    {
                        transfers.ElementAt(index).IsCollecting = false;
                        transfers.ElementAt(index).AddTotalCount(files.Count);
                        AddFilesToVideoCollection(index, files);
                    }

                    transfers.ElementAt(index).IsActive = false;
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidepanel);
                    await TransferCleanup();
                });
            }
        }

        public async Task ImportCollectionAsync()
        {
            StorageFile? file = await PickFileOpen("Import", [".vhc"]);

            if (file != null)
            {
                await Task.Run(async () =>
                {
                    var index = transfers.Count;
                    var transfer = new Transfer();
                    transfers.Enqueue(transfer);
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidepanel);

                    transfers.ElementAt(index).IsCollecting = true;

                    var rawContent = File.ReadLines(file.Path);
                    var files = rawContent.Select(p =>
                    {
                        var awaiter = StorageFile.GetFileFromPathAsync(p);
                        awaiter.Wait();
                        return awaiter.GetResults();
                    });

                    transfer.AddTotalCount(files.Count());
                    transfers.Enqueue(transfer);
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidepanel);

                    AddFilesToVideoCollection(index, files);

                    transfers.ElementAt(index).IsActive = false;
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidepanel);
                    await TransferCleanup();
                });
            }
        }
        public async Task ExportCollectionAsync()
        {
            StorageFile? file = await PickFileSave("Export", ".vhc", "VidHub Collection");

            if (file != null)
            {
                await FileIO.WriteTextAsync(file, string.Join('\n', service.GetAllVideos().Select(v => v.FilePath)));
            }
        }


        private static async Task<IReadOnlyList<StorageFile>> PickFilesOpen(string commitButtonText, List<string> fileTypeFilters)
        {
            var picker = new FileOpenPicker
            {
                CommitButtonText = commitButtonText,
                SuggestedStartLocation = PickerLocationId.HomeGroup,
                ViewMode = PickerViewMode.Thumbnail
            };
            foreach (var filter in fileTypeFilters)
            {
                picker.FileTypeFilter.Add(filter);
            }

            InitializeWithWindow.Initialize(picker, Context.MainWindow.HWND);
            return await picker.PickMultipleFilesAsync();
        }

        private static async Task<StorageFolder?> PickFolderOpen(string commitButtonText)
        {
            var picker = new FolderPicker
            {
                CommitButtonText = commitButtonText,
                SuggestedStartLocation = PickerLocationId.VideosLibrary,
                ViewMode = PickerViewMode.Thumbnail
            };

            InitializeWithWindow.Initialize(picker, Context.MainWindow.HWND);
            return await picker.PickSingleFolderAsync();
        }

        private static async Task<StorageFile?> PickFileOpen(string commitButtonText, List<string> fileTypeFilters)
        {
            var picker = new FileOpenPicker
            {
                CommitButtonText = commitButtonText,
                SuggestedStartLocation = PickerLocationId.HomeGroup,
                ViewMode = PickerViewMode.Thumbnail
            };
            foreach (var filter in fileTypeFilters)
            {
                picker.FileTypeFilter.Add(filter);
            }

            InitializeWithWindow.Initialize(picker, Context.MainWindow.HWND);
            return await picker.PickSingleFileAsync();
        }

        private static async Task<StorageFile?> PickFileSave(string commitButtonText, string defaultFileExtension, string suggestedFileName)
        {
            var picker = new FileSavePicker
            {
                CommitButtonText = commitButtonText,
                DefaultFileExtension = defaultFileExtension,
                SuggestedFileName = suggestedFileName,
                SuggestedStartLocation = PickerLocationId.HomeGroup
            };
            picker.FileTypeChoices.Add(suggestedFileName, [defaultFileExtension]);

            InitializeWithWindow.Initialize(picker, Context.MainWindow.HWND);
            return await picker.PickSaveFileAsync();
        }


        private static async Task<IEnumerable<StorageFile>> CollectFilesAsync(StorageFolder folder, bool includeSubfolders, List<string> fileTypeFilters)
        {
            var files = new List<StorageFile>();
            files.AddRange(await folder.GetFilesAsync());

            if (includeSubfolders)
            {
                foreach (var subfolder in await folder.GetFoldersAsync())
                {
                    files.AddRange(await CollectFilesAsync(subfolder, includeSubfolders, fileTypeFilters));
                }
            }

            return files.Where(f => fileTypeFilters.Contains(f.FileType, StringComparer.OrdinalIgnoreCase));
        }

        private void AddFilesToVideoCollection(int index, IEnumerable<StorageFile> files)
        {
            lock (locker)
            {
                transfers.ElementAt(index).IsLoading = true;

                if (settings.ConcurrentVideoLoading)
                {
                    Parallel.ForEach(files, file =>
                    {
                        var video = new Video(file.Path);
                        if (settings.RelativePosition)
                        {
                            video.TryLoad(settings.CacheLoad, settings.FramePercentage);
                        }
                        else
                        {
                            video.TryLoad(settings.CacheLoad, new TimeSpan(0, settings.Hours, settings.Minutes, settings.Seconds, settings.Milliseconds));
                        }
                        customization.CustomizeTitle(video);
                        service.AddVideo(video);
                        transfers.ElementAt(index).Increment();
                        manager.SetTaskbar(transfers);
                        service.LoadedID.Add(video.ID);
                    });
                }
                else
                {
                    foreach (var file in files)
                    {
                        var video = new Video(file.Path);
                        if (settings.RelativePosition)
                        {
                            video.TryLoad(settings.CacheLoad, settings.FramePercentage);
                        }
                        else
                        {
                            video.TryLoad(settings.CacheLoad, new TimeSpan(0, settings.Hours, settings.Minutes, settings.Seconds, settings.Milliseconds));
                        }
                        customization.CustomizeTitle(video);
                        service.AddVideo(video);
                        transfers.ElementAt(index).Increment();
                        manager.SetTaskbar(transfers);
                        service.LoadedID.Add(video.ID);
                    }
                }

                transfers.ElementAt(index).IsLoading = false;
                manager.SetTaskbar(transfers);
                service.Update(UpdateType.UpdateSidepanel);
            }
        }

        private async Task TransferCleanup()
        {
            if (transfers.All(t => !t.IsActive))
            {
                if (!settings.DontShowTitleCustomizationAgain)
                {
                    customization.IsTemplateMode = false;
                    Context.MainWindow.TryEnqueue(() => Context.MainWindow.ShowDialogAsync(ModalType.CustomizeLoading, "Customize video title", "Confirm"));
                }

                manager.DisplayToast("Video loading finished!", $"{LoadedCount} videos were loaded successfully.");
                while (transfers.TryDequeue(out _)) ;
                service.Update(UpdateType.UpdateSidepanel);
                service.LoadedID.Clear();
            }
        }
    }
}
