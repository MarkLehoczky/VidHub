using VidHub.Services.Base.Interfaces;

namespace VidHub.Services.Connectors.Modals.Interfaces
{
    public interface IVideoPreviewImageCustomizationConnector : IUpdateService
    {
        bool ExtractEmbeddedImage { get; set; }
        int FixedHours { get; set; }
        int FixedMilliseconds { get; set; }
        int FixedMinutes { get; set; }
        int RelativePercentage { get; set; }
        bool RelativePosition { get; set; }
        int FixedSeconds { get; set; }
        bool UseFileContentHash { get; set; }
    }
}
