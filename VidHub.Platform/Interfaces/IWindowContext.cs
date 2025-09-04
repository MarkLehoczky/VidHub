namespace VidHub.Platform.Interfaces
{
    public interface IWindowContext
    {
        object Window { get; }
        nint HWND { get; }
        bool TryEnqueue(Action callback);
    }
}
