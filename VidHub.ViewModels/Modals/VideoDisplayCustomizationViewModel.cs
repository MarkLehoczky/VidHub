using CommunityToolkit.Mvvm.ComponentModel;
using VidHub.Platform;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.ViewModels.Modals
{
    public partial class VideoDisplayCustomizationViewModel(ISettingsService settings) : ObservableRecipient
    {
        public string DateFormat
        {
            get => settings.DisplayCustomization.DateFormat;
            set => settings.DisplayCustomization.DateFormat = value;
        }

        public string DurationDayFormat
        {
            get => settings.DisplayCustomization.DurationDayFormat;
            set => settings.DisplayCustomization.DurationDayFormat = value;
        }
        public string DurationHourFormat
        {
            get => settings.DisplayCustomization.DurationHourFormat;
            set => settings.DisplayCustomization.DurationHourFormat = value;
        }
        public string DurationMinuteFormat
        {
            get => settings.DisplayCustomization.DurationMinuteFormat;
            set => settings.DisplayCustomization.DurationMinuteFormat = value;
        }
        public string DurationSecondFormat
        {
            get => settings.DisplayCustomization.DurationSecondFormat;
            set => settings.DisplayCustomization.DurationSecondFormat = value;
        }


        public double PreviewImageWidth
        {
            get => settings.DisplayCustomization.PreviewImageWidth;
            set => settings.DisplayCustomization.PreviewImageWidth = value;
        }
        public double PreviewImageHeight
        {
            get => settings.DisplayCustomization.PreviewImageHeight;
            set => settings.DisplayCustomization.PreviewImageHeight = value;
        }


        public VideoDisplayCustomizationViewModel() : this(Context.Host.GetService<ISettingsService>()) { }
    }
}
