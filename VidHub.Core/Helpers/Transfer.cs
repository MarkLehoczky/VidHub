namespace VidHub.Core.Helpers
{
    public class Transfer
    {
        public bool IsActive { get; set; } = true;
        public bool IsLoading { get; set; } = false;
        public int LoadedCount { get; set; } = 0;
        public int TotalCount { get; set; } = 0;


        public Transfer() { }

        public void Increment()
        {
            LoadedCount++;
        }

        public void AddTotalCount(int totalCount)
        {
            TotalCount = totalCount;
        }
    }
}
