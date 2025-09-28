using Microsoft.UI.Xaml.Data;
using System;
using VidHub.Platform;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.WinUI.Converters
{
    internal partial class DurationFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is TimeSpan duration)
            {
                if (duration.TotalDays >= 1) return duration.ToString(Context.MainHost.GetService<ISettingsService>().DurationDayFormat);
                if (duration.TotalHours >= 1) return duration.ToString(Context.MainHost.GetService<ISettingsService>().DurationHourFormat);
                if (duration.TotalMinutes >= 1) return duration.ToString(Context.MainHost.GetService<ISettingsService>().DurationMinuteFormat);
                return duration.ToString(Context.MainHost.GetService<ISettingsService>().DurationSecondFormat);
            }
            return "n/a";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
