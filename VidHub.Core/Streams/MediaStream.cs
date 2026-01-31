namespace VidHub.Core.Streams
{
    public class MediaStream(IDictionary<string, string> metadata) : MetadataSource(metadata)
    {
        public string Codec { get; set; } = metadata.TryGetValue("codec_name", out string? value)
            ? value
            : "n/a";
        public string DetailedCodec { get; set; } = metadata.TryGetValue("codec_long_name", out string? value)
            ? value
            : "n/a";


        public MediaStream() : this(new Dictionary<string, string>()) { }
    }
}
