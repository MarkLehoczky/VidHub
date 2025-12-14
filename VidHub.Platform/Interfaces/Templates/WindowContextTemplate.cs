
namespace VidHub.Platform.Interfaces.Templates
{
    public class WindowContextTemplate : IWindowContext
    {
        public nint HWND => throw new NotImplementedException();

        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task OpenDisplayFormatModal()
        {
            throw new NotImplementedException();
        }

        public Task OpenLicenseModal()
        {
            throw new NotImplementedException();
        }

        public Task OpenPreviewImageFormatModal()
        {
            throw new NotImplementedException();
        }

        public Task OpenTitleFormatModal()
        {
            throw new NotImplementedException();
        }

        public Task OpenTitleFutureFormatModal()
        {
            throw new NotImplementedException();
        }

        public Task OpenVersionModal()
        {
            throw new NotImplementedException();
        }

        public Task OpenVideoRenameModal(object obj)
        {
            throw new NotImplementedException();
        }

        public bool TryEnqueue(Action callback)
        {
            throw new NotImplementedException();
        }
    }
}
