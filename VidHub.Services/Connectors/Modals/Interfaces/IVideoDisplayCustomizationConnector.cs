using VidHub.Services.Base.Interfaces;

namespace VidHub.Services.Connectors.Modals.Interfaces
{
    public interface IVideoDisplayCustomizationConnector : IUpdateService
    {
        string DateFormat { get; set; }
        string DurationDayFormat { get; set; }
        string DurationHourFormat { get; set; }
        string DurationMinuteFormat { get; set; }
        string DurationSecondFormat { get; set; }
        double PreviewImageWidth { get; set; }
        double PreviewImageHeight { get; set; }
    }
}
