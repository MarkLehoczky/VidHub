namespace VidHub.Core.Streams
{
    public class AudioStream(IDictionary<string, string> metadata) : MediaStream(metadata)
    {
        public int SampleRate { get; set; } = metadata.TryGetValue("sample_rate", out string? value) && int.TryParse(value, out int result) ? result : 0;
        public int ChannelCount { get; set; } = metadata.TryGetValue("channels", out string? value) && int.TryParse(value, out int result) ? result : 0;
        public string ChannelLayout { get; set; } = metadata.TryGetValue("channel_layout", out string? value) ? value : string.Empty;
        public TimeSpan Duration { get; set; } = metadata.TryGetValue("duration", out string? value) && double.TryParse(value, out double durationSeconds)
                    ? TimeSpan.FromSeconds(durationSeconds)
                    : TimeSpan.Zero;
        public int Bitrate { get; set; } = metadata.TryGetValue("bit_rate", out string? value) && int.TryParse(value, out int result) ? result : 0;
        public int FrameCount { get; set; } = metadata.TryGetValue("nb_frames", out string? value) && int.TryParse(value, out int result) ? result : 0;
        public bool IsDefault { get; set; } = metadata.TryGetValue("disposition.default", out string? value) && value == "1";
    }
}
