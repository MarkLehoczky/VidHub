using System.Drawing;
using System.Runtime.InteropServices;
using VidHub.Platform.Windows.Taskbar.Classes;
using VidHub.Platform.Windows.Taskbar.Enums;
using VidHub.Platform.Windows.Taskbar.Interfaces;
using VidHub.Platform.Windows.Window.Enums;
using VidHub.Platform.Windows.Window.Native;
using VidHub.Platform.Windows.Window.Structs;

namespace VidHub.Platform.Windows
{
    public partial class TaskbarManager : IDisposable
    {
        private readonly ITaskbarList4 _taskbarInstance = (ITaskbarList4)new CTaskbarList();

        public TaskbarManager()
        {
            _taskbarInstance.HrInit();
        }

        #region Progress Bar
        public void SetProgressState(nint windowHandle, TaskbarProgressState state) =>
            _taskbarInstance.SetProgressState(windowHandle, (TBPFLAG)state);

        public void SetProgressValue(nint windowHandle, ulong current, ulong maximum) =>
            _taskbarInstance.SetProgressValue(windowHandle, current, maximum);

        public void ClearProgress(nint windowHandle) =>
            SetProgressState(windowHandle, TaskbarProgressState.NoProgress);
        #endregion

        #region Flash Window
        public void FlashWindow(nint windowHandle, int count = 3, uint timeout = 0)
        {
            var flashInfo = new FLASHWINFO
            {
                cbSize = (uint)Marshal.SizeOf(typeof(FLASHWINFO)),
                hwnd = windowHandle,
                dwFlags = FlashWindowFlags.FLASHW_ALL,
                uCount = (uint)count,
                dwTimeout = timeout
            };

            NativeMethods.FlashWindowEx(ref flashInfo);
        }
        #endregion

        #region Overlay Icon / Badge
        public void SetOverlayIcon(nint windowHandle, Icon icon, string description) =>
            _taskbarInstance.SetOverlayIcon(windowHandle, icon?.Handle ?? nint.Zero, description);

        public void ClearOverlayIcon(nint windowHandle) =>
            SetOverlayIcon(windowHandle, null, null);
        #endregion

        public void Dispose()
        {
            if (_taskbarInstance != null && Marshal.IsComObject(_taskbarInstance))
                Marshal.ReleaseComObject(_taskbarInstance);
        }
    }
}
