namespace VidHub.Platform.Environment
{
    internal class HostContextTemplate : IHostContext
    {
        public T GetService<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }
}
