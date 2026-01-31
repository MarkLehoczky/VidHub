using VidHub.Core.Utilities;
using VidHub.Services.Connectors.Dialogs;
using Microsoft.Extensions.Logging;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.ViewModels.Dialogs
{
    public partial class PreviewImageFormatViewModel(IPreviewImageFormatConnector connector) : ViewModelTemplate(connector)
    {
        private readonly ILogger logger = VidHubContext.Logger;
        public PreviewImageFormatViewModel() : this(VidHubContext.Host.GetService<IPreviewImageFormatConnector>()) { }


        public int Hours
        {
            get => connector.FixedHours;
            set { connector.FixedHours = value; logger.LogDebug("Preview fixed hours set to {Value}", value); }
        }
        public int Minutes
        {
            get => connector.FixedMinutes;
            set { connector.FixedMinutes = value; logger.LogDebug("Preview fixed minutes set to {Value}", value); }
        }
        public int Seconds
        {
            get => connector.FixedSeconds;
            set { connector.FixedSeconds = value; logger.LogDebug("Preview fixed seconds set to {Value}", value); }
        }
        public int Milliseconds
        {
            get => connector.FixedMilliseconds;
            set { connector.FixedMilliseconds = value; logger.LogDebug("Preview fixed milliseconds set to {Value}", value); }
        }
        public int Percentage
        {
            get => connector.RelativePercentage;
            set { connector.RelativePercentage = value; logger.LogDebug("Preview relative percentage set to {Value}", value); }
        }

        public bool FixedPosition => !connector.RelativePosition;
        public bool ExtractEmbeddedImageCommand
        {
            get => connector.ExtractEmbeddedImage;
            set { connector.ExtractEmbeddedImage = value; logger.LogDebug("ExtractEmbeddedImage set to {Value}", value); }
        }

        public bool RelativePosition
        {
            get => connector.RelativePosition;
            set
            {
                connector.RelativePosition = value;
                OnPropertyChanged(nameof(FixedPosition));
                OnPropertyChanged(nameof(RelativePosition));
                logger.LogDebug("RelativePosition set to {Value}", value);
            }
        }


        public override void Update(IEnumerable<UpdateSection> sections) { }
    }
}
