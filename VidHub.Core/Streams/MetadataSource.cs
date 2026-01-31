namespace VidHub.Core.Streams
{
    public class MetadataSource(IDictionary<string, string> metadata)
    {
        public IDictionary<string, string> Metadata { get; set; } = metadata;


        public MetadataSource() : this(new Dictionary<string, string>()) { }
    }
}
