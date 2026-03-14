using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Base;

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

        public void Update<T>(params T[] items)
        {
            Update(items.AsEnumerable());
        }
        public void Update<T>(IEnumerable<T> items)
        {
            if (typeof(T) == typeof(UpdateSection) && VidHubContext.Window.IsActive)
            {
                var convertedItems = items as IEnumerable<UpdateSection> ?? [];
                GetService<IVideoService>()?.Update(convertedItems);
            }

        }
    }
}
