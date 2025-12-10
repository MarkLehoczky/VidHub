using VidHub.Core.Utilities.Helper;

namespace VidHub.Core.Utilities.Internal
{
    internal class QueueCollectItem(IEnumerable<string> items, bool includeSubfolders, WrapActions<string> collectActions, WrapActions<string> loadActions)
    {
        public IEnumerable<string> Items { get; } = items;
        public bool IncludeSubfolders { get; } = includeSubfolders;
        public WrapActions<string> CollectActions { get; } = collectActions;
        public WrapActions<string> LoadActions { get; } = loadActions;
    }
}
