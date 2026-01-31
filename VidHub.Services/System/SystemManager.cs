using VidHub.Core.Utilities;
using VidHub.Platform.Windows;
using VidHub.Platform.Windows.Taskbar.Enums;
using Microsoft.Extensions.Logging;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.Services.System
{
    public class SystemManager : ISystemManager
    {
        private readonly TaskbarManager taskbar = new();
        private readonly ILogger logger = VidHubContext.Logger;


        public void FlashWindow()
        {
            logger.LogTrace("FlashWindow called");
            if (VidHubContext.Window.IsActive)
            {
                logger.LogDebug("Window is active; skipping flash");
                return;
            }

            TaskbarManager.FlashWindow(VidHubContext.Window.HWND);
            logger.LogInformation("Flashed window to notify user");
        }

        public void SetIndeterminateProgressbar()
        {
            logger.LogTrace("SetIndeterminateProgressbar called");
            taskbar.SetProgressState(VidHubContext.Window.HWND, TaskbarProgressState.Indeterminate);
        }

        public void SetProgressbar(int completed, int total)
        {
            logger.LogTrace("SetProgressbar called completed={Completed} total={Total}", completed, total);
            taskbar.SetProgressState(VidHubContext.Window.HWND, TaskbarProgressState.Normal);
            taskbar.SetProgressValue(VidHubContext.Window.HWND, (ulong)completed, (ulong)total);
        }

        public void ClearProgressbar()
        {
            logger.LogTrace("ClearProgressbar called");
            taskbar.ClearProgress(VidHubContext.Window.HWND);
        }

        public void SetTaskbar(LoadingManager manager)
        {
            logger.LogTrace("SetTaskbar called IsCollecting={Collecting} IsLoading={Loading} IsActive={Active}", manager.IsCollecting, manager.IsLoading, manager.IsActive);
            if (manager.IsCollecting)
            {
                SetIndeterminateProgressbar();
            }
            else if (!manager.IsCollecting && manager.IsLoading)
            {
                SetProgressbar(manager.LoadedFileCount, manager.TotalFileCount);
            }

            if (!manager.IsActive)
            {
                ClearProgressbar();
                FlashWindow();
            }
        }
    }
}
