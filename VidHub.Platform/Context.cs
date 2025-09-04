using VidHub.Platform.Interfaces;

namespace VidHub.Platform
{
    public class Context
    {
        public static IWindowContext MainWindow { get; set; }
        public static IHostContext MainHost { get; set; }
    }
}
