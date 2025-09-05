using System.Runtime.InteropServices;

namespace VidHub.Platform.Windows.Window.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int left, top, right, bottom;
    }
}
