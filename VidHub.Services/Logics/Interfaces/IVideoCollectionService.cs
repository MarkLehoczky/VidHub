using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Core.Notifications.Bar;

namespace VidHub.Services.Logics.Interfaces
{
    public interface IVideoCollectionService
    {
        ObservableCollection<Video> DisplayedVideos { get; }
        ObservableCollection<BarNotification> DisplayedNotifications { get; }
    }
}
