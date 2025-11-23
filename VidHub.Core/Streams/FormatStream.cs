namespace VidHub.Core.Streams
{
    public class FormatStream(IDictionary<string, string> metadata) : MediaStream(metadata)
    {
        public int Bitrate
        {
            get
            {
                if (Metadata.TryGetValue("bit_rate", out string? value) && int.TryParse(value, out int result))
                    return result;
                return 0;
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
        public string Format
        {
            get
            {
                if (Metadata.TryGetValue("format_long_name", out string? value))
                    return value;
                return string.Empty;
            }
        }
        public long Size
        {
            get
            {
                if (Metadata.TryGetValue("size", out string? value) && long.TryParse(value, out long result))
                    return result;
                return 0;
            }
        }
        public DateTime CreationTime
        {
            get
            {
                if (Metadata.TryGetValue("tags.creation_time", out string? value) && DateTime.TryParse(value, out DateTime result))
                    return result;
                return DateTime.MinValue;
            }
        }
    }
}
