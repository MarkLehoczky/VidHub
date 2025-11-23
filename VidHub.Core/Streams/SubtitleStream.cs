namespace VidHub.Core.Streams
{
    public class SubtitleStream(IDictionary<string, string> metadata) : MediaStream(metadata)
    {
        public string Codec => Metadata.TryGetValue("codec_name", out string? value) ? value : string.Empty;

        public string DetailedCodec => Metadata.TryGetValue("codec_long_name", out string? value) ? value : string.Empty;

        public TimeSpan Duration => Metadata.TryGetValue("duration", out string? value) && double.TryParse(value, out double durationSeconds)
                    ? TimeSpan.FromSeconds(durationSeconds)
                    : TimeSpan.Zero;
    }
}
