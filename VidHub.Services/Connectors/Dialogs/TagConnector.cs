using System.Collections.ObjectModel;
using VidHub.Core.Models;
using VidHub.Core.Settings;
using VidHub.Services.Base;

namespace VidHub.Services.Connectors.Dialogs
{
    public class TagConnector(IVideoService vs, IVidHubSettings settings) : ConnectorTemplate(vs), ITagConnector
    {
        public ObservableCollection<Tag> Tags => settings.General.Tags;

        public void AddTag()
        {
            //Tags.Insert(0, new Tag());
        }

        public void ChangeColorPickerState(Tag tag)
        {
            tag.ColorPickerOpen = !tag.ColorPickerOpen;
        }

        public void RemoveTag(Tag tag)
        {
            Tags.Remove(tag);
            foreach (var video in vs)
            {
                video.TagID.Remove(tag.ID);
            }
        }
    }
}
