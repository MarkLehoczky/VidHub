using System.Runtime.InteropServices;
using VidHub.Platform.Windows.Window.Enums;

namespace VidHub.Platform.Windows.Window.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct FLASHWINFO
    {
        public uint cbSize;
        public nint hwnd;
        public FlashWindowFlags dwFlags;
        public uint uCount;
        public uint dwTimeout;
    }
}
