using CommunityToolkit.Mvvm.ComponentModel;
using VidHub.Platform;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.ViewModels.Modals
{
    public partial class FieldSizeCustomizationViewModel(ISettingsService settings) : ObservableRecipient
    {
        public double FieldWidth
        {
            get => settings.FieldWidth;
            set => settings.FieldWidth = value;
        }
        public double FieldHeight
        {
            get => settings.FieldHeight;
            set => settings.FieldHeight = value;
        }

        public FieldSizeCustomizationViewModel() : this(Context.MainHost.GetService<ISettingsService>()) { }
    }
}
