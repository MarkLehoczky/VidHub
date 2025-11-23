using System.Text.Json.Serialization;

namespace VidHub.Core.Settings
{
    public class PreviewImageCustomizationSettings
    {
        public bool RelativePosition { get; set; } = true;
        public bool ExtractEmbeddedImageCommand { get; set; } = true;
        public int Hours { get; set; } = 0;
        public int Minutes { get; set; } = 1;
        public int Seconds { get; set; } = 30;
        public int Milliseconds { get; set; } = 0;
        public int Percentage { get; set; } = 50;

        [JsonIgnore] public TimeSpan FrameTime => new(0, Hours, Minutes, Seconds, Milliseconds);
        [JsonIgnore] public double FramePercentage => Percentage / 100.0;
    }
}
