namespace VidHub.Services.Modals.Interfaces
{
    public interface IVideoPreviewImageCustomizationService
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
