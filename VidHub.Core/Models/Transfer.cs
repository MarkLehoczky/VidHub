namespace VidHub.Core.Models
{
    public class Transfer
    {
        public bool IsActive { get; set; } = true;
        public bool IsCollecting { get; set; } = false;
        public bool IsLoading { get; set; } = false;
        public int LoadedCount { get; set; } = 0;
        public int TotalCount { get; set; } = 0;
    }
}
