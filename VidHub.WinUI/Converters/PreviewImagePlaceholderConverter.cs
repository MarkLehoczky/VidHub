using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Data;
using System;
using System.IO;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.WinUI.Converters
{
    public partial class PreviewImagePlaceholderConverter : IValueConverter
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value is string path && File.Exists(path))
                {
                    logger.LogTrace("PreviewImagePlaceholderConverter: Image exists at {Path}", path);
                    return path;
                }
                else
                {
                    logger.LogTrace("PreviewImagePlaceholderConverter: Image not found or value is not a valid path, using placeholder");
                    return "..\\Assets\\Placeholder.Image.png";
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "PreviewImagePlaceholderConverter: Error processing value {Value}", value);
                return "..\\Assets\\Placeholder.Image.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}