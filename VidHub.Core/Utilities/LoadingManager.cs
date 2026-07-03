using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using VidHub.Core.Settings;
using VidHub.Platform.VidHubEnvironment;
using Windows.Storage;

namespace VidHub.Core.Utilities
{
    internal class CollectSource(IEnumerable<string> items, bool includeSubfolders, WrapActions<string> collectActions, WrapActions<string> loadActions)
    {
        public IEnumerable<string> Items { get; } = items;
        public bool IncludeSubfolders { get; } = includeSubfolders;
        public WrapActions<string> CollectActions { get; } = collectActions;
        public WrapActions<string> LoadActions { get; } = loadActions;
    }


    internal class LoadSource(string file, WrapActions<string> loadActions)
    {
        public string File { get; } = file;
        public WrapActions<string> LoadActions { get; } = loadActions;

        public void PreActionInvoke()
        {
            LoadActions.PreAction(File);
        }
        public void PostActionInvoke()
        {
            LoadActions.PostAction(File);
        }
    }


    public class LoadingManager
    {
        private readonly ILogger logger = VidHubContext.Logger;
        private readonly object locker = new();
        private readonly ConcurrentQueue<CollectSource> collectQueue = new();
        private readonly ConcurrentQueue<LoadSource> loadQueue = new();
        private Task? collectTask = null;
        private Task? loadTask = null;
        private int loadedFileCount = 0;
        private int totalFileCount = 0;


        public event Action? CollectingFinished;
        public event Action? LoadingFinished;
        public readonly CancellationTokenSource LoadCancellation = new();
        public bool IsActive => IsCollecting || IsLoading;
        public bool IsCollecting { get; private set; } = false;
        public bool IsLoading { get; private set; } = false;
        public int LoadedFileCount => loadedFileCount;
        public int TotalFileCount => totalFileCount;


        public LoadingManager()
        {
            logger.LogTrace("LoadingManager initialized");
            CollectingFinished += () =>
            {
                logger.LogDebug("CollectingFinished invoked (no-op handler)");
            };
            LoadingFinished += () =>
            {
                loadedFileCount = 0;
                totalFileCount = 0;
                logger.LogDebug("LoadingFinished invoked, counters reset");
            };
        }


        public void QueueVideoCollecting(IEnumerable<IStorageItem> items, bool includeSubfolders, WrapActions<string> collectActions, WrapActions<string> loadActions)
        {
            logger.LogTrace("QueueVideoCollecting called with IStorageItem collection, includeSubfolders={Include}", includeSubfolders);
            QueueVideoCollecting(items.Select(i => i.Path), includeSubfolders, collectActions, loadActions);
        }
        public void QueueVideoCollecting(IEnumerable<string> items, bool includeSubfolders, WrapActions<string> collectActions, WrapActions<string> loadActions)
        {
            logger.LogTrace("QueueVideoCollecting called with string collection, includeSubfolders={Include}", includeSubfolders);
            collectQueue.Enqueue(new CollectSource(items, includeSubfolders, collectActions, loadActions));


            lock (locker)
            {
                if (collectTask is null || collectTask.IsCompleted)
                {
                    logger.LogDebug("Collection started");
                    IsCollecting = true;
                    collectTask = Task.Run(ProcessNextCollecting, LoadCancellation.Token);
                }
                else
                {
                    logger.LogTrace("Collection already in progress, enqueued sources will be processed later");
                }
            }
        }

        private void QueueVideoLoading(string file, WrapActions<string> loadActions)
        {
            logger.LogTrace("QueueVideoLoading called for file={File}", file);
            loadQueue.Enqueue(new LoadSource(file, loadActions));

            lock (locker)
            {
                if (loadTask is null || loadTask.IsCompleted)
                {
                    logger.LogDebug("Loading started");
                    IsLoading = true;
                    loadTask = Task.Run(ProcessNextLoading, LoadCancellation.Token);
                }
                else
                {
                    logger.LogTrace("Loading already in progress, file enqueued");
                }
            }
        }


