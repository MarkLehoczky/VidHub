namespace VidHub.Core.Settings.Models
{
    public class PerformanceSettings
    {
        public bool UseCacheLoading { get; set; } = true;
        public bool UseConcurrentLoading { get; set; } = false;
    }
}
