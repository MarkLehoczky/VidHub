using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VidHub.Platform;
using VidHub.Services.Modals.Interfaces;

namespace VidHub.ViewModels.Modals
{
    public partial class VideoPreviewImageCustomizationViewModel(IVideoPreviewImageCustomizationService service) : ObservableRecipient
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
        public int Percentage
        {
            get => service.Percentage;
            set => service.Percentage = value;
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
        private async Task RemoveAllPreviewImagesAsync()
        {
            ChangeButtonExecution(true);
            await service.RemoveAllPreviewImagesAsync();
            ChangeButtonExecution(false);
        }

        [RelayCommand(CanExecute = nameof(InactiveThumbnailAction))]
        private async Task ExtractLoadedVideoPreviewImagesAsync()
        {
            ChangeButtonExecution(true);
            await service.ExtractLoadedVideoPreviewImagesAsync();
            ChangeButtonExecution(false);
        }

        private void ChangeButtonExecution(bool status)
        {
            ActiveThumbnailAction = status;
            RemoveAllPreviewImagesCommand.NotifyCanExecuteChanged();
            ExtractLoadedVideoPreviewImagesCommand.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(ActiveThumbnailAction));
            //Context.MainHost.GetService<IMainService>().Update(UpdateType.ResetVideoCollection);
        }

        public VideoPreviewImageCustomizationViewModel() : this(Context.Host.GetService<IVideoPreviewImageCustomizationService>()) { }
    }
}
