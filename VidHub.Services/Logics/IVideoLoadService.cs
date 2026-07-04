using Windows.Storage;

namespace VidHub.Services.Logics
{
    public interface IVideoLoadService
    {
        bool HasActiveTransfer { get; }
        int LoadedFileCount { get; }
        int TotalFileCount { get; }
        string TransferDescription { get; }

        Task Export();
        Task Import();
        Task LoadItems(IEnumerable<IStorageItem> items, bool includeSubfolders);
        Task LoadFiles();
        Task LoadFolders(bool includeSubfolders);
        void CancelLoading();
    }
}
