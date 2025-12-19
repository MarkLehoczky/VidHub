using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;

namespace VidHub.WinUI.Converters
{
    internal partial class SearchIconConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            return value is bool liveFiltering && liveFiltering ? null : (object)new SymbolIcon(Symbol.Find);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
