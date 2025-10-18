using VidHub.Core.Manager;

namespace VidHub.Services.System.Interfaces
{
    public interface ISystemManager
    {
        void ClearProgressbar();
        void DisplayToast(params string[] texts);
        void FlashWindow();
        void SetIndeterminateProgressbar();
        void SetProgressbar(int completed, int total);
        void SetTaskbar(LoadingManager manager);
    }
}
