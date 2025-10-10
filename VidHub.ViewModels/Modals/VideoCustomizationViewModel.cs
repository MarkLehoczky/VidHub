using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidHub.Platform;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.ViewModels.Modals
{
    public partial class VideoCustomizationViewModel(ISettingsService settings) : ObservableRecipient
    {
        public string? DateFormat
        {
            get => settings.DateFormat;
            set => settings.DateFormat = value;
        }

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


        public VideoCustomizationViewModel() : this(Context.MainHost.GetService<ISettingsService>()) { }
    }
}
