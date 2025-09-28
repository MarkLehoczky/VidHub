using CommunityToolkit.Mvvm.ComponentModel;
using VidHub.Platform;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.ViewModels.Modals
{
    public partial class DurationCustomizationViewModel(ISettingsService settings) : ObservableRecipient
    {
        public string? DurationDayFormat
        {
            get => settings.DurationDayFormat;
            set => settings.DurationDayFormat = value;
        }
        public string? DurationHourFormat
        {
            get => settings.DurationHourFormat;
            set => settings.DurationHourFormat = value;
        }
        public string? DurationMinuteFormat
        {
            get => settings.DurationMinuteFormat;
            set => settings.DurationMinuteFormat = value;
        }
        public string? DurationSecondFormat
        {
            get => settings.DurationSecondFormat;
            set => settings.DurationSecondFormat = value;
        }

        public DurationCustomizationViewModel() : this(Context.MainHost.GetService<ISettingsService>()) { }
    }
}
