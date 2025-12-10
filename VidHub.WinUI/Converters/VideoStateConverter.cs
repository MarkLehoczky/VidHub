using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using VidHub.Core.Enums;

namespace VidHub.WinUI.Converters
{
    internal partial class VideoStateIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is VideoHealth state
                ? state switch
                {
                    VideoHealth.NOTCHECKED => "\uf141",
                    VideoHealth.INPROGRESS => "\uf143",
                    VideoHealth.HEALTHY => "\uf13e",
                    VideoHealth.MINORCORRUPTION => "\uf13e",
                    VideoHealth.SERIOUSCORRUPTION => "\uf13d",
                    VideoHealth.CRITICALCORRUPTION => "\uf13d",
                    VideoHealth.FILENOTFOUND => "\uf140",
                    VideoHealth.UNKNOWNERROR => "\uf13c",
                    _ => "\uf142",
                }
                : "\uf142";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    internal partial class VideoStateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is VideoHealth state
                ? state switch
                {
                    VideoHealth.NOTCHECKED => new SolidColorBrush(Colors.DimGray),
                    VideoHealth.INPROGRESS => new SolidColorBrush(Colors.DeepSkyBlue),
                    VideoHealth.HEALTHY => new SolidColorBrush(Colors.Green),
                    VideoHealth.MINORCORRUPTION => new SolidColorBrush(Colors.Orange),
                    VideoHealth.SERIOUSCORRUPTION => new SolidColorBrush(Colors.Orange),
                    VideoHealth.CRITICALCORRUPTION => new SolidColorBrush(Colors.Red),
                    VideoHealth.FILENOTFOUND => new SolidColorBrush(Colors.Red),
                    VideoHealth.UNKNOWNERROR => new SolidColorBrush(Colors.DarkRed),
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
