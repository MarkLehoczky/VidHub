using System.Collections;
using System.Diagnostics;
using VidHub.Core;
using VidHub.Core.Data;
using VidHub.Core.Notifications;
using VidHub.Core.Settings;
using VidHub.Core.Utilities;
using VidHub.Platform.Environment;

namespace VidHub.Services.Base
{
    public class VideoService : IVideoService
    {
        private readonly object locker = new();
        private readonly RecurrenceManager recurringActionManager = new();
        private event Action<IEnumerable<UpdateSection>>? UpdateEvent;

        private readonly IList<Video> videos = [];
        private readonly IList<BarNotification> notifications = [];

        public Func<Video, bool> Predicate { get; set; } = _ => true;
        public Comparer<Video> Comparer { get; set; } = Comparer<Video>.Default;


        public VideoService()
        {
            notifications.Add(NotificationData.NotCheckedVideosNotification(GetAllVideos));
            notifications.Add(NotificationData.MediumCacheSizeNotification());
            notifications.Add(NotificationData.HealthyVideosNotification(videos));
            notifications.Add(NotificationData.UnhealthyVideosNotification(GetAllVideos));
            notifications.Add(NotificationData.LargeCacheSizeNotification());
            notifications.Add(NotificationData.FFmpegNotInstalledNotification());

            recurringActionManager.Add(PeriodicDisplayUpdate, TimeSpan.FromSeconds(5));
            recurringActionManager.Add(PeriodicHealthCheck, TimeSpan.FromSeconds(60));
            recurringActionManager.Add(PeriodicNotificationUpdate, TimeSpan.FromSeconds(5));
        }


        public IList<BarNotification> GetAllNotifications()
        {
            lock (locker)
            {
                return [.. notifications];
            }
        }
        public IList<Video> GetAllVideos()
        {
            lock (locker)
            {
                return [.. videos];
            }
        }
        public IList<BarNotification> GetDisplayedNotifications()
        {
            lock (locker)
            {
                return [.. notifications.Where(n => n.Display && VidHubSettings.Instance.DisplayNotification(n))];
            }
        }
        public IList<Video> GetDisplayedVideos()
        {
            lock (locker)
            {
                return [.. videos.Where(Predicate).Order(Comparer)];
            }
        }


        private void PeriodicDisplayUpdate()
        {
            Debug.WriteLine("Periodic Display Update");
            try { Update(UpdateSections.ALL); }
            catch { }
        }
        private void PeriodicHealthCheck()
        {
            Debug.WriteLine("Periodic Health Check");
            IList<Video> snapshot;
            lock (locker)
            {
                snapshot = [.. videos];
            }
            foreach (Video video in snapshot)
            {
                video.CheckHealth();
            }
        }
        private void PeriodicNotificationUpdate()
        {
            Debug.WriteLine("Periodic Notification Update");
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
        }


        public void SubscribeToUpdateEvent(Action<IEnumerable<UpdateSection>> action)
        {
            lock (locker)
            {
                UpdateEvent += action;
            }
        }
        public void UnsubscribeFromUpdateEvent(Action<IEnumerable<UpdateSection>> action)
        {
            lock (locker)
            {
                UpdateEvent -= action;
            }
        }
        public void Update(IEnumerable<UpdateSection> sections)
        {
            lock (locker)
            {
                _ = Context.Window.TryEnqueue(() => UpdateEvent?.Invoke(sections));
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
            }
        }

        public bool Remove(Video item)
        {
            lock (locker)
            {
                return videos.Remove(item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (locker)
            {
                videos.RemoveAt(index);
            }
        }
    }
}
