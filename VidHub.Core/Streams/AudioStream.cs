namespace VidHub.Core.Streams
{
    public class AudioStream(IDictionary<string, string> metadata) : MediaStream(metadata)
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

        public int SampleRate
        {
            get
            {
                if (Metadata.TryGetValue("sample_rate", out string? value) && int.TryParse(value, out int result))
                    return result;
                return 0;
            }
        }

        public int ChannelCount
        {
            get
            {
                if (Metadata.TryGetValue("channels", out string? value) && int.TryParse(value, out int result))
                    return result;
                return 0;
            }
        }

        public string ChannelLayout
        {
            get
            {
                if (Metadata.TryGetValue("channel_layout", out string? value))
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

        public int Bitrate
        {
            get
            {
                if (Metadata.TryGetValue("bit_rate", out string? value) && int.TryParse(value, out int result))
                    return result;
                return 0;
            }
        }

        public int FrameCount
        {
            get
            {
                if (Metadata.TryGetValue("nb_frames", out string? value) && int.TryParse(value, out int result))
                    return result;
                return 0;
            }
        }

        public bool IsDefault
        {
            get
            {
                if (Metadata.TryGetValue("disposition.default", out string? value))
                    return value == "1";
                return false;
            }
        }
    }
}
