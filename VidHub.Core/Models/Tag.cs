using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;
using Windows.UI;

namespace VidHub.Core.Models
{
    public class Tag : ObservableObject
    {
        private string name = "New tag";
        private Color color = Color.FromArgb(255, 127, 127, 127);
        private bool colorPickerOpen = false;
        private bool isSelected = false;
        private readonly Action selected = () => { };

        public string Name { get => name; set => SetProperty(ref name, value); }
        public Color Color { get => color; set => SetProperty(ref color, value); }
        public long ID { get; set; } = DateTime.Now.Ticks;
        public bool IsSelected { get => isSelected; set { isSelected = value; VidHubContext.Host.Update(UpdateSection.VIDEOCOLLECTION); } }
        [JsonIgnore] public bool ColorPickerOpen { get => colorPickerOpen; set { SetProperty(ref colorPickerOpen, value); OnPropertyChanged(nameof(ColorPickerIcon)); } }
        [JsonIgnore] public string ColorPickerIcon => ColorPickerOpen ? "\ue70d" : "\ue70e";
    }
}
