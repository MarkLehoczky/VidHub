using System.Runtime.InteropServices;
using VidHub.Platform.Windows.Window.Structs;

namespace VidHub.Platform.Windows.Window.Native
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FlashWindowEx(ref FLASHWINFO pwfi);
    }
}
