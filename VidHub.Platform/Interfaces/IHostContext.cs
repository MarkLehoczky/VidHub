namespace VidHub.Platform.Interfaces
{
    public interface IHostContext
    {
        T GetService<T>() where T : class;
    }
}
