using System.Text.Json.Serialization;
using VidHub.Core.Settings;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.Core.Streams
{
    public class FixedFramerate
    {
        private bool isSelected = false;

        public int Framerate { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsSelected { get => isSelected; set { isSelected = value; VidHubContext.Host.Update(UpdateSection.VIDEOCOLLECTION); } }


        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not FixedFramerate other) return false;
            return Framerate == other.Framerate;
        }
        override public int GetHashCode()
        {
            return Framerate.GetHashCode();
        }
    }
    public class Framerate
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }
        public double Value { get; set; }


        public Framerate()
        {
            Numerator = 0;
            Denominator = 1;
            Value = double.NaN;
        }
        public Framerate(IDictionary<string, string> metadata)
        {
            if (metadata.TryGetValue("avg_frame_rate", out string? value))
            {
                string[] parts = value.Split('/');
                if (parts.Length == 2 && int.TryParse(parts[0], out int num) && int.TryParse(parts[1], out int den))
                {
                    Numerator = num;
                    Denominator = den;
                }
            }
            Value = Denominator != 0 ? (double)Numerator / Denominator : double.NaN;
        }
    }


    public class FixedResolution
    {
        private bool isSelected = false;

        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsSelected { get => isSelected; set { isSelected = value; VidHubContext.Host.Update(UpdateSection.VIDEOCOLLECTION); } }
        public int Value => Width * Height;


        override public bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not FixedResolution other) return false;
            return Value == other.Value;
        }
        override public int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
    public class Resolution
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Value { get; set; }


        public Resolution()
        {
            Width = 0;
            Height = 0;
            Value = 0;
        }
        public Resolution(IDictionary<string, string> metadata)
        {
            Width = metadata.TryGetValue("width", out string? widthString) && int.TryParse(widthString, out int widthPixel)
                ? widthPixel
                : 0;
            Height = metadata.TryGetValue("height", out string? heightString) && int.TryParse(heightString, out int heightPixel)
                ? heightPixel
                : 0;
            Value = Width * Height;
        }
    }


    public class VideoStream(IDictionary<string, string> metadata) : MediaStream(metadata)
    {
        public int Width { get; set; } = metadata.TryGetValue("width", out string? value) && int.TryParse(value, out int result)
            ? result
            : 0;
        public int Height { get; set; } = metadata.TryGetValue("height", out string? value) && int.TryParse(value, out int result)
            ? result
            : 0;
        public TimeSpan Duration { get; set; } = metadata.TryGetValue("duration", out string? value) && double.TryParse(value, out double result)
            ? TimeSpan.FromSeconds(result)
            : TimeSpan.Zero;
        public int Bitrate { get; set; } = metadata.TryGetValue("bit_rate", out string? value) && int.TryParse(value, out int result)
            ? result
            : 0;
        public int FrameCount { get; set; } = metadata.TryGetValue("nb_frames", out string? value) && int.TryParse(value, out int result)
            ? result
            : 0;
        public string AspectRatio { get; set; } = metadata.TryGetValue("display_aspect_ratio", out string? value)
            ? value
            : "n/a";
        public bool IsDefault { get; set; } = metadata.TryGetValue("disposition.default", out string? value) && value == "1";
        public Framerate Framerate { get; set; } = new Framerate(metadata);
        public Resolution Resolution { get; set; } = new Resolution(metadata);

        [JsonIgnore] public FixedFramerate? DefinedFramerate { get; set; }
        [JsonIgnore] public FixedResolution? DefinedResolution { get; set; }


        public VideoStream() : this(new Dictionary<string, string>()) { }


        public void SetFixedFramerate()
        {
            DefinedFramerate = VidHubSettings.Instance.General.Framerates.Where(f => f.Framerate >= Framerate.Value).OrderBy(f => f.Framerate).FirstOrDefault();
        }
        public void SetFixedResolution()
        {
            DefinedResolution = VidHubSettings.Instance.General.Resolutions.Where(r => r.Value >= Resolution.Value).OrderBy(r => r.Value).FirstOrDefault();
        }
    }
}
