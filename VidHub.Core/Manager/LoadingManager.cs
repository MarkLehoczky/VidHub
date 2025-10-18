using System.Collections.Concurrent;
using System.Data;
using Windows.Storage;

namespace VidHub.Core.Manager
{
    public class LoadingManager
    {
        internal class QueueCollectItem(IEnumerable<string> items, bool includeSubfolders, WrapActions<string> collectActions, WrapActions<string> loadActions)
        {
            public IEnumerable<string> Items { get; } = items;
            public bool IncludeSubfolders { get; } = includeSubfolders;
            public WrapActions<string> CollectActions { get; } = collectActions;
            public WrapActions<string> LoadActions { get; } = loadActions;
        }

        internal class CollectedItem(string file, WrapActions<string> loadActions)
        {
            public string File { get; } = file;
            public WrapActions<string> LoadActions { get; } = loadActions;
        }

        internal class QueueLoadItem(IEnumerable<string> files, WrapActions<string> loadActions)
        {
            public IEnumerable<string> Files { get; } = files;
            public WrapActions<string> LoadActions { get; } = loadActions;

            public QueueLoadItem(IEnumerable<CollectedItem> items) : this(items.Select(x => x.File), items.FirstOrDefault()?.LoadActions ?? new WrapActions<string>(_ => { })) { }
        }


        private readonly ConcurrentQueue<QueueCollectItem> collectQueue = new();
        private readonly ConcurrentQueue<CollectedItem> collectedItems = new();
        private readonly ConcurrentQueue<QueueLoadItem> loadQueue = new();
        private bool concurrentLoading = true;
        private const int loadBatchCount = 15;
        private int loadedFileCount = 0;
        private int totalFileCount = 0;


        public event Action? CollectingFinished;
        public event Action? LoadingFinished;

        public bool ConcurrentLoading { get => concurrentLoading; set => concurrentLoading = value; }
        public bool IsActive => IsCollecting || IsLoading;
        public bool IsCollecting { get; private set; } = false;
        public bool IsLoading { get; private set; } = false;
        public int LoadedFileCount => loadedFileCount;
        public int TotalFileCount => totalFileCount;


        public LoadingManager()
        {
            CollectingFinished += async () =>
            {
                await QueueRemainingVideoLoading();
            };

            LoadingFinished += () =>
            {
                loadedFileCount = 0;
                totalFileCount = 0;
            };
        }

        public async Task QueueVideoCollecting(IEnumerable<IStorageItem> items, bool includeSubfolders, WrapActions<string> collectActions, WrapActions<string> loadActions)
        {
            collectQueue.Enqueue(new QueueCollectItem(items.Select(i => i.Path), includeSubfolders, collectActions, loadActions));
            if (!IsCollecting) await ProcessNextCollecting();
        }
        public async Task QueueVideoCollecting(IEnumerable<string> items, bool includeSubfolders, WrapActions<string> collectActions, WrapActions<string> loadActions)
        {
            collectQueue.Enqueue(new QueueCollectItem(items, includeSubfolders, collectActions, loadActions));
            if (!IsCollecting) await ProcessNextCollecting();
        }

        public async Task QueueRemainingVideoLoading()
        {
            if (!collectedItems.IsEmpty)
            {
                loadQueue.Enqueue(new QueueLoadItem(collectedItems));
                collectedItems.Clear();
                if (!IsLoading) await ProcessNextLoading();
            }
        }
        public async Task QueueVideoLoading(IEnumerable<string> files, WrapActions<string> loadActions)
        {
            foreach (var file in files.Where(l => Path.Exists(l) && Video.ExtensionTypes.Contains(Path.GetExtension(l))))
            {
                await QueueVideoLoading(file, loadActions);
                Interlocked.Increment(ref totalFileCount);
            }
        }
        public async Task QueueVideoLoading(string file, WrapActions<string> loadActions)
        {
            collectedItems.Enqueue(new CollectedItem(file, loadActions));

            if (collectedItems.Count >= loadBatchCount)
            {
                List<CollectedItem> loadBatch = [];
                for (int i = 0; i < loadBatchCount; i++)
                {
                    if (collectedItems.TryDequeue(out var batchItem))
                    {
                        loadBatch.Add(batchItem);
                    }
                }
                loadQueue.Enqueue(new QueueLoadItem(loadBatch));
            }

            if (!IsLoading) await ProcessNextLoading();
        }


        private async Task ProcessNextCollecting()
        {
            if (!collectQueue.TryDequeue(out var currentCollectQueue) || currentCollectQueue is null)
            {
                currentCollectQueue = null;
                IsCollecting = false;
                CollectingFinished?.Invoke();
                return;
            }

            IsCollecting = true;


            var files = currentCollectQueue.Items.Where(File.Exists).Where(f => Video.ExtensionTypes.Contains(Path.GetExtension(f)));
            var folders = currentCollectQueue.Items.Where(Directory.Exists);

            foreach (var file in files)
            {
                currentCollectQueue.CollectActions.PreAction(file);
                Interlocked.Increment(ref totalFileCount);
                _ = Task.Run(async () => await QueueVideoLoading(file, currentCollectQueue.LoadActions));
                currentCollectQueue.CollectActions.PostAction(file);
            }

            if (currentCollectQueue.IncludeSubfolders)
            {
                foreach (var folder in folders)
                {
                    foreach (var file in CollectFiles(folder))
                    {
                        if (Video.ExtensionTypes.Contains(Path.GetExtension(file)))
                        {
                            currentCollectQueue.CollectActions.PreAction(file);
                            Interlocked.Increment(ref totalFileCount);
                            _ = Task.Run(async () => await QueueVideoLoading(file, currentCollectQueue.LoadActions));
                            currentCollectQueue.CollectActions.PostAction(file);
                        }
                    }
                }
            }

            await ProcessNextCollecting();
        }

        private async Task ProcessNextLoading()
        {
            if (!loadQueue.TryDequeue(out var currentLoadQueue) || currentLoadQueue is null)
            {
                currentLoadQueue = null;
                IsLoading = false;
                if (!IsCollecting) LoadingFinished?.Invoke();
                return;
            }

            IsLoading = true;


            if (ConcurrentLoading)
            {
                Parallel.ForEach(currentLoadQueue.Files, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, file =>
                {
                    currentLoadQueue.LoadActions.PreAction(file);
                    Interlocked.Increment(ref loadedFileCount);
                    currentLoadQueue.LoadActions.PostAction(file);
                });
            }
            else
            {
                foreach (var file in currentLoadQueue.Files)
                {
                    currentLoadQueue.LoadActions.PreAction(file);
                    Interlocked.Increment(ref loadedFileCount);
                    currentLoadQueue.LoadActions.PostAction(file);
                }
            }

            await ProcessNextLoading();
        }

        private IEnumerable<string> CollectFiles(string folder)
        {
            IEnumerable<string> selectedFiles;
            try { selectedFiles = Directory.GetFiles(folder); }
            catch { selectedFiles = []; }
            foreach (var selectedFile in selectedFiles)
                yield return selectedFile;

            IEnumerable<string> selectedFolders;
            try { selectedFolders = Directory.GetDirectories(folder); }
            catch { selectedFolders = []; }

            foreach (var selectedFolder in selectedFolders)
                foreach (var selectedFolderFile in CollectFiles(selectedFolder))
                    yield return selectedFolderFile;
        }
    }
}
