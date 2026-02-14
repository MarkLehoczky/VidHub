using VidHub.Core;
using VidHub.Core.Notifications;

namespace VidHub.Services.Base
{
    public interface IVideoService : IList<Video>, IUpdateService
    {
        Comparer<Video> Comparer { get; set; }

        IList<BarNotification> GetAllNotifications();
        IList<Video> GetAllVideos();
        IList<BarNotification> GetDisplayedNotifications();
        IList<Video> GetDisplayedVideos();
    }
}
