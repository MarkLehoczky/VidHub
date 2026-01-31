namespace VidHub.Platform.VidHubEnvironment
{
    internal class HostContextTemplate : IHostContext
    {
        public T GetService<T>() where T : class
        {
            return default;
        }
    }
}
