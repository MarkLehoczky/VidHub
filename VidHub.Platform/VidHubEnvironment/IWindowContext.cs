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
        Task OpenResolutionDialog();
        Task OpenFramerateDialog();
        Task OpenTagsDialog();
        Task OpenRenameDialog(object obj);
        Task OpenModifyTagsDialog(object obj);
        bool TryEnqueue(Action callback);
    }
}