using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using VidHub.Core.Streams;

namespace VidHub.WinUI.Converters
{
    internal class VideoResolutionTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is VideoStream stream && stream != null)
            {
                return stream.ConvertResolution(new Dictionary<string, string>
                {
                    { "8K", "8K UHD" },
                    { "4K", "4K UHD" },
                    { "1440p", "1440p" },
                    { "1080p", "1080p" },
                    { "720p", "720p" },
                    { "480p", "480p" },
                    { "low", "Low" },
                });
            }
            return "n/a";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


    internal class VideoResolutionColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is VideoStream stream && stream != null)
            {
                return stream.ConvertResolution(new Dictionary<string, SolidColorBrush>
                {
                    { "8K", new SolidColorBrush(Colors.DeepSkyBlue) },
                    { "4K", new SolidColorBrush(Colors.DodgerBlue) },
                    { "1440p", new SolidColorBrush(Colors.MediumSeaGreen) },
                    { "1080p", new SolidColorBrush(Colors.Green) },
                    { "720p", new SolidColorBrush(Colors.DarkGoldenrod) },
                    { "480p", new SolidColorBrush(Colors.Sienna) },
                    { "low", new SolidColorBrush(Colors.DarkRed) },
                });
            }
            return "n/a";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
