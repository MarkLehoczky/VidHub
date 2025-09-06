namespace VidHub.Platform.Interfaces
{
    public interface IWindowContext
    {
        object Window { get; }
        nint HWND { get; }
        bool IsActive { get; set; }
        bool TryEnqueue(Action callback);
    }
}
