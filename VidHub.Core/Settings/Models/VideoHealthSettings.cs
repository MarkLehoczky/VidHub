using VidHub.Core.Enums;

namespace VidHub.Core.Settings.Models
{
    public class VideoHealthSettings
    {
        public HealthCheckLevel Level { get; set; } = HealthCheckLevel.QUICKCHECK;
    }
}
