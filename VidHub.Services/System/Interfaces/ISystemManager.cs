using VidHub.Core.Manager;
using VidHub.Core.Notifications.System;

namespace VidHub.Services.System.Interfaces
{
    public interface ISystemManager
    {
        void ClearProgressbar();
        void DisplayToast(SystemNotification notification);
        void FlashWindow();
        void SetIndeterminateProgressbar();
        void SetProgressbar(int completed, int total);
        void SetTaskbar(LoadingManager manager);
    }
}
