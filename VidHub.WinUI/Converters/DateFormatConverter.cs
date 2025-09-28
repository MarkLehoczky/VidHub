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
            return value is DateTime date
                ? date.ToString(Context.MainHost.GetService<ISettingsService>().DateFormat)
                : "n/a";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
