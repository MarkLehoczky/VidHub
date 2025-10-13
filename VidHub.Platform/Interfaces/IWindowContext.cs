using VidHub.Core.Helpers;

namespace VidHub.Platform.Interfaces
{
    public interface IWindowContext
    {
        nint HWND { get; }
        bool IsActive { get; set; }
        bool TryEnqueue(Action callback);
        Task ShowDialogAsync(ModalType type, string title, string closeButton);
        Task ShowDialogAsync(ModalType type, string title, string closeButton, object obj);
    }
}