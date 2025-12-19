namespace VidHub.Platform.Environment
{
    public class Context
    {
        public static IWindowContext Window { get; set; } = new WindowContextTemplate();
        public static IHostContext Host { get; set; } = new HostContextTemplate();
    }
}
