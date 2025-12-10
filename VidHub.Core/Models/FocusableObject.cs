using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using VidHub.Platform;

namespace VidHub.Core.Models
{
    public class FocusableObject : ObservableObject
    {
        protected bool SetFocusedProperty<T>([NotNullIfNotNull(nameof(newValue))] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }
            try
            {
                _ = Context.Window.TryEnqueue(() =>
                {
                    OnPropertyChanging(propertyName);
                });
            }
            catch { }
            field = newValue;
            try
            {
                _ = Context.Window.TryEnqueue(() =>
                {
                    OnPropertyChanged(propertyName);
                });
            }
            catch { }
            return true;
        }
    }
}
