namespace VidHub.Platform.VidHubEnvironment
{
    public interface IWindowContext
    {
        nint HWND { get; }
        bool IsActive { get; set; }
        Task OpenDisplayFormatDialog();
        Task OpenLicensesDialog();
        Task OpenPreviewImageFormatDialog();
        Task OpenPassiveTitleFormatDialog();
        Task OpenActiveTitleFormatDialog();
        Task OpenVersionsDialog();
        Task OpenRenameDialog(object obj);
        bool TryEnqueue(Action callback);
    }
}