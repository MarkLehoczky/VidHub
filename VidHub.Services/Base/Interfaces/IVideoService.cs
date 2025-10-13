using VidHub.Core;

namespace VidHub.Services.Base.Interfaces
{
    public interface IVideoService : IUpdateService, IList<Video>
    {
        Comparer<Video> Comparer { get; set; }
        Func<Video, bool> Predicate { get; set; }
        IList<Video> GetDisplayVideos();
    }
}
