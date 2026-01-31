using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.Core.Utilities.Internal
{
    public class FocusableObject : ObservableObject
    {
        private readonly ILogger logger = VidHubContext.Logger;

        protected virtual bool SetFocusedProperty<T>([NotNullIfNotNull(nameof(newValue))] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                logger.LogTrace("SetFocusedProperty no change for {Property}", propertyName);
                return false;
            }
            try
            {
                _ = VidHubContext.Window.TryEnqueue(() =>
                {
                    OnPropertyChanging(propertyName);
                });
            }
            catch { }
            field = newValue;
            try
            {
                _ = VidHubContext.Window.TryEnqueue(() =>
                {
                    OnPropertyChanged(propertyName);
                });
            }
            catch { }
            logger.LogTrace("SetFocusedProperty changed {Property}", propertyName);
            return true;
        }
    }
}
