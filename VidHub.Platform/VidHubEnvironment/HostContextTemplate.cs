namespace VidHub.Platform.VidHubEnvironment
{
    internal class HostContextTemplate : IHostContext
    {
        public T? GetService<T>() where T : class
        {
            return default;
        }

        public void Update<T>(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void Update<T>(params T[] items)
        {
            throw new NotImplementedException();
        }
    }
}
