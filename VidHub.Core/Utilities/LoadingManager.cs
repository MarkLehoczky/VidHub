using System.Collections.Concurrent;
using System.Data;
using VidHub.Core.Settings;
using VidHub.Core.Utilities.Helper;
using VidHub.Core.Utilities.Internal;
using Windows.Storage;

namespace VidHub.Core.Utilities
{
    public class LoadingManager
    {
        private readonly ConcurrentQueue<QueueCollectItem> collectQueue = new();
        private readonly ConcurrentQueue<QueueLoadItem> loadQueue = new();
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
            collectQueue.Enqueue(new QueueCollectItem(items, includeSubfolders, collectActions, loadActions));
            if (!IsCollecting)
            {
                await ProcessNextCollecting();
            }
        }

        private async Task QueueVideoLoading(string file, WrapActions<string> loadActions)
        {
            loadQueue.Enqueue(new QueueLoadItem(file, loadActions));
            if (!IsLoading)
            {
                await ProcessNextLoading();
            }
        }


        private async Task ProcessNextCollecting()
        {
            if (!collectQueue.TryDequeue(out QueueCollectItem? currentCollectQueue) || currentCollectQueue is null)
            {
                currentCollectQueue = null;
                IsCollecting = false;
                CollectingFinished?.Invoke();
                return;
            }

            IsCollecting = true;


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

            await ProcessNextCollecting();
        }

        private async Task ProcessNextLoading()
        {
            if (!loadQueue.TryDequeue(out QueueLoadItem? currentLoadQueue) || currentLoadQueue is null)
            {
                currentLoadQueue = null;
                IsLoading = false;
                if (!IsCollecting)
                {
                    LoadingFinished?.Invoke();
                }

                return;
            }

            IsLoading = true;


            if (VidHubSettings.Instance.Performance.UseConcurrentLoading)
            {
                List<QueueLoadItem> batch = [currentLoadQueue];
                for (int i = 1; i < (Environment.ProcessorCount * 3); i++)
                {
                    if (loadQueue.TryDequeue(out QueueLoadItem? nextLoadQueue) && nextLoadQueue is not null)
                    {
                        batch.Add(nextLoadQueue);
                    }
                }
                _ = Parallel.ForEach(batch, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, loadItem =>
                {
                    loadItem.PreActionInvoke();
                    _ = Interlocked.Increment(ref loadedFileCount);
                    loadItem.PostActionInvoke();
                });
            }
            else
            {
                currentLoadQueue.PreActionInvoke();
                _ = Interlocked.Increment(ref loadedFileCount);
                currentLoadQueue.PostActionInvoke();
            }

            await ProcessNextLoading();
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
