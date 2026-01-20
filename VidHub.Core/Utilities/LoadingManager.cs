using System.Collections.Concurrent;
using System.Data;
using VidHub.Core.Settings;
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
        private readonly object locker = new();
        private readonly ConcurrentQueue<CollectSource> collectQueue = new();
        private readonly ConcurrentQueue<LoadSource> loadQueue = new();
        private int loadedFileCount = 0;
        private int totalFileCount = 0;


        public event Action? CollectingFinished;
        public event Action? LoadingFinished;
        public bool IsActive => IsCollecting || IsLoading;
        public bool IsCollecting { get; private set; } = false;
        public bool IsLoading { get; private set; } = false;
        public int LoadedFileCount => loadedFileCount;
        public int TotalFileCount => totalFileCount;


        public LoadingManager()
        {
            CollectingFinished += () =>
            {
            };
            LoadingFinished += () =>
            {
                loadedFileCount = 0;
                totalFileCount = 0;
            };
        }


        public async Task QueueVideoCollecting(IEnumerable<IStorageItem> items, bool includeSubfolders, WrapActions<string> collectActions, WrapActions<string> loadActions)
        {
            await QueueVideoCollecting(items.Select(i => i.Path), includeSubfolders, collectActions, loadActions);
        }
        public async Task QueueVideoCollecting(IEnumerable<string> items, bool includeSubfolders, WrapActions<string> collectActions, WrapActions<string> loadActions)
        {
            bool shouldStartProcessing;
            collectQueue.Enqueue(new CollectSource(items, includeSubfolders, collectActions, loadActions));
            lock (locker)
            {
                shouldStartProcessing = IsCollecting;
                if (!IsCollecting)
                {
                    IsCollecting = true;
                }
            }
            if (!shouldStartProcessing)
            {
                await ProcessNextCollecting();
            }
        }

        private async Task QueueVideoLoading(string file, WrapActions<string> loadActions)
        {
            bool shouldStartProcessing;
            loadQueue.Enqueue(new LoadSource(file, loadActions));
            lock (locker)
            {
                shouldStartProcessing = IsLoading;
                if (!IsLoading)
                {
                    IsLoading = true;
                }
            }
            if (!shouldStartProcessing)
            {
                await ProcessNextLoading();
            }
        }


        private async Task ProcessNextCollecting()
        {
            while (collectQueue.TryDequeue(out CollectSource? currentCollectQueue) && currentCollectQueue is not null)
            {
                IEnumerable<string> files = currentCollectQueue.Items.Where(File.Exists);
                IEnumerable<string> folders = currentCollectQueue.Items.Where(Directory.Exists);

                foreach (string? file in files.Where(f => Video.ExtensionTypes.Contains(Path.GetExtension(f))))
                {
                    currentCollectQueue.CollectActions.PreAction(file);
                    _ = Interlocked.Increment(ref totalFileCount);
                    _ = Task.Run(async () => await QueueVideoLoading(file, currentCollectQueue.LoadActions));
                    currentCollectQueue.CollectActions.PostAction(file);
                }

                if (currentCollectQueue.IncludeSubfolders)
                {
                    foreach (string? folder in folders)
                    {
                        foreach (string file in CollectFiles(folder).Where(f => Video.ExtensionTypes.Contains(Path.GetExtension(f))))
                        {
                            currentCollectQueue.CollectActions.PreAction(file);
                            _ = Interlocked.Increment(ref totalFileCount);
                            _ = Task.Run(async () => await QueueVideoLoading(file, currentCollectQueue.LoadActions));
                            currentCollectQueue.CollectActions.PostAction(file);
                        }
                    }
                }
            }

            IsCollecting = false;
            CollectingFinished?.Invoke();
        }

        private async Task ProcessNextLoading()
        {
            while (loadQueue.TryDequeue(out LoadSource? currentLoadQueue) && currentLoadQueue is not null)
            {
                if (VidHubSettings.Instance.Performance.UseConcurrentLoading)
                {
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
                        currentLoad.PreActionInvoke();
                        _ = Interlocked.Increment(ref loadedFileCount);
                        currentLoad.PostActionInvoke();
                    });
                }
                else
                {
                    currentLoadQueue.PreActionInvoke();
                    _ = Interlocked.Increment(ref loadedFileCount);
                    currentLoadQueue.PostActionInvoke();
                }
            }

            if (!IsCollecting)
            {
                IsLoading = false;
                LoadingFinished?.Invoke();
            }
        }

        private IEnumerable<string> CollectFiles(string folder)
        {
            IEnumerable<string> selectedFiles;
            try
            {
                selectedFiles = Directory.GetFiles(folder);
            }
            catch
            {
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
            }
            catch
            {
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
