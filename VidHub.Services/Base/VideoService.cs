using System.Collections;
using VidHub.Core;
using VidHub.Core.Enums;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;

namespace VidHub.Services.Base
{
    public class VideoService : IVideoService, IDisposable
    {
        private readonly object locker = new();
        private event Action<UpdateType>? UpdateEvent;
        private readonly IList<Video> Videos = [];
        private readonly Task healthCheckTask;

        public Func<Video, bool> Predicate { get; set; } = _ => true;
        public Comparer<Video> Comparer { get; set; } = Comparer<Video>.Default;

        public int Count => Videos.Count;
        public bool IsReadOnly => Videos.IsReadOnly;
        public Video this[int index] { get => Videos[index]; set => Videos[index] = value; }



        public VideoService()
        {
            healthCheckTask = StartHealthCheck();
        }

        private Task StartHealthCheck()
        {
            return Task.Run(async () =>
            {
                while (true)
                {
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

        public IList<Video> GetDisplayVideos()
        {
            lock (locker)
            {
                return [.. Videos.Where(Predicate).Order(Comparer)];
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
        }
    }
}
