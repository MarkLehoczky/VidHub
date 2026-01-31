using System.Text.Json.Serialization;
using VidHub.Core.Streams;
using VidHub.Core.Utilities.Internal;

namespace VidHub.Core.Models
{
    public class VideoMetadata : FocusableObject
    {
        private VideoFormat? format = null;
        private IEnumerable<VideoStream> videoStreams = [];
        private IEnumerable<AudioStream> audioStreams = [];
        private IEnumerable<SubtitleStream> subtitleStreams = [];
        private IEnumerable<MediaStream> unknownStreams = [];

        public VideoFormat? Format { get => format; set => SetFocusedProperty(ref format, value); }
        public IEnumerable<VideoStream> VideoStreams
        {
            get => videoStreams;
            set
            {
                SetFocusedProperty(ref videoStreams, value);
                DefaultVideoStream = videoStreams.FirstOrDefault(s => s.IsDefault) ?? videoStreams.FirstOrDefault();
            }
        }
        public IEnumerable<AudioStream> AudioStreams
        {
            get => audioStreams;
            set
            {
                SetFocusedProperty(ref audioStreams, value);
                DefaultAudioStream = audioStreams.FirstOrDefault(s => s.IsDefault) ?? audioStreams.FirstOrDefault();
            }
        }
        public IEnumerable<SubtitleStream> SubtitleStreams { get => subtitleStreams; set => SetFocusedProperty(ref subtitleStreams, value); }
        public IEnumerable<MediaStream> UnknownStreams { get => unknownStreams; set => SetFocusedProperty(ref unknownStreams, value); }
        [JsonIgnore] public VideoStream? DefaultVideoStream { get; private set; } = null;
        [JsonIgnore] public AudioStream? DefaultAudioStream { get; private set; } = null;
    }
}
