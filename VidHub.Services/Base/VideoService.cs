using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using VidHub.Core;
using VidHub.Core.Enums;
using VidHub.Core.Notifications.Bar;
using VidHub.Core.Notifications.Base;
using VidHub.Core.Settings;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;

namespace VidHub.Services.Base
{
    public class VideoService : IVideoService, IDisposable
    {
        private readonly object locker = new();
        private event Action<UpdateType>? UpdateEvent;
        private readonly IList<Video> Videos = [];
        public ObservableCollection<BarNotification> Notifications { get; } = [];
        private readonly Task healthCheckTask;
        private readonly Task updateNotificationTask;
        private readonly Task periodicUpdateTask;

        public Func<Video, bool> Predicate { get; set; } = _ => true;
        public Comparer<Video> Comparer { get; set; } = Comparer<Video>.Default;

        public int Count => Videos.Count;
        public bool IsReadOnly => Videos.IsReadOnly;
        public Video this[int index] { get => Videos[index]; set => Videos[index] = value; }



        public VideoService()
        {
            Notifications.Add(HealthyVideosNotification());
            Notifications.Add(NotCheckedVideosNotification());
            Notifications.Add(LargeCacheSizeNotification(1, 10, NotificationSeverity.Informational));
            Notifications.Add(LargeCacheSizeNotification(10, int.MaxValue, NotificationSeverity.Warning));
            Notifications.Add(UnhealthyVideosNotification());
            Notifications.Add(FFmpegNotInstalledNotification());

            healthCheckTask = StartHealthCheck();
            updateNotificationTask = UpdateNotification();
            periodicUpdateTask = PeriodicUpdate();
        }

