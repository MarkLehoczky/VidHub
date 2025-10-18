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
                    if (duration.TotalDays >= 1)
                    {
                        return duration.ToString(Platform.Context.Host.GetService<ISettingsService>().DisplayCustomization.DurationDayFormat);
                    }

                    if (duration.TotalHours >= 1)
                    {
                        return duration.ToString(Platform.Context.Host.GetService<ISettingsService>().DisplayCustomization.DurationHourFormat);
                    }

                    return duration.TotalMinutes >= 1
                        ? duration.ToString(Platform.Context.Host.GetService<ISettingsService>().DisplayCustomization.DurationMinuteFormat)
                        : duration >= TimeSpan.Zero
                        ? duration.ToString(Platform.Context.Host.GetService<ISettingsService>().DisplayCustomization.DurationSecondFormat)
                        : "n/a";
                }
                catch
                {
                    if (duration.TotalDays >= 1)
                    {
                        return duration.ToString("d' day(s) 'hh':'mm':'ss");
                    }

                    if (duration.TotalHours >= 1)
                    {
                        return duration.ToString("hh':'mm':'ss");
                    }

                    return duration.TotalMinutes >= 1
                        ? duration.ToString("mm':'ss")
                        : duration >= TimeSpan.Zero ? duration.ToString("ss' second(s)'") : "n/a";
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
