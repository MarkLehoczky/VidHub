using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VidHub.Core.Helpers;
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
            set => settings.SystemNotifications = value;
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

        public bool ShowTitles
        {
            get => settings.ShowTitles;
            set => settings.ShowTitles = value;
        }
        public bool ShowDates
        {
            get => settings.ShowDates;
            set => settings.ShowDates = value;
        }
        public bool ShowDurations
        {
            get => settings.ShowDurations;
            set => settings.ShowDurations = value;
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

        [RelayCommand]
        private async Task ImportCollectionAsync()
        {
            await service.ImportCollectionAsync();
        }

        [RelayCommand]
        private async Task ExportCollectionAsync()
        {
            await service.ExportCollectionAsync();
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

        [RelayCommand]
        private async Task FormatDateAsync()
        {
            await Context.MainWindow.ShowDialogAsync(ModalType.FormatDate, "Date format", "Close");
        }

        [RelayCommand]
        private async Task FormatDurationAsync()
        {
            await Context.MainWindow.ShowDialogAsync(ModalType.FormatDuration, "Duration format", "Close");
        }


        public TitlebarViewModel() : this(
            Context.MainHost.GetService<IVideoLoadService>(),
            Context.MainHost.GetService<ISettingsService>())
        { }
    }
}
