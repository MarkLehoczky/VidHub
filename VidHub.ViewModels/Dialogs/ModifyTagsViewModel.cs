using CommunityToolkit.Mvvm.ComponentModel;
using VidHub.Core.Models;
using VidHub.Core.Settings;

namespace VidHub.ViewModels.Dialogs
{
    public class VideoTag : Tag
    {
        private bool videoContains;

        public bool VideoContains { get => videoContains; set => SetProperty(ref videoContains, value); }

        public static VideoTag FromTag(Tag tag, bool videoContains = false)
        {
            return new VideoTag
            {
                ID = tag.ID,
                Name = tag.Name,
                Color = tag.Color,
                VideoContains = videoContains
            };
        }
    }

    public partial class ModifyTagsViewModel : ObservableRecipient
    {
        public List<VideoTag> Tags { get; } = [.. VidHubSettings.Instance.General.Tags.Select(t => VideoTag.FromTag(t))];
    }
}
