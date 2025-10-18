using System.Collections.ObjectModel;
using VidHub.Core;
using VidHub.Core.Enums;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;

namespace VidHub.Services.Logics
{
    public class VideoCollectionService : IVideoCollectionService
    {
        private readonly IVideoService service;
        public ObservableCollection<Video> DisplayedVideos { get; } = [];


        public VideoCollectionService(IVideoService service)
        {
            this.service = service;
            service.SubscribeToUpdateEvent(UpdateDisplayedVideos);
        }

        ~VideoCollectionService()
        {
            service.UnsubscribeFromUpdateEvent(UpdateDisplayedVideos);
        }


        // TODO: Optimize update logic
        private void UpdateDisplayedVideos(UpdateType type)
        {
            IList<Video> nextDisplayVideos = service.GetDisplayVideos();

            if (type == UpdateType.UpdateVideoCollection)
            {
                for (int i = 0; i < Math.Min(DisplayedVideos.Count, nextDisplayVideos.Count); i++)
                {
                    if (!Equals(DisplayedVideos[i], nextDisplayVideos[i]))
                    {
                        DisplayedVideos[i] = nextDisplayVideos[i];
                    }
                }

                while (DisplayedVideos.Count > nextDisplayVideos.Count)
                {
                    DisplayedVideos.RemoveAt(DisplayedVideos.Count - 1);
                }

                for (int i = DisplayedVideos.Count; i < nextDisplayVideos.Count; i++)
                {
                    DisplayedVideos.Add(nextDisplayVideos[i]);
                }
            }
            else if (type == UpdateType.ForceUpdateVideoCollection)
            {
                DisplayedVideos.Clear();

                for (int i = DisplayedVideos.Count; i < nextDisplayVideos.Count; i++)
                {
                    DisplayedVideos.Add(nextDisplayVideos[i]);
                }
            }
        }
    }
}
