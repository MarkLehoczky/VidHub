using VidHub.Core.Enums;

namespace VidHub.Platform.Interfaces.Templates
{
    public class WindowContextTemplate : IWindowContext
    {
        public nint HWND => throw new NotImplementedException();

        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task ShowDialogAsync(ModalType type, string title, string closeButton)
        {
            throw new NotImplementedException();
        }

        public Task ShowDialogAsync(ModalType type, string title, string closeButton, object obj)
        {
            throw new NotImplementedException();
        }

        public bool TryEnqueue(Action callback)
        {
            throw new NotImplementedException();
        }
    }
}
