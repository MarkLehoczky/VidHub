namespace VidHub.Platform.Interfaces
{
    public interface IWindowContext
    {
        nint HWND { get; }
        bool IsActive { get; set; }
        bool TryEnqueue(Action callback);
        Task ShowDialogAsync(string type, string title, string closeButton);
        Task ShowDialogAsync(string type, string title, string closeButton, object obj);
    }
}