namespace VidHub.Platform.VidHubEnvironment
{
    public interface IHostContext
    {
        T GetService<T>() where T : class;
    }
}
