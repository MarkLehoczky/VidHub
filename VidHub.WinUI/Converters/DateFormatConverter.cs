using Microsoft.UI.Xaml.Data;
using System;
using VidHub.Core.Settings;

namespace VidHub.WinUI.Converters
{
    internal partial class DateFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                DateTime date = (DateTime)value;
                try
                {
                    return date.ToString(VidHubSettings.Instance.Modals.DisplayFormat.DateFormat);
                }
                catch
                {
                    return date.ToString("yyyy-MM-dd");
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
