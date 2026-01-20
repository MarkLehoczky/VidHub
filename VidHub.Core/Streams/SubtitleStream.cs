namespace VidHub.Core.Streams
{
    public class SubtitleStream(IDictionary<string, string> metadata) : MediaStream(metadata)
    {
        public TimeSpan Duration { get; set; } = metadata.TryGetValue("duration", out string? value) && double.TryParse(value, out double durationSeconds)
                    ? TimeSpan.FromSeconds(durationSeconds)
                    : TimeSpan.Zero;
    }
}
