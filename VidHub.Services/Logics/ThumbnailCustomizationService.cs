using VidHub.Platform;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.Services.Logics
{
    public class ThumbnailCustomizationService(ISettingsService settings) : IThumbnailCustomizationService
    {
        public TimeSpan ExactTime => new(0, Hours, Minutes, Seconds, Milliseconds);
        public double Percentage => FramePercentage / 100.0;

        public bool RelativePosition
        {
            get => settings.RelativePosition;
            set => settings.RelativePosition = value;
        }
        public int Hours
        {
            get => settings.Hours;
            set => settings.Hours = value;
        }
        public int Minutes
        {
            get => settings.Minutes;
            set => settings.Minutes = value;
        }
        public int Seconds
        {
            get => settings.Seconds;
            set => settings.Seconds = value;
        }
        public int Milliseconds
        {
            get => settings.Milliseconds;
            set => settings.Milliseconds = value;
        }
        public int FramePercentage
        {
            get => settings.FramePercentage;
            set => settings.FramePercentage = value;
        }

        public async Task ExtractLoadedVideoThumbnailsAsync()
        {
            await Task.Run(() =>
            {
                if (Directory.Exists(Path.Combine(Path.GetTempPath(), "VidHub", "Thumbnails")))
                {
                    Directory.Delete(Path.Combine(Path.GetTempPath(), "VidHub", "Thumbnails"), true);
                }
            });
        }

        public async Task RemoveAllThumbnailsAsync()
        {
            await Task.Run(() =>
            {
                foreach (var item in Context.MainHost.GetService<IMainService>().GetAllVideos())
                {
                    if (RelativePosition)
                    {
                        item.ExtractThumbnail(item.Duration * Percentage);
                    }
                    else
                    {
                        item.ExtractThumbnail(ExactTime);
                    }
                }
            });
        }
    }
}
