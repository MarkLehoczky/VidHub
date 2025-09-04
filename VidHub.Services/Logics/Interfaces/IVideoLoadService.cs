using Windows.Storage;

namespace VidHub.Services.Logics.Interfaces
{
    public interface IVideoLoadService
    {
        Task LoadFilesAsync();
        Task LoadFoldersAsync(bool includeSubfolders);
        Task LoadExternal(IEnumerable<IStorageItem> items);
    }
}
