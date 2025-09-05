using VidHub.Core.Helpers;
using VidHub.Platform;
using VidHub.Platform.Windows;
using VidHub.Platform.Windows.Taskbar.Enums;
using VidHub.Services.Settings.Interfaces;
using VidHub.Services.System.Interfaces;

namespace VidHub.Services.System
{
    public class SystemManager(ISettingsService settings) : ISystemManager
    {
        private readonly TaskbarManager taskbar = new();

        public void FlashWindow()
        {
            if (Context.MainWindow.IsActive) return;

            taskbar.FlashWindow(Context.MainWindow.HWND);
        }

        public void SetIndeterminateProgressbar()
        {
            taskbar.SetProgressState(Context.MainWindow.HWND, TaskbarProgressState.Indeterminate);
        }

        public void SetProgressbar(int completed, int total)
        {
            taskbar.SetProgressState(Context.MainWindow.HWND, TaskbarProgressState.Normal);
            taskbar.SetProgressValue(Context.MainWindow.HWND, (ulong)completed, (ulong)total);
        }

        public void ClearProgressbar()
        {
            taskbar.ClearProgress(Context.MainWindow.HWND);
        }

        public void SetTaskbar(IEnumerable<Transfer> transfers)
        {
            if (transfers.Where(t => t.IsActive).Any(t => t.IsCollecting))
            {
                SetIndeterminateProgressbar();
            }
            else if (transfers.Where(t => t.IsActive).All(t => !t.IsCollecting))
            {
                SetProgressbar(transfers.Sum(t => t.LoadedCount), transfers.Sum(t => t.TotalCount));
            }
            if (!transfers.Any(t => t.IsActive))
            {
                ClearProgressbar();
                FlashWindow();
            }
        }
    }
}
