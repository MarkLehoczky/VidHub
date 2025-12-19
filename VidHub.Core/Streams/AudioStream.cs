namespace VidHub.Core.Streams
{
    public class AudioStream(IDictionary<string, string> metadata) : MediaStream(metadata)
    {
        public int SampleRate => Metadata.TryGetValue("sample_rate", out string? value) && int.TryParse(value, out int result) ? result : 0;
        public int ChannelCount => Metadata.TryGetValue("channels", out string? value) && int.TryParse(value, out int result) ? result : 0;
        public string ChannelLayout => Metadata.TryGetValue("channel_layout", out string? value) ? value : string.Empty;
        public TimeSpan Duration => Metadata.TryGetValue("duration", out string? value) && double.TryParse(value, out double durationSeconds)
                    ? TimeSpan.FromSeconds(durationSeconds)
                    : TimeSpan.Zero;
        public int Bitrate => Metadata.TryGetValue("bit_rate", out string? value) && int.TryParse(value, out int result) ? result : 0;
        public int FrameCount => Metadata.TryGetValue("nb_frames", out string? value) && int.TryParse(value, out int result) ? result : 0;
        public bool IsDefault => Metadata.TryGetValue("disposition.default", out string? value) && value == "1";
    }
}
