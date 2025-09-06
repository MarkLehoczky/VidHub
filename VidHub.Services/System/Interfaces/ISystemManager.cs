using VidHub.Core.Helpers;

namespace VidHub.Services.System.Interfaces
{
    public interface ISystemManager
    {
        void DisplayToast(params string[] texts);
        void FlashWindow();
        void SetIndeterminateProgressbar();
        void SetProgressbar(int completed, int total);
        void ClearProgressbar();
        void SetTaskbar(IEnumerable<Transfer> transfers);
    }
}
