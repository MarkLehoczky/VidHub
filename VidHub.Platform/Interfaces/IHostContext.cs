namespace VidHub.Platform.Interfaces
{
    public interface IHostContext
    {
        object Host { get; }
        T GetService<T>() where T : class;
    }
}
