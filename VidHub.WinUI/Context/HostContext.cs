using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.WinUI.Context
{
    public class HostContext(IHost host) : IHostContext
    {
        public T? GetService<T>() where T : class
        {
            try
            {
                return host.Services.GetRequiredService<T>();
            }
            catch (Exception ex)
            {
                try
                {
                    ILogger? logger = host.Services.GetService<ILoggerFactory>()?.CreateLogger("HostContext");
                    logger?.LogWarning(ex, "Requested service {Service} not available", typeof(T).FullName);
                }
                catch { }

                return default;
            }
        }
    }
}
