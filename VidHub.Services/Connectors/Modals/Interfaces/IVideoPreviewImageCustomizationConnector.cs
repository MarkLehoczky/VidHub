using VidHub.Services.Base.Interfaces;

namespace VidHub.Services.Connectors.Modals.Interfaces
{
    public interface IVideoPreviewImageCustomizationConnector : IUpdateService
    {
        int Hours { get; set; }
        int Milliseconds { get; set; }
        int Minutes { get; set; }
        int Percentage { get; set; }
        bool RelativePosition { get; set; }
        int Seconds { get; set; }
        Task ExtractLoadedVideoPreviewImagesAsync();
        Task RemoveAllPreviewImagesAsync();
    }
}
