using VidHub.Platform.Interfaces;
using VidHub.Platform.Interfaces.Templates;

namespace VidHub.Platform
{
    public class Context
    {
        public static IWindowContext Window { get; set; } = new WindowContextTemplate();
        public static IHostContext Host { get; set; } = new HostContextTemplate();
    }
}
