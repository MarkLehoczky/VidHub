namespace VidHub.Core.Streams
{
    public class MetadataSource
    {
        protected IDictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();


        private MetadataSource() { }
        public MetadataSource(IDictionary<string, string> metadata)
        {
            Metadata = metadata;
        }
    }
}
