using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VidHub.Platform;
using VidHub.Services.Logics.Interfaces;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.ViewModels
{
    public partial class TitlebarViewModel(IVideoLoadService service, ISettingsService settings) : ObservableRecipient
    {
        private bool CanOpenSidePanel() => !settings.OpenPanel;
        private bool CanCloseSidePanel() => settings.OpenPanel;


        public bool SystemNotifications
        {
            get => settings.SystemNotifications;
            set
            {
                if (settings.SystemNotifications == value) return;
                settings.SystemNotifications = value;
            }
        }

        public bool CacheLoad
        {
            get => settings.CacheLoad;
            set => settings.CacheLoad = value;
        }

        public bool ConcurrentVideoLoading
        {
            get => settings.ConcurrentVideoLoading;
            set => settings.ConcurrentVideoLoading = value;
        }

        public bool KeepFilterStatus
        {
            get => settings.KeepFilterStatus;
            set => settings.KeepFilterStatus = value;
        }

        public bool LiveTextFiltering
        {
            get => settings.LiveTextFiltering;
            set => settings.LiveTextFiltering = value;
        }

        public bool CaseSensitiveTextFiltering
        {
            get => settings.CaseSensitiveTextFiltering;
            set => settings.CaseSensitiveTextFiltering = value;
        }

        public bool TextSuggestions
        {
            get => settings.TextSuggestions;
            set => settings.TextSuggestions = value;
        }


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


        [RelayCommand(CanExecute = nameof(CanOpenSidePanel))]
        private void OpenSidePanel()
        {
            settings.OpenPanel = true;
            OpenSidePanelCommand.NotifyCanExecuteChanged();
            CloseSidePanelCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanCloseSidePanel))]
        private void CloseSidePanel()
        {
            settings.OpenPanel = false;
            OpenSidePanelCommand.NotifyCanExecuteChanged();
            CloseSidePanelCommand.NotifyCanExecuteChanged();
        }


        public TitlebarViewModel() : this(
            Context.MainHost.GetService<IVideoLoadService>(),
            Context.MainHost.GetService<ISettingsService>()) { }
    }
}
