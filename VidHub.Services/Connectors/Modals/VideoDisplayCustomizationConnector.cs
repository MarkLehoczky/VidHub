using VidHub.Core.Enums;
using VidHub.Core.Settings;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Connectors.Modals.Interfaces;

namespace VidHub.Services.Connectors.Modals
{
    public class VideoDisplayCustomizationConnector(IVideoService vs, IVidHubSettings settings) : IVideoDisplayCustomizationConnector
    {
        public string DateFormat
        {
            get => settings.DisplayCustomization.DateFormat;
            set
            {
                settings.DisplayCustomization.DateFormat = value;
                vs.Update(UpdateType.FORCEUPDATEVIDEOCOLLECTION);
            }
        }
        public string DurationDayFormat
        {
            get => settings.DisplayCustomization.DurationDayFormat;
            set
            {
                settings.DisplayCustomization.DurationDayFormat = value;
                vs.Update(UpdateType.FORCEUPDATEVIDEOCOLLECTION);
            }
        }
        public string DurationHourFormat
        {
            get => settings.DisplayCustomization.DurationHourFormat;
            set
            {
                settings.DisplayCustomization.DurationHourFormat = value;
                vs.Update(UpdateType.FORCEUPDATEVIDEOCOLLECTION);
            }
        }
        public string DurationMinuteFormat
        {
            get => settings.DisplayCustomization.DurationMinuteFormat;
            set
            {
                settings.DisplayCustomization.DurationMinuteFormat = value;
                vs.Update(UpdateType.FORCEUPDATEVIDEOCOLLECTION);
            }
        }
        public string DurationSecondFormat
        {
            get => settings.DisplayCustomization.DurationSecondFormat;
            set
            {
                settings.DisplayCustomization.DurationSecondFormat = value;
                vs.Update(UpdateType.FORCEUPDATEVIDEOCOLLECTION);
            }
        }
        public double PreviewImageWidth
        {
            get => settings.DisplayCustomization.PreviewImageWidth;
            set
            {
                settings.DisplayCustomization.PreviewImageWidth = value;
                vs.Update(UpdateType.FORCEUPDATEVIDEOCOLLECTION);
            }
        }
        public double PreviewImageHeight
        {
            get => settings.DisplayCustomization.PreviewImageHeight;
            set
            {
                settings.DisplayCustomization.PreviewImageHeight = value;
                vs.Update(UpdateType.FORCEUPDATEVIDEOCOLLECTION);
            }
        }

        public void SubscribeToUpdateEvent(Action<UpdateType> action)
        {
            vs.SubscribeToUpdateEvent(action);
        }

        public void UnsubscribeFromUpdateEvent(Action<UpdateType> action)
        {
            vs.UnsubscribeFromUpdateEvent(action);
        }

        public void Update(UpdateType type)
        {
            vs.Update(type);
        }
    }
}
