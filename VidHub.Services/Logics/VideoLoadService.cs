using VidHub.Core;
using VidHub.Core.Notifications;
using VidHub.Core.Settings;
using VidHub.Core.Utilities;
using VidHub.Core.Utilities.Helper;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;
using VidHub.Services.System.Interfaces;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace VidHub.Services.Logics
{
    public class VideoLoadService(IVideoService service, IVidHubSettings settings, ISystemManager system) : IVideoLoadService
    {
        private readonly LoadingManager manager = new();

        public bool HasActiveTransfer => manager.IsActive;

        public int LoadedFileCount => manager.LoadedFileCount;

        public int TotalFileCount => manager.TotalFileCount;

        public string TransferDescription => !manager.IsActive
            ? "No active loading..."
            : manager.IsCollecting
                ? "Collecting and loading videos"
                : "Loading videos";


        public async Task ExportCollectionAsync()
        {
            InitLoadingManager();
            StorageFile? file = await FileSaver("Export");

            if (file != null)
            {
                await FileIO.WriteTextAsync(file, string.Join('\n', service.Select(v => v.FilePath)));
            }
        }

        public async Task ImportCollectionAsync()
        {
            InitLoadingManager();
            StorageFile? file = await SingleFileOpener("Import");

            if (file != null)
            {
                await Task.Run(async () =>
                {
                    await Task.Run(async () =>
                    {
                        IList<string> files = await FileIO.ReadLinesAsync(file);
                        WrapActions<string> collectActions = new(WrapActions<string>.NoAction, UpdateUI);
                        WrapActions<string> loadActions = new(LoadVideo, UpdateUI);
                        await manager.QueueVideoCollecting(files, false, collectActions, loadActions);
                    });

                });
            }
        }

        public async Task LoadItems(IEnumerable<IStorageItem> items, bool includeSubfolders)
        {
            InitLoadingManager();
            if (items.Any())
            {
                await Task.Run(async () =>
                {
                    WrapActions<string> collectActions = new(WrapActions<string>.NoAction, UpdateUI);
                    WrapActions<string> loadActions = new(LoadVideo, UpdateUI);
                    await manager.QueueVideoCollecting(items, includeSubfolders, collectActions, loadActions);
                });
            }
        }

        public async Task LoadFilesAsync()
        {
            InitLoadingManager();
            IReadOnlyList<StorageFile> files = await MultiFileOpener("Load");

            if (files.Count > 0)
            {
                await Task.Run(async () =>
                {
                    WrapActions<string> collectActions = new(WrapActions<string>.NoAction, UpdateUI);
                    WrapActions<string> loadActions = new(LoadVideo, UpdateUI);
                    await manager.QueueVideoCollecting(files, false, collectActions, loadActions);
                });
            }
        }

        public async Task LoadFoldersAsync(bool includeSubfolders)
        {
            InitLoadingManager();
            StorageFolder? folder = await FolderOpener("Load");

            if (folder != null)
            {
                await Task.Run(async () =>
                {
                    WrapActions<string> collectActions = new(WrapActions<string>.NoAction, UpdateUI);
                    WrapActions<string> loadActions = new(LoadVideo, UpdateUI);
                    await manager.QueueVideoCollecting([folder], includeSubfolders, collectActions, loadActions);
                });
            }
        }


        private static async Task<StorageFile?> SingleFileOpener(string commitButtonText)
        {
            FileOpenPicker picker = new()
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
            FileOpenPicker picker = new()
            {
                CommitButtonText = commitButtonText,
                SuggestedStartLocation = PickerLocationId.HomeGroup,
                ViewMode = PickerViewMode.Thumbnail
            };
            foreach (string filter in Video.ExtensionTypes)
            {
                picker.FileTypeFilter.Add(filter);
            }

            InitializeWithWindow.Initialize(picker, Context.Window.HWND);
            return await picker.PickMultipleFilesAsync();
        }

        private static async Task<StorageFolder?> FolderOpener(string commitButtonText)
        {
            FolderPicker picker = new()
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
            FileSavePicker picker = new()
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
            List<StorageFile> files = [.. await folder.GetFilesAsync()];

            if (includeSubfolders)
            {
                foreach (StorageFolder? subfolder in await folder.GetFoldersAsync())
                {
                    files.AddRange(await CollectFilesAsync(subfolder, includeSubfolders, fileTypeFilters));
                }
            }

            return files.Where(f => fileTypeFilters.Contains(f.FileType, StringComparer.OrdinalIgnoreCase));
        }


        private Action<string> LoadVideo => file =>
        {
            Video video = new(file);
            video.Load();
            service.Add(video);
        };

        private Action<string> UpdateUI => _ =>
        {
            system.SetTaskbar(manager);
            service.Update(UpdateSections.SIDEPANEL);
            service.Update(UpdateSection.VIDEOCOLLECTION);
        };


        private bool initializedLoadingManager = false;

        private void InitLoadingManager()
        {
            if (initializedLoadingManager)
            {
                return;
            }

            initializedLoadingManager = true;
            manager.LoadingFinished += async () =>
            {
                if (!settings.Modals.TitleFormat.HideTitleCustomization)
                {
                    _ = Context.Window;
                }
                SystemNotification notification = new()
                {
                    Title = "Video loading finished!",
                    Details = $"{service.GetAllVideos().Where(v => !v.LoadingFinished).Count()} videos were loaded successfully.",
                    Severity = NotificationSeverity.SUCCESS
                };
                notification.Display();
                service.Update(UpdateSections.SIDEPANEL);
                service.Update(UpdateSection.VIDEOCOLLECTION);
                if (!settings.Modals.TitleFormat.HideTitleCustomization)
                {
                    await Context.Window.OpenTitleFormatModal();
                }
            };
        }
    }
}
