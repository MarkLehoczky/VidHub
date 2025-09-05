using VidHub.Core;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;

namespace VidHub.Services.Base
{
    public class MainService : IMainService
    {
        private readonly object locker = new();
        private event Action? UpdateEvent;
        private readonly List<Video> videos = [];

        public Func<Video, bool> Predicate { get; set; } = _ => true;
        public Comparer<Video> Comparer { get; set; } = Comparer<Video>.Default;


        public void AddVideo(Video video)
        {
            lock (locker)
            {
                videos.Add(video);
            }
            Update();
        }

        public IEnumerable<Video> GetAllVideos()
        {
            lock (locker)
            {
                return videos;
            }
        }

        public IEnumerable<Video> GetDisplayVideos()
        {
            lock (locker)
            {
                return videos.Where(Predicate).Order(Comparer);
            }
        }


        public void SubscribeToUpdateEvent(Action action)
        {
            UpdateEvent += action;
        }

        public void UnsubscribeFromUpdateEvent(Action action)
        {
            UpdateEvent -= action;
        }

        public void Update()
        {
            Context.MainWindow.TryEnqueue(() =>
            {
                UpdateEvent?.Invoke();
            });
        }
    }
}
