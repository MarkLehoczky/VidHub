using Microsoft.Extensions.Logging;
using VidHub.Core;
using VidHub.Core.Notifications;
using VidHub.Core.Settings;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Base;
using VidHub.Services.System;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace VidHub.Services.Logics
{
    public class VideoLoadService(IVideoService service, IVidHubSettings settings, ISystemManager system) : IVideoLoadService
    {
        private readonly ILogger logger = VidHubContext.Logger;
        private bool initializedLoadingManager = false;
        private readonly LoadingManager manager = new();
        private Action<string> LoadVideo => file =>
        {
            logger.LogTrace("LoadVideo action invoked for file={File}", file);
            Video video = new(file);
            video.Load();
            if (video.Metadata.DefaultVideoStream != null)
            {
                video.Metadata.DefaultVideoStream.SetFixedResolution();
                video.Metadata.DefaultVideoStream.SetFixedFramerate();
            }
            service.Add(video);
            logger.LogDebug("Video loaded and added: {File}", file);
        };
        private Action<string> UpdateUI => _ =>
        {
            logger.LogTrace("UpdateUI action invoked");
            system.SetTaskbar(manager);
            service.Update(UpdateSections.ALL);
        };

        public bool HasActiveTransfer => manager.IsActive;
        public int LoadedFileCount => manager.LoadedFileCount;
        public int TotalFileCount => manager.TotalFileCount;
        public string TransferDescription => !manager.IsActive
            ? "No active loading..."
            : manager.IsCollecting
                ? "Collecting and loading videos"
                : "Loading videos";


        public async Task Export()
        {
            logger.LogTrace("Export called");
            InitLoadingManager();
            StorageFile? file = await FileSaver("Export");

            if (file != null)
            {
                logger.LogDebug("Exporting list to {Path}", file.Path);
                await FileIO.WriteTextAsync(file, string.Join('\n', service.Select(v => v.FilePath)));
                logger.LogInformation("Export completed to {Path}", file.Path);
            }
            else
            {
                logger.LogTrace("Export cancelled by user");
            }
        }

        public async Task Import()
        {
            logger.LogTrace("Import called");
            InitLoadingManager();
            StorageFile? file = await SingleFileOpener("Import");

            if (file != null)
            {
                logger.LogDebug("Importing from {Path}", file.Path);
                await Task.Run(async () =>
                {
                    await Task.Run(async () =>
                    {
                        IList<string> files = await FileIO.ReadLinesAsync(file);
                        WrapActions<string> collectActions = new(WrapActions<string>.NoAction, UpdateUI);
                        WrapActions<string> loadActions = new(LoadVideo, UpdateUI);
                        manager.QueueVideoCollecting(files, false, collectActions, loadActions);
                        logger.LogInformation("Import enqueued {Count} items from {Path}", files.Count, file.Path);
                    });

                });
            }
            else
            {
                logger.LogTrace("Import cancelled by user");
            }
        }

        public async Task LoadItems(IEnumerable<IStorageItem> items, bool includeSubfolders)
        {
            logger.LogTrace("LoadItems called with includeSubfolders={Include} count={Count}", includeSubfolders, items?.Count() ?? 0);
            InitLoadingManager();
            if (items.Any())
            {
                await Task.Run(async () =>
                {
                    WrapActions<string> collectActions = new(WrapActions<string>.NoAction, UpdateUI);
                    WrapActions<string> loadActions = new(LoadVideo, UpdateUI);
                    manager.QueueVideoCollecting(items, includeSubfolders, collectActions, loadActions);
                    logger.LogInformation("LoadItems enqueued {Count} items (includeSubfolders={Include})", items.Count(), includeSubfolders);
                });
            }
            else
            {
                logger.LogTrace("LoadItems called with empty items");
            }
        }

        public async Task LoadFiles()
        {
            logger.LogTrace("LoadFiles called");
            InitLoadingManager();
            IReadOnlyList<StorageFile> files = await MultiFileOpener("Load");

            if (files.Count > 0)
            {
                await Task.Run(async () =>
                {
                    WrapActions<string> collectActions = new(WrapActions<string>.NoAction, UpdateUI);
                    WrapActions<string> loadActions = new(LoadVideo, UpdateUI);
                    manager.QueueVideoCollecting(files, false, collectActions, loadActions);
                    logger.LogInformation("LoadFiles enqueued {Count} files", files.Count);
                });
            }
            else
            {
                logger.LogTrace("LoadFiles selection cancelled or no files chosen");
            }
        }

        public async Task LoadFolders(bool includeSubfolders)
        {
            logger.LogTrace("LoadFolders called includeSubfolders={Include}", includeSubfolders);
            InitLoadingManager();
            StorageFolder? folder = await FolderOpener("Load");

            if (folder != null)
            {
                await Task.Run(async () =>
                {
                    WrapActions<string> collectActions = new(WrapActions<string>.NoAction, UpdateUI);
                    WrapActions<string> loadActions = new(LoadVideo, UpdateUI);
                    manager.QueueVideoCollecting([folder], includeSubfolders, collectActions, loadActions);
                    logger.LogInformation("LoadFolders enqueued folder {Folder} includeSubfolders={Include}", folder.Path, includeSubfolders);
                });
            }
            else
            {
                logger.LogTrace("LoadFolders cancelled by user");
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

            InitializeWithWindow.Initialize(picker, VidHubContext.Window.HWND);
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

            InitializeWithWindow.Initialize(picker, VidHubContext.Window.HWND);
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

            InitializeWithWindow.Initialize(picker, VidHubContext.Window.HWND);
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

            InitializeWithWindow.Initialize(picker, VidHubContext.Window.HWND);
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

        private void InitLoadingManager()
        {
            logger.LogTrace("InitLoadingManager called, initializedLoadingManager={Initialized}", initializedLoadingManager);
            if (initializedLoadingManager)
            {
                logger.LogTrace("InitLoadingManager already initialized, returning");
                return;
            }

            initializedLoadingManager = true;
            manager.LoadingFinished += async () =>
            {
                logger.LogDebug("LoadingFinished event handler invoked");
                if (!settings.Dialogs.TitleFormat.HideTitleFormatDialog)
                {
                    _ = VidHubContext.Window;
                }
                SystemNotification notification = new()
                {
                    Title = "Video loading finished!",
                    Details = $"{service.GetAllVideos().Where(v => !v.LoadingFinished).Count()} videos were loaded successfully.",
                    Severity = NotificationSeverity.SUCCESS
                };
                notification.Display();
                logger.LogDebug("Displayed loading finished notification");
                system.SetTaskbar(manager);
                service.Update(UpdateSections.SIDEPANEL);
                service.Update(UpdateSection.VIDEOCOLLECTION);
                if (!settings.Dialogs.TitleFormat.HideTitleFormatDialog)
                {
                    await VidHubContext.Window.OpenActiveTitleFormatDialog();
                }
            };
        }
    }
}
