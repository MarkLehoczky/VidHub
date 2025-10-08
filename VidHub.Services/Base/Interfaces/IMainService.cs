using VidHub.Core;

namespace VidHub.Services.Base.Interfaces
{
    public interface IMainService : IUpdateService
    {
        List<int> LoadedID { get; set; }
        Func<Video, bool> Predicate { get; set; }
        Comparer<Video> Comparer { get; set; }
        void AddVideo(Video video);
        Video GetVideo(int ID);
        List<Video> GetAllVideos();
        List<Video> GetDisplayVideos();
        List<Video> GetLastLoadedVideos();
    }
}
