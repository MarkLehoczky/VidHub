using VidHub.Core;
using VidHub.Core.Settings;
using VidHub.Services.Base.Interfaces;

namespace VidHub.Services.Connectors.Modals
{
    public class VideoTitleLoadFormatCustomizationConnector(IVideoService vs, IVidHubSettings settings) : VideoTitleFormatCustomizationConnector(vs, settings)
    {
        public override void UpdateTitles()
        {
            base.UpdateTitles();
            foreach (Video video in vs.GetAllVideos().Where(v => !v.LoadingFinished))
            {
                video.Title = settings.GetCustomizedVideoTitle(video, UseRegex && !InvalidRegex);
            }
        }
    }
}
