using Microsoft.Extensions.Logging;

namespace VidHub.Platform.VidHubEnvironment
{
    public class VidHubContext
    {
        public static IWindowContext Window { get; set; } = new WindowContextTemplate();
        public static IHostContext Host { get; set; } = new HostContextTemplate();
        public static ILogger Logger { get; set; } = new LoggerFactory(
        [
            new ConsoleLoggerProvider(LogLevel.Debug),
            new LastRunFileLoggerProvider(LogLevel.Trace),
            new PermanentFileLoggerProvider(LogLevel.Information),
        ]).CreateLogger("VidHub.Logger");
    }
}
