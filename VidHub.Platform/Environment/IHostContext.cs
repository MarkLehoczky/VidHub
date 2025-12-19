namespace VidHub.Platform.Environment
{
    public interface IHostContext
    {
        T GetService<T>() where T : class;
    }
}
