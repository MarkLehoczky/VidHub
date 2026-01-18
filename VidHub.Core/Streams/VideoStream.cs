using System.Text.Json.Serialization;

namespace VidHub.Core.Streams
{
    public class AspectRatio(int width, int height)
    {
        public int Width { get; set; } = width;
        public int Height { get; set; } = height;

        public AspectRatio() : this(0, 0) { }

        public override string ToString()
        {
            return Width > 0 && Height > 0 ? $"{Width}:{Height}" : "n/a";
        }
    }


    public class Framerate(int numerator, int denominator)
    {
        public int Numerator { get; set; } = numerator;
        public int Denominator { get; set; } = denominator;

        public double Value => Denominator != 0 ? (double)Numerator / Denominator : 0;
        public string Definition
        {
            get
            {
                Dictionary<double, string> dictionary = new()
                {
                    { 240.0, "240fps" },
                    { 120.0, "120fps" },
                    { 90.0, "90fps" },
                    { 60.0, "60fps" },
                    { 30.0, "30fps" },
                    { 24.0, "24fps" },
                    { 12.0, "12fps" },
                    { 1.0, "Low" },
                };
                foreach (var item in dictionary)
                {
                    if (Value >= item.Key)
                    {
                        return item.Value;
                    }
                }
                return "n/a";
            }
        }

        public Framerate() : this(0, 0) { }

        public override string ToString()
        {
            return Numerator > 0 && Denominator > 0 ? $"{Numerator / Denominator:0} fps" : "n/a";
        }
    }

    public class Resolution(int width, int height)
    {
        public int Width { get; set; } = width;
        public int Height { get; set; } = height;

        public int Value => Width * Height;
        public string Definition
        {
            get
            {
                Dictionary<int, string> dictionary = new()
                {
                    { 7680 * 4320, "8K UHD" },
                    { 3840 * 2160, "4K UHD" },
                    { 2560 * 1440, "1440p" },
                    { 1920 * 1080, "1080p" },
                    { 1280 * 720, "720p" },
                    { 640 * 480, "480p" },
                    { 1, "Low" },
                };
                foreach (var item in dictionary)
                {
                    if (Value >= item.Key)
                    {
                        return item.Value;
                    }
                }
                return "n/a";
            }
        }


        public Resolution() : this(0, 0) { }
    }


    public class VideoStream(IDictionary<string, string> metadata) : MediaStream(metadata)
    {
        public int Width => Metadata.TryGetValue("width", out string? value) && int.TryParse(value, out int result) ? result : 0;
        public int Height => Metadata.TryGetValue("height", out string? value) && int.TryParse(value, out int result) ? result : 0;
        public AspectRatio AspectRatio
        {
            get
            {
                if (Metadata.TryGetValue("display_aspect_ratio", out string? value))
                {
                    string[] parts = value.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int a) && int.TryParse(parts[1], out int b))
                    {
                        return new AspectRatio(a, b);
                    }
                }
                return new AspectRatio();
            }
        }
        public Framerate Framerate
        {
            get
            {
                if (Metadata.TryGetValue("avg_frame_rate", out string? value))
                {
                    string[] parts = value.Split('/');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int num) && int.TryParse(parts[1], out int den))
                    {
                        return new Framerate(num, den);
                    }
                }
                return new Framerate();
            }
        }
        public TimeSpan Duration => Metadata.TryGetValue("duration", out string? value) && double.TryParse(value, out double durationSeconds)
                    ? TimeSpan.FromSeconds(durationSeconds)
                    : TimeSpan.Zero;
        public int Bitrate => Metadata.TryGetValue("bit_rate", out string? value) && int.TryParse(value, out int result) ? result : 0;
        public int FrameCount => Metadata.TryGetValue("nb_frames", out string? value) && int.TryParse(value, out int result) ? result : 0;
        public bool IsDefault => Metadata.TryGetValue("disposition.default", out string? value) && value == "1";

        [JsonIgnore] public Resolution Resolution => new(Width, Height);
    }
}
