using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.WinUI.Converters
{
    internal partial class SearchIconConverter : IValueConverter
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                bool liveFiltering = value is bool && (bool)value;
                logger.LogTrace("SearchIconConverter: Converting with liveFiltering={LiveFiltering}", liveFiltering);
                return liveFiltering ? null : (object)new SymbolIcon(Symbol.Find);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "SearchIconConverter: Error converting value {Value}", value);
                return new SymbolIcon(Symbol.Find);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
