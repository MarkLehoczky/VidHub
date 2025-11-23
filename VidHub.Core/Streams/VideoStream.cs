namespace VidHub.Core.Streams
{
    public class VideoStream(IDictionary<string, string> metadata) : MediaStream(metadata)
    {
        public string Codec => Metadata.TryGetValue("codec_name", out string? value) ? value : string.Empty;

        public string DetailedCodec => Metadata.TryGetValue("codec_long_name", out string? value) ? value : string.Empty;

        public int Width => Metadata.TryGetValue("width", out string? value) && int.TryParse(value, out int result) ? result : 0;

        public int Height => Metadata.TryGetValue("height", out string? value) && int.TryParse(value, out int result) ? result : 0;

        public Tuple<int, int> AspectRatio
        {
            get
            {
                if (Metadata.TryGetValue("display_aspect_ratio", out string? value))
                {
                    string[] parts = value.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int a) && int.TryParse(parts[1], out int b))
                    {
                        return new Tuple<int, int>(a, b);
                    }
                }
                return new Tuple<int, int>(0, 0);
            }
        }

        public Tuple<int, int> Framerate
        {
            get
            {
                if (Metadata.TryGetValue("avg_frame_rate", out string? value))
                {
                    string[] parts = value.Split('/');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int num) && int.TryParse(parts[1], out int den))
                    {
                        return new Tuple<int, int>(num, den);
                    }
                }
                return new Tuple<int, int>(0, 1);
            }
        }

        public TimeSpan Duration => Metadata.TryGetValue("duration", out string? value) && double.TryParse(value, out double durationSeconds)
                    ? TimeSpan.FromSeconds(durationSeconds)
                    : TimeSpan.Zero;

        public int Bitrate => Metadata.TryGetValue("bit_rate", out string? value) && int.TryParse(value, out int result) ? result : 0;

        public int FrameCount => Metadata.TryGetValue("nb_frames", out string? value) && int.TryParse(value, out int result) ? result : 0;

        public bool IsDefault => Metadata.TryGetValue("disposition.default", out string? value) && value == "1";
    }
}
