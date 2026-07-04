using System.Reflection.Metadata.Ecma335;

namespace VidHub.Platform.VidHubEnvironment
{
    public class WindowContextTemplate : IWindowContext
    {
        public nint HWND => throw new NotImplementedException();

        public bool IsActive { get => false; set { } }

        public Task OpenActiveTitleFormatDialog()
        {
            throw new NotImplementedException();
        }

        public Task OpenDisplayFormatDialog()
        {
            throw new NotImplementedException();
        }

        public Task OpenLicensesDialog()
        {
            throw new NotImplementedException();
        }

        public Task OpenPassiveTitleFormatDialog()
        {
            throw new NotImplementedException();
        }

        public Task OpenPreviewImageFormatDialog()
        {
            throw new NotImplementedException();
        }

        public Task OpenVersionsDialog()
        {
            throw new NotImplementedException();
        }

        public Task OpenRenameDialog(object obj)
        {
            throw new NotImplementedException();
        }

        public bool TryEnqueue(Action callback)
        {
            throw new NotImplementedException();
        }

        public Task OpenTagsDialog()
        {
            throw new NotImplementedException();
        }

        public Task OpenModifyTagsDialog(object obj)
        {
            throw new NotImplementedException();
        }

        public Task OpenResolutionDialog()
        {
            throw new NotImplementedException();
        }

        public Task OpenFramerateDialog()
        {
            throw new NotImplementedException();
        }

        public Task<bool> OpenCloseInterruptedDialog()
        {
            throw new NotImplementedException();
        }
    }
}
