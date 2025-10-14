using VidHub.Core.Helpers;
using VidHub.Platform;
using VidHub.Services.Connectors.Modals.Interfaces;
using VidHub.ViewModels.Base;

namespace VidHub.ViewModels.Modals
{
    public partial class VideoDisplayCustomizationViewModel(IVideoDisplayCustomizationConnector connector) : ViewModelTemplate(connector)
    {
        public VideoDisplayCustomizationViewModel() : this(Context.Host.GetService<IVideoDisplayCustomizationConnector>()) { }


        public string DateFormat
        {
            get => connector.DateFormat;
            set => connector.DateFormat = value;
        }

        public string DurationDayFormat
        {
            get => connector.DurationDayFormat;
            set => connector.DurationDayFormat = value;
        }
        public string DurationHourFormat
        {
            get => connector.DurationHourFormat;
            set => connector.DurationHourFormat = value;
        }
        public string DurationMinuteFormat
        {
            get => connector.DurationMinuteFormat;
            set => connector.DurationMinuteFormat = value;
        }
        public string DurationSecondFormat
        {
            get => connector.DurationSecondFormat;
            set => connector.DurationSecondFormat = value;
        }


        public double PreviewImageWidth
        {
            get => connector.PreviewImageWidth;
            set => connector.PreviewImageWidth = value;
        }
        public double PreviewImageHeight
        {
            get => connector.PreviewImageHeight;
            set => connector.PreviewImageHeight = value;
        }


        override public void Update(UpdateType type) { }
    }
}
