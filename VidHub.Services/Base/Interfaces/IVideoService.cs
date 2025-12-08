using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Core.Notifications.Bar;

namespace VidHub.Services.Base.Interfaces
{
    public interface IVideoService : IUpdateService, IList<Video>
    {
        Comparer<Video> Comparer { get; set; }
        Func<Video, bool> Predicate { get; set; }
        IList<Video> GetDisplayVideos();
        IList<BarNotification> GetDisplayNotifications();
        void AddNotification(BarNotification notification);
    }
}
