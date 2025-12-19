using VidHub.Core.Utilities;
using VidHub.Platform.Environment;
using VidHub.Services.Connectors.Dialogs;

namespace VidHub.ViewModels.Dialogs
{
    public partial class PreviewImageFormatViewModel(IPreviewImageFormatConnector connector) : ViewModelTemplate(connector)
    {
        public PreviewImageFormatViewModel() : this(Context.Host.GetService<IPreviewImageFormatConnector>()) { }


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


        public override void Update(IEnumerable<UpdateSection> sections) { }
    }
}
