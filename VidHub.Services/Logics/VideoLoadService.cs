using System.Collections.Concurrent;
using VidHub.Core;
using VidHub.Core.Helpers;
using VidHub.Core.Models;
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
    // TODO: Implement transfer amnager and proper queue system
    // TODO: Optimize video loading with yielding
    // TODO: Optimize video loading by using string instead of StorageItem
    public class VideoLoadService(IVideoService service, ISettingsService settings, ISystemManager manager) : IVideoLoadService
    {
        private readonly ConcurrentQueue<Transfer> transfers = [];
        private readonly List<int> IDCollection = [];

        public bool HasActiveTransfer => !transfers.IsEmpty;

        public string TransferDescription => transfers.Where(t => t.IsActive).All(t => t.IsCollecting)
            ? "Collecting videos"
            : transfers.Where(t => t.IsActive).Any(t => t.IsCollecting)
                ? "Collecting and loading videos"
                : "Loading videos";

        public int LoadedFileCount => transfers.Sum(t => t.LoadedCount);

        public int TotalFileCount => transfers.Sum(t => t.TotalCount);


        public async Task LoadFilesAsync()
        {
            IReadOnlyList<StorageFile> files = await MultiFileOpener("Load");

            if (files.Count > 0)
            {
                await Task.Run(() =>
                {
                    var index = transfers.Count;
                    var transfer = new Transfer
                    {
                        TotalCount = files.Count
                    };
                    transfers.Enqueue(transfer);
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidePanel);
                    service.Update(UpdateType.UpdateVideoCollection);

                    AddFilesToVideoCollection(index, files);

                    transfers.ElementAt(index).IsActive = false;
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidePanel);
                    service.Update(UpdateType.UpdateVideoCollection);
                    TransferCleanup();
                });
            }
        }

        public async Task LoadFoldersAsync(bool includeSubfolders)
        {
            StorageFolder? folder = await FolderOpener("Load");

            if (folder != null)
            {
                await Task.Run(async () =>
                {
                    var index = transfers.Count;
                    var transfer = new Transfer
                    {
                        IsCollecting = true
                    };
                    transfers.Enqueue(transfer);
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidePanel);
                    service.Update(UpdateType.UpdateVideoCollection);

                    transfers.ElementAt(index).IsCollecting = true;
                    var files = await CollectFilesAsync(folder, includeSubfolders, Video.ExtensionTypes);
                    transfers.ElementAt(index).IsCollecting = false;
                    transfers.ElementAt(index).TotalCount = files.Count();
                    AddFilesToVideoCollection(index, files);

                    transfers.ElementAt(index).IsActive = false;
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidePanel);
                    service.Update(UpdateType.UpdateVideoCollection);
                    TransferCleanup();
                });
            }
        }

        public async Task LoadItems(IEnumerable<IStorageItem> items, bool includeSubfolders)
        {
            if (items.Any())
            {
                await Task.Run(async () =>
                {
                    var index = transfers.Count;
                    var transfer = new Transfer();
                    transfers.Enqueue(transfer);
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidePanel);
                    service.Update(UpdateType.UpdateVideoCollection);

                    transfers.ElementAt(index).IsCollecting = true;
                    var files = items.OfType<StorageFile>().Where(f => Video.ExtensionTypes.Contains(f.FileType)).ToList();
                    foreach (var folder in items.OfType<StorageFolder>())
                    {
                        files.AddRange(await CollectFilesAsync(folder, includeSubfolders, Video.ExtensionTypes));
                    }
                    if (files.Count > 0)
                    {
                        transfers.ElementAt(index).IsCollecting = false;
                        transfers.ElementAt(index).TotalCount = files.Count;
                        AddFilesToVideoCollection(index, files);
                    }

                    transfers.ElementAt(index).IsActive = false;
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidePanel);
                    service.Update(UpdateType.UpdateVideoCollection);
                    TransferCleanup();
                });
            }
        }

        public async Task ImportCollectionAsync()
        {
            StorageFile? file = await SingleFileOpener("Import");

            if (file != null)
            {
                await Task.Run(() =>
                {
                    var index = transfers.Count;
                    var transfer = new Transfer();
                    transfers.Enqueue(transfer);
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidePanel);
                    service.Update(UpdateType.UpdateVideoCollection);

                    transfers.ElementAt(index).IsCollecting = true;

                    var rawContent = File.ReadLines(file.Path);
                    var files = rawContent.Select(p =>
                    {
                        var awaiter = StorageFile.GetFileFromPathAsync(p);
                        awaiter.Wait();
                        return awaiter.GetResults();
                    });

                    transfer.TotalCount = files.Count();
                    transfers.Enqueue(transfer);
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidePanel);
                    service.Update(UpdateType.UpdateVideoCollection);

                    AddFilesToVideoCollection(index, files);

                    transfers.ElementAt(index).IsActive = false;
                    manager.SetTaskbar(transfers);
                    service.Update(UpdateType.UpdateSidePanel);
                    service.Update(UpdateType.UpdateVideoCollection);
                    TransferCleanup();
                });
            }
        }
        public async Task ExportCollectionAsync()
        {
            StorageFile? file = await FileSaver("Export");

            if (file != null)
                await FileIO.WriteTextAsync(file, string.Join('\n', service.Select(v => v.FilePath)));
        }


        private static async Task<StorageFile?> SingleFileOpener(string commitButtonText)
        {
            var picker = new FileOpenPicker
            {
                CommitButtonText = commitButtonText,
                SuggestedStartLocation = PickerLocationId.HomeGroup,
                ViewMode = PickerViewMode.Thumbnail
            };
            picker.FileTypeFilter.Add(".vhc");

            InitializeWithWindow.Initialize(picker, Context.Window.HWND);
            return await picker.PickSingleFileAsync();
        }

        private static async Task<IReadOnlyList<StorageFile>> MultiFileOpener(string commitButtonText)
        {
            var picker = new FileOpenPicker
            {
                CommitButtonText = commitButtonText,
                SuggestedStartLocation = PickerLocationId.HomeGroup,
                ViewMode = PickerViewMode.Thumbnail
            };
            foreach (var filter in Video.ExtensionTypes)
                picker.FileTypeFilter.Add(filter);

            InitializeWithWindow.Initialize(picker, Context.Window.HWND);
            return await picker.PickMultipleFilesAsync();
        }

        private static async Task<StorageFolder?> FolderOpener(string commitButtonText)
        {
            var picker = new FolderPicker
            {
                CommitButtonText = commitButtonText,
                SuggestedStartLocation = PickerLocationId.VideosLibrary,
                ViewMode = PickerViewMode.Thumbnail
            };

            InitializeWithWindow.Initialize(picker, Context.Window.HWND);
            return await picker.PickSingleFolderAsync();
        }

        // TODO: Implement auto file name increment
        private static async Task<StorageFile?> FileSaver(string commitButtonText)
        {
            var picker = new FileSavePicker
            {
                CommitButtonText = commitButtonText,
                DefaultFileExtension = ".vhc",
                SuggestedFileName = "VidHub Collection",
                SuggestedStartLocation = PickerLocationId.HomeGroup
            };
            picker.FileTypeChoices.Add("VidHub Collection", [".vhc"]);

            InitializeWithWindow.Initialize(picker, Context.Window.HWND);
            return await picker.PickSaveFileAsync();
        }


        private static async Task<IEnumerable<StorageFile>> CollectFilesAsync(StorageFolder folder, bool includeSubfolders, List<string> fileTypeFilters)
        {
            var files = new List<StorageFile>();
            files.AddRange(await folder.GetFilesAsync());

            if (includeSubfolders)
                foreach (var subfolder in await folder.GetFoldersAsync())
                    files.AddRange(await CollectFilesAsync(subfolder, includeSubfolders, fileTypeFilters));

            return files.Where(f => fileTypeFilters.Contains(f.FileType, StringComparer.OrdinalIgnoreCase));
        }

        private void AddFilesToVideoCollection(int index, IEnumerable<StorageFile> files)
        {
            transfers.ElementAt(index).IsLoading = true;

            if (settings.Organizer.Global.EnableConcurrentLoading)
            {
                Parallel.ForEach(files, file =>
                {
                    var video = new Video(file.Path);
                    if (settings.PreviewImageCustomization.RelativePosition)
                        video.Load(settings.Organizer.Global.EnableCacheLoading, settings.PreviewImageCustomization.FramePercentage);

                    else
                        video.Load(settings.Organizer.Global.EnableConcurrentLoading, settings.PreviewImageCustomization.FrameTime);

                    settings.TitleCustomization.ChangeTitle(video);
                    service.Add(video);
                    transfers.ElementAt(index).LoadedCount++;
                    manager.SetTaskbar(transfers);
                    IDCollection.Add(video.ID);
                });
            }
            else
            {
                foreach (var file in files)
                {
                    var video = new Video(file.Path);
                    if (settings.PreviewImageCustomization.RelativePosition)
                        video.Load(settings.Organizer.Global.EnableCacheLoading, settings.PreviewImageCustomization.FramePercentage);

                    else
                        video.Load(settings.Organizer.Global.EnableConcurrentLoading, settings.PreviewImageCustomization.FrameTime);

                    settings.TitleCustomization.ChangeTitle(video);
                    service.Add(video);
                    transfers.ElementAt(index).LoadedCount++;
                    manager.SetTaskbar(transfers);
                    IDCollection.Add(video.ID);
                }
            }

            transfers.ElementAt(index).IsLoading = false;
            manager.SetTaskbar(transfers);
            service.Update(UpdateType.UpdateSidePanel);
            service.Update(UpdateType.UpdateVideoCollection);
        }


        private void TransferCleanup()
        {
            if (transfers.All(t => !t.IsActive))
            {
                if (!settings.TitleCustomization.DontShowTitleCustomizationAgain)
                    Context.Window.TryEnqueue(() => Context.Window.ShowDialogAsync(ModalType.CustomizeTitleFormat, "Customize video title", "Confirm", new Tuple<bool, IEnumerable<int>>(false, IDCollection)));

                manager.DisplayToast("Video loading finished!", $"{LoadedFileCount} videos were loaded successfully.");
                while (transfers.TryDequeue(out _)) ;
                service.Update(UpdateType.UpdateSidePanel);
                service.Update(UpdateType.UpdateVideoCollection);
                IDCollection.Clear();
            }
        }
    }
}
