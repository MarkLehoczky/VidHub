using VidHub.Core;
using VidHub.Core.Notifications;

namespace VidHub.Services.Base.Interfaces
{
    public interface IVideoService : IList<Video>, IUpdateService
    {
        Comparer<Video> Comparer { get; set; }
        Func<Video, bool> Predicate { get; set; }

        IList<BarNotification> GetAllNotifications();
        IList<Video> GetAllVideos();
        IList<BarNotification> GetDisplayedNotifications();
        IList<Video> GetDisplayedVideos();
    }
}
