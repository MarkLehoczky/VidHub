using VidHub.Platform;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Modals.Interfaces;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.Services.Modals
{
    public class VideoPreviewImageCustomizationService(ISettingsService settings) : IVideoPreviewImageCustomizationService
    {
        public bool RelativePosition
        {
            get => settings.PreviewImageCustomization.RelativePosition;
            set => settings.PreviewImageCustomization.RelativePosition = value;
        }
        public int Hours
        {
            get => settings.PreviewImageCustomization.Hours;
            set => settings.PreviewImageCustomization.Hours = value;
        }
        public int Minutes
        {
            get => settings.PreviewImageCustomization.Minutes;
            set => settings.PreviewImageCustomization.Minutes = value;
        }
        public int Seconds
        {
            get => settings.PreviewImageCustomization.Seconds;
            set => settings.PreviewImageCustomization.Seconds = value;
        }
        public int Milliseconds
        {
            get => settings.PreviewImageCustomization.Milliseconds;
            set => settings.PreviewImageCustomization.Milliseconds = value;
        }
        public int Percentage
        {
            get => settings.PreviewImageCustomization.Percentage;
            set => settings.PreviewImageCustomization.Percentage = value;
        }

        public async Task ExtractLoadedVideoPreviewImagesAsync()
        {
            await Task.Run(() =>
            {
                if (Directory.Exists(Path.Combine(Path.GetTempPath(), "VidHub", "Previews")))
                    Directory.Delete(Path.Combine(Path.GetTempPath(), "VidHub", "Previews"), true);
            });
        }

        public async Task RemoveAllPreviewImagesAsync()
        {
            await Task.Run(() =>
            {
                foreach (var item in Context.Host.GetService<IVideoService>())
                    if (RelativePosition)
                        item.ExtractPreviewImage(item.Duration * settings.PreviewImageCustomization.FramePercentage);

                    else
                        item.ExtractPreviewImage(settings.PreviewImageCustomization.FrameTime);
            });
        }
    }
}