        private async Task ProcessNextCollecting()
        {
            logger.LogTrace("ProcessNextCollecting entered");
            while (collectQueue.TryDequeue(out CollectSource? currentCollectQueue) && currentCollectQueue is not null)
            {
                logger.LogDebug("Processing CollectSource with {ItemCount} items, includeSubfolders={Include}", currentCollectQueue.Items.Count(), currentCollectQueue.IncludeSubfolders);
                IEnumerable<string> files = currentCollectQueue.Items.Where(File.Exists);
                IEnumerable<string> folders = currentCollectQueue.Items.Where(Directory.Exists);

                foreach (string? file in files.Where(f => Video.ExtensionTypes.Contains(Path.GetExtension(f))))
                {
                    currentCollectQueue.CollectActions.PreAction(file);
                    _ = Interlocked.Increment(ref totalFileCount);
                    _ = Task.Run(() => QueueVideoLoading(file, currentCollectQueue.LoadActions));
                    currentCollectQueue.CollectActions.PostAction(file);
                }

                if (currentCollectQueue.IncludeSubfolders)
                {
                    logger.LogDebug("IncludeSubfolders is true, collecting files from subfolders");
                    foreach (string? folder in folders)
                    {
                        foreach (string file in CollectFiles(folder).Where(f => Video.ExtensionTypes.Contains(Path.GetExtension(f))))
                        {
                            currentCollectQueue.CollectActions.PreAction(file);
                            _ = Interlocked.Increment(ref totalFileCount);
                            _ = Task.Run(() => QueueVideoLoading(file, currentCollectQueue.LoadActions));
                            currentCollectQueue.CollectActions.PostAction(file);
                        }
                    }
                }
                else
                {
                    logger.LogTrace("IncludeSubfolders is false, skipping subfolder collection");
                }
            }

            IsCollecting = false;
            logger.LogDebug("Collection queue drained, IsCollecting set to false");
            CollectingFinished?.Invoke();
        }

        private void ProcessNextLoading()
        {
            logger.LogTrace("ProcessNextLoading entered");
            while (loadQueue.TryDequeue(out LoadSource? currentLoadQueue) && currentLoadQueue is not null)
            {
                logger.LogDebug("Processing LoadSource for file={File}", currentLoadQueue.File);
                if (VidHubSettings.Instance.Performance.UseConcurrentLoading)
                {
                    logger.LogDebug("Using concurrent loading mode");
                    List<LoadSource> batch = [currentLoadQueue];
                    for (int i = 1; i < (Environment.ProcessorCount * 3); i++)
                    {
                        if (loadQueue.TryDequeue(out LoadSource? nextLoadQueue) && nextLoadQueue is not null)
                        {
                            batch.Add(nextLoadQueue);
                        }
                    }
                    _ = Parallel.ForEach(batch, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, currentLoad =>
                    {
                        logger.LogTrace("Using concurrent loading mode for file={File}", currentLoad.File);
                        currentLoad.PreActionInvoke();
                        _ = Interlocked.Increment(ref loadedFileCount);
                        currentLoad.PostActionInvoke();
                    });
                }
                else
                {
                    logger.LogTrace("Using sequential loading mode for file={File}", currentLoadQueue.File);
                    currentLoadQueue.PreActionInvoke();
                    _ = Interlocked.Increment(ref loadedFileCount);
                    currentLoadQueue.PostActionInvoke();
                }
            }

            if (!IsCollecting)
            {
                IsLoading = false;
                logger.LogDebug("Loading queue drained, IsLoading set to false");
                LoadingFinished?.Invoke();
            }
            else
            {
                logger.LogTrace("Collecting still in progress, loading will continue after collecting finishes");
            }
        }

        private IEnumerable<string> CollectFiles(string folder)
        {
            logger.LogTrace("CollectFiles entered for folder={Folder}", folder);
            IEnumerable<string> selectedFiles;
            try
            {
                selectedFiles = Directory.GetFiles(folder);
                logger.LogDebug("Found {Count} files in folder {Folder}", selectedFiles.Count(), folder);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to get files from folder {Folder}", folder);
                selectedFiles = [];
            }
            foreach (string selectedFile in selectedFiles)
            {
                yield return selectedFile;
            }

            IEnumerable<string> selectedFolders;
            try
            {
                selectedFolders = Directory.GetDirectories(folder);
                logger.LogDebug("Found {Count} subfolders in folder {Folder}", selectedFolders.Count(), folder);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to get subfolders from folder {Folder}", folder);
                selectedFolders = [];
            }

            foreach (string selectedFolder in selectedFolders)
            {
                foreach (string selectedFolderFile in CollectFiles(selectedFolder))
                {
                    yield return selectedFolderFile;
                }
            }
        }
    }
}
