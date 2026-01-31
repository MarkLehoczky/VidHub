using Microsoft.Extensions.Logging;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using VidHub.Core.Models;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.WinUI.Converters
{
    internal partial class HealthIconConverter : IValueConverter
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value is HealthState state)
                {
                    string result = state switch
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
                    };
                    logger.LogTrace("HealthIconConverter: Converted HealthState {State} to icon {Icon}", state, result);
                    return result;
                }
                logger.LogTrace("HealthIconConverter: Value is not HealthState, returning default icon");
                return "\uf142";
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "HealthIconConverter: Error converting value {Value}", value);
                return "\uf142";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    internal partial class HealthColorConverter : IValueConverter
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value is HealthState state)
                {
                    SolidColorBrush result = state switch
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
                    };
                    logger.LogTrace("HealthColorConverter: Converted HealthState {State} to color", state);
                    return result;
                }
                logger.LogTrace("HealthColorConverter: Value is not HealthState, returning default color");
                return new SolidColorBrush(Colors.DimGray);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "HealthColorConverter: Error converting value {Value}", value);
                return new SolidColorBrush(Colors.DimGray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
