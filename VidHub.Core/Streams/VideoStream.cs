using System.Text.Json.Serialization;

namespace VidHub.Core.Streams
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



    public class Framerate
    {
        private int numerator;
        private int denominator;

        public int Numerator
        {
            get => numerator;
            set
            {
                numerator = value;
                Value = Denominator != 0 ? (double)Numerator / Denominator : double.NaN;
                Definition = GetDefinition(Value);
            }
        }
        public int Denominator
        {
            get => denominator;
            set
            {
                denominator = value;
                Value = Denominator != 0 ? (double)Numerator / Denominator : double.NaN;
                Definition = GetDefinition(Value);
            }
        }
        [JsonIgnore] public double Value { get; set; }
        [JsonIgnore] public DefinedFramerate Definition { get; set; }


        public Framerate()
        {
            Numerator = 0;
            Denominator = 1;
            Value = double.NaN;
            Definition = DefinedFramerate.UNKNOWN;
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
            Definition = GetDefinition(Value);
        }


        public static DefinedFramerate GetDefinition(double framerate)
        {
            if (framerate >= 240.0)
            {
                return DefinedFramerate.FPS240;
            }
            else if (framerate >= 120.0)
            {
                return DefinedFramerate.FPS120;
            }
            else if (framerate >= 90.0)
            {
                return DefinedFramerate.FPS90;
            }
            else if (framerate >= 60.0)
            {
                return DefinedFramerate.FPS60;
            }
            else if (framerate >= 30.0)
            {
                return DefinedFramerate.FPS30;
            }
            else if (framerate >= 24.0)
            {
                return DefinedFramerate.FPS30;
            }
            else if (framerate >= 20.0)
            {
                return DefinedFramerate.FPS20;
            }
            else if (framerate >= 12.0)
            {
                return DefinedFramerate.FPS12;
            }
            else if (framerate > 0.0)
            {
                return DefinedFramerate.LOW;
            }
            else
            {
                return DefinedFramerate.UNKNOWN;
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
        private int width;
        private int height;

        public int Width
        {
            get => width;
            set
            {
                width = value;
                Value = Width * Height;
                Definition = GetDefinition(Value);
            }
        }
        public int Height
        {
            get => height;
            set
            {
                height = value;
                Value = Width * Height;
                Definition = GetDefinition(Value);
            }
        }
        [JsonIgnore] public int Value { get; set; }
        [JsonIgnore] public DefinedResolution Definition { get; set; }


        public Resolution()
        {
            Width = 0;
            Height = 0;
            Value = 0;
            Definition = DefinedResolution.UNKNOWN;
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
            Definition = GetDefinition(Value);
        }


        public static DefinedResolution GetDefinition(int resolution)
        {
            if (resolution >= 7680 * 4320)
            {
                return DefinedResolution.UHD8K;
            }
            else if (resolution >= 3840 * 2160)
            {
                return DefinedResolution.UHD4K;
            }
            else if (resolution >= 2560 * 1440)
            {
                return DefinedResolution.QHD;
            }
            else if (resolution >= 1920 * 1080)
            {
                return DefinedResolution.FHD;
            }
            else if (resolution >= 1280 * 720)
            {
                return DefinedResolution.HD;
            }
            else if (resolution >= 720 * 480)
            {
                return DefinedResolution.SD;
            }
            else if (resolution > 0)
            {
                return DefinedResolution.LOW;
            }
            else
            {
                return DefinedResolution.UNKNOWN;
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


        public VideoStream() : this(new Dictionary<string, string>()) { }
    }
}
