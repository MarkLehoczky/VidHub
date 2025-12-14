using VidHub.Core.Utilities.Helper;
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
            get => connector.FixedHours;
            set => connector.FixedHours = value;
        }
        public int Minutes
        {
            get => connector.FixedMinutes;
            set => connector.FixedMinutes = value;
        }
        public int Seconds
        {
            get => connector.FixedSeconds;
            set => connector.FixedSeconds = value;
        }
        public int Milliseconds
        {
            get => connector.FixedMilliseconds;
            set => connector.FixedMilliseconds = value;
        }
        public int Percentage
        {
            get => connector.RelativePercentage;
            set => connector.RelativePercentage = value;
        }

        public bool FixedPosition => !connector.RelativePosition;
        public bool ExtractEmbeddedImageCommand
        {
            get => connector.ExtractEmbeddedImage;
            set => connector.ExtractEmbeddedImage = value;
        }
        public bool UseContentHash
        {
            get => connector.UseFileContentHash;
            set => connector.UseFileContentHash = value;
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


        public override void Update(IEnumerable<UpdateSection> sections) { }
    }
}
