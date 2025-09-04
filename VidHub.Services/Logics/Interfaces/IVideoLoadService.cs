namespace VidHub.Services.Logics.Interfaces
{
    public interface IVideoLoadService
    {
        Task LoadFilesAsync();
        Task LoadFoldersAsync(bool includeSubfolders);
    }
}
