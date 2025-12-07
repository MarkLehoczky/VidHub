using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using static VidHub.Core.VideoCondition;

namespace VidHub.WinUI.Converters
{
    internal partial class VideoStateIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is State state
                ? state switch
                {
                    State.NOTCHECKED => "\uf142",
                    State.INPROGRESS => "\uf143",
                    State.HEALTHY => "\uf13e",
                    State.CORRUPTED => "\uf13d",
                    State.UNKNOWNERROR => "\uf13c",
                    State.FILENOTFOUND => "\uf140",
                    _ => "\uf141",
                }
                : "\uf137";
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
            return value is State state
                ? state switch
                {
                    State.NOTCHECKED => new SolidColorBrush(ColorHelper.FromArgb(255, 105, 105, 105)),
                    State.INPROGRESS => new SolidColorBrush(ColorHelper.FromArgb(255, 30, 144, 255)),
                    State.HEALTHY => new SolidColorBrush(ColorHelper.FromArgb(255, 0, 128, 0)),
                    State.CORRUPTED => new SolidColorBrush(ColorHelper.FromArgb(255, 255, 0, 0)),
                    State.UNKNOWNERROR => new SolidColorBrush(ColorHelper.FromArgb(255, 178, 34, 34)),
                    State.FILENOTFOUND => new SolidColorBrush(ColorHelper.FromArgb(255, 139, 0, 0)),
                    _ => new SolidColorBrush(ColorHelper.FromArgb(255, 105, 105, 105)),
                }
                : new SolidColorBrush(ColorHelper.FromArgb(255, 105, 105, 105));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
