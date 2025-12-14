using VidHub.Core.Utilities;

namespace VidHub.Services.System.Interfaces
{
    public interface ISystemManager
    {
        void ClearProgressbar();
        void FlashWindow();
        void SetIndeterminateProgressbar();
        void SetProgressbar(int completed, int total);
        void SetTaskbar(LoadingManager manager);
    }
}
