using Microsoft.UI.Xaml.Data;
using System;
using VidHub.Platform;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.WinUI.Converters
{
    internal partial class DateFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                DateTimeOffset date = (DateTimeOffset)value;
                return date.ToString(Context.MainHost.GetService<ISettingsService>().DateFormat);
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
