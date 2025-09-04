using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VidHub.Platform;
using VidHub.Services.Logics.Interfaces;

namespace VidHub.ViewModels
{
    public partial class TitlebarViewModel(IVideoLoadService service) : ObservableRecipient
    {
        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task LoadFilesAsync()
        {
            await service.LoadFilesAsync();
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task LoadSingleFolderAsync()
        {
            await service.LoadFoldersAsync(false);
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task LoadAllFolderAsync()
        {
            await service.LoadFoldersAsync(true);
        }


        public TitlebarViewModel() : this(Context.MainHost.GetService<IVideoLoadService>()) { }
    }
}
