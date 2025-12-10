using VidHub.Core.Settings;
using VidHub.Core.Utilities.Helper;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Connectors.Modals.Interfaces;

namespace VidHub.Services.Connectors.Modals
{
    public class VideoDisplayCustomizationConnector(IVideoService vs, IVidHubSettings settings) : IVideoDisplayCustomizationConnector
    {
        public string DateFormat
        {
            get => settings.Modals.DisplayFormat.DateFormat;
            set
            {
                settings.Modals.DisplayFormat.DateFormat = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public string DurationDayFormat
        {
            get => settings.Modals.DisplayFormat.DurationDayFormat;
            set
            {
                settings.Modals.DisplayFormat.DurationDayFormat = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public string DurationHourFormat
        {
            get => settings.Modals.DisplayFormat.DurationHourFormat;
            set
            {
                settings.Modals.DisplayFormat.DurationHourFormat = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public string DurationMinuteFormat
        {
            get => settings.Modals.DisplayFormat.DurationMinuteFormat;
            set
            {
                settings.Modals.DisplayFormat.DurationMinuteFormat = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public string DurationSecondFormat
        {
            get => settings.Modals.DisplayFormat.DurationSecondFormat;
            set
            {
                settings.Modals.DisplayFormat.DurationSecondFormat = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public double PreviewImageWidth
        {
            get => settings.Modals.DisplayFormat.PreviewImageWidth;
            set
            {
                settings.Modals.DisplayFormat.PreviewImageWidth = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }
        public double PreviewImageHeight
        {
            get => settings.Modals.DisplayFormat.PreviewImageHeight;
            set
            {
                settings.Modals.DisplayFormat.PreviewImageHeight = value;
                vs.Update(UpdateSection.VIDEOCOLLECTION);
            }
        }

        public void SubscribeToUpdateEvent(Action<IEnumerable<UpdateSection>> action)
        {
            vs.SubscribeToUpdateEvent(action);
        }

        public void UnsubscribeFromUpdateEvent(Action<IEnumerable<UpdateSection>> action)
        {
            vs.UnsubscribeFromUpdateEvent(action);
        }

        public void Update(IEnumerable<UpdateSection> sections)
        {
            vs.Update(sections);
        }
        public void Update(params UpdateSection[] sections)
        {
            vs.Update(sections);
        }
    }
}
