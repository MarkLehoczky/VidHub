using Windows.Storage;

namespace VidHub.Services.Logics.Interfaces
{
    public interface IVideoLoadService
    {
        string TransferDescription { get; }
        bool HasTransfer { get; }
        bool HasActiveTransfer { get; }
        int LoadedCount { get; }
        int TotalCount { get; }
        Task LoadFilesAsync();
        Task LoadFoldersAsync(bool includeSubfolders);
        Task LoadExternal(IEnumerable<IStorageItem> items);
    }
}
