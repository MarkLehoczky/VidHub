using VidHub.Core.Enums;

namespace VidHub.Core.Settings.Models
{
    public class VideoHealthSettings
    {
        public VideoHealthCheckType Type { get; set; } = VideoHealthCheckType.QUICKCHECK;
    }
}
