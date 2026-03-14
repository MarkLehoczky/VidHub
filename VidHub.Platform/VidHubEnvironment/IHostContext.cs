namespace VidHub.Platform.VidHubEnvironment
{
    public interface IHostContext
    {
        T GetService<T>() where T : class;
        void Update<T>(params T[] items);
        void Update<T>(IEnumerable<T> items);
    }
}
