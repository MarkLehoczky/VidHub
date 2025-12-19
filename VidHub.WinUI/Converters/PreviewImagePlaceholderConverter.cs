using Microsoft.UI.Xaml.Data;
using System;
using System.IO;

namespace VidHub.WinUI.Converters
{
    public partial class PreviewImagePlaceholderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string path && File.Exists(path))
            {
                return path;
            }
            else return "..\\Assets\\Placeholder.Image.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}