using VidHub.Core.Enums;
using VidHub.Core.Settings;
using VidHub.Platform;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Connectors.Modals.Interfaces;

namespace VidHub.Services.Connectors.Modals
{
    public class VideoPreviewImageCustomizationConnector(IVideoService vs, IVidHubSettings settings) : IVideoPreviewImageCustomizationConnector
    {
        public int Hours { get => settings.PreviewImageCustomization.Hours; set => settings.PreviewImageCustomization.Hours = value; }
        public int Milliseconds { get => settings.PreviewImageCustomization.Milliseconds; set => settings.PreviewImageCustomization.Milliseconds = value; }
        public int Minutes { get => settings.PreviewImageCustomization.Minutes; set => settings.PreviewImageCustomization.Minutes = value; }
        public int Percentage { get => settings.PreviewImageCustomization.Percentage; set => settings.PreviewImageCustomization.Percentage = value; }
        public bool RelativePosition { get => settings.PreviewImageCustomization.RelativePosition; set => settings.PreviewImageCustomization.RelativePosition = value; }
        public int Seconds { get => settings.PreviewImageCustomization.Seconds; set => settings.PreviewImageCustomization.Seconds = value; }
        public bool ExtractEmbeddedImageCommand { get => settings.PreviewImageCustomization.ExtractEmbeddedImageCommand; set => settings.PreviewImageCustomization.ExtractEmbeddedImageCommand = value; }

        public async Task ExtractLoadedVideoPreviewImagesAsync()
        {
            await Task.Run(() =>
            {
                if (Directory.Exists(Path.Combine(Path.GetTempPath(), "VidHub", "Previews")))
                {
                    Directory.Delete(Path.Combine(Path.GetTempPath(), "VidHub", "Previews"), true);
                }
            });
            vs.Update(UpdateType.ForceUpdateVideoCollection);
        }

        public async Task RemoveAllPreviewImagesAsync()
        {
            await Task.Run(() =>
            {
                foreach (Core.Video item in Context.Host.GetService<IVideoService>())
                {
                    if (RelativePosition)
                    {
                        item.ExtractPreviewImage();
                    }
                    else
                    {
                        item.ExtractPreviewImage();
                    }
                }
            });
            vs.Update(UpdateType.ForceUpdateVideoCollection);
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
