using System.Text.RegularExpressions;
using VidHub.Core;
using VidHub.Core.Settings;
using VidHub.Services.Base.Interfaces;

namespace VidHub.Services.Connectors.Modals
{
    public class VideoTitleLoadFormatCustomizationConnector(IVideoService vs, IVidHubSettings settings) : VideoTitleFormatCustomizationConnector(vs, settings)
    {
        public override void UpdateTitles()
        {
            try
            {
                Regex regex = new(RegexPattern);
                InvalidRegex = false;
            }
            catch
            {
                InvalidRegex = true;
            }

            IList<Video> videos = [.. vs.GetAllVideos().Where(v => !v.LoadingFinished)];
            for (int i = 0; i < Math.Min(Titles.Count, videos.Count); i++)
            {
                if (!Equals(Titles[i], videos[i]))
                {
                    Titles[i] = settings.GetCustomizedVideoTitle(videos[i], UseRegex && !InvalidRegex);
                    videos[i].Title = settings.GetCustomizedVideoTitle(videos[i], UseRegex && !InvalidRegex);
                }
            }
            while (Titles.Count > videos.Count)
            {
                Titles.RemoveAt(Titles.Count - 1);
            }
            for (int i = Titles.Count; i < videos.Count; i++)
            {
                Titles.Add(settings.GetCustomizedVideoTitle(videos[i], UseRegex && !InvalidRegex));
                videos[i].Title = settings.GetCustomizedVideoTitle(videos[i], UseRegex && !InvalidRegex);
            }
            foreach (Video video in vs.GetAllVideos().Where(v => !v.LoadingFinished))
            {
                video.Title = settings.GetCustomizedVideoTitle(video, UseRegex && !InvalidRegex);
            }
        }
    }
}
