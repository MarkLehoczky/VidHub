namespace VidHub.Platform.Interfaces
{
    public interface IWindowContext
    {
        object Window { get; }
        nint HWND { get; }
        bool IsActive { get; set; }
        bool TryEnqueue(Action callback);
        Task ShowDialogAsync(object type, string title, string closeButton);
        Task ShowDialogAsync(object type, string title, string closeButton, object instance);
    }
}