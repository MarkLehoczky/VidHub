namespace VidHub.Services.Logics.Interfaces
{
    public interface IThumbnailCustomizationService
    {
        bool RelativePosition { get; set; }
        int Hours { get; set; }
        int Minutes { get; set; }
        int Seconds { get; set; }
        int Milliseconds { get; set; }
        int FramePercentage { get; set; }
        Task ExtractLoadedVideoThumbnailsAsync();
        Task RemoveAllThumbnailsAsync();
    }
}
