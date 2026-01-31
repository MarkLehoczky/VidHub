using System.Text.Json.Serialization;

namespace VidHub.Core.Streams
{
    public class Framerate
    {
        public enum DefinedFramerate
        {
            UNKNOWN,
            LOW,
            FPS12,
            FPS20,
            FPS24,
            FPS30,
            FPS60,
            FPS90,
            FPS120,
            FPS240
        }

        public int Numerator { get; set; }
        public int Denominator { get; set; }
        [JsonIgnore] public double Value { get; set; }
        [JsonIgnore] public DefinedFramerate Definition { get; set; }


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
                else
                {
                    Numerator = 0;
                    Denominator = 1;
                }
            }
            else
            {
                Numerator = 0;
                Denominator = 1;
            }
            Value = Denominator != 0 ? (double)Numerator / Denominator : -1;

            if (Value >= 240.0)
            {
                Definition = DefinedFramerate.FPS240;
            }
            else if (Value >= 120.0)
            {
                Definition = DefinedFramerate.FPS120;
            }
            else if (Value >= 90.0)
            {
                Definition = DefinedFramerate.FPS90;
            }
            else if (Value >= 60.0)
            {
                Definition = DefinedFramerate.FPS60;
            }
            else if (Value >= 30.0)
            {
                Definition = DefinedFramerate.FPS30;
            }
            else
            {
                Definition = Value >= 24.0
                    ? DefinedFramerate.FPS24
                    : Value >= 20.0
                    ? DefinedFramerate.FPS20
                    : Value >= 12.0 ? DefinedFramerate.FPS12 : Value > 0.0 ? DefinedFramerate.LOW : DefinedFramerate.UNKNOWN;
            }
        }

        public override string ToString()
        {
            return Definition switch
            {
                DefinedFramerate.UNKNOWN => "n/a",
                _ => $"{Math.Round(Value):0} fps"
            };
        }
    }


    public class Resolution
    {
        public enum DefinedResolution
        {
            UNKNOWN,
            LOW,
            SD,
            HD,
            FHD,
            QHD,
            UHD4K,
            UHD8K
        }

        public int Width { get; set; }
        public int Height { get; set; }
        [JsonIgnore] public int Value { get; set; }
        [JsonIgnore] public DefinedResolution Definition { get; set; }


        public Resolution(IDictionary<string, string> metadata)
        {
            Width = metadata.TryGetValue("width", out string? widthString) && int.TryParse(widthString, out int widthPixel)
                ? widthPixel
                : 0;
            Height = metadata.TryGetValue("height", out string? heightString) && int.TryParse(heightString, out int heightPixel)
                ? heightPixel
                : 0;
            Value = Width * Height;

            if (Value >= 7680 * 4320)
            {
                Definition = DefinedResolution.UHD8K;
            }
            else if (Value >= 3840 * 2160)
            {
                Definition = DefinedResolution.UHD4K;
            }
            else if (Value >= 2560 * 1440)
            {
                Definition = DefinedResolution.QHD;
            }
            else
            {
                Definition = Value >= 1920 * 1080
                    ? DefinedResolution.FHD
                    : Value >= 1280 * 720
                    ? DefinedResolution.HD
                    : Value >= 720 * 480 ? DefinedResolution.SD : Value > 0 ? DefinedResolution.LOW : DefinedResolution.UNKNOWN;
            }
        }

        public override string ToString()
        {
            return Definition switch
            {
                DefinedResolution.UHD8K => "8K UHD",
                DefinedResolution.UHD4K => "4K UHD",
                DefinedResolution.QHD => "1440p",
                DefinedResolution.FHD => "1080p",
                DefinedResolution.HD => "720p",
                DefinedResolution.SD => "480p",
                DefinedResolution.LOW => "Low",
                _ => "n/a",
            };
        }
    }


    public class VideoStream(IDictionary<string, string> metadata) : MediaStream(metadata)
    {
        public int Width { get; set; } = metadata.TryGetValue("width", out string? value) && int.TryParse(value, out int widthPixel)
            ? widthPixel
            : 0;
        public int Height { get; set; } = metadata.TryGetValue("height", out string? value) && int.TryParse(value, out int heightPixel)
            ? heightPixel
            : 0;
        public TimeSpan Duration { get; set; } = metadata.TryGetValue("duration", out string? value) && double.TryParse(value, out double durationSeconds)
                ? TimeSpan.FromSeconds(durationSeconds)
                : TimeSpan.Zero;
        public int Bitrate { get; set; } = metadata.TryGetValue("bit_rate", out string? value) && int.TryParse(value, out int bitrateSeconds)
                ? bitrateSeconds
                : 0;
        public int FrameCount { get; set; } = metadata.TryGetValue("nb_frames", out string? value) && int.TryParse(value, out int frameCount)
                ? frameCount
                : 0;
        public string AspectRatio { get; set; } = metadata.TryGetValue("display_aspect_ratio", out string? value) ? value : "n/a";
        public bool IsDefault { get; set; } = metadata.TryGetValue("disposition.default", out string? value) && value == "1";
        public Framerate Framerate { get; set; } = new Framerate(metadata);
        public Resolution Resolution { get; set; } = new Resolution(metadata);
    }
}
