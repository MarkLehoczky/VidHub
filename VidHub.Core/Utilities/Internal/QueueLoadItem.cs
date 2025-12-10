using VidHub.Core.Utilities.Helper;

namespace VidHub.Core.Utilities.Internal
{
    internal class QueueLoadItem(string file, WrapActions<string> loadActions)
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
}
