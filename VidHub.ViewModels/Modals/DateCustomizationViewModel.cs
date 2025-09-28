using CommunityToolkit.Mvvm.ComponentModel;
using VidHub.Platform;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.ViewModels.Modals
{
    public partial class DateCustomizationViewModel(ISettingsService settings) : ObservableRecipient
    {
        public string? DateFormat
        {
            get => settings.DateFormat;
            set => settings.DateFormat = value;
        }

        public DateCustomizationViewModel() : this(Context.MainHost.GetService<ISettingsService>()) { }
    }
}
