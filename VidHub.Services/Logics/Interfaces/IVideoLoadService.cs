using Windows.Storage;

namespace VidHub.Services.Logics.Interfaces
{
    public interface IVideoLoadService
    {
        bool HasActiveTransfer { get; }
        int LoadedFileCount { get; }
        int TotalFileCount { get; }
        string TransferDescription { get; }
        Task ExportCollectionAsync();
        Task ImportCollectionAsync();
        Task LoadItems(IEnumerable<IStorageItem> items, bool includeSubfolders);
        Task LoadFilesAsync();
        Task LoadFoldersAsync(bool includeSubfolders);
    }
}
