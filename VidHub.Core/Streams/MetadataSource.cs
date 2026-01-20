namespace VidHub.Core.Streams
{
    public class MetadataSource(IDictionary<string, string> metadata)
    {
        protected IDictionary<string, string> Metadata { get; set; } = metadata;
    }
}
