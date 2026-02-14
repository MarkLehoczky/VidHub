using Microsoft.Extensions.Logging;
using System.Collections;
using VidHub.Core;
using VidHub.Core.Data;
using VidHub.Core.Notifications;
using VidHub.Core.Settings;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.Services.Base
{
    public class VideoService : IVideoService
    {
        private readonly ILogger logger = VidHubContext.Logger;
        private readonly object locker = new();
        private readonly RecurrenceManager recurringActionManager = new();
        private event Action<IEnumerable<UpdateSection>>? UpdateEvent;

        private readonly IList<Video> videos = [];
        private readonly IList<BarNotification> notifications = [];

        public Comparer<Video> Comparer { get; set; } = Comparer<Video>.Default;


        public VideoService()
        {
            logger.LogTrace("VideoService initializing");
            notifications.Add(NotificationData.NotCheckedVideosNotification(GetAllVideos));
            notifications.Add(NotificationData.MediumCacheSizeNotification());
            notifications.Add(NotificationData.HealthyVideosNotification(videos));
            notifications.Add(NotificationData.UnhealthyVideosNotification(GetAllVideos));
            notifications.Add(NotificationData.LargeCacheSizeNotification());
            notifications.Add(NotificationData.FFmpegNotInstalledNotification());

            recurringActionManager.Add(PeriodicDisplayUpdate, TimeSpan.FromSeconds(5), "Display update");
            recurringActionManager.Add(PeriodicHealthCheck, TimeSpan.FromSeconds(60), "Health check");
            recurringActionManager.Add(PeriodicNotificationUpdate, TimeSpan.FromSeconds(5), "Notification update");
            logger.LogDebug("VideoService initialized with {NotificationCount} notifications", notifications.Count);
        }


        public IList<BarNotification> GetAllNotifications()
        {
            lock (locker)
            {
                logger.LogTrace("GetAllNotifications returning {Count}", notifications.Count);
                return [.. notifications];
            }
        }
        public IList<Video> GetAllVideos()
        {
            lock (locker)
            {
                logger.LogTrace("GetAllVideos returning {Count}", videos.Count);
                return [.. videos];
            }
        }
        public IList<BarNotification> GetDisplayedNotifications()
        {
            lock (locker)
            {
                List<BarNotification> list = notifications.Where(n => n.Display && VidHubSettings.Instance.DisplayNotification(n)).ToList();
                logger.LogTrace("GetDisplayedNotifications returning {Count}", list.Count);
                return list;
            }
        }
        public IList<Video> GetDisplayedVideos()
        {
            lock (locker)
            {
                List<Video> list = videos.Where(VidHubSettings.Instance.ValidVideo).ToList();
                list.Sort(Comparer);
                logger.LogTrace("GetDisplayedVideos returning {Count}", list.Count);
                return list;
            }
        }


        private void PeriodicDisplayUpdate()
        {
            logger.LogTrace("PeriodicDisplayUpdate invoked");
            try { Update(UpdateSections.ALL); }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "PeriodicDisplayUpdate failed to update");
            }
        }
        private void PeriodicHealthCheck()
        {
            logger.LogTrace("PeriodicHealthCheck invoked");
            IList<Video> snapshot;
            lock (locker)
            {
                snapshot = [.. videos];
            }
            foreach (Video video in snapshot)
            {
                video.CheckHealth();
            }
            logger.LogDebug("PeriodicHealthCheck completed for {Count} videos", snapshot.Count);
        }
        private void PeriodicNotificationUpdate()
        {
            logger.LogTrace("PeriodicNotificationUpdate invoked");
            IList<BarNotification> snapshot;
            lock (locker)
            {
                snapshot = [.. notifications];
            }
            foreach (BarNotification notif in snapshot)
            {
                bool before = notif.Display;
                bool after = notif.DisplayCondition?.Invoke() ?? true;
                notif.Display = after;
            }
            logger.LogDebug("PeriodicNotificationUpdate updated {Count} notifications", snapshot.Count);
        }


        public void SubscribeToUpdateEvent(Action<IEnumerable<UpdateSection>> action)
        {
            lock (locker)
            {
                UpdateEvent += action;
                logger.LogTrace("Subscriber added to UpdateEvent");
            }
        }
        public void UnsubscribeFromUpdateEvent(Action<IEnumerable<UpdateSection>> action)
        {
            lock (locker)
            {
                UpdateEvent -= action;
                logger.LogTrace("Subscriber removed from UpdateEvent");
            }
        }
        public void Update(IEnumerable<UpdateSection> sections)
        {
            lock (locker)
            {
                logger.LogTrace("Update called with sections count={Count}", sections?.Count() ?? 0);
                _ = VidHubContext.Window.TryEnqueue(() => UpdateEvent?.Invoke(sections));
            }
        }
        public void Update(params UpdateSection[] sections)
        {
            Update(sections.AsEnumerable());
        }


        public Video this[int index] { get => videos[index]; set => videos[index] = value; }
        public int Count => videos.Count;
        public bool IsReadOnly => videos.IsReadOnly;


        public void Add(Video item)
        {
            lock (locker)
            {
                videos.Add(item);
                logger.LogDebug("Video added: {File}", item?.FilePath);
            }
        }

        public int IndexOf(Video item)
        {
            lock (locker)
            {
                return videos.IndexOf(item);
            }
        }

        public void Clear()
        {
            lock (locker)
            {
                videos.Clear();
                logger.LogDebug("All videos cleared");
            }
        }

        public bool Contains(Video item)
        {
            lock (locker)
            {
                return videos.Contains(item);
            }
        }

        public void CopyTo(Video[] array, int arrayIndex)
        {
            lock (locker)
            {
                videos.CopyTo(array, arrayIndex);
            }
        }

        public IEnumerator<Video> GetEnumerator()
        {
            lock (locker)
            {
                return videos.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (locker)
            {
                return ((IEnumerable)videos).GetEnumerator();
            }
        }

        public void Insert(int index, Video item)
        {
            lock (locker)
            {
                videos.Insert(index, item);
                logger.LogDebug("Video inserted at {Index}: {File}", index, item?.FilePath);
            }
        }

        public bool Remove(Video item)
        {
            lock (locker)
            {
                bool result = videos.Remove(item);
                logger.LogDebug("Video removal attempted for {File}, success={Result}", item?.FilePath, result);
                return result;
            }
        }

        public void RemoveAt(int index)
        {
            lock (locker)
            {
                videos.RemoveAt(index);
                logger.LogDebug("Video removed at index {Index}", index);
            }
        }
    }
}
