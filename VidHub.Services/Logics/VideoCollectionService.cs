using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Core.Notifications;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Base;

namespace VidHub.Services.Logics
{
    public class VideoCollectionService : IVideoCollectionService, IDisposable
    {
        private readonly ILogger logger = VidHubContext.Logger;
        private readonly IVideoService service;
        public ObservableCollection<Video> DisplayedVideos { get; } = [];
        public ObservableCollection<BarNotification> DisplayedNotifications { get; } = [];


        public VideoCollectionService(IVideoService service)
        {
            this.service = service;
            service.SubscribeToUpdateEvent(UpdateDisplayedVideos);
            service.SubscribeToUpdateEvent(UpdateDisplayedNotifications);
            logger.LogTrace("VideoCollectionService initialized and subscribed to updates");
        }


        // TODO: Optimize update logic
        private void UpdateDisplayedVideos(IEnumerable<UpdateSection> sections)
        {
            logger.LogTrace("UpdateDisplayedVideos invoked with sections count={Count}", sections?.Count() ?? 0);
            IList<Video> nextDisplayedVideos = service.GetDisplayedVideos();

            if (sections.Contains(UpdateSection.VIDEOCOLLECTION))
            {
                for (int i = 0; i < Math.Min(DisplayedVideos.Count, nextDisplayedVideos.Count); i++)
                {
                    if (!Equals(DisplayedVideos[i], nextDisplayedVideos[i]))
                    {
                        DisplayedVideos[i] = nextDisplayedVideos[i];
                    }
                }

                while (DisplayedVideos.Count > nextDisplayedVideos.Count)
                {
                    DisplayedVideos.RemoveAt(DisplayedVideos.Count - 1);
                }

                for (int i = DisplayedVideos.Count; i < nextDisplayedVideos.Count; i++)
                {
                    DisplayedVideos.Add(nextDisplayedVideos[i]);
                }
                logger.LogDebug("DisplayedVideos synchronized to count={Count}", DisplayedVideos.Count);
            }
            else
            {
                logger.LogTrace("UpdateDisplayedVideos called but sections does not contain VIDEOCOLLECTION");
            }
        }

        // TODO: Optimize update logic
        private void UpdateDisplayedNotifications(IEnumerable<UpdateSection> sections)
        {
            logger.LogTrace("UpdateDisplayedNotifications invoked with sections count={Count}", sections?.Count() ?? 0);
            IList<BarNotification> nextDisplayedNotifications = service.GetDisplayedNotifications();

            if (sections.Contains(UpdateSection.NOTIFICATIONS))
            {
                for (int i = 0; i < Math.Min(DisplayedNotifications.Count, nextDisplayedNotifications.Count); i++)
                {
                    if (!Equals(DisplayedNotifications[i], nextDisplayedNotifications[i]))
                    {
                        DisplayedNotifications[i] = nextDisplayedNotifications[i];
                    }
                }

                while (DisplayedNotifications.Count > nextDisplayedNotifications.Count)
                {
                    DisplayedNotifications.RemoveAt(DisplayedNotifications.Count - 1);
                }

                for (int i = DisplayedNotifications.Count; i < nextDisplayedNotifications.Count; i++)
                {
                    DisplayedNotifications.Add(nextDisplayedNotifications[i]);
                }
                logger.LogDebug("DisplayedNotifications synchronized to count={Count}", DisplayedNotifications.Count);
            }
            else
            {
                logger.LogTrace("UpdateDisplayedNotifications called but sections does not contain NOTIFICATIONS");
            }
        }

        public void Dispose()
        {
            service.UnsubscribeFromUpdateEvent(UpdateDisplayedVideos);
            service.UnsubscribeFromUpdateEvent(UpdateDisplayedNotifications);
            GC.SuppressFinalize(this);
            logger.LogTrace("VideoCollectionService disposed and unsubscribed from updates");
        }
    }
}
