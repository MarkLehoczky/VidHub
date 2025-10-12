using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VidHub.Platform;
using VidHub.Services.Logics.Interfaces;

namespace VidHub.ViewModels.Modals
{
    public partial class ThumbnailCustomizationViewModel(IThumbnailCustomizationService service) : ObservableRecipient
    {
        public int Hours
        {
            get => service.Hours;
            set => service.Hours = value;
        }
        public int Minutes
        {
            get => service.Minutes;
            set => service.Minutes = value;
        }
        public int Seconds
        {
            get => service.Seconds;
            set => service.Seconds = value;
        }
        public int Milliseconds
        {
            get => service.Milliseconds;
            set => service.Milliseconds = value;
        }
        public int FramePercentage
        {
            get => service.FramePercentage;
            set => service.FramePercentage = value;
        }

        public bool FixedPosition => !service.RelativePosition;

        public bool RelativePosition
        {
            get => service.RelativePosition;
            set
            {
                service.RelativePosition = value;
                OnPropertyChanged(nameof(FixedPosition));
                OnPropertyChanged(nameof(RelativePosition));
            }
        }

        public bool InactiveThumbnailAction => !ActiveThumbnailAction;
        public bool ActiveThumbnailAction { get; set; } = false;


        [RelayCommand(CanExecute = nameof(InactiveThumbnailAction))]
        private async Task RemoveThumbnailsAsync()
        {
            ChangeButtonExecution(true);
            await service.RemoveAllThumbnailsAsync();
            ChangeButtonExecution(false);
        }

        [RelayCommand(CanExecute = nameof(InactiveThumbnailAction))]
        private async Task ExtractThumbnailsAsync()
        {
            ChangeButtonExecution(true);
            await service.ExtractLoadedVideoThumbnailsAsync();
            ChangeButtonExecution(false);
        }

        private void ChangeButtonExecution(bool status)
        {
            ActiveThumbnailAction = status;
            RemoveThumbnailsCommand.NotifyCanExecuteChanged();
            ExtractThumbnailsCommand.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(ActiveThumbnailAction));
            //Context.MainHost.GetService<IMainService>().Update(UpdateType.ResetVideoCollection);
        }

        public ThumbnailCustomizationViewModel() : this(Context.MainHost.GetService<IThumbnailCustomizationService>()) { }
    }
}
