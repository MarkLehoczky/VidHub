using System;
using System.Collections.Concurrent;
using VidHub.Core;
using VidHub.Core.Helpers;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;
using VidHub.Services.Settings.Interfaces;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace VidHub.Services.Logics
{
    public class VideoLoadService(IMainService service, ISettingsService settings) : IVideoLoadService
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
                await Task.Run(() =>
                {
                    var index = transfers.Count;
                    var transfer = new Transfer();
                    transfer.AddTotalCount(files.Count);
                    transfers.Enqueue(transfer);
                    service.Update();

                    AddFilesToVideoCollection(index, files);

                    transfers.ElementAt(index).IsActive = false;
                    service.Update();
                    TransferCleanup();
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
                    service.Update();

                    transfers.ElementAt(index).IsCollecting = true;
                    var files = await CollectFilesAsync(folder, includeSubfolders, Video.ExtensionTypes);
                    transfers.ElementAt(index).IsCollecting = false;
                    transfers.ElementAt(index).AddTotalCount(files.Count());
                    AddFilesToVideoCollection(index, files);

                    transfers.ElementAt(index).IsActive = false;
                    service.Update();
                    TransferCleanup();
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
                    service.Update();

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

                    service.Update();
                    TransferCleanup();
                });
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
                        video.TryLoad();
                        service.AddVideo(video);
                        transfers.ElementAt(index).Increment();
                    });
                }
                else
                {
                    foreach (var file in files)
                    {
                        var video = new Video(file.Path);
                        video.TryLoad();
                        service.AddVideo(video);
                        transfers.ElementAt(index).Increment();
                    }
                }

                transfers.ElementAt(index).IsLoading = false;
                service.Update();
            }
        }

        private void TransferCleanup()
        {
            if (transfers.All(t => !t.IsActive))
            {
                while (transfers.TryDequeue(out _)) ;
            }
        }
    }
}
