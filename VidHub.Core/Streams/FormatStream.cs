namespace VidHub.Core.Streams
{
    public class FormatStream(IDictionary<string, string> metadata) : MediaStream(metadata)
    {
        public int Bitrate => Metadata.TryGetValue("bit_rate", out string? value) && int.TryParse(value, out int result) ? result : 0;
        public TimeSpan Duration => Metadata.TryGetValue("duration", out string? value) && double.TryParse(value, out double durationSeconds)
                    ? TimeSpan.FromSeconds(durationSeconds)
                    : TimeSpan.Zero;
        public string Format => Metadata.TryGetValue("format_long_name", out string? value) ? value : string.Empty;
        public long Size => Metadata.TryGetValue("size", out string? value) && long.TryParse(value, out long result) ? result : 0;
        public DateTime CreationTime => Metadata.TryGetValue("tags.creation_time", out string? value) && DateTime.TryParse(value, out DateTime result)
                    ? result
                    : DateTime.MinValue;
    }
}
