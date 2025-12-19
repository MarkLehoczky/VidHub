using VidHub.Services.Base;

namespace VidHub.Services.Connectors.Dialogs
{
    public interface IDisplayFormatConnector : IUpdateService
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
