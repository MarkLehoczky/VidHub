using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using VidHub.Core.Streams;

namespace VidHub.WinUI.Converters
{
    class VideoFramerateTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is VideoStream stream && stream != null)
            {
                return stream.Framerate.ToString();
            }
            return "n/a";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


    class VideoFramerateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is VideoStream stream && stream != null)
            {
                return stream.ConvertFramerate<SolidColorBrush>(new()
                {
                    { "120", new SolidColorBrush(Colors.DeepSkyBlue) },
                    { "90", new SolidColorBrush(Colors.DodgerBlue) },
                    { "60", new SolidColorBrush(Colors.MediumSeaGreen) },
                    { "30", new SolidColorBrush(Colors.Green) },
                    { "23", new SolidColorBrush(Colors.DarkGoldenrod) },
                    { "12", new SolidColorBrush(Colors.Sienna) },
                    { "low", new SolidColorBrush(Colors.DarkRed) },
                }) ?? new SolidColorBrush(Colors.DimGray);
            }
            return new SolidColorBrush(Colors.DimGray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
