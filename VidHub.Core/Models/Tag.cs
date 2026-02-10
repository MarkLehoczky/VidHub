using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;
using Windows.UI;

namespace VidHub.Core.Models
{
    public class Tag : ObservableObject
    {
        private string name = "New tag";
        private Color color = Color.FromArgb(255, 127, 127, 127);
        private bool colorPickerOpen = false;

        public string Name { get => name; set => SetProperty(ref name, value); }
        public Color Color { get => color; set => SetProperty(ref color, value); }
        [JsonIgnore] public bool ColorPickerOpen { get => colorPickerOpen; set { SetProperty(ref colorPickerOpen, value); OnPropertyChanged(nameof(ColorPickerIcon)); } }
        [JsonIgnore] public string ColorPickerIcon => ColorPickerOpen ? "\ue70d" : "\ue70e";
    }
}
