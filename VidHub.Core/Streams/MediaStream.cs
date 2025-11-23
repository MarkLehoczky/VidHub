namespace VidHub.Core.Streams
{
    public class MediaStream
    {
        public IDictionary<string, string> Metadata { get; init; }

        private MediaStream()
        {
            Metadata = new Dictionary<string, string>();
        }

        public MediaStream(IDictionary<string, string> metadata)
        {
            Metadata = metadata;
        }
    }
}
