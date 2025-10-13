using Microsoft.UI.Xaml.Data;
using System;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.WinUI.Converters
{
    internal partial class DurationFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                TimeSpan duration = (TimeSpan)value;
                try
                {
                    if (duration.TotalDays >= 1) return duration.ToString(Platform.Context.Host.GetService<ISettingsService>().DisplayCustomization.DurationDayFormat);
                    if (duration.TotalHours >= 1) return duration.ToString(Platform.Context.Host.GetService<ISettingsService>().DisplayCustomization.DurationHourFormat);
                    if (duration.TotalMinutes >= 1) return duration.ToString(Platform.Context.Host.GetService<ISettingsService>().DisplayCustomization.DurationMinuteFormat);
                    if (duration >= TimeSpan.Zero) return duration.ToString(Platform.Context.Host.GetService<ISettingsService>().DisplayCustomization.DurationSecondFormat);
                    return "n/a";
                }
                catch
                {
                    if (duration.TotalDays >= 1) return duration.ToString("d' day(s) 'hh':'mm':'ss");
                    if (duration.TotalHours >= 1) return duration.ToString("hh':'mm':'ss");
                    if (duration.TotalMinutes >= 1) return duration.ToString("mm':'ss");
                    if (duration >= TimeSpan.Zero) return duration.ToString("ss' second(s)'");
                    return "n/a";
                }
            }
            catch
            {
                return "n/a";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
