namespace VidHub.Platform.Windows.Window.Enums
{
    [Flags]
    internal enum FlashWindowFlags : uint
    {
        FLASHW_STOP = 0,
        FLASHW_CAPTION = 1,
        FLASHW_TRAY = 2,
        FLASHW_ALL = FLASHW_CAPTION | FLASHW_TRAY,
        FLASHW_TIMER = 4,
        FLASHW_TIMERNOFG = 12
    }
}
