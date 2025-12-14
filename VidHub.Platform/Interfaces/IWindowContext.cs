namespace VidHub.Platform.Interfaces
{
    public interface IWindowContext
    {
        nint HWND { get; }
        bool IsActive { get; set; }
        Task OpenDisplayFormatModal();
        Task OpenLicenseModal();
        Task OpenPreviewImageFormatModal();
        Task OpenTitleFutureFormatModal();
        Task OpenTitleFormatModal();
        Task OpenVersionModal();
        Task OpenVideoRenameModal(object obj);
        bool TryEnqueue(Action callback);
    }
}