namespace VidHub.Core.Streams
{
    public class MediaStream
    {
        public string Codec => Metadata.TryGetValue("codec_name", out string? value) ? value : "n/a";
        public string DetailedCodec => Metadata.TryGetValue("codec_long_name", out string? value) ? value : "n/a";
        public IDictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        private MediaStream() { }

        public MediaStream(IDictionary<string, string> metadata)
        {
            Metadata = metadata;
        }
    }
}
