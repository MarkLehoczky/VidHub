using VidHub.Core;

namespace VidHub.Services.Base.Interfaces
{
    public interface IMainService : IUpdateService
    {
        void AddVideo(Video video);
        IEnumerable<Video> GetAllVideos();
    }
}
