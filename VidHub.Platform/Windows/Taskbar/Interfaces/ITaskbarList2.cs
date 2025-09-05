using System.Runtime.InteropServices;

namespace VidHub.Platform.Windows.Taskbar.Interfaces
{
    [ComImport]
    [Guid("602D4995-B13A-429b-A66E-1935E44F4316")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ITaskbarList2
    {
        // ITaskbarList
        void HrInit();
        void AddTab(nint hwnd);
        void DeleteTab(nint hwnd);
        void ActivateTab(nint hwnd);
        void SetActiveAlt(nint hwnd);

        // ITaskbarList2
        void MarkFullscreenWindow(nint hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);
    }
}
