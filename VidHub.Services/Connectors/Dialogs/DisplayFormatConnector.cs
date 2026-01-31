using VidHub.Core.Settings;
using VidHub.Core.Utilities;
using VidHub.Services.Base;
using Microsoft.Extensions.Logging;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.Services.Connectors.Dialogs
{
    public class DisplayFormatConnector(IVideoService vs, IVidHubSettings settings) : ConnectorTemplate(vs), IDisplayFormatConnector
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public string DateFormat
        {
            get => settings.Dialogs.DisplayFormat.DateFormat;
            set
            {
                settings.Dialogs.DisplayFormat.DateFormat = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
                logger.LogDebug("DateFormat set to {Format}", value);
            }
        }

        public string DurationDayFormat
        {
            get => settings.Dialogs.DisplayFormat.DurationDayFormat;
            set
            {
                settings.Dialogs.DisplayFormat.DurationDayFormat = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
                logger.LogDebug("DurationDayFormat set to {Format}", value);
            }
        }
        public string DurationHourFormat
        {
            get => settings.Dialogs.DisplayFormat.DurationHourFormat;
            set
            {
                settings.Dialogs.DisplayFormat.DurationHourFormat = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
                logger.LogDebug("DurationHourFormat set to {Format}", value);
            }
        }
        public string DurationMinuteFormat
        {
            get => settings.Dialogs.DisplayFormat.DurationMinuteFormat;
            set
            {
                settings.Dialogs.DisplayFormat.DurationMinuteFormat = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
                logger.LogDebug("DurationMinuteFormat set to {Format}", value);
            }
        }
        public string DurationSecondFormat
        {
            get => settings.Dialogs.DisplayFormat.DurationSecondFormat;
            set
            {
                settings.Dialogs.DisplayFormat.DurationSecondFormat = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
                logger.LogDebug("DurationSecondFormat set to {Format}", value);
            }
        }

        public double PreviewImageWidth
        {
            get => settings.Dialogs.DisplayFormat.PreviewImageWidth;
            set
            {
                settings.Dialogs.DisplayFormat.PreviewImageWidth = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
                logger.LogDebug("PreviewImageWidth set to {Width}", value);
            }
        }
        public double PreviewImageHeight
        {
            get => settings.Dialogs.DisplayFormat.PreviewImageHeight;
            set
            {
                settings.Dialogs.DisplayFormat.PreviewImageHeight = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
                logger.LogDebug("PreviewImageHeight set to {Height}", value);
            }
        }
    }
}
