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
                return [.. videos];
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
