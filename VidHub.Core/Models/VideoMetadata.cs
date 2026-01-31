using System.Text.Json.Serialization;
using VidHub.Core.Streams;

namespace VidHub.Core.Models
{
    public class VideoMetadata
    {
        private VideoFormat? format = null;
        private IEnumerable<VideoStream> videoStreams = [];
        private IEnumerable<AudioStream> audioStreams = [];
        private IEnumerable<SubtitleStream> subtitleStreams = [];
        private IEnumerable<MediaStream> unknownStreams = [];

        public VideoFormat? Format { get => format; set => format = value; }
        public IEnumerable<VideoStream> VideoStreams
        {
            get => videoStreams;
            set
            {
                videoStreams = value ?? [];
                DefaultVideoStream = videoStreams.FirstOrDefault(s => s.IsDefault) ?? videoStreams.FirstOrDefault();
            }
        }
        public IEnumerable<AudioStream> AudioStreams
        {
            get => audioStreams;
            set
            {
                audioStreams = value ?? [];
                DefaultAudioStream = audioStreams.FirstOrDefault(s => s.IsDefault) ?? audioStreams.FirstOrDefault();
            }
        }
        public IEnumerable<SubtitleStream> SubtitleStreams { get => subtitleStreams; set => subtitleStreams = value ?? []; }
        public IEnumerable<MediaStream> UnknownStreams { get => unknownStreams; set => unknownStreams = value ?? []; }

        [JsonIgnore] public VideoStream? DefaultVideoStream { get; private set; } = null;
        [JsonIgnore] public AudioStream? DefaultAudioStream { get; private set; } = null;

        public VideoMetadata() { }
    }
}
