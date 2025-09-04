using System.Collections.Concurrent;
using VidHub.Core;
using VidHub.Core.Helpers;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace VidHub.Services.Logics
{
    public class VideoLoadService(IMainService service) : IVideoLoadService
    {
        private readonly object locker = new();
        private readonly ConcurrentQueue<Transfer> transfers = [];


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
                    transfers.Enqueue(transfer);
                    service.Update();

                    var files = await CollectFilesAsync(folder, includeSubfolders, Video.ExtensionTypes);
                    transfers.ElementAt(index).AddTotalCount(files.Count());
                    AddFilesToVideoCollection(index, files);

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

                foreach (var file in files)
                {
                    var video = new Video(file.Path);
                    video.TryLoad();
                    service.AddVideo(video);
                    transfers.ElementAt(index).Increment();
                }

                transfers.ElementAt(index).IsLoading = false;
                transfers.ElementAt(index).IsActive = false;
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
