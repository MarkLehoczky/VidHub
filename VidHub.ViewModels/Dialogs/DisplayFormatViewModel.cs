using VidHub.Core.Utilities;
using VidHub.Platform.Environment;
using VidHub.Services.Connectors.Dialogs;

namespace VidHub.ViewModels.Dialogs
{
    public partial class DisplayFormatViewModel(IDisplayFormatConnector connector) : ViewModelTemplate(connector)
    {
        public DisplayFormatViewModel() : this(Context.Host.GetService<IDisplayFormatConnector>()) { }


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


        public override void Update(IEnumerable<UpdateSection> sections) { }
    }
}
