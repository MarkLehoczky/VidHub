using VidHub.Core.Utilities;
using VidHub.Services.Connectors.Dialogs;
using Microsoft.Extensions.Logging;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.ViewModels.Dialogs
{
    public partial class DisplayFormatViewModel(IDisplayFormatConnector connector) : ViewModelTemplate(connector)
    {
        private readonly ILogger logger = VidHubContext.Logger;
        public DisplayFormatViewModel() : this(VidHubContext.Host.GetService<IDisplayFormatConnector>()) { }


        public string DateFormat
        {
            get => connector.DateFormat;
            set { connector.DateFormat = value; logger.LogDebug("DateFormat set to {Format}", value); }
        }

        public string DurationDayFormat
        {
            get => connector.DurationDayFormat;
            set { connector.DurationDayFormat = value; logger.LogDebug("DurationDayFormat set to {Format}", value); }
        }
        public string DurationHourFormat
        {
            get => connector.DurationHourFormat;
            set { connector.DurationHourFormat = value; logger.LogDebug("DurationHourFormat set to {Format}", value); }
        }
        public string DurationMinuteFormat
        {
            get => connector.DurationMinuteFormat;
            set { connector.DurationMinuteFormat = value; logger.LogDebug("DurationMinuteFormat set to {Format}", value); }
        }
        public string DurationSecondFormat
        {
            get => connector.DurationSecondFormat;
            set { connector.DurationSecondFormat = value; logger.LogDebug("DurationSecondFormat set to {Format}", value); }
        }


        public double PreviewImageWidth
        {
            get => connector.PreviewImageWidth;
            set { connector.PreviewImageWidth = value; logger.LogDebug("PreviewImageWidth set to {Width}", value); }
        }
        public double PreviewImageHeight
        {
            get => connector.PreviewImageHeight;
            set { connector.PreviewImageHeight = value; logger.LogDebug("PreviewImageHeight set to {Height}", value); }
        }


        public override void Update(IEnumerable<UpdateSection> sections) { }
    }
}
