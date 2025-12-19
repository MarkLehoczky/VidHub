using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using VidHub.Core.Models;

namespace VidHub.WinUI.Converters
{
    internal partial class HealthIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is HealthState state
                ? state switch
                {
                    HealthState.NOTCHECKED => "\uf141",
                    HealthState.INPROGRESS => "\uf143",
                    HealthState.HEALTHY => "\uf13e",
                    HealthState.MINORCORRUPTION => "\uf13e",
                    HealthState.SERIOUSCORRUPTION => "\uf13d",
                    HealthState.CRITICALCORRUPTION => "\uf13d",
                    HealthState.FILENOTFOUND => "\uf140",
                    HealthState.UNKNOWNERROR => "\uf13c",
                    _ => "\uf142",
                }
                : "\uf142";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    internal partial class HealthColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is HealthState state
                ? state switch
                {
                    HealthState.NOTCHECKED => new SolidColorBrush(Colors.DimGray),
                    HealthState.INPROGRESS => new SolidColorBrush(Colors.DeepSkyBlue),
                    HealthState.HEALTHY => new SolidColorBrush(Colors.Green),
                    HealthState.MINORCORRUPTION => new SolidColorBrush(Colors.Orange),
                    HealthState.SERIOUSCORRUPTION => new SolidColorBrush(Colors.Orange),
                    HealthState.CRITICALCORRUPTION => new SolidColorBrush(Colors.Red),
                    HealthState.FILENOTFOUND => new SolidColorBrush(Colors.Red),
                    HealthState.UNKNOWNERROR => new SolidColorBrush(Colors.DarkRed),
                    _ => new SolidColorBrush(Colors.DimGray),
                }
                : new SolidColorBrush(Colors.DimGray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
