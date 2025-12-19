using VidHub.Core.Settings;
using VidHub.Services.Base;

namespace VidHub.Services.Connectors.Dialogs
{
    public class PreviewImageFormatConnector(IVideoService vs, IVidHubSettings settings) : ConnectorTemplate(vs), IPreviewImageFormatConnector
    {
        public int FixedHours { get => settings.Dialogs.PreviewImageFormat.FixedHours; set => settings.Dialogs.PreviewImageFormat.FixedHours = value; }
        public int FixedMinutes { get => settings.Dialogs.PreviewImageFormat.FixedMinutes; set => settings.Dialogs.PreviewImageFormat.FixedMinutes = value; }
        public int FixedSeconds { get => settings.Dialogs.PreviewImageFormat.FixedSeconds; set => settings.Dialogs.PreviewImageFormat.FixedSeconds = value; }
        public int FixedMilliseconds { get => settings.Dialogs.PreviewImageFormat.FixedMilliseconds; set => settings.Dialogs.PreviewImageFormat.FixedMilliseconds = value; }

        public int RelativePercentage { get => settings.Dialogs.PreviewImageFormat.RelativePercentage; set => settings.Dialogs.PreviewImageFormat.RelativePercentage = value; }
        
        public bool RelativePosition { get => settings.Dialogs.PreviewImageFormat.RelativePosition; set => settings.Dialogs.PreviewImageFormat.RelativePosition = value; }
        public bool ExtractEmbeddedImage { get => settings.Dialogs.PreviewImageFormat.ExtractEmbeddedImage; set => settings.Dialogs.PreviewImageFormat.ExtractEmbeddedImage = value; }
    }
}
