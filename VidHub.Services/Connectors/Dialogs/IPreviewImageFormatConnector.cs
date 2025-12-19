using VidHub.Services.Base;

namespace VidHub.Services.Connectors.Dialogs
{
    public interface IPreviewImageFormatConnector : IUpdateService
    {
        bool ExtractEmbeddedImage { get; set; }
        int FixedHours { get; set; }
        int FixedMilliseconds { get; set; }
        int FixedMinutes { get; set; }
        int RelativePercentage { get; set; }
        bool RelativePosition { get; set; }
        int FixedSeconds { get; set; }
    }
}
