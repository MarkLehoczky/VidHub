using VidHub.Core;

namespace VidHub.Services.Base.Interfaces
{
    public interface IMainService : IUpdateService
    {
        Func<Video, bool> Predicate { get; set; }
        Comparer<Video> Comparer { get; set; }
        void AddVideo(Video video);
        IEnumerable<Video> GetAllVideos();
        IEnumerable<Video> GetDisplayVideos();
    }
}
