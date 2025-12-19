using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VidHub.Platform.Environment;

namespace VidHub.WinUI.Context
{
    public class HostContext(IHost host) : IHostContext
    {
        public T GetService<T>() where T : class
        {
            return host.Services.GetRequiredService<T>();
        }
    }
}
