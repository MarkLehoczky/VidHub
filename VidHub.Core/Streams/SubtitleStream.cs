namespace VidHub.Core.Streams
{
    public class SubtitleStream(IDictionary<string, string> metadata) : MediaStream(metadata)
    {
        public string Codec
        {
            get
            {
                if (Metadata.TryGetValue("codec_name", out string? value))
                    return value;
                return string.Empty;
            }
        }

        public string DetailedCodec
        {
            get
            {
                if (Metadata.TryGetValue("codec_long_name", out string? value))
                    return value;
                return string.Empty;
            }
        }

        public TimeSpan Duration
        {
            get
            {
                if (Metadata.TryGetValue("duration", out string? value) && double.TryParse(value, out double durationSeconds))
                    return TimeSpan.FromSeconds(durationSeconds);
                return TimeSpan.Zero;
            }
        }
    }
}
