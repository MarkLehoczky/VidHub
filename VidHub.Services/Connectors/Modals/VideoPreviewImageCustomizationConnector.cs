using VidHub.Core.Settings;
using VidHub.Core.Utilities.Helper;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Connectors.Modals.Interfaces;

namespace VidHub.Services.Connectors.Modals
{
    public class VideoPreviewImageCustomizationConnector(IVideoService vs, IVidHubSettings settings) : IVideoPreviewImageCustomizationConnector
    {
        public int Hours { get => settings.Modals.PreviewImageFormat.FixedHours; set => settings.Modals.PreviewImageFormat.FixedHours = value; }
        public int Milliseconds { get => settings.Modals.PreviewImageFormat.FixedMilliseconds; set => settings.Modals.PreviewImageFormat.FixedMilliseconds = value; }
        public int Minutes { get => settings.Modals.PreviewImageFormat.FixedMinutes; set => settings.Modals.PreviewImageFormat.FixedMinutes = value; }
        public int Percentage { get => settings.Modals.PreviewImageFormat.RelativePercentage; set => settings.Modals.PreviewImageFormat.RelativePercentage = value; }
        public bool RelativePosition { get => settings.Modals.PreviewImageFormat.RelativePosition; set => settings.Modals.PreviewImageFormat.RelativePosition = value; }
        public int Seconds { get => settings.Modals.PreviewImageFormat.FixedSeconds; set => settings.Modals.PreviewImageFormat.FixedSeconds = value; }
        public bool ExtractEmbeddedImageCommand { get => settings.Modals.PreviewImageFormat.ExtractEmbeddedImage; set => settings.Modals.PreviewImageFormat.ExtractEmbeddedImage = value; }
        public bool UseContentHash { get => settings.General.UseFileContentHash; set => settings.General.UseFileContentHash = value; }

        public async Task ExtractLoadedVideoPreviewImagesAsync()
        {
            await Task.Run(() =>
            {
                if (Directory.Exists(Path.Combine(Path.GetTempPath(), "VidHub", "Previews")))
                {
                    Directory.Delete(Path.Combine(Path.GetTempPath(), "VidHub", "Previews"), true);
                }
            });
            vs.Update(UpdateType.FORCEUPDATEVIDEOCOLLECTION);
        }

        public async Task RemoveAllPreviewImagesAsync()
        {
            await Task.Run(() =>
            {
                foreach (Core.Video item in Context.Host.GetService<IVideoService>())
                {
                    if (RelativePosition)
                    {
                        item.ProcessPreviewImage();
                    }
                    else
                    {
                        item.ProcessPreviewImage();
                    }
                }
            });
            vs.Update(UpdateType.FORCEUPDATEVIDEOCOLLECTION);
        }

        public void SubscribeToUpdateEvent(Action<UpdateType> action)
        {
            vs.SubscribeToUpdateEvent(action);
        }

        public void UnsubscribeFromUpdateEvent(Action<UpdateType> action)
        {
            vs.UnsubscribeFromUpdateEvent(action);
        }

        public void Update(UpdateType type)
        {
            vs.Update(type);
        }
    }
}
