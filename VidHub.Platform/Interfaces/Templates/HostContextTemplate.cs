namespace VidHub.Platform.Interfaces.Templates
{
    internal class HostContextTemplate : IHostContext
    {
        public T GetService<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }
}
