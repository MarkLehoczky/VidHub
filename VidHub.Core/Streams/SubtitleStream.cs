namespace VidHub.Core.Streams
{
    public class SubtitleStream(IDictionary<string, string> metadata) : MediaStream(metadata)
    {
        public TimeSpan Duration => Metadata.TryGetValue("duration", out string? value) && double.TryParse(value, out double durationSeconds)
                    ? TimeSpan.FromSeconds(durationSeconds)
                    : TimeSpan.Zero;
    }
}