        private Task PeriodicUpdate()
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    Debug.WriteLine("GUI update...");
                    await Task.Run(() =>
                    {
                        try
                        {
                            Update(UpdateType.UpdateVideoCollection);
                        }
                        catch { }
                    });
                    await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
                }
            });
        }
        private Task StartHealthCheck()
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    Debug.WriteLine("Health check update...");
                    lock (locker)
                    {
                        IList<Video> snapshot = [.. Videos];
                        foreach (Video video in snapshot)
                        {
                            video.CheckCondition();
                        }
                    }

                    await Task.Delay(TimeSpan.FromSeconds(60)).ConfigureAwait(false);
                }
            });
        }
        private Task UpdateNotification()
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    Debug.WriteLine("Notification update...");
                    IList<BarNotification> snapshot;

                    lock (locker)
                    {
                        snapshot = [.. Notifications];
                    }

                    await Task.Run(() =>
                    {
                        try
                        {
                            foreach (BarNotification notif in snapshot)
                            {
                                bool before = notif.DisplayNotification;
                                bool after = notif.OpenCondition?.Invoke() ?? true;
                                notif.DisplayNotification = after;
                            }
                        }
                        catch { }
                    });

                    await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
                }
            });
        }

        private static BarNotification NotCheckedVideosNotification()
        {
            string title = "Not Checked Videos Found";
            string message = "Some loaded videos' health have not been checked yet.";
            NotificationSeverity severity = NotificationSeverity.Informational;
            bool isClosable = true;
            static bool openCondition()
            {
                IList<Video> snapshot = Context.Host.GetService<IVideoService>().GetAllVideos();
                return snapshot.Count > 0 && snapshot.Any(video => video.Condition.VideoState == VideoCondition.State.NOTCHECKED);
            }
            BarNotification notification = new(title, message, severity, isClosable, openCondition);
            return notification;
        }
        private static BarNotification HealthyVideosNotification()
        {
            string title = "All Videos Passed Health Check";
            string message = "All loaded videos are healthy based on the set health level.";
            NotificationSeverity severity = NotificationSeverity.Success;
            bool isClosable = true;
            static bool openCondition()
            {
                IList<Video> snapshot = Context.Host.GetService<IVideoService>().GetAllVideos();
                return snapshot.Count > 0 && snapshot.All(video => video.Condition.VideoState is VideoCondition.State.HEALTHY or VideoCondition.State.INPROGRESS);
            }
            BarNotification notification = new(title, message, severity, isClosable, openCondition);
            return notification;
        }
        private static BarNotification LargeCacheSizeNotification(int minSize, int maxSize, NotificationSeverity severity)
        {
            string buttonText = "Clear Cache";
            string buttonDescription = "Clears cached files to recover disk space. With cache loading enabled, all previously cached videos has to be extracted again.";
            async void buttonAction()
            {
                await Task.Run(() =>
                {
                    foreach (string item in Directory.GetFiles(Path.Combine(Path.GetTempPath(), "VidHub"), "*", SearchOption.AllDirectories))
                    {
                        File.Delete(item);
                    }
                });
            }

            string title = "Large Cache Size";
            string message = $"The application's cache data has reached more than {minSize} GB";
            bool isClosable = true;
            bool openCondition()
            {
                DirectoryInfo info = new(Path.Combine(Path.GetTempPath(), "VidHub"));
                ulong size = (ulong)info.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);
                bool aboveMinSize = size >= Math.Pow(1024, 3) * minSize;
                bool belowMaxSize = size < Math.Pow(1024, 3) * maxSize;
                return aboveMinSize && belowMaxSize;
            }
            ActionNotificationButton button = new(buttonText, buttonAction, buttonDescription);

            BarNotification notification = new(title, message, severity, isClosable, openCondition, button);
            return notification;
        }
        private static BarNotification FFmpegNotInstalledNotification()
        {
            string buttonText = "Install FFmpeg";
            string buttonDescription = "Installs FFmpeg using Winget";
            static async void buttonAction()
            {
                await Task.Run(() =>
                {
                    try
                    {
                        ProcessStartInfo info = new()
                        {
                            FileName = "winget",
                            Arguments = "install ffmpeg",
                            UseShellExecute = true,
                            CreateNoWindow = true
                        };
                        using Process? process = Process.Start(info);
                        process!.WaitForExit();
                    }
                    catch { }
                });
            }

            string title = "FFmpeg Not Detected";
            string message = "FFmpeg is required for generating video thumbnails and rendering previews. It was not found in the system PATH.";
            bool isClosable = false;
            NotificationSeverity severity = NotificationSeverity.Error;
            static bool openCondition()
            {
                ProcessStartInfo info = new()
                {
                    FileName = "winget",
                    Arguments = "list --query ffmpeg",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using Process? process = Process.Start(info);
                string output = process!.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return !output.Contains("FFmpeg");
            }
            ActionNotificationButton button = new(buttonText, buttonAction, buttonDescription);

            BarNotification notification = new(title, message, severity, isClosable, openCondition, button);
            return notification;
        }
        private static BarNotification UnhealthyVideosNotification()
        {
            string title = "Some Videos Failed Health Check";
            string message = "Some loaded videos are not healthy based on the set health level.";
            NotificationSeverity severity = NotificationSeverity.Warning;
            bool isClosable = true;
            static bool openCondition()
            {
                IList<Video> snapshot = Context.Host.GetService<IVideoService>().GetAllVideos();
                return snapshot.Count > 0 && snapshot.Any(video =>
                video.Condition.VideoState is VideoCondition.State.FILENOTFOUND
                or VideoCondition.State.CORRUPTED
                or VideoCondition.State.UNKNOWNERROR);
            }
            BarNotification notification = new(title, message, severity, isClosable, openCondition);
            return notification;
        }

        public IList<Video> GetDisplayVideos()
        {
            lock (locker)
            {
                return [.. Videos.Where(Predicate).Order(Comparer)];
            }
        }
        public IList<Video> GetAllVideos()
        {
            lock (locker)
            {
                return [.. Videos];
            }
        }


        public void SubscribeToUpdateEvent(Action<UpdateType> action)
        {
            lock (locker)
            {
                UpdateEvent += action;
            }
        }

        public void UnsubscribeFromUpdateEvent(Action<UpdateType> action)
        {
            lock (locker)
            {
                UpdateEvent -= action;
            }
        }

        public void Update(UpdateType type)
        {
            lock (locker)
            {
                _ = Context.Window.TryEnqueue(() => UpdateEvent?.Invoke(type));
            }
        }

        public int IndexOf(Video item)
        {
            lock (locker)
            {
                return Videos.IndexOf(item);
            }
        }

        public void Insert(int index, Video item)
        {
            lock (locker)
            {
                Videos.Insert(index, item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (locker)
            {
                Videos.RemoveAt(index);
            }
        }

        public void Add(Video item)
        {
            lock (locker)
            {
                Videos.Add(item);
            }
        }

        public void Clear()
        {
            lock (locker)
            {
                Videos.Clear();
            }
        }

        public bool Contains(Video item)
        {
            lock (locker)
            {
                return Videos.Contains(item);
            }
        }

        public void CopyTo(Video[] array, int arrayIndex)
        {
            lock (locker)
            {
                Videos.CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(Video item)
        {
            lock (locker)
            {
                return Videos.Remove(item);
            }
        }

        public IEnumerator<Video> GetEnumerator()
        {
            lock (locker)
            {
                return Videos.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (locker)
            {
                return ((IEnumerable)Videos).GetEnumerator();
            }
        }

        public void Dispose()
        {
            healthCheckTask.Dispose();
            updateNotificationTask.Dispose();
            periodicUpdateTask.Dispose();
        }

        public IList<BarNotification> GetDisplayNotifications()
        {
            lock (locker)
            {
                return [.. Notifications.Where(n => n.DisplayNotification && VidHubSettings.Instance.DisplayBarNotification(n))];
            }
        }

        public IList<BarNotification> GetAllNotifications()
        {
            lock (locker)
            {
                return [.. Notifications];
            }
        }
        public void AddNotification(BarNotification notification)
        {
            lock (locker)
            {
                Notifications.Add(notification);
                Update(UpdateType.UpdateVideoCollection);
            }
        }
    }
}
