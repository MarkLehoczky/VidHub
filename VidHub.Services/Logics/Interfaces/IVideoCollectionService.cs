using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Core.Notifications;

namespace VidHub.Services.Logics.Interfaces
{
    public interface IVideoCollectionService
    {
        ObservableCollection<BarNotification> DisplayedNotifications { get; }
        ObservableCollection<Video> DisplayedVideos { get; }
    }
}
