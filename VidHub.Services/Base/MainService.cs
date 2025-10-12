using VidHub.Core;
using VidHub.Core.Helpers;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;

namespace VidHub.Services.Base
{
    public class MainService : IMainService
    {
        private readonly object locker = new();
        private event Action<UpdateType>? UpdateEvent;
        private readonly List<Video> videos = [];

        public List<int> LoadedID { get; set; } = [];
        public Func<Video, bool> Predicate { get; set; } = _ => true;
        public Comparer<Video> Comparer { get; set; } = Comparer<Video>.Default;


        public void AddVideo(Video video)
        {
            lock (locker)
            {
                videos.Add(video);
            }
            Update(UpdateType.UpdateVideoCollection);
        }

        public void RemoveVideo(Video video)
        {
            lock (locker)
            {
                videos.Remove(video);
            }
            Update(UpdateType.UpdateVideoCollection);
        }

        public Video GetVideo(int ID)
        {
            return videos.FirstOrDefault(v => v.ID == ID) ?? throw new ArgumentException("Video not found");
        }
        

        public List<Video> GetAllVideos()
        {
            lock (locker)
            {
                return [.. videos];
            }
        }

        public List<Video> GetDisplayVideos()
        {
            lock (locker)
            {
                return [.. videos.Where(Predicate).Order(Comparer)];
            }
        }

        public List<Video> GetLastLoadedVideos()
        {
            lock (locker)
            {
                return [.. videos.Where(v => LoadedID.Contains(v.ID))];
            }
        }


        public void SubscribeToUpdateEvent(Action<UpdateType> action)
        {
            UpdateEvent += action;
        }

        public void UnsubscribeFromUpdateEvent(Action<UpdateType> action)
        {
            UpdateEvent -= action;
        }

        public void Update(UpdateType type)
        {
            Context.MainWindow.TryEnqueue(() =>
            {
                UpdateEvent?.Invoke(type);
            });
        }
    }
}
