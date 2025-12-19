using VidHub.Core.Settings;
using VidHub.Core.Utilities;
using VidHub.Services.Base;

namespace VidHub.Services.Connectors.Dialogs
{
    public class DisplayFormatConnector(IVideoService vs, IVidHubSettings settings) : ConnectorTemplate(vs), IDisplayFormatConnector
    {
        public string DateFormat
        {
            get => settings.Dialogs.DisplayFormat.DateFormat;
            set
            {
                settings.Dialogs.DisplayFormat.DateFormat = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }

        public string DurationDayFormat
        {
            get => settings.Dialogs.DisplayFormat.DurationDayFormat;
            set
            {
                settings.Dialogs.DisplayFormat.DurationDayFormat = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public string DurationHourFormat
        {
            get => settings.Dialogs.DisplayFormat.DurationHourFormat;
            set
            {
                settings.Dialogs.DisplayFormat.DurationHourFormat = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public string DurationMinuteFormat
        {
            get => settings.Dialogs.DisplayFormat.DurationMinuteFormat;
            set
            {
                settings.Dialogs.DisplayFormat.DurationMinuteFormat = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public string DurationSecondFormat
        {
            get => settings.Dialogs.DisplayFormat.DurationSecondFormat;
            set
            {
                settings.Dialogs.DisplayFormat.DurationSecondFormat = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }

        public double PreviewImageWidth
        {
            get => settings.Dialogs.DisplayFormat.PreviewImageWidth;
            set
            {
                settings.Dialogs.DisplayFormat.PreviewImageWidth = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public double PreviewImageHeight
        {
            get => settings.Dialogs.DisplayFormat.PreviewImageHeight;
            set
            {
                settings.Dialogs.DisplayFormat.PreviewImageHeight = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
    }
}
