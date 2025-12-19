using VidHub.Core.Utilities;
using VidHub.Platform.Environment;
using VidHub.Platform.Windows;
using VidHub.Platform.Windows.Taskbar.Enums;

namespace VidHub.Services.System
{
    public class SystemManager : ISystemManager
    {
        private readonly TaskbarManager taskbar = new();


        public void FlashWindow()
        {
            if (Context.Window.IsActive)
            {
                return;
            }

            TaskbarManager.FlashWindow(Context.Window.HWND);
        }

        public void SetIndeterminateProgressbar()
        {
            taskbar.SetProgressState(Context.Window.HWND, TaskbarProgressState.Indeterminate);
        }

        public void SetProgressbar(int completed, int total)
        {
            taskbar.SetProgressState(Context.Window.HWND, TaskbarProgressState.Normal);
            taskbar.SetProgressValue(Context.Window.HWND, (ulong)completed, (ulong)total);
        }

        public void ClearProgressbar()
        {
            taskbar.ClearProgress(Context.Window.HWND);
        }

        public void SetTaskbar(LoadingManager manager)
        {
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
