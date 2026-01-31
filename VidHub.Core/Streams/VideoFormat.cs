namespace VidHub.Core.Streams
{
    public class VideoFormat(IDictionary<string, string> metadata) : MetadataSource(metadata)
    {
        public int Bitrate { get; set; } = metadata.TryGetValue("bit_rate", out string? value) && int.TryParse(value, out int result)
            ? result
            : 0;
        public TimeSpan Duration { get; set; } = metadata.TryGetValue("duration", out string? value) && double.TryParse(value, out double result)
            ? TimeSpan.FromSeconds(result)
            : TimeSpan.Zero;
        public string Format { get; set; } = metadata.TryGetValue("format_long_name", out string? value)
            ? value
            : string.Empty;
        public long Size { get; set; } = metadata.TryGetValue("size", out string? value) && long.TryParse(value, out long result)
            ? result
            : 0;
        public DateTime CreationTime { get; set; } = metadata.TryGetValue("tags.creation_time", out string? value) && DateTime.TryParse(value, out DateTime result)
            ? result
            : DateTime.MinValue;


        public VideoFormat() : this(new Dictionary<string, string>()) { }
    }
}
