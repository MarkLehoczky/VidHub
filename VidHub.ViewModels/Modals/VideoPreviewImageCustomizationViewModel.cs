using CommunityToolkit.Mvvm.Input;
using VidHub.Core.Enums;
using VidHub.Platform;
using VidHub.Services.Connectors.Modals.Interfaces;
using VidHub.ViewModels.Base;

namespace VidHub.ViewModels.Modals
{
    public partial class VideoPreviewImageCustomizationViewModel(IVideoPreviewImageCustomizationConnector connector) : ViewModelTemplate(connector)
    {
        public VideoPreviewImageCustomizationViewModel() : this(Context.Host.GetService<IVideoPreviewImageCustomizationConnector>()) { }


        public int Hours
        {
            get => connector.Hours;
            set => connector.Hours = value;
        }
        public int Minutes
        {
            get => connector.Minutes;
            set => connector.Minutes = value;
        }
        public int Seconds
        {
            get => connector.Seconds;
            set => connector.Seconds = value;
        }
        public int Milliseconds
        {
            get => connector.Milliseconds;
            set => connector.Milliseconds = value;
        }
        public int Percentage
        {
            get => connector.Percentage;
            set => connector.Percentage = value;
        }

        public bool FixedPosition => !connector.RelativePosition;
        public bool ExtractEmbeddedImageCommand
        {
            get => connector.ExtractEmbeddedImageCommand;
            set => connector.ExtractEmbeddedImageCommand = value;
        }
        public bool UseContentHash
        {
            get => connector.UseContentHash;
            set => connector.UseContentHash = value;
        }

        public bool RelativePosition
        {
            get => connector.RelativePosition;
            set
            {
                connector.RelativePosition = value;
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
            await connector.RemoveAllPreviewImagesAsync();
            ChangeButtonExecution(false);
        }

        [RelayCommand(CanExecute = nameof(InactiveThumbnailAction))]
        private async Task ExtractLoadedVideoPreviewImagesAsync()
        {
            ChangeButtonExecution(true);
            await connector.ExtractLoadedVideoPreviewImagesAsync();
            ChangeButtonExecution(false);
        }

        private void ChangeButtonExecution(bool status)
        {
            ActiveThumbnailAction = status;
            RemoveAllPreviewImagesCommand.NotifyCanExecuteChanged();
            ExtractLoadedVideoPreviewImagesCommand.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(ActiveThumbnailAction));
        }


        public override void Update(UpdateType type) { }
    }
}
