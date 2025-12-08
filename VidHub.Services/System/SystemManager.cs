using Microsoft.Toolkit.Uwp.Notifications;
using VidHub.Core.Manager;
using VidHub.Core.Notifications.System;
using VidHub.Core.Settings;
using VidHub.Platform;
using VidHub.Platform.Windows;
using VidHub.Platform.Windows.Taskbar.Enums;
using VidHub.Services.System.Interfaces;
using Windows.UI.Notifications;

namespace VidHub.Services.System
{
    public class SystemManager(IVidHubSettings settings) : ISystemManager
    {
        private readonly TaskbarManager taskbar = new();


        public void DisplayToast(SystemNotification notification)
        {
            if (!settings.DisplaySystemNotification(notification))
            {
                return;
            }

            ToastContentBuilder content = new();
            content.AddText(notification.Title);
            content.AddText(notification.Message);
            _ = content.GetToastContent();

            ToastNotification toast = new(content.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

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
