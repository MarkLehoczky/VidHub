namespace VidHub.Core.Streams
{
    public class MediaStream(IDictionary<string, string> metadata) : MetadataSource(metadata)
    {
        public string Codec => Metadata.TryGetValue("codec_name", out string? value) ? value : "n/a";
        public string DetailedCodec => Metadata.TryGetValue("codec_long_name", out string? value) ? value : "n/a";
    }
}
