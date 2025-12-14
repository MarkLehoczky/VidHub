using VidHub.Core.Settings;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Connectors.Modals.Interfaces;

namespace VidHub.Services.Connectors.Modals
{
    public class VideoPreviewImageCustomizationConnector(IVideoService vs, IVidHubSettings settings) : ServiceTemplate(vs), IVideoPreviewImageCustomizationConnector
    {
        public int FixedHours { get => settings.Modals.PreviewImageFormat.FixedHours; set => settings.Modals.PreviewImageFormat.FixedHours = value; }
        public int FixedMinutes { get => settings.Modals.PreviewImageFormat.FixedMinutes; set => settings.Modals.PreviewImageFormat.FixedMinutes = value; }
        public int FixedSeconds { get => settings.Modals.PreviewImageFormat.FixedSeconds; set => settings.Modals.PreviewImageFormat.FixedSeconds = value; }
        public int FixedMilliseconds { get => settings.Modals.PreviewImageFormat.FixedMilliseconds; set => settings.Modals.PreviewImageFormat.FixedMilliseconds = value; }

        public int RelativePercentage { get => settings.Modals.PreviewImageFormat.RelativePercentage; set => settings.Modals.PreviewImageFormat.RelativePercentage = value; }

        public bool RelativePosition { get => settings.Modals.PreviewImageFormat.RelativePosition; set => settings.Modals.PreviewImageFormat.RelativePosition = value; }
        public bool ExtractEmbeddedImage { get => settings.Modals.PreviewImageFormat.ExtractEmbeddedImage; set => settings.Modals.PreviewImageFormat.ExtractEmbeddedImage = value; }
        public bool UseFileContentHash { get => settings.General.UseFileContentHash; set => settings.General.UseFileContentHash = value; }
    }
}
